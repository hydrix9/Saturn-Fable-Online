using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SyncCasting : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts


    public static readonly string currentTarget = "currentTarget"; //label for accessing current target

    public bool init;

    public struct cooldownEntry
    {
        public DateTime castStarted;
        public DateTime cooldownFinished;

        public cooldownEntry(DateTime _castStarted, DateTime _cooldownFinished)
        {
            castStarted = _castStarted; cooldownFinished = _cooldownFinished;
        }
    }
    public Dictionary<int, cooldownEntry> cooldowns = new Dictionary<int, cooldownEntry>();
    public CoroutineHandle currentCastingInterruptCheck;

    [HideInInspector]
    public List<int> spellsKnown = new List<int>();
    [SerializeField]
    private List<string> spellsKnownByName = new List<string>(); //gets shown in inspector. For manual assignment, enter them there


    [HideInInspector]
    /// <summary>
    /// time when attack animation is done, allowing another attack that needs to wait for animation to finish
    /// </summary>
    public DateTime timeDoneAttacking = DateTime.UtcNow;
    public SyncObject autoAttackTarget;
    public CoroutineHandle autoAttackRoutine = default(CoroutineHandle); //keeps trying to see if we're in range
    public string autoAttackSpell;


    // IMPROVEMENT - ideally should remove these and handle the data with delegates or from within these classes themselves
    public SyncCurrentCasting syncCurrentCasting;
    public SyncUnsentCasts syncUnsentCasts;
    public SyncUnsentOnAnimationHits syncAnimationHits;

    public SyncCombatData syncCombatData;
    public SyncUnsentEffects syncUnsentEffects;
    public SyncInt16Field syncCurrentTarget;

    public delegate void OnCast();
    public event OnCast onCast;
    public delegate void OnCastInt(int spellID);
    public event OnCastInt onCastInt;

    public delegate void OnStartCast(Entity me);
    public event OnStartCast onStartCast;

    public delegate void OnInturrupted();
    public event OnInturrupted onInturrupted;

    public delegate void OnChangeTarget(Entity newTarget);
#pragma warning disable CS0414 // The field 'SyncCasting.onChangeTarget' is assigned but its value is never used
    public event OnChangeTarget onChangeTarget;
