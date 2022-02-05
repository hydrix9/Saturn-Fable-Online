using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;

/// <summary>
/// synchronizes the capping of a point. Extends SyncComponent which attaches to a SyncObject because this object will have an id to differentiate between different points on client
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class SyncCapZone : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    WorldModularGUI worldModularGUI; //trigger for the capture popup that goes through OnlineGUI

    SyncInt16Field attackingFactionField;
    SyncInt16Field controllingFactionField;
    SyncInt8Field numAttackingField;
    SyncBoolField captureProgressingField;
    SyncFloat16Field_1024 captureProgressPercentField;

    public float captureTime = 10f;
    [HideInInspector]
    DateTime captureStart = default;

    public string startControllingFaction = SyncFaction.pirates;

    public bool isCapturing => captureStart != default;
    public bool isContesting => !captureProgressingField.value; //if it isn't progressing then we are contesting
    public int numOnPoint { get { entitiesOnPoint.RemoveAll(entry => entry == null); return entitiesOnPoint.Count; } }
    public int numAttacking => numAttackingField.value;
    public int attackingFaction => attackingFactionField.value;
    public int controllingFaction => controllingFactionField.value;

    public float secsUntilCapture; //timer countdown to capture, doesn't move when contested

    List<Entity> entitiesOnPoint = new List<Entity>();
    CoroutineHandle captureRoutine;

    public delegate void OnCapture(SyncCapZone capZone, int newFaction, int oldFaction);
    public event OnCapture onCapture;

    //codepoints
    public static readonly string
        controllingFactionFieldName = "controllingFaction",
        attackingFactionFieldName = "attackingFaction",
        numAttackingFieldName = "numAttacking",
        captureProgressingFieldName = "captureProgressing",
        captureProgressPercentFieldName = "captureProgress"
    ;


    private void Awake()
    {
        worldModularGUI = GetComponentInChildren<WorldModularGUI>();
        gameObject.layer = LayerMask.NameToLayer(WorldFunctions.entityInteractionLayerName);

    } //end Awake

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        if (parent != null)
        { //called from compile, which won't give TeamSpellModifier to register during Awake for new factions...
            if (startControllingFaction != "")
                GameMode.instance.TryAddFaction(startControllingFaction, false); //make sure faction exists
        }
        int startFactionID = GameMode.FactionExists(startControllingFaction) ? GameMode.GetFactionID(startControllingFaction) : 0;

        Logger.Log(name + " starts on faction " + startFactionID);
        controllingFactionField = new SyncInt16Field(controllingFactionFieldName, false, startFactionID, parent);
        attackingFactionField = new SyncInt16Field(attackingFactionFieldName, false, 0, parent);
        numAttackingField = new SyncInt8Field(numAttackingFieldName, false, 0, parent);
        captureProgressingField = new SyncBoolField(captureProgressingFieldName, false, false, parent);
        captureProgressPercentField = new SyncFloat16Field_1024(captureProgressPercentFieldName, false, 0, parent);

        //WARNING- these fields will call ResetToDefaultValue when game resets from base.ResetFieldsToDefaults during this.OnResetGame called from GameMode.onResetGame_Server

        syncFields = new SyncField[]
        { //set this array for base
            controllingFactionField,
            attackingFactionField,
            numAttackingField,
            captureProgressingField,
            captureProgressPercentField
        };

    } //end SetDefault

    public void OnTriggerEnter(Collider other)
    {
        TryAddEntity(other.GetComponent<Entity>());
    }

    public void OnTriggerExit(Collider other)
    {
        TryRemoveEntity(other.GetComponent<Entity>());
    }

    void TryAddEntity(Entity attacker)
    {
        if (attacker == null)
            return;

        string attackerF = attacker.faction;
        string defenderF = ""; //set if defender exists

        if (controllingFactionField.value > 0) //if defined
            defenderF = GameMode.GetFaction(controllingFactionField.value);

        if (!isCapturing)
        { //if not already being attacked
            if(controllingFactionField.value <= 0 || GameMode.instance.CheckFriendly(ref attackerF, ref defenderF))
            { //if isn't owned by anyone or hostile toward each other/defender doesn't exist
                StartAttacking(attacker, ref attackerF); //try to start capturing point if not contested
            } else
            {
                StartDefending(attacker, ref attackerF); //the new entity on point defends because they are friendly
            }
        }

    } //end function TryAdd

    void TryRemoveEntity(Entity entity)
    {
        if(entity != null)
        {
            RemoveOnPoint(entity);
        }
    }

    //begin countdown and add attacker
    void StartAttacking(Entity attacker, ref string attackerFaction)
    {
        if(attackingFactionField.value <= 0) //if an attack isn't in progress
            attackingFactionField.value = GameMode.GetFactionID(attackerFaction);
        SetOnPoint(attacker);

        worldModularGUI.ForceTriggerArea(); //this should trigger the defenders already colliding with the collider trigger to also show the popup
    }

    void StartDefending(Entity defender, ref string defenderFaction)
    {
        captureProgressingField.value = false;
        SetOnPoint(defender);
    }

    //add ref that entity is either attacking or defending
    void SetOnPoint(Entity entity)
    {
        if (entity != null)
        {
            entitiesOnPoint.Add(entity);
            entity.onDeath += OnEntityDeath; //remove defending ref if they die

            TryStartTickCapture(); //try to start capturing if not in progress because num entities on point changed
        }
    }

    void OnEntityDeath(Entity entity, spellHit killingBlow = null)
    {
        if (Server.isServer)
        {
            RemoveOnPoint(entity);
        }
    } //end func OnEntityDeath

    void RemoveOnPoint(Entity entity)
    {
        if (entity != null)
        {
            entitiesOnPoint.Remove(entity);
            entity.onDeath -= OnEntityDeath; //remove listener
            TryStartTickCapture(); //try to start capturing if not in progress because num entities on point changed
        }

    }

    //check all entities on point and try start capturing if nobody contests
    void TryStartTickCapture()
    {
        string controllingFaction = GameMode.GetFaction(controllingFactionField.value);
        int numAttacking = 0;
        int numContesting = 0;
        List<Entity> attackers = new List<Entity>();
        for (int i = 0; i < entitiesOnPoint.Count; i++)
        { //check if any are contesting
            if (GameMode.instance.CheckFriendly(entitiesOnPoint[i], ref controllingFaction))
            { //if friendly, halt progression
                numContesting++;
            }
            else
            { //hostile
                numAttacking++;
                attackers.Add(entitiesOnPoint[i]);
            }
        }
        if (numContesting <= 0 && numAttacking > 0)
        { //if nobody contested
            captureProgressingField.value = true;
            if (!isCapturing)
            { //if haven't started routine yet
                if(attackers.Count <= 0)
                    Logger.LogError("no attackers during TryStartTickCapture"); //something weird happened
                attackingFactionField.value = attackers[0].factionID;
                StartTickCapture();
            }
        } else
        {
            captureProgressingField.value = false;
        }

        if(numAttacking <= 0)
        { //if nobody attacking...
            if(isCapturing)
            { //if capture routine is running
                attackingFactionField.value = 0; //set to empty
                StopResetCaptureTimer();
            }
        }

        numAttackingField.value = numAttacking; //sync

        worldModularGUI.ForceTriggerArea(); //force update, because num on point changed
    }

    //start countdown to capture
    void StartTickCapture()
    {
        Timing.KillCoroutines(captureRoutine);
        captureRoutine = Timing.RunCoroutine(TickCapture());
    }

    //stop or reset capture timer
    void StopResetCaptureTimer()
    {
        Timing.KillCoroutines(captureRoutine);
        captureStart = default;
        captureRoutine = default;
        captureProgressingField.value = false;
    }

    //enumerator that counts down timer to capture
    IEnumerator<float> TickCapture()
    {
        secsUntilCapture = captureTime;
        captureStart = DateTime.UtcNow;

        while(secsUntilCapture > 0)
        {
            yield return Time.deltaTime;
            if (!isContesting)
            { //if not contested by opposing faction
                secsUntilCapture -= Time.deltaTime; //count down timer
                captureProgressPercentField.value =  1 - (secsUntilCapture / captureTime);
            }
        }
        //wasn't inturrupted, so must be successful
        if(attackingFactionField.value > 0)
            FinishCapture();

        StopResetCaptureTimer();
    }

    void FinishCapture()
    {
        if(attackingFactionField.value <= 0)
        {
            Logger.LogError("faction <= 0 tried to capture a point");
        }

        int oldFaction = attackingFactionField.value; //temp, just for call order safety below.. in case something calls attackingFactionField from onCapture
        controllingFactionField.value = attackingFactionField.value; //set new owner
        attackingFactionField.value = 0; //set to empty
        numAttackingField.value = 0;

        onCapture?.Invoke(this, controllingFactionField.value, oldFaction);
    }

    public override void OnClean(SyncObject owner)
    {
        base.OnClean(owner);

        onCapture = null;
        attackingFactionField = null;
        controllingFactionField = null;
        numAttackingField = null;
        captureProgressingField = null;
        captureProgressPercentField = null;
        entitiesOnPoint.Clear();
        Timing.KillCoroutines(captureRoutine);
        captureRoutine = default;

    }

} //end class SyncCapZone