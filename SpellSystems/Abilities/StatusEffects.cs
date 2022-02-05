using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;
using System;
using System.Diagnostics;

/// <summary>
/// contains functions for buff / debuff DoT style spells
/// </summary>
public class StatusEffects : MonoBehaviour
{

    static void Serialize(spellHit spellHit)
    {
        spellHit.castStarted = DateTime.UtcNow; //reuse this field for when ticking begins, accessed when we send so client can calculate exact time left. Not going to be used for any other logic...
        AddRaw(spellHit, spellHit.target);
    }

    /// <summary>
    /// add it to the target and their syncCasting directly (discouraged), don't want to do that except from a sanitized/centralized location like here
    /// </summary>
    static void AddRaw(spellHit spellHit, Entity target)
    {
        if(!Server.isServer) //add this to avoid doing it twice on Server
            target.statusEffects.Add(spellHit);

        if (target == null)
            return;

        if (!Server.isServer)
            return;

        ///decided to do away with this optimization of only adding status effects if a player is nearby, because it was causing problems with statusEffects.Contains checks during .Manage() when the effect is added on the first frame (like for talent auras on turrets, since they're not players)
        //if (target.isPlayer || target.renderInstance.residesInHasPlayers)
        //{
        target.syncCasting.syncUnsentEffects.unsentEffects.Add(spellHit);
        target.statusEffects.Add(spellHit); //now Server can add
        /*
        }
        else
        {
            Ext.CallDelayed(50, () =>
            {
                //try again in 50ms (arbitrary value)
                if (target != null && target.gameObject != null && target.renderInstance != null && (target.isPlayer || target.renderInstance.residesInHasPlayers))
                    AddRaw(spellHit, target);
            });
        }
        */
    } //end func AddRaw

    #region statmods