#pragma warning restore CS0414 // The field 'SyncCasting.onChangeTarget' is assigned but its value is never used

    [HideInInspector]
    public Entity parent;
    [HideInInspector]
    public AnimationController animationController;

    spellHit _currentCasting;
    public spellHit currentCasting { get => _currentCasting; set { _currentCasting = value; syncCurrentCasting.spellID.value = (value == null) ? 0 : _currentCasting.spell.id; } } //automatically sync id to clients

    /// <summary>
    /// return currentCasting != null
    /// </summary>
    public bool isCasting { get { return currentCasting != null; } }


    public override void ServerInit(SyncObject parent)
    {
        base.ServerInit(parent);
        this.parent = parent as Entity; //cache

    }


    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        //Logger.Log("synccasting init, load " + spellsKnownByName.Count + " spells on " + parent);

        this.parent = parent as Entity;
        if (this.parent != null)
        {
            this.animationController = this.parent.animationController;
            this.parent.onDeath += OnDeath;
        }

        AddDefaultSpells(); //add spells known by name

        syncCurrentCasting = new SyncCurrentCasting(this, parent);
        syncCombatData = new SyncCombatData(parent);
        syncUnsentCasts = new SyncUnsentCasts(this, parent);
        syncAnimationHits = new SyncUnsentOnAnimationHits(this, parent);
        syncUnsentEffects = new SyncUnsentEffects(parent);
        syncCurrentTarget = new SyncInt16Field(SyncCasting.currentTarget, false, 0, parent);
        syncFields = new SyncField[]
        {
            syncCurrentCasting,
            syncCombatData,
            syncUnsentCasts,
            syncAnimationHits,
            syncUnsentEffects,
            syncCurrentTarget, //current target field
        };

        init = true;

    } //end func SetDefaults

    //called from delegate
    void OnDeath(Entity self, spellHit killingBlow = null)
    {
        Inturrupted(); //stop casting current spell
    } //end func OnDeath

    public void ResetDefaultSpells()
    {
        spellsKnown.Clear(); //remove old
        AddDefaultSpells(); //add all spells known by name
    }


    void AddDefaultSpells()
    {
        spellsKnown.Clear(); //somehow this list got corrupted from serializing in editor, so resetting it now on startup, because there's no harm... should be empty anyways
        for (int i = 0; i < spellsKnownByName.Count; i++)
        {
            if (Spell.SpellExists(spellsKnownByName[i]))
                spellsKnown.Add(Spell.GetSpellID(spellsKnownByName[i]));
            else
                Logger.Log("unable to find spell " + spellsKnownByName[i]);
        }
        //spellsKnownByName.Clear(); //shouldn't need it...
    }

    /// <summary>
    /// start cast bar, then call FinishCast when done from Coroutine... called on both client (during write) and server (during initial cast..if it has a cast time)
    /// </summary>
    public void StartCast(spellHit spellHit)
    {
        PlayCastingAnimations();
        spellHit.spell.PlayCastStartedSFX(spellHit); //play this sound for server's local player... will play on clients during Write
        spellHit.spell.CreateCastStartedFX(spellHit); //create spell effects, based on their respective configs

        currentCasting = spellHit;

        if (spellHit.castStarted == null) //get rid of this null reference exception on casts with cast time on Server
            spellHit.castStarted = DateTime.UtcNow;

        if (Server.isServer)
        {
            if (!init)
                return;
            //client just called .Write which already set these values...
            //syncCurrentCasting.spellID.value = spellHit.spell.id; ///actually is set automatically from setter field
            syncCurrentCasting.castStartedMS.value = (int)(spellHit.castStarted.Millisecond / 4);
            syncCurrentCasting.castStartedSec.value = spellHit.castStarted.Second;
            if(spellHit.target != null)
                syncCurrentCasting.castTarget.value = spellHit.target.id;
            syncCurrentCasting.castTime.value = spellHit.castTime;


            //if (parent.isOnLocalServer) //client doesn't manage this
        } //end if isServer
        currentCastingInterruptCheck = Timing.RunCoroutine(CheckInturrupted()); //should keep checking if we're inturrupted, but if we finish, cast the spell

        TriggerOnStartCast();
    } //end func StartCast


    /// <summary>
    /// done with cast time, now do everything
    /// </summary>
    public void FinishCast(spellHit spellHit)
    {
        if (parent == null)
            return; //was destroyed

        if (!spellHit.spell.isAutoAttack) /// ? should this be spellHit.spell.targetsNone?
        {
            Timing.KillCoroutines(currentCastingInterruptCheck);
            currentCastingInterruptCheck = default(CoroutineHandle);
            currentCasting = null;
        } //end if !isAutoAttack

        //this is where the magic happens.


        //assign at least some value to these before potentially using them in an AoE calculation
        if (spellHit.origin == default)
            spellHit.origin = spellHit.caster.transform.position;
        if (spellHit.vTarget == default)
        {
            if (spellHit.target != null)
                spellHit.vTarget = spellHit.target.transform.position;
            else
                spellHit.vTarget = spellHit.caster.transform.position;
        }

        if (parent.isOnLocalServer)
        {


            if (spellHit.castTime > 0 && 
                (
                    (spellHit.spell.targetsSelected && (spellHit.target == null || spellHit.range < Vector3.Distance(spellHit.target.transform.position, spellHit.caster.transform.position))) //is a TargetSelected and out of range
                    //|| (spellHit.spell.spellTargeterConfig.usesVTargetRange && Vector3.Distance(spellHit.vTarget, spellHit.caster.transform.position) > spellHit.range) //uses vTarget range and out of range ///don't need to check this again... inpossible for vTarget to have moved
                )
            )
            { //if has a cast time and moved out of range while was casting

                //need to return castFailType?
                Logger.Log(spellHit.caster + "'s target moved out of range/died during the cast of " + spellHit.spell.name);
                return;
            }


            /* don't do this here, do it in Damage.Attack or whatever instead
            spellHit.target.CalcCastFlatDefensive(spellHit);
            spellHit.target.CalcCastPerDefensive(spellHit);
            */

            //finally, try do stuff

            //if configured to do so, do actual hard physical actions at a slight lag/offset for client-side fairness/perception/determinism
            //if you wanted, you could actually encode a DateTime for the future for it to be executed, ensuring it is 100% deterministic
            if (Server.instance.castFinishedLagOffsetMS > 0)
                Ext.CallDelayed(Server.instance.castFinishedLagOffsetMS, () => DoCastServerSideActions(spellHit));
            else
                DoCastServerSideActions(spellHit);
        }


        SyncFinishCast(spellHit);

    }

    //do all hard actions like spawning things, adding charge, subtracting energy, Damage.Attack...
    void DoCastServerSideActions(spellHit spellHit)
    {
        if (spellHit.caster == null)
            return; //they were destroyed mid cast... spellHit should always have a caster

        spellHit.spell.OnSuccessfullyFinishCast(spellHit); //actually call the action of the spell

        spellHit.spell.OnTryHitOffensive(spellHit); //doesn't actually do anything any more
        spellHit.caster.OnTryHitOffensive(spellHit); //trigger effects that were listening for this to finish casting
        TeamSpellModifier.instance.OnTryHitOffensive(spellHit); //perform team bonus effects on caster

        if (spellHit.cost > 0)  //if have a cost
            spellHit.caster.Subtraction(SyncEnergy.energy, ref spellHit.cost); //remove energy from caster

        //lose charge and call OnGainCharge
        spellHit.RemoveChargeFromCaster(); //spellHit class will handle removing charge to let chargeUsed remain private, which is based on maxChargeUsed

        spellHit.spell.OnUseCharge(spellHit);
        spellHit.caster.OnUseCharge(spellHit);
        TeamSpellModifier.instance.OnUseCharge(spellHit);

        //gain charge and call OnGainCharge
        if (spellHit.chargeGained > 0 && spellHit.caster.ContainsSyncField(SyncCharge.charge)) //if should gain charge and has a SyncCharge component as evident from having SyncCharge.charge field
            spellHit.caster.Addition_Clamped<float>(SyncCharge.charge, ref spellHit.chargeGained, 0, spellHit.caster.Get<float>(SyncCharge.maxCharge)); //add charge, clamped to maxCharge

        spellHit.spell.OnGainCharge(spellHit);
        spellHit.caster.OnGainCharge(spellHit);
        TeamSpellModifier.instance.OnGainCharge(spellHit);

        //DoAnimationsServerSide(spellHit); //do this here so that it is after all OnFinishCast stuff and also affected by the Server castFinishedLagOffsetMS if configured ///if you do this, it messes up the order of syncing things to the client...

        if (spellHit.spell.isAutoAttack && spellHit.caster.syncCasting.autoAttackRoutine != default(CoroutineHandle))
        { //if auto attacking and already auto attacking
            return;
        }

    } //end DoCastServerSideActions

    //do animations that are played on both Client and Server for Server
    void DoAnimationsServerSide(spellHit spellHit)
    {
        //animation and SFX
        if (spellHit.spell.animationClip != null && parent.animationController != null)
            parent.animationController.PlayClip(spellHit.spell.animationClip); //play spell's animation
        if (spellHit.spell.animationTimeMS != 0)
            timeDoneAttacking = DateTime.UtcNow.AddMilliseconds(spellHit.spell.animationTimeMS); //set variable to not allow other attacks which require animation state to be clear
        spellHit.spell.PlayCastFinishedSFX(spellHit); //play this sound for server's local player... will play on clients during Write
        spellHit.spell.CreateCastFinishedFX(spellHit); //create spell effects, based on their respective configs

        spellHit.spell.DoAnimation(spellHit); //do animations

        //end animation and SFX

    }

    /// <summary>
    /// this function is separated for the sole purpose of allowing a auto attack to finish itself directly rather than the either-or of (wait for interval) and (return)
    /// </summary>
    /// <param name="spellHit"></param>
    public void SyncFinishCast(spellHit spellHit)
    {
        if (parent == null)
            return;

        if (parent.isOnLocalServer)
        {
            cooldowns.Remove(spellHit.spell.id);
            //if(spellHit.cooldown > 0) //if has a cooldown ///if you do this it will read the wrong index during .Write
            cooldowns.Add(spellHit.spell.id, new cooldownEntry(DateTime.UtcNow, DateTime.UtcNow.AddSeconds(spellHit.cooldown))); //add entry that finishes by now + cooldown

            if (parent.renderInstance != null && (parent.isPlayer || parent.renderInstance.residesInHasPlayers)) //if there are any active players to witness this and need this data...
                syncUnsentCasts.unsentCastsRay.Add(new SyncUnsentCasts.unsentCastRay(spellHit, DateTime.UtcNow)); //sync this cast to clients
            else
                syncUnsentCasts.ResetToDefaultValue(); //remove all items that need to be synced, since nobody is around

        }

        DoAnimationsServerSide(spellHit); //do DoAnimation, fx, and sfx

        onCast?.Invoke(); //does gui
        onCastInt?.Invoke(spellHit.spell.id);



        if (spellHit.target != null) //check made for cone casts
            spellHit.target.TriggerOnUpdate(); //trigger update from their side being updated...

        spellHit.caster.TriggerOnUpdate(); //trigger update from their side being updated... do things like energy

    }


    public int TryInturrupt()
    {
        int success = -1;
        if (isCasting)
        {
            Inturrupted();
            success = 1;
        }
        currentCasting = null;
        Timing.KillCoroutines(currentCastingInterruptCheck);
        currentCastingInterruptCheck = default(CoroutineHandle);
        return success;
    }



    IEnumerator<float> CheckInturrupted()
    {
        while (currentCasting != null && (DateTime.UtcNow - currentCasting.castStarted).TotalSeconds < currentCasting.castTime) //keep checking every frame until completed cast
        {

            if (currentCasting.inturruptedByMovement && (((parent.nm.left || parent.nm.right) && parent.nm.strafe) || parent.nm.forward || parent.nm.backward))
            { //if we either strafed (without just turning) or moved forward/back
                Inturrupted();
                //currentCasting.resultCode = castResultCode.inturruptedByMovement;
            }
            yield return 0f; //would normally be something else but instead yield times are handled by custom Coroutine class Timing
        }

        //yay! finished casting


        if (currentCasting != null)
        { //if something weird didn't happen, like canceling the spell somewhere else...
            FinishCast(currentCasting);
        }
        currentCastingInterruptCheck = default(CoroutineHandle);
    }

    /// <summary>
    /// called when a entity casts an inturrupt as well as when client or server detects they moved before finishing from CheckInturrupted
    /// </summary>
    public void Inturrupted()
    {
        Logger.Log(name + " inturrupted ");

        if (currentCastingInterruptCheck != null)
            Timing.KillCoroutines(currentCastingInterruptCheck);

        PlayInturruptedAnimation();
        if (onInturrupted != null)
            onInturrupted();

        currentCasting = null;
        if (Server.isServer) {
            syncCurrentCasting.ResetToDefaultValue();
        }
    }

    public void TrySetAutoAttack(Entity target)
    {
        if (autoAttackTarget == target || autoAttackSpell == "" || autoAttackSpell == default || autoAttackRoutine != default(CoroutineHandle))
            return; //already set

        autoAttackTarget = target;
        Spell.GetSpell(autoAttackSpell).TryStartCast(target, parent, default, default);

    }

    /// <summary>
    /// makes sure it isn't cooling, is in range, and isn't blocked by animation
    /// </summary>
    public bool CanUseAbility(Spell spell, Entity target = null)
    {
        return spell != null && !IsSpellCooling(ref spell.id) && (animationController == null || animationController.CanUseAttack(spell)) && Spell.IsWithinRange(spell, parent, target);
    }

    /// <summary>
    /// tells whether someting is cooling down
    /// </summary>
    public bool IsSpellCooling(ref int id)
    {
        return cooldowns.ContainsKey(id) && 
            (Server.isServer ? 
            cooldowns[id].cooldownFinished : //is server, just behave normally
            cooldowns[id].cooldownFinished.AddMilliseconds(Math.Min(0, -Math.Max(Client.instance.currentToServerPing, Client.instance.averageToServerPing)))) //is client, try account for ping to server time with this cooldown
            > DateTime.UtcNow;
    }

    /// <summary>
    /// tells whether someting is cooling down
    /// </summary>
    public bool IsSpellCooling(int id)
    {
        return IsSpellCooling(ref id);
    }


    public void PlayInturruptedAnimation()
    {

    }


    public void PlayCastingAnimations()
    {
        //currentCasting...
    }



    public void TriggerOnStartCast()
    {
            onStartCast?.Invoke(parent);
    }

    public void TriggerOnCastInt(int spellID)
    {
            onCastInt?.Invoke(spellID);
    }

    public override void OnClean(SyncObject owner)
    {
        init = false;

        base.OnClean(owner);
        onCast = null;
        onCastInt = null;
        onInturrupted = null;
        onChangeTarget = null;
        onStartCast = null;
        cooldowns.Clear();
        parent = null;
        timeDoneAttacking = default;
        Timing.KillCoroutines(autoAttackRoutine);
        autoAttackRoutine = default;
        autoAttackTarget = null;
        Timing.KillCoroutines(currentCastingInterruptCheck);
        currentCastingInterruptCheck = default;

        _currentCasting = null;
    } //end func OnClean

} //end SyncCasting




/// <summary>
/// implementations must define fields for a ref to own entry in spellHit.serialization_id and whether to destroy automatically during OnAnimationHit, as well as how to do that
/// </summary>
public interface ISerializableCastAnimationObject
{
    int self_serializableCastID { get; set; }
    bool destroyOn_OnAnimationHit { get; set; }

    void CleanDestroy();
}

public class SyncUnsentCasts : SyncField
{
    public struct unsentCastRay
    { //used to allow us to also send date finished, so the ray can appear in the right place on client
        public spellHit spellHit;
        public DateTime dateFinished;

        public unsentCastRay(spellHit spellHit, DateTime dateFinished)
        {
            this.spellHit = spellHit;
            this.dateFinished = dateFinished;
        }
    }

    public const int
        unsentCastsRayLength = 36,
        lengthPerSerializationID_Entry = 2
        ;

    const byte maxUnsentCasts = 8;
    const int durationPrecision = 20; //fractions of a second, ex- 20 = 50ms precision

    public new static readonly string label = "syncUnsentCasts";

    static int currentSerializationID = 1;
    public static int nextSerializationID { get { currentSerializationID++; if (currentSerializationID >= short.MaxValue - 1) currentSerializationID = 1; return currentSerializationID; } }

    //used on client to go back and call OnAnimationHit using the respective object, possibly destroying it
    public Dictionary<int, ISerializableCastAnimationObject> serializedObjects = new Dictionary<int, ISerializableCastAnimationObject>();
    public Dictionary<int, DateTime> serializedObjects_dateAdded = new Dictionary<int, DateTime>();
    DateTime lastPurgedSerialisedObjects = DateTime.UtcNow; //used as a crude timer
    const float purgeSerialisedObjectsEvery = 60;
    const float persistSerializedObjectsDuration = 60f; //how long things last in the dictionary

    Dictionary<int, SyncCasting.cooldownEntry> cooldowns; //cache
    SyncCasting container;

    //public List<int> unsentCasts = new List<int>();
    //public List<spellHit> unsentCastsAoE = new List<spellHit>();
    public List<unsentCastRay> unsentCastsRay = new List<unsentCastRay>();
    public int currentSerializationIDsLength = 1; //should be 1 + (numItems * 2) but don't know num from underlying spellHits automatically so we track it here

    public SyncUnsentCasts(SyncCasting container, SyncObject parent) : base(label, false, parent) {
        this.container = container;
        cooldowns = container.cooldowns;
    }

    //mark the spellHit as having created this target (add to list), so that client knows what is being referenced during OnAnimationHit
    public void SetObjectSerializationID(int index, spellHit spellHit, ISerializableCastAnimationObject target)
    {
        if (Server.isServer)
        { //is server, write new serialization_id
            target.self_serializableCastID = nextSerializationID; //get next id from pool and set object's own reference to its id, used when it calls OnAnimationHit
            spellHit.serialization_ids.Add(target.self_serializableCastID); //add to spellHit itself
            currentSerializationIDsLength += lengthPerSerializationID_Entry; //add to total length for when we build this data and alloc
        } else
        { //is client, read what is already on the spellHit
            target.self_serializableCastID = spellHit.serialization_ids[index]; //read id already present on spellHit
            if (!serializedObjects.ContainsKey(target.self_serializableCastID))
            {
                serializedObjects.Add(target.self_serializableCastID, target); //importantly, also track it for when we eventually receive an OnAnimationHit for this
                serializedObjects_dateAdded.Add(target.self_serializableCastID, DateTime.UtcNow); //also keep track of when it was added so we can remove it later
            }
            if (DateTime.UtcNow.Subtract(lastPurgedSerialisedObjects).TotalSeconds > purgeSerialisedObjectsEvery)
                PurgeOldSerializedObjects(); //remove old entries from dictionary
        }

    } //end SetObjectSerializationID