    /// <summary>
    /// increase/decrease speed for duration set on spellhit from *= amount
    /// </summary>
    public static int MultSpeed(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<float, SyncObject, float> mod = (currentSpeed, parent) => { return currentSpeed * amount; };
        return TryAddStatMod<float>(spellHit, mod, SyncSpeed.maxSpeed, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// decrease speed for duration set on spellhit from /= amount
    /// </summary>
    public static int DivideSpeed(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<float, SyncObject, float> mod = (currentSpeed, parent) => { return currentSpeed / amount; };
        return TryAddStatMod<float>(spellHit, mod, SyncSpeed.maxSpeed, hasDuration, uniqueOnTarget, uniqueInWorld);
    }

    /// <summary>
    /// increase speed for duration set on spellhit from += amount
    /// </summary>
    public static int AddSpeed(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<float, SyncObject, float> mod = (currentSpeed, parent) => { return currentSpeed + amount; };
        return TryAddStatMod<float>(spellHit, mod, SyncSpeed.maxSpeed, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// decrease speed for duration set on spellhit from -= amount
    /// </summary>
    public static int ReduceSpeed(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<float, SyncObject, float> mod = (currentSpeed, parent) => { return currentSpeed - amount; };
        return TryAddStatMod<float>(spellHit, mod, SyncSpeed.maxSpeed, hasDuration, uniqueOnTarget, uniqueInWorld);
    }

    /// <summary>
    /// decrease speed for duration set on spellhit from /= amount
    /// </summary>
    public static int DivideMaxHealth(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<int, SyncObject, int> mod = (currentMaxHealth, parent) => { return (int)(currentMaxHealth / amount); };
        return TryAddStatMod<int>(spellHit, mod, SyncHealth.maxHealth, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// increase/decrease speed for duration set on spellhit from *= amount
    /// </summary>
    public static int MultMaxHealth(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<int, SyncObject, int> mod = (currentMaxHealth, parent) => { return (int)(currentMaxHealth * amount); };
        return TryAddStatMod<int>(spellHit, mod, SyncHealth.maxHealth, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// increase speed for duration set on spellhit from += amount
    /// </summary>
    public static int AddMaxHealth(spellHit spellHit, int amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<int, SyncObject, int> mod = (currentMaxHealth, parent) => { return currentMaxHealth + amount; };
        return TryAddStatMod<int>(spellHit, mod, SyncHealth.maxHealth, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// decrease speed for duration set on spellhit from -= amount
    /// </summary>
    public static int ReduceMaxHealth(spellHit spellHit, int amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<int, SyncObject, int> mod = (currentMaxHealth, parent) => { return currentMaxHealth - amount; };
        return TryAddStatMod<int>(spellHit, mod, SyncHealth.maxHealth, hasDuration, uniqueOnTarget, uniqueInWorld);
    }


    /// <summary>
    /// decrease energy for duration set on spellhit from /= amount
    /// </summary>
    public static int DivideMaxEnergy(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<int, SyncObject, int> mod = (currentMaxEnergy, parent) => { return (int)(currentMaxEnergy / amount); };
        return TryAddStatMod<int>(spellHit, mod, SyncEnergy.maxEnergy, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// increase/decrease energy for duration set on spellhit from *= amount
    /// </summary>
    public static int MultMaxEnergy(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<int, SyncObject, int> mod = (currentMaxEnergy, parent) => { return (int)(currentMaxEnergy * amount); };
        return TryAddStatMod<int>(spellHit, mod, SyncEnergy.maxEnergy, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// increase energy for duration set on spellhit from += amount
    /// </summary>
    public static int AddMaxEnergy(spellHit spellHit, int amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<int, SyncObject, int> mod = (currentMaxEnergy, parent) => { return currentMaxEnergy + amount; };
        return TryAddStatMod<int>(spellHit, mod, SyncEnergy.maxEnergy, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// decrease energy for duration set on spellhit from -= amount
    /// </summary>
    public static int ReduceMaxEnergy(spellHit spellHit, int amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<int, SyncObject, int> mod = (currentMaxEnergy, parent) => { return currentMaxEnergy - amount; };
        return TryAddStatMod<int>(spellHit, mod, SyncEnergy.maxEnergy, hasDuration, uniqueOnTarget, uniqueInWorld);
    }



    /// <summary>
    /// increase/decrease scale for duration set on spellhit from *= amount
    /// </summary>
    public static int MultScale(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<Vector3, SyncObject, Vector3> mod = (currentScale, parent) => { return currentScale * amount; };
        return TryAddStatMod<Vector3>(spellHit, mod, SyncScale.scale, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// decrease scale for duration set on spellhit from /= amount
    /// </summary>
    public static int DivideScale(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<Vector3, SyncObject, Vector3> mod = (currentScale, parent) => { return currentScale / amount; };
        return TryAddStatMod<Vector3>(spellHit, mod, SyncScale.scale, hasDuration, uniqueOnTarget, uniqueInWorld);
    }

    /// <summary>
    /// increase scale for duration set on spellhit from += amount
    /// </summary>
    public static int AddScale(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<Vector3, SyncObject, Vector3> mod = (currentScale, parent) => { currentScale.x += amount; currentScale.y += amount; currentScale.z += amount; return currentScale; };
        return TryAddStatMod<Vector3>(spellHit, mod, SyncScale.scale, hasDuration, uniqueOnTarget, uniqueInWorld);
    }
    /// <summary>
    /// decrease scale for duration set on spellhit from -= amount
    /// </summary>
    public static int ReduceScale(spellHit spellHit, float amount, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<Vector3, SyncObject, Vector3> mod = (currentScale, parent) => { currentScale.x -= amount; currentScale.y -= amount; currentScale.z -= amount; return currentScale; };
        return TryAddStatMod<Vector3>(spellHit, mod, SyncScale.scale, hasDuration, uniqueOnTarget, uniqueInWorld);
    }


    #endregion



    /// <summary>
    /// immobilize for duration set on spellhit
    /// </summary>
    public static int Immobilize(spellHit spellHit, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<bool, SyncObject, bool> mod = (currentImmobilized, parent) => { return true; };
        return TryAddStatMod<bool>(spellHit, mod, SyncSpeed.immobilized, true, uniqueOnTarget, uniqueInWorld);
    }

    /// <summary>
    /// stun for duration set on spellhit
    /// </summary>
    public static int Stun(spellHit spellHit, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Func<bool, SyncObject, bool> mod = (currentImmobilized, parent) => { return true; };
        return TryAddStatMod<bool>(spellHit, mod, SyncSpeed.stunned, true, uniqueOnTarget, uniqueInWorld);
    }


    /// <summary>
    /// calculates hit chance and tries to upsert previous StatusEffect from caster if uniqueOnTarget and removes after duration if hasDuration, and removes previous effect in world if uniqueInWorld
    /// </summary>
    public static int TryAdd(spellHit spellHit, bool hasDuration = true, bool uniqueOnTarget = true, bool uniqueInWorld = false)
    {
        Logger.Log("trying to add effect from " + spellHit.spell.name);

        if(hasDuration && spellHit.duration <= 0)
        {
            Logger.LogError("said hasDuration on " + spellHit.spell.name + " but didn't find any duration");
        }

        if (Server.isServer)
        {
            //if (spellHit.target == null)
            //    return castFailCodes.noTarget;

            if (spellHit.target.health <= 0)
                return castFailCodes.dead;

            //spellHit.SetCalculatedTarget(spellHit.target); //do OnCalcDefensive calculations in a locked, safe way, to prevent double calculations due to jumping targets on spellHit or something
            Damage.DefensiveCalc(spellHit); //perform bonuses from defender's mods, like Armor

            //calcualte if resisted
            int blocked = Damage.CalcHitChance(spellHit);
            if (blocked != castFailCodes.success)
                return blocked;
        }

        TryUpsert(spellHit, uniqueOnTarget, uniqueInWorld);

        if (Server.isServer)
        {
            spellHit.buffDebuff = GameMode.instance.CheckFriendly(spellHit.caster, spellHit.target); //set "is a buff or debuff" to whether target is friendly with caster

            //do we want already applied status effects to have infinite hit chance? If so, we can set hit chance to 1,000,000 or something here
            Serialize(spellHit); //actually add to entity
        } else
        { 
            //client, skip serialization step and go straight to AddRaw
            AddRaw(spellHit, spellHit.target);
        }

        if (spellHit.buffDebuff) //if it is a buff
            spellHit.target.TriggerOnAddBuff(spellHit); //trigger GUI and stuff
        else
        { //is a debuff
            spellHit.target.TriggerOnAddDebuff(spellHit); //trigger GUI and stuff
            SyncCombatState.SetFrom(spellHit); //put target and caster "in combat"
        }

        if (hasDuration || spellHit.interval > 0) //if has a duration effect or has an interval effect
            Manage(spellHit, hasDuration);

        return castFailCodes.success;
    }

    //try remove old if uniqueOnTarget or uniqueInWorld and exists respective
    public static void TryUpsert(spellHit spellHit, bool uniqueOnTarget, bool uniqueInWorld)
    {
        //try upsert spell
        if ((uniqueOnTarget || spellHit.maxStacks > 1) && spellHit.target.statusEffects.Count > 0)
        {
            //WARNING- this assumes that if it is uniqueOnTarget the FirstOrDefault will always return ALL stacks (1)... possible that multiple get added due to thread/concurrency then not removed properly?... maybe need to RemoveAll() instead 
            spellHit replace = spellHit.target.statusEffects.FirstOrDefault(entry => entry.spell.id == spellHit.spell.id && (uniqueOnTarget || (entry.caster != null && entry.caster.id == spellHit.caster.id))); //try find spell with same ID and caster to be replaced

            if (replace != null) //if we found the same spell cast by us, remove it, replace, and make sure server will sync
            {
                spellHit.target.RemoveStatusEffect_Raw(replace); //the CoroutineHandle managing this one should break automatically
                if (spellHit.maxStacks > 1 && replace.caster.id == spellHit.caster.id) //if we can support a higher stack and the one we replaced belongs to us...
                    spellHit.stacks = Math.Min(spellHit.stacks + 1, spellHit.maxStacks); //add stack up to max stacks because target already had one 
            }
        }

        if (uniqueInWorld)
        {
            spellHit.caster.TryUpsertOwnUniqueEffect(spellHit);
        }

    }

    /// <summary>
    /// modify a stat for duration with SyncField<T>.OnCalcDefault, which will recalculate the value using this delegate each time it is modified for better security and safety
    /// </summary>
    public static int TryAddStatMod<T>(spellHit spellHit, Func<T, SyncObject, T> mod, string fieldName, bool hasDuration, bool uniqueOnTarget, bool uniqueInWorld)
    {
        Logger.Log("trying to add effect from " + spellHit.spell.name);

        if (Server.isServer)
        {
            //if (spellHit.target == null)
            //    return castFailCodes.noTarget;

            if (spellHit.target == null || spellHit.target.health <= 0)
                return castFailCodes.dead;

            //spellHit.SetCalculatedTarget(spellHit.target); //do OnCalcDefensive calculations in a locked, safe way, to prevent double calculations due to jumping targets on spellHit or something

            Damage.DefensiveCalc(spellHit); //perform bonuses from defender's mods, like Armor

            //calcualte if resisted
            int blocked = Damage.CalcHitChance(spellHit);
            if (blocked != castFailCodes.success)
                return blocked;

        }

        if(!spellHit.target.ContainsSyncField(fieldName))
        {
            Logger.LogWarning(spellHit.target.name + " doesn't contain field named " + fieldName);
            return castFailCodes.unexpectedError;
        }

        SyncField<T> field = spellHit.target.GetSyncField<T>(fieldName);

        spellHit.castStarted = DateTime.UtcNow; //reuse this field for when ticking begins, accessed when we send so client can calculate exact time left. Not going to be used for any other logic...


        //Serialize(spellHit);

        //do we want already applied status effects to have infinite hit chance? If so, we can set hit chance to 1,000,000 or something here?

        TryAdd(spellHit, hasDuration, uniqueOnTarget, uniqueInWorld); //add the actual status effect

        field.AddMod(spellHit, mod, hasDuration ? DateTime.UtcNow.AddSeconds(spellHit.duration - tryExpireTimeOffset) : DateTime.MaxValue, ref uniqueOnTarget); //add the mod which will change the stat, and remove it self if the associated statusEffect gets removed



        if (hasDuration)
            Manage<T>(spellHit, field); //also manage the removing of the stat after it expires

        if (uniqueInWorld)
            throw new NotImplementedException(); //need to add the actual status effect.. then register to add and remove this statmod when it is added/removed or something... or change onCalcDefault to take a status effect that may/may not still be on target (remove if it isn't)

        return castFailCodes.success;
    }



    /*
    /// <summary>
    /// add a spellMod and only remove it when checkValid passes. ---- while(target!= null && checkValid()) { yield return retryInterval; }
    /// </summary>
    /// <param name="checkValid">function that decides whether to break IEnumerator wait loop</param>
    /// <param name="retryInterval">how often to wait in IEnumerator to call checkValid again</param>
    /// <param name="ensureUnique">whether this spellID should only exist one time on target</param>
    public static void AddManaged(Entity target, SpellMod spellMod, Func<bool> checkValid, float retryInterval,, bool uniqueOnTarget = true, bool uniqueInWorld = false)
    {
        if (!uniqueOnTarget || !target.spellMods.Contains(spellMod))
        { //if we don't care or they dont have it
            target.spellMods.Add(spellMod);
            Timing.RunCoroutine(Manage(target, spellMod, checkValid, retryInterval));
        }
    }
    */

    public static IEnumerator<float> Manage(Entity target, SpellMod spellMod, Func<bool> checkValid, float retryInterval)
    {
        while (target != null && checkValid())
        {
            yield return retryInterval;
        }

        Logger.Log("removing " + spellMod.ToString() + " from " + target.name);

        if (target != null)
        {
            target.spellMods.Remove(spellMod);
        }
    }


    static void Manage<T>(spellHit spellHit, SyncField<T> field)
    {
        Ext.CallDelayed((int)(spellHit.duration * 1000), field.CalcDefault);  //will remove effects that expired or don't exist as statusEffects on target
    }

    static Stopwatch sw; //UnityEngine.Debug
    public static void Manage(spellHit spellHit, bool hasDuration)
    {
#if UNITY_EDITOR
        //Logger.Log("managing status effect from " + spellHit.spell.name);
        Logger.Log("StatusEffect expires after " + spellHit.duration + "... or " + ((float)(spellHit.castStarted.AddSeconds(spellHit.duration) - DateTime.UtcNow).TotalSeconds));
        sw = new Stopwatch();
        sw.Start();
#endif
        //if it has no interval, fire and forget until duration, else, use effect every interval

        if (spellHit.interval <= 0)
        {

            if(hasDuration)
                Ext.CallDelayed((int)(spellHit.duration * 1000), () => { TryExpire(spellHit, hasDuration); }); //calculate when to end it compared to DateTime.UtcNow to allow us to keep delaying it when we extend duration

        }
        else
        { //has interval effect

            CoroutineHandle coroutine = default(CoroutineHandle);

            if (hasDuration)
            {
                coroutine = Timing.CallPeriodically(spellHit.duration, spellHit.interval, () =>
                {
                    //normally we would call OnTryHitOffensive, but that would try add another StatusEffect
                    if (spellHit.target != null && spellHit.target.statusEffects.Contains(spellHit))
                        spellHit.spell.IntervalEffect(spellHit);
                    else
                        Timing.KillCoroutines(coroutine);

                }, () => { TryExpire(spellHit, hasDuration); });
            } else
            { //doesn't have duration, therefore never expires
                Timing.RunCoroutine(ForeverIntevalManageRoutine(spellHit));
            }
        } //end if has interval effect
    } //end func Manage

    //used when there is an interval effect but no duration
    static IEnumerator<float> ForeverIntevalManageRoutine(spellHit spellHit)
    {
        //normally we would call OnTryHitOffensive, but that would try add another StatusEffect
        while (spellHit.target != null && spellHit.target.statusEffects.Contains(spellHit))
        {
            spellHit.spell.IntervalEffect(spellHit);
            yield return Timing.WaitForSeconds(spellHit.interval);
        }

        TryExpire(spellHit, false);
    } //end func ForeverIntervalManageRoutine

    //buffer given to allow expiring things slightly early due to lag spikes/ DateTime.Now precision
    //won't actually call the delegate early, just will allow it to pass "expired" check just in case of slight precision error
    //DateTime is usually accurate to no more than 100ms and no less than 5ms
    const float tryExpireTimeOffset = 0.12f;

    /// <summary>
    /// sanely tries removing status effect by first checking if castStarted + (duration, which might have changed) means it should be removed
    /// </summary>
    /// <param name="spellHit"></param>
    public static void TryExpire(spellHit spellHit, bool hasDuration)
    {
        //try remove it if it's valid, but if it appears duration was extended, continue trying to do effect
        if (spellHit.target == null || !spellHit.target.statusEffects.Contains(spellHit))
            return;
#if UNITY_EDITOR
        sw.Stop();
        Logger.Log(sw.Elapsed.ToString());
#endif
        if (spellHit.castStarted.AddSeconds(spellHit.duration - tryExpireTimeOffset) <= DateTime.UtcNow)
        { //if duration is over
            spellHit.target.StatusEffect_Expire(spellHit);

        }
        else
        {
            Manage(spellHit, hasDuration); //try again in a while
        }

    }

    /// <summary>
    /// remove debuff from target, ensuring it canBeDispelled/ not permanent first
    /// </summary>
    public static void Cleanse(spellHit spellHit, int count)
    {
        for (int i = spellHit.target.statusEffects.Count - 1, n = 0; i > 0 && n < count; i--)
        { //loop as long as they have status effects and we haven't exceeded count
            if (!spellHit.target.statusEffects[i].buffDebuff && spellHit.target.statusEffects[i].spell.canBeDispelled)
            { //if it is a debuff and isn't some permanent StatusEffect like a permanent aura from talents
                spellHit.target.StatusEffect_Dispel(spellHit, spellHit.target.statusEffects[i]);
                n++;
            }
        }
    }


    /// <summary>
    /// remove buff from target, ensuring it canBeDispelled/ not permanent first
    /// </summary>
    public static void Purge(spellHit spellHit, int count)
    {
        SyncCombatState.SetFrom(spellHit); //put target and caster "in combat"

        for (int i = spellHit.target.statusEffects.Count - 1, n = 0; i >= 0 && n < count; i--)
        { //loop as long as they have status effects and we haven't exceeded count
            if (spellHit.target.statusEffects[i].buffDebuff && spellHit.target.statusEffects[i].spell.canBeDispelled)
            { //if it is a debuff and isn't some permanent StatusEffect like a permanent aura from talents
                spellHit.target.StatusEffect_Dispel(spellHit, spellHit.target.statusEffects[i]);
                n++;
            }
        }
    }

    public static void TryRemove_Client(Entity target, int spell, int caster)
    {
        int index = target.statusEffects.FindIndex(entry => entry.casterID == caster && entry.spell.id == spell);
        if (index >= 0)
        {
            //target.statusEffects.RemoveAt(index);
            target.RemoveStatusEffect_Raw(target.statusEffects[index]);
        }
    } //end func TryRemove_Client






    public static bool Has_FromAny(Entity target, string name)
    {
        if (target.statusEffects.Count == 0)
            return false;

        for (int i = 0; i < target.statusEffects.Count; i++)
        {
            if (target.statusEffects[i].spell.name == name)
                return true; //found match on name
        }

        return false;
    }

    public static bool Has_FromCaster(Entity target, Entity caster, string name)
    {
        if (target.statusEffects.Count == 0)
            return false; //null short circuit

        for (int i = 0; i < target.statusEffects.Count; i++)
        {
            if (target.statusEffects[i].spell.name == name && target.statusEffects[i].casterID == caster.id)
                return true; //found match on name and caster
        }

        return false; //didn't find any
    }

    public static bool Has_FromFriendly(Entity target, Entity caster, string name)
    {
        if (target.statusEffects.Count == 0)
            return false;

        for (int i = 0; i < target.statusEffects.Count; i++)
        {
            if (target.statusEffects[i].spell.name == name && GameMode.instance.CheckFriendly(target.statusEffects[i].caster, caster))
                return true; //found match on name and friendly between caster and us...
        }

        return false;
    }



    const float checkAuraInterval = 1.5f;
    /// <summary>
    /// makes all valid entities within spherecast have effect, re-checking if they remain in AoE area every 0.3f with spellHit.radius
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="ensureUnique_selfCast">whether we can intially add this aura if it exists on ourself</param>
    /// <param name="ensureUnique_auraEffect">whether we can add this aura when checking on others during the periodic aura check</param>
    /// <param name="hasDuration"></param>
    public static void Aura_SpellMod(spellHit spellHit, SpellMod effect, bool ensureUnique_selfCast, bool ensureUnique_auraEffect, bool hasDuration)
    {
        TryAdd(spellHit, hasDuration, ensureUnique_selfCast, false); //add actual status effect to caster
        Timing.RunCoroutine(IAura_SpellMod(spellHit, effect, ensureUnique_auraEffect, hasDuration)); //continuously apply status effects and check entities in the area

    } //end function Zone_SpellMod


    private static IEnumerator<float> IAura_SpellMod(spellHit spellHit, SpellMod effect, bool ensureUnique, bool hasDuration)
    {

        Collider[] hitsNew;
        Collider[] hitsOld = new Collider[0];

        Entity e = null;
        float timer = spellHit.duration; //countdown
        while (spellHit.caster != null && spellHit.caster.statusEffects.Contains(spellHit))
        {
            hitsNew = Physics.OverlapSphere(spellHit.caster.transform.position, spellHit.radius, WorldFunctions.entityMask);

            //try add new effects for valid targets
            for (int i = 0; i < hitsNew.Length; i++)
            { //loop through all hit colliders
                e = hitsNew[i].GetComponent<Entity>();
                if (!hitsOld.Contains(hitsNew[i]) && spellHit.spell.CalcValidTarget(e, spellHit.caster) == castFailCodes.success && (!ensureUnique || !e.spellMods.Contains(effect)))
                { //if they're not seen before and a valid target and (doesn't contain effect already or doesn't matter)
                    e.spellMods.Add(effect); //add effect
                }
            } //end for loop

            //try remove effects for targets that are no longer in area
            for (int i = 0; i < hitsOld.Length; i++)
            {
                if (hitsOld[i] != null)
                {
                    e = hitsNew[i].GetComponent<Entity>();
                    if (e != null && !hitsNew.Contains(hitsOld[i]))
                    { //if not found in area
                        e.spellMods.Remove(effect); //remove effect
                    }
                }
            } //end for loop
            hitsOld = hitsNew; //new are now old
            yield return checkAuraInterval;
        }

        for(int i = 0; i < hitsOld.Length; i++)
        { //this should be all effected
            if (hitsOld[i] != null)
            {
                e = hitsOld[i].GetComponent<Entity>();
                if (e != null)
                    e.spellMods.Remove(effect);
            }
        }
    } //end function IAura_SpellMod



    /// <summary>
    /// makes all valid entities within spherecast have effect, re-checking if they remain in AoE area every 0.3f with spellHit.radius
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="ensureUnique_selfCast">whether we can intially add this aura if it exists on ourself</param>
    /// <param name="ensureUnique_auraEffect">whether we can add this aura when checking on others during the periodic aura check</param>
    /// <param name="hasDuration"></param>
    public static void Aura(spellHit spellHit, bool ensureUnique_selfCast, bool ensureUnique_auraEffect, bool hasDuration, Func<Entity, Entity, int> calcValidTargetOverride = null)
    {
        TryAdd(spellHit, hasDuration, ensureUnique_selfCast, false); //add actual status effect to caster
        Timing.RunCoroutine(IAura(spellHit, ensureUnique_auraEffect, hasDuration, calcValidTargetOverride != null ? calcValidTargetOverride : spellHit.spell.CalcValidTarget)); //continuously apply status effects and check entities in the area

    } //end function Zone_SpellMod


    static IEnumerator<float> IAura(spellHit spellHit, bool ensureUnique, bool hasDuration, Func<Entity, Entity, int> calcValidTarget)
    {

        Collider[] hitsNew;
        Collider[] hitsOld = new Collider[0];

        Entity e = null;
        float timer = spellHit.duration; //countdown
        while (spellHit.caster != null && spellHit.caster.statusEffects.Contains(spellHit))
        {
            hitsNew = Physics.OverlapSphere(spellHit.caster.transform.position, spellHit.radius, WorldFunctions.entityMask);

            //try add new effects for valid targets
            for (int i = 0; i < hitsNew.Length; i++)
            { //loop through all hit colliders
                e = hitsNew[i].GetComponent<Entity>();
                if (!hitsOld.Contains(hitsNew[i]) && calcValidTarget(e, spellHit.caster) == castFailCodes.success && (!ensureUnique || !e.statusEffects.Contains(spellHit)))
                { //if they're not seen before and a valid target and (doesn't contain effect already or doesn't matter)
                    AddRaw(spellHit, e);
                }
            } //end loop

            //try remove effects for targets that are no longer in area
            for (int i = 0; i < hitsOld.Length; i++)
            {
                if (hitsOld[i] != null)
                {
                    e = hitsOld[i].GetComponent<Entity>();
                    if (e != null && !hitsNew.Contains(hitsOld[i]) && e != spellHit.caster)
                    { //if not found in area... importantly, shouldn't perform this check on caster (found out it was doing this on warp!)
                        e.RemoveStatusEffect_Raw(spellHit);

                    }
                }
            }
            hitsOld = hitsNew; //new are now old
            yield return checkAuraInterval;
        }

        for (int i = 0; i < hitsOld.Length; i++)
        { //this should be all effected
            if (hitsOld[i] != null)
            {
                e = hitsOld[i].GetComponent<Entity>();
                if (e != null)
                    e.RemoveStatusEffect_Raw(spellHit);
            }
        }
    } //end function IAura_SpellMod


    /// <summary>
    /// makes all valid entities within spherecast have effect, re-checking if they remain in AoE area every 0.3f with spellHit.radius
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="ensureUnique_selfCast">whether we can intially add this aura if it exists on ourself</param>
    /// <param name="ensureUnique_auraEffect">whether we can add this aura when checking on others during the periodic aura check</param>
    /// <param name="hasDuration"></param>
    public static void Aura_StatMod<T>(spellHit spellHit, Func<T, SyncObject, T> mod, string fieldName, bool ensureUnique_selfCast, bool ensureUnique_auraEffect, bool hasDuration)
    {
        TryAdd(spellHit, hasDuration, ensureUnique_selfCast, false); //add actual status effect to caster
        Timing.RunCoroutine(IAura_StatMod<T>(spellHit, mod, fieldName, ensureUnique_auraEffect)); //continuously apply status effects and check entities in the area

    } //end function Zone_SpellMod


    static IEnumerator<float> IAura_StatMod<T>(spellHit spellHit, Func<T, SyncObject, T> mod, string fieldName, bool ensureUnique)
    {

        Collider[] hitsNew;
        Collider[] hitsOld = new Collider[0];

        Entity e = null;
        float timer = spellHit.duration; //countdown
        while (spellHit.caster != null && spellHit.caster.statusEffects.Contains(spellHit))
        {
            hitsNew = Physics.OverlapSphere(spellHit.caster.transform.position, spellHit.radius, WorldFunctions.entityMask);

            //try add new effects for valid targets
            for (int i = 0; i < hitsNew.Length; i++)
            { //loop through all hit colliders
                if (hitsOld[i] != null)
                {
                    e = hitsNew[i].GetComponent<Entity>();
                    if (!hitsOld.Contains(hitsNew[i]) && spellHit.spell.CalcValidTarget(e, spellHit.caster) == castFailCodes.success)
                    { //if they're not seen before and a valid target and (doesn't contain effect already or doesn't matter)
                        if (!e.ContainsSyncField(fieldName))
                        {
                            Logger.LogWarning(e.name + " doesn't contain field named " + fieldName);
                            continue;
                        }

                        e.GetSyncField<T>(fieldName).AddMod(spellHit, mod, DateTime.MaxValue, ref ensureUnique);

                    }
                }
            } //end loop

            //try remove effects for targets that are no longer in area
            for (int i = 0; i < hitsOld.Length; i++)
            {
                if (hitsOld[i] != null)
                {
                    e = hitsNew[i].GetComponent<Entity>();
                    if (e != null && !hitsNew.Contains(hitsOld[i]) && e != spellHit.caster)
                    { //if not found in area.. importantly, shouldn't perform this check on caster (found out it was doing this on warp!)
                        if (!e.ContainsSyncField(fieldName))
                        {
                            Logger.LogWarning(e.name + " doesn't contain field named " + fieldName);
                            continue;
                        }
                        e.RemoveStatusEffect_Raw(spellHit);
                        e.GetSyncField<T>(fieldName).CalcDefault(); //will remove effects that expired or don't exist as statusEffects on target
                    }
                }
            } //end for loop
            hitsOld = hitsNew; //new are now old
            yield return checkAuraInterval;
        }

        //finally, remove from everyone

        for (int i = 0; i < hitsOld.Length; i++)
        { //this should be all effected
            if (hitsOld[i] != null)
            {
                e = hitsOld[i].GetComponent<Entity>();
                if (e != null)
                {
                    if (!e.ContainsSyncField(fieldName))
                    {
                        Logger.LogWarning(e.name + " doesn't contain field named " + fieldName);
                        continue;
                    }
                    e.RemoveStatusEffect_Raw(spellHit);
                    e.GetSyncField<T>(fieldName).CalcDefault(); //will remove effects that expired or don't exist as statusEffects on target
                }
            }
        } //end for loop
    } //end function IAura_StatMod




} //end class StatusEffect