    //remove old items from the dictioanry
    void PurgeOldSerializedObjects()
    {
        lastPurgedSerialisedObjects = DateTime.UtcNow; //reset timer
        //remove all entries older than duration
        foreach(KeyValuePair<int, DateTime> entry in serializedObjects_dateAdded.Where(entry => entry.Value == null || DateTime.UtcNow.Subtract(entry.Value).TotalSeconds >= persistSerializedObjectsDuration).ToList())
        {
            serializedObjects_dateAdded.Remove(entry.Key);
            serializedObjects.Remove(entry.Key);
        }
    } //end PurgeOldSerializedObjects

    public override bool NeedsBuild()
    {
        //return entity.unsentCasts || entity.unsentAoE...
        //return unsentCasts.Count > 0 || unsentCastsAoE.Count > 0 || unsentCastsRay.Count > 0;
        return unsentCastsRay.Count > 0;
    }

    public override int GetCurrentTotalFieldLength()
    {
        //return 3 + (unsentCasts.Count * unsentCastsLength) + (unsentCastsAoE.Count * unsentCastsAoELength) + (unsentCastsRay.Count * unsentCastsRayLength);
        return 1 + (Math.Min(maxUnsentCasts, unsentCastsRay.Count) * unsentCastsRayLength) + currentSerializationIDsLength;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario... used for initial buffer alllocation
        //return 3 + (5 * unsentCastsLength) + (5 * unsentCastsAoELength) + (5 * unsentCastsRayLength);
        return 1 + (maxUnsentCasts * unsentCastsRayLength) + 1 + (maxUnsentCasts * lengthPerSerializationID_Entry);
    }

    public override bool MoveReadPosAfterWrite()
    {
        return false;
    }

    spellHit unsentCastsRayTemp;
    int durationTemp;
    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        target[lastWritePos] = Math.Min((byte)unsentCastsRay.Count, maxUnsentCasts);
        //Logger.Log("building " + unsentCastsRay.Count + " unsent casts");
        lastWritePos += 1;

        for (int i = 0; i < unsentCastsRay.Count && i < maxUnsentCasts; i++) //255 the hard limit
        {
            unsentCastsRayTemp = unsentCastsRay[i].spellHit;
            target[lastWritePos] = (byte)unsentCastsRayTemp.spell.id; //send id
            target[lastWritePos + 1] = (byte)(unsentCastsRayTemp.spell.id >> 8);

            if (unsentCastsRayTemp.target != null)
            {
                target[lastWritePos + 2] = (byte)unsentCastsRayTemp.target.id; //send id
                target[lastWritePos + 3] = (byte)(unsentCastsRayTemp.target.id >> 8);
            }

            if (cooldowns.ContainsKey(unsentCastsRayTemp.spell.id))
            {
                target[lastWritePos + 4] = (byte)(cooldowns[unsentCastsRayTemp.spell.id].cooldownFinished.Millisecond / 4); //encode to nearest 4 milliseconds to fit 999 milliseconds into one byte (0-255)
                target[lastWritePos + 5] = (byte)cooldowns[unsentCastsRayTemp.spell.id].cooldownFinished.Second; //send second within the last 60 seconds it started...
                target[lastWritePos + 6] = (byte)cooldowns[unsentCastsRayTemp.spell.id].cooldownFinished.Minute; //send minute as byte
                target[lastWritePos + 7] = (byte)unsentCastsRay[i].dateFinished.Second; //send second within the last 60 seconds it started...
                target[lastWritePos + 8] = (byte)(unsentCastsRay[i].dateFinished.Millisecond / 4); //send second within the last 60 seconds it started...
            }

            durationTemp = (int)(unsentCastsRay[i].spellHit.duration * durationPrecision);
            //need duration mostly just for DoAnimation animations, some of which persist based on the duration of the spellHit
            target[lastWritePos + 9] = (byte)(durationTemp);
            target[lastWritePos + 10] = (byte)(durationTemp >> 8);
            target[lastWritePos + 11] = (byte)(durationTemp >> 16);

            NetworkIO.Vector3Serialize(ref unsentCastsRayTemp.origin, ref target, lastWritePos + 12);
            NetworkIO.Vector3Serialize(ref unsentCastsRayTemp.vTarget, ref target, lastWritePos + 24);

            lastWritePos += unsentCastsRayLength; //move write pos

            //write object serialization_ids (as for projectiles)
            target[lastWritePos] = (byte)unsentCastsRayTemp.serialization_ids.Count; //write num ids included
            lastWritePos++;
            
            if (unsentCastsRayTemp.serialization_ids.Count > 0)
            {
                for (int x = 0; x < unsentCastsRayTemp.serialization_ids.Count; x++)
                {
                    target[lastWritePos] = (byte)unsentCastsRayTemp.serialization_ids[x];
                    target[lastWritePos + 1] = (byte)(unsentCastsRayTemp.serialization_ids[x] >> 8);
                    lastWritePos += lengthPerSerializationID_Entry;
                }
            }
            //Ext.DebugByte(target, lastWritePos, lastWritePos + unsentCastsRayLength, "build unsent cast: ");

        }

        if (unsentCastsRay.Count > maxUnsentCasts)
            unsentCastsRay.RemoveRange(0, maxUnsentCasts);
        else
            unsentCastsRay.Clear();

        currentSerializationIDsLength = 1; //should just be 1 (byte for current count) until new items are added
    }

    int tempSpellID;
    Spell tempSpell;
    DateTime tempTime;
    //called on client when we receive an update
    public override void Write(ref byte[] data, ref int readPos, ref SyncObject target, Client context)
    {
        if (target == null)
            return;

        int numRay = data[readPos ];
        int num_serializedIDs = 0;
        int currentTargetTemp;
        
        readPos += 1;

        if(numRay > maxUnsentCasts)
        {
            //Debug.Break();
            Logger.LogWarning("trying to serialize an anomalous amount of data");
        }
 
        for (int i = 0; i < numRay; i++)
        { //read unsent casts

            currentTargetTemp = data[readPos + 2] | (data[readPos + 3] << 8);

            tempSpellID = data[readPos] | (data[readPos + 1] << 8);
            if(tempSpellID == 0)
            { //is zero.. didn't cast anything...

            } else
            if (Spell.TryGetSpell(ref tempSpellID, out tempSpell))
            {
                TryAddSelfClientCooldown(target, data, ref readPos, context); //add cooldown to ourself it is our own id
                                                                     //use cast here

                //get the time, which will allow us to set the real position of it after lag
               /* 
                tempTime = new DateTime( //date cooldown finishes
                            DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, data[readPos + 6], data[readPos + 5], (data[readPos + 4] * 4) //set new datetime because (DateTime).seconds doesn't have setter field.. with seconds and milliseconds(divided by 4 to fit <1 byte)
                            );
                */
                spellHit spellHit = new spellHit(tempSpell.GetBaseValues(), target.e, context.EntityExists(currentTargetTemp) ? context.GetEntity(currentTargetTemp).e : null, NetworkIO.Vector3Deserialize(ref data, readPos + 12), NetworkIO.Vector3Deserialize(ref data, readPos + 24));
                spellHit.duration = (data[readPos + 9] | (data[readPos + 10] << 8) | (data[readPos + 11] << 16)) / durationPrecision;
                /*
                if ((data[readPos + 5]) > DateTime.UtcNow.Second) //if rare case that being at this second NOW would put us into the future...
                    tempTime.Subtract(new TimeSpan(0, 1, 0)); //subtract a minute to avoid bug

                spellHit.castStarted = tempTime;
                */

                readPos += unsentCastsRayLength;
                num_serializedIDs = data[readPos];
                readPos++;

                if (num_serializedIDs > 0)
                { //if have any ids to add (most likely from projectiles)
                    spellHit.serialization_ids = new List<int>(num_serializedIDs);
                    for (int x = 0; x < num_serializedIDs; x++)
                    {
                        spellHit.serialization_ids.Add(data[readPos] | (data[readPos + 1] << 8)); //add entry
                        readPos += lengthPerSerializationID_Entry; //move write pos
                    }
                }
                
                
                FinishCast_Write(spellHit); //do effects from cast and animations... also, projectiles should read their ids in the same order as creation
            }
            else
            { //didn't recognize spell id
                Logger.LogWarning(target.id + " unable to handle unknown spell id " + tempSpell);
                
                //just move the readPos..
                readPos += unsentCastsRayLength;
                num_serializedIDs = data[readPos];
                readPos += num_serializedIDs * lengthPerSerializationID_Entry;
            }

        } //end loop over numRay

    } //end Write

    /// <summary>
    /// add a cooldown to ourself on client if it is our own entity id being targeted
    /// </summary>
    void TryAddSelfClientCooldown(SyncObject target, byte[] data, ref int readPos, Client context)
    {
        //set cooldown if our ID

        //if (context.player != null && target.id == context.player.id) ///actually have an effect on portals where it hides the inner ring if on CD
        //{ //only care about processing cooldowns of ourself
            Logger.Log(" min " + data[readPos + 6] + " sec " + data[readPos + 5] + " ms " + (data[readPos + 4] * 4));

            tempTime = new DateTime( //date cooldown finishes
                DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, data[readPos + 6], data[readPos + 5], (data[readPos + 4] * 4) //set new datetime because (DateTime).seconds doesn't have setter field.. with seconds and milliseconds(divided by 4 to fit <1 byte)
                );

            if ((data[readPos + 4]) > DateTime.UtcNow.Minute) //if rare case that being at this minute NOW would put us into the future...
                tempTime.Subtract(new TimeSpan(1, 0, 0)); //subtract an hour to avoid bug

            if (!target.e.syncCasting.cooldowns.ContainsKey(tempSpellID))
            {
                target.e.syncCasting.cooldowns.Add(tempSpellID, new SyncCasting.cooldownEntry(DateTime.UtcNow, tempTime)); //add to dictionary of cooldowns. CastStarted actually was DateTime.UtcNow - latency, but it doesn't matter that much, as long as it finishes on time
            }
            else
            {
                target.e.syncCasting.cooldowns[tempSpellID] = new SyncCasting.cooldownEntry(DateTime.UtcNow, tempTime);
            }
        //} //end if is self
    } //end func TryAddSelfClientCooldown


    //do clientside stuff like animations
    void FinishCast_Write(spellHit spellHit)
    {

        //TODO: now we can adjust the animation for lag in DoAnimation if we're client based on the time stored at spellHit.castStarted. This is the time that the cast finished on server. Go forward in time by (DateTime.UtcNow - spellHit.castStarted). Divide this by tick time for num ticks.

        //do clientside animations
        if (spellHit.spell.animationClip != null && spellHit.caster.animationController != null)
            spellHit.caster.animationController.PlayClip(spellHit.spell.animationClip); //play spell's animation

        if (spellHit.spell.breaksStealth)
            spellHit.caster.TryRemoveStealth(null);

        //Debug.LogWarning("DoAnimation " + spellHit.spell.name);

        spellHit.spell.DoAnimation(spellHit);
        spellHit.spell.PlayCastFinishedSFX(spellHit); //play this sound for server's local player... will play on clients during Write
        spellHit.spell.CreateCastFinishedFX(spellHit);

        //container.currentCasting = null; //not casting anything anymore... this is also done during FinishCast

        //OnlineGUI.From(parent)

    }

    public override void Destroy()
    {
        base.Destroy();
        tempSpell = null;
        tempTime = default;
        unsentCastsRayTemp = null;
        unsentCastsRay.Clear();
        cooldowns.Clear();
        serializedObjects.Clear();
        serializedObjects_dateAdded.Clear();
    }

    public override void ResetToDefaultValue()
    {
        tempSpell = null;
        tempTime = default;
        unsentCastsRayTemp = null;
        unsentCastsRay.Clear();
        cooldowns.Clear();
        serializedObjects.Clear();
        serializedObjects_dateAdded.Clear();
        currentSerializationIDsLength = 1;
    }

} //end SyncUnsentCasts

public class SyncUnsentOnAnimationHits : SyncField
{
    public new static readonly string label = "SyncUnsentOnAnimationHits";

    //these are used on Server to batch/save a list of OnHitAnimations (and their respective triggering serialization_id)
    private List<spellHit> unsent = new List<spellHit>();
    private List<int> triggeringSerializationIDs = new List<int>();

    public const int max_sync_perUpdate = 8; //max number to sync per update per entity/caster before dropping updates..should be doing 10/20 updates a sec depending on Fixed Time config so this is plenty

    public const int
        unsentEntryLength = 30
    ;

    SyncUnsentCasts syncUnsentCasts; //keep ref so we can read off recently created serialized_ids

    public SyncUnsentOnAnimationHits(SyncCasting container, SyncObject parent) : base(label, false, parent)
    {
        this.syncUnsentCasts = container.syncUnsentCasts;
    }

    //called on Server during OnAnimationHit
    //add an unsent OnAnimationHit with an optional ID to designate to client which object is triggering
    public void Add(spellHit spellHit, int fromSerializationID = 0)
    {
        if (parent != null && parent.renderInstance != null && (parent.isPlayer || parent.renderInstance.residesInHasPlayers))
        {
            unsent.Add(spellHit);
            triggeringSerializationIDs.Add(fromSerializationID);
        }
    }

    public override bool MoveReadPosAfterWrite()
    {
        return false; //exert manual control over write pos during Write, rather than reading GetCurrentTotalFieldLength or GetWorstTotalFieldLength
    }


    public override bool NeedsBuild()
    {
        return unsent.Count > 0;
    }

    public override int GetCurrentTotalFieldLength()
    {
        return 1 + (Math.Min(max_sync_perUpdate, unsent.Count) * unsentEntryLength);
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario... used for initial alllocation
        return 1 + (max_sync_perUpdate * unsentEntryLength);
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        target[lastWritePos] = (byte)Math.Min(unsent.Count, max_sync_perUpdate);
        //Logger.Log("building " + unsent.Count + " unsent casts");
        lastWritePos += 1;

        for (int i = 0; i < unsent.Count && i < max_sync_perUpdate; i++) //max_sync_perUpdate the hard limit
        {
            target[lastWritePos] = (byte)unsent[i].spell.id; //send id
            target[lastWritePos + 1] = (byte)(unsent[i].spell.id >> 8);

            if (unsent[i].target != null)
            {
                target[lastWritePos + 2] = (byte)unsent[i].casterID; //send id
                target[lastWritePos + 3] = (byte)(unsent[i].casterID >> 8);
            }

            NetworkIO.Vector3Serialize(ref unsent[i].origin, ref target, lastWritePos + 4);
            NetworkIO.Vector3Serialize(ref unsent[i].vTarget, ref target, lastWritePos + 16);

            if (triggeringSerializationIDs[i] != 0)
            {
                //also write the id of the object that triggered this, if any
                target[lastWritePos + 28] = (byte)triggeringSerializationIDs[i];
                target[lastWritePos + 29] = (byte)(triggeringSerializationIDs[i] >> 8);
            }

            //Ext.DebugByte(target, lastWritePos, lastWritePos + unsentLength, "build unsent cast: ");
            lastWritePos += unsentEntryLength;
        }

        if (unsent.Count > max_sync_perUpdate)
        {
            unsent.RemoveRange(0, max_sync_perUpdate);
            triggeringSerializationIDs.RemoveRange(0, max_sync_perUpdate);
        }
        else
        {
            unsent.Clear();
            triggeringSerializationIDs.Clear();
        }

    }

    int numRay;
    int currentTargetTemp;
    int serializedIDTemp;
    int tempSpellID;
    Spell tempSpell;
    //called on client when we receive an update
    public override void Write(ref byte[] data, ref int readPos, ref SyncObject target, Client context)
    {
        if (target == null)
            return;

        if(readPos >= data.Length)
            return;

        numRay = data[readPos];
        readPos += 1;

        if (numRay > max_sync_perUpdate)
        {
            //TODO- implement handling more than max_sync_perUpdate OnAnimationHits per frame?... (due to GetWorstTotalFieldLength)
            Logger.LogWarning("trying to serialize an anomalous amount of data");
            return;
        }

        for (int i = 0; i < numRay; i++)
        { //read unsent casts

            tempSpellID = data[readPos] | (data[readPos + 1] << 8);
            currentTargetTemp = data[readPos + 2] | (data[readPos + 3] << 8);

            if (tempSpellID == 0)
            { //is zero.. didn't cast anything...

            }
            else
            if (Spell.TryGetSpell(ref tempSpellID, out tempSpell))
            { //if spell exists
                spellHit spellHit = new spellHit(tempSpell.GetBaseValues(), context.EntityExists(currentTargetTemp) ? context.GetEntity(currentTargetTemp).e : null, target.e, NetworkIO.Vector3Deserialize(ref data, readPos + 4), NetworkIO.Vector3Deserialize(ref data, readPos + 16));

                serializedIDTemp = data[readPos + 28] | (data[readPos + 29] << 8);

                //do the effect on the client
                //if contain a reference to this recently created object/projectile
                if (spellHit.caster != null && spellHit.caster.syncCasting.syncUnsentCasts.serializedObjects.ContainsKey(serializedIDTemp))
                    spellHit.spell.OnAnimationHit(spellHit, spellHit.caster.syncCasting.syncUnsentCasts.serializedObjects[serializedIDTemp]); //use it
                else
                    spellHit.spell.OnAnimationHit(spellHit); //don't need it or don't know what projectile they're talking about
            }
            else
            {
                Logger.LogWarning(target.id + " unable to handle unknown spell id " + tempSpell);
            }

            readPos += unsentEntryLength;
        }

    } //end Write


    public override void Destroy()
    {
        base.Destroy();
        tempSpell = null;
        syncUnsentCasts = null;
        unsent.Clear();
        triggeringSerializationIDs.Clear();

    }

    public override void ResetToDefaultValue()
    {
        tempSpell = null;
        syncUnsentCasts = null;
        unsent.Clear();
        triggeringSerializationIDs.Clear();

    }

} //end class SyncUnsentOnAnimationHits



public class SyncCurrentCasting : SyncFieldGroup
{

    public SyncInt16Field spellID;
    public SyncFloat16Field_1024 castTime;
    public SyncInt8Field castStartedSec;
    public SyncInt8Field castStartedMS;
    public SyncInt16Field castTarget;

    SyncCasting syncCasting;

    public new static readonly string label = "SyncCurrentCasting";
    public SyncCurrentCasting(SyncCasting syncCasting, SyncObject parent) : base(label, true, parent)
    {
        this.syncCasting = syncCasting;
    }

    public override void SetDefaults(out SyncField[] fields)
    {
        spellID = new SyncInt16Field("spellID", false, 0, parent);
        castTime = new SyncFloat16Field_1024("castTime", false, 0, parent);
        castStartedSec = new SyncInt8Field("castStartedSec", false, 0, parent);
        castStartedMS = new SyncInt8Field("castStartedMS", false, 0, parent);
        castTarget = new SyncInt16Field("castTarget", false, 0, parent);

        fields = new SyncField[] { spellID, castTime, castStartedSec, castStartedMS, castTarget };
    }

    //base.BuildTo()

    public override bool NeedsBuild()
    {
        //how it originally was...
        // return (syncCasting.currentCasting != null && oldCurrentCasting != syncCasting.currentCasting.spell.id); //currentCasting.id has been set! something happened...
        return (syncCasting.currentCasting != null && syncCasting.currentCasting.spell.id != oldCurrentCasting) || (oldCurrentCasting != 0 && syncCasting.currentCasting == null); //if either started casting or stopped casting
    }

    /*
    * override void BuildTo()
    * Don't need to override BuildTo because all of the fields are generic int/float and no additional actionis needed
    *
    */

    public int oldCurrentCasting = 0;
    spellHit currentCastingTemp;
    Entity castTargetTemp;
    //called on client when we receive an update
    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        base.Write(ref data, ref startPos, ref target, context); //write all fields in this group

        if (target == null)
            return;

        //could do this during onUpdate, except don't know which is being written last, so instead we do it from this overloaded method
        //take the values that were written, and recreate the spellHit that was casted
        //then do GUI stuff if applicable
        if (spellID.value != 0)
        { //if they casted something rather than 0 which would indicate finishing or inturrupted...

            if (context.EntityExists(castTarget.value))
                castTargetTemp = context.idToEntity[castTarget.value].e;
            else
                castTargetTemp = null; //not found, maybe destroyed or out of render distance?

            //syncCasting.TryInturrupt(); //remove old currentCasting

            syncCasting.currentCasting = new spellHit(Spell.GetSpell(spellID.value).GetBaseValues(), target.e, castTargetTemp, default, default);

            currentCastingTemp = syncCasting.currentCasting; //cache

            currentCastingTemp.castTime = castTime.value; //cast time
            currentCastingTemp.castStarted = new DateTime( //date started
                DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, castStartedSec, (castStartedMS * 4) //set new datetime because (DateTime).seconds doesn't have setter field.. with seconds and milliseconds(divided by 4 to fit <1 byte)
                );
            if ((castStartedSec) > DateTime.UtcNow.Second) //if rare case that being at this second NOW would put us into the future...
                currentCastingTemp.castStarted.Subtract(new TimeSpan(0, 1, 0)); //subtract a minute to avoid bug

            if (context.onlineGUI.wowTargetingControls != null && context.onlineGUI.wowTargetingControls.currentTarget == null && context.player != null && castTarget == context.player.id && target.id != castTarget) //if we don't have a target, they attacked us, and they are an enemy, and not casting at self
                context.onlineGUI.wowTargetingControls.AssignTarget(parent as Entity); //auto Target them because they casted at us...

            //start casting in same manner that server would
            syncCasting.StartCast(currentCastingTemp); //should trigger GUI updates if we're targeting them already

        }
        else
        {
            //Logger.Log("currentCasting cleared");
            syncCasting.currentCasting = null; //clear
        }
    } //end func Write

    public override void Destroy()
    {
        base.Destroy();
        spellID = null;
        castTime = null;
        castStartedSec = null;
        castStartedMS = null;
        castTarget = null;
        currentCastingTemp = null;
        castTargetTemp = null;
        syncCasting = null;
    }

    public override void ResetToDefaultValue()
    {
        syncCasting.currentCasting = null;
        spellID.ResetToDefaultValue();
        castTime.ResetToDefaultValue();
        castStartedSec.ResetToDefaultValue();
        castStartedMS.ResetToDefaultValue();
        castTarget.ResetToDefaultValue();

    }

} //end class SyncCurrentCasting

public class SyncCombatData : SyncField
{
    public struct castTextCodes
    { //indicate over the network the context of the number received
        public const int
            damage = 1,
            heal = 2,
            dodged = 3,
            resisted = 4,
            evaded = 5,
            miss = 6,
            parry = 7,
            blocked = 8,
            stun = 9,
            immobilized = 10,
            enterCombat = 11,
            exitCombat = 12,
            other = 13
        ;
    }


    public new static readonly string label = "SyncCombatData";

    /// <summary>
    /// list of structs to be sent to players
    /// </summary>
    public PrelocArray unsentCombatData = new PrelocArray(80, true, 20); //array that holds data about damage and healing done to this player

    public SyncCombatData(SyncObject parent) : base(label, false, parent)
    {
        unsentCombatData = new PrelocArray(80, true, 20); //array that holds data about damage and healing done to this player
    }

    const int entryLength = 5;

    public void AddEntry(ref int casterID, ref int power, int castTextCode)
    {
        if (parent != null && parent.renderInstance != null && (parent.isPlayer || parent.renderInstance.residesInHasPlayers))
        {
            unsentCombatData.AddInt2(ref casterID);
            unsentCombatData.AddInt2(ref power);
            unsentCombatData.AddInt1(castTextCode);
        }
    } //end func AddEntry

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {

        if (unsentCombatData.writePos > 0)
        {
            /*
            if (target.Length < lastWritePos + 1 + (unsentCombatData.writePos) + 1) //if can't fit current data, unsent effects and count of unsent effects
                Array.Resize<byte>(ref target, lastWritePos + 1 + (unsentCombatData.writePos) + 1); //resize to fit this data
                */
            int startPos = lastWritePos;
            target[lastWritePos] = (byte)((unsentCombatData.writePos) / entryLength); //inform of length where each entry is 5 in length
            //Logger.Log("writing combat data starting at " + unsentCombatData.writePos + "... " + ((byte)((unsentCombatData.writePos) / 5)) + " entries");
            lastWritePos++; //move past byte representing length
                            //could cause an error if there are more than 255 entries

            // Logger.Log("data written before: " + Ext.ByteToString(target));

            unsentCombatData.CopyToAndResetIndexMod(ref target, ref lastWritePos); //will mod lastwritepos based on length as well
           // Ext.DebugByte(target, startPos, lastWritePos, "build combat data:");
            // Logger.Log("data written after: " + Ext.ByteToString(target));
        }


        //Logger.Log("data written after: " + Ext.ByteToString(target));
    }


    /// BUG: --- not sure what info to put in this field 

    public override int GetCurrentTotalFieldLength()
    {
        return unsentCombatData.writePos;
    }
    public override int GetWorstTotalFieldLength()
    {
        return unsentCombatData.maxSize;
    }

    public override bool NeedsBuild()
    {
        return unsentCombatData.writePos > 0;
    }

    public override bool MoveReadPosAfterWrite()
    {
        return false;
    }

    int casterIDTemp;
    int powerTemp;
    int textEventCodeTemp;
    public override void Write(ref byte[] data, ref int readPos, ref SyncObject target, Client context)
    {
        if (target == null)
            return;

        //Logger.Log("reading combat data starting at " + readPos);
        int count = data[readPos];

        //Ext.DebugByte(data, readPos, readPos + ((count * entryLength) + 1), "writing combat data: ");


        readPos++; //move past info on length
        for (int i = 0; i < count; i++) // count number specified as number of entries
        {
            casterIDTemp = data[readPos] | (data[readPos + 1] << 8);
            powerTemp = data[readPos + 2] | (data[readPos + 3] << 8);
            textEventCodeTemp = data[readPos + 4];
            if (target.e != null)
            {
                if (textEventCodeTemp == castTextCodes.damage)
                { //if power is 0, wasn't technically damaged...
                    target.e.TriggerOnDamaged(null); //should unstealth, trigger AI and maybe others
                }

                OnlineGUI.TryMakeCombatText(target.e, ref casterIDTemp, powerTemp, textEventCodeTemp); //read castedby,power and add combat text
            }
            readPos += entryLength;
        }
    }


    public override void Destroy()
    {
        base.Destroy();
        unsentCombatData.Clear();
        unsentCombatData = null;
    }

    public override void ResetToDefaultValue()
    {
        unsentCombatData.Clear();
    }

} //end SyncCombatData


public class SyncUnsentEffects : SyncField
{
    public new static readonly string label = "SyncUnsentEffects";
    public const int 
        unsentEffectLength = 11,
        unsentRemovedEffectLength = 4
        ;

    /// <summary>
    /// merely what hasn't been sent yet. For full list of current effects, use currentEffects
    /// </summary>
    public List<spellHit> unsentEffects = new List<spellHit>();
    public List<spellHit> unsentRemovedEffects = new List<spellHit>(); //unsynced removed effects

    const int maxEffectsPerUpdate = 3;

    public SyncUnsentEffects(SyncObject parent) : base(label, false, parent)
    {
    }

    int durationTemp;
    spellHit unsentEffectTemp;
    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {

        //Logger.Log("writing unsent effects starting at " + lastWritePos + " which should take up " + ((unsentEffects.Count * 9) + 1));
        //Logger.Log("data written before: " + Ext.ByteToString(target));

        target[lastWritePos] = (byte)Math.Min(maxEffectsPerUpdate, unsentEffects.Count); //inform of count
        lastWritePos += 1;
        target[lastWritePos] = (byte)Math.Min(maxEffectsPerUpdate, unsentRemovedEffects.Count);
        lastWritePos += 1;

        for (int i = 0; i < unsentEffects.Count && i < maxEffectsPerUpdate; i++)
        {
            unsentEffectTemp = unsentEffects[i];
            target[lastWritePos] = (byte)unsentEffectTemp.spell.id;
            target[lastWritePos + 1] = (byte)(unsentEffectTemp.spell.id >> 8);
            target[lastWritePos + 2] = (byte)(unsentEffectTemp.caster.id);
            target[lastWritePos + 3] = (byte)(unsentEffectTemp.caster.id >> 8);
            target[lastWritePos + 4] = (byte)unsentEffectTemp.castStarted.Second; //inform of second started ticking so they know when to start counting duration (ping). Written in Ability.TryAddEffect
            durationTemp = (int)(unsentEffectTemp.duration * 1000);
            target[lastWritePos + 5] = (byte)durationTemp;
            target[lastWritePos + 6] = (byte)((int)durationTemp >> 8);
            target[lastWritePos + 7] = (byte)((int)durationTemp >> 16);

            target[lastWritePos + 8] = (byte)((unsentEffectTemp.interval * 100));
            target[lastWritePos + 9] = (byte)((int)(unsentEffectTemp.interval * 100) >> 8);
            target[lastWritePos + 10] = (byte)(unsentEffectTemp.buffDebuff ? 1 : 0);
            lastWritePos += unsentEffectLength;
        }

        if (unsentEffects.Count > maxEffectsPerUpdate)
            unsentEffects.RemoveRange(0, maxEffectsPerUpdate);
        else
            unsentEffects.Clear();

        for (int i = 0; i < unsentRemovedEffects.Count && i < maxEffectsPerUpdate; i++)
        {
            unsentEffectTemp = unsentRemovedEffects[i];
            target[lastWritePos] = (byte)unsentEffectTemp.spell.id;

            target[lastWritePos + 1] = (byte)(unsentEffectTemp.spell.id >> 8);
            target[lastWritePos + 2] = (byte)(unsentEffectTemp.caster.id);
            target[lastWritePos + 3] = (byte)(unsentEffectTemp.caster.id >> 8);
            lastWritePos += unsentRemovedEffectLength;
        }
        if (unsentRemovedEffects.Count > maxEffectsPerUpdate)
            unsentRemovedEffects.RemoveRange(0, maxEffectsPerUpdate);
        else
           unsentRemovedEffects.Clear();
    }

    public override int GetWorstTotalFieldLength()
    { //how long the serialized data is in bytes worst case
        return 2 + (maxEffectsPerUpdate * unsentEffectLength) + (maxEffectsPerUpdate * unsentRemovedEffectLength); //just guessing that allocating for max 10 will be enough.... can improve this design with auto resizing allocation
    }
    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 2 + (Math.Min(maxEffectsPerUpdate, unsentEffects.Count) * unsentEffectLength) + (Math.Min(maxEffectsPerUpdate, unsentRemovedEffects.Count) * unsentRemovedEffectLength);
    }

    public override bool NeedsBuild()
    {
        return unsentEffects.Count > 0 || unsentRemovedEffects.Count > 0;
    }

    public override bool MoveReadPosAfterWrite()
    {
        return false;
    }

    int tempSpellID;
    Spell tempSpell;

    SyncObject spellCasterTemp;
    int spellCaster;
    //called on client when we receive an update
    public override void Write(ref byte[] data, ref int readPos, ref SyncObject target, Client context)
    {
        if (target == null)
            return;

        if (readPos >= data.Length)
            return; //oob

        //Logger.Log("reading status effects data starting at " + readPos + " with data " + Ext.ByteToString(data));
        int statusEffects = data[readPos];
        int removedEffects = data[readPos + 1];
        readPos += 2; //move past info on length

        for (int i = 0; i < statusEffects; i++)
        { //read unsent casts

            tempSpellID = data[readPos] | (data[readPos + 1] << 8);
            spellCaster = (data[readPos + 2] | (data[readPos + 3] << 8));

            if (!Spell.TryGetSpell(ref tempSpellID, out tempSpell))
            {
                Logger.LogWarning(target.id + " unable to handle unknown spell id " + (data[readPos] | (data[readPos + 1] << 8)));
                return;
            }
            else
            {
                
                spellHit statusEffect = tempSpell.GetFullStats(target.e, context.idToEntity.TryGetValue(spellCaster, out spellCasterTemp) && spellCasterTemp != null ? spellCasterTemp.e : null, default, default);

                statusEffect.casterID = spellCaster; //set just in case the caster wasn't in context.idToEntity and therefore didn't get set automatically.. need this so we can find and remove later

                statusEffect.castStarted = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, data[readPos + 4]);

                if ((data[readPos + 4]) > DateTime.UtcNow.Second) //if rare case that being at this second NOW would put us into the future...
                    statusEffect.castStarted.Subtract(new TimeSpan(0, 1, 0)); //subtract a minute to avoid bug

                statusEffect.duration = (data[readPos + 5] | (data[readPos + 6] << 8) | (data[readPos + 7] << 16)) / 1000f;
                statusEffect.interval = (data[readPos + 8] | (data[readPos + 9] << 8));
                statusEffect.buffDebuff = data[readPos + 10] == 1; //bool byte...

                //target.e.statusEffects.Add(statusEffect);
                StatusEffects.TryAdd(statusEffect, statusEffect.duration > 0, false, false); //add the StatusEffect the same way the server would... if it gets re-applied on the Server... server should also send a remove event anyways

                //Logger.LogWarning("adding status effect " + statusEffect.spell.name + " with duration " + statusEffect.duration);
                /* don't need this anymore since we're actually adding from StatusEffect.TryAdd
                if (statusEffect.buffDebuff)
                    target.e.TriggerOnAddBuff(statusEffect);
                else
                    target.e.TriggerOnAddDebuff(statusEffect);
                */
                readPos += unsentEffectLength;
            }
        } //end loop over StatusEffects

        for (int i = 0; i < removedEffects; i++)
        {
            StatusEffects.TryRemove_Client(target.e, data[readPos] | (data[readPos + 1] << 8), data[readPos + 2] | (data[readPos + 3] << 8));
            readPos += 4;
        }

    } //end func Write

    public override void Destroy()
    {
        base.Destroy();
        unsentEffects.Clear();
        unsentRemovedEffects.Clear();
        tempSpell = null;
        unsentEffectTemp = null;
        spellCasterTemp = default;

    }

    public override void ResetToDefaultValue()
    {
        unsentEffects.Clear();
        //unsentRemovedEffects.Clear();
    }
} //End SyncUnsentEffects