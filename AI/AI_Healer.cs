using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI_Healer : AI_CombatBehaviour
{

    public static readonly SpellBehaviour beamHealTotem = new SpellBehaviour<S_HealerBeamHealTotem>(UseBeamHealTotem, CanBeamHealTotem, 8);
    public static readonly SpellBehaviour basicHeal = new SpellBehaviour<S_HealerBasicHeal>(UseBasicHeal, CanBasicHeal, 4);
    public SpellBehaviour basicHoT;
    public SpellBehaviour speedBuff;

    public SyncSummons syncSummons;

    const float stayWithinLowestHealthDistance = 8f;

    const float closestFriendRallyDistance = 30f; //how far to search for nearby friend to rally from
    const float closestFriendRallyToRange = 3f; //how far away to get

    const float tryCastHotRange = 20f;
    const float tryCastSpeedBuffRange = 999;

    const float maintainMinDistanceWhileRunning = 7f;
    const float maintainMaxDistanceWhileRunning = 20f;
    const float runAwayDistance = 5f; //how close they get before running away

    protected override void Start_Init()
    {

    }

    Entity tryCastHotTarget = default;
    Entity tryCastSpeedBuffTarget = default;

    const float checkNearbyFriendliesRange = 100f;
    const float checkNearbyFriendliesInterval = 1f; //how often to check area for friends to rally to
    float checkNearbyFriendliesTimer = 0;
    Collider[] c_nearbyFriendlies;
    Entity[] nearbyFriendlies = default;
    Entity nearbyFriendlyTemp = default;
    int lowestFriendlyHealth = int.MaxValue;

    Entity lowestHealthNearbyFriend;

    protected override void PreStep()
    {
        tryCastHotTarget = default; //who to try and cast HoT on
        tryCastSpeedBuffTarget = default; //who to try and cast speed buff on

        //loop through nearby friendlies, if they don't have hot, cast hot on them
        //decide who to cast speed buff on.. (maybe if they're a certain class?) e.ai.combatBehavour.GetType()?...
        //start healing lowest member
        if (nearbyFriendlies != default)
            for (int i = 0; i < nearbyFriendlies.Length; i++)
            {
                if (nearbyFriendlies[i] != default)
                { //if doesn't have a HoT..
                    if (tryCastHotTarget == default)
                    { //if haven't found yet and they don't have it yet
                        if (!nearbyFriendlies[i].ContainsStatusEffectID(ref basicHoT.spell.id))
                        {
                            tryCastHotTarget = nearbyFriendlies[i]; //try to cast hot on them next Step()
                            if (self.currentTarget == default || self.currentTarget != tryCastHotTarget)
                                self.currentTarget = tryCastHotTarget; //target the one we want to cast the HoT on
                        }
                        else
                        if (tryCastSpeedBuffTarget == default && !nearbyFriendlies[i].ContainsStatusEffectID(ref speedBuff.spell.id))
                        { //if haven't found yet and they don't have it yet
                            tryCastSpeedBuffTarget = nearbyFriendlies[i]; //try to cast hot on them next Step()
                            if (self.currentTarget != tryCastHotTarget && self.currentTarget != tryCastSpeedBuffTarget) //if not already casting hot..
                                self.currentTarget = tryCastSpeedBuffTarget; //target the one we want to cast the HoT on

                        }
                    } //end if tryCastHotTarget == default

                } //end if nearby != null
            } //end loop over nearby Friendlies

        if (!self.isNavigating)
        {
            //try to find where to move to
            if (lowestHealthNearbyFriend != owner && lowestHealthNearbyFriend != default && Vector3.Distance(lowestHealthNearbyFriend.transform.position, transform.position) > stayWithinLowestHealthDistance)
            { //if have any friend nearby who's low on health
                self.NavigateToRange(lowestHealthNearbyFriend, 5f, 5f, checkNearbyFriendliesRange + 15);
            }
            else
            //navigate to HoT target if we have one
            if (tryCastHotTarget != default)
            { //if have someone we want to cast HoT on but they're out of range
                if (tryCastHotTarget != owner && Vector3.Distance(tryCastHotTarget.transform.position, transform.position) > tryCastHotRange)
                    self.NavigateToRange(tryCastHotTarget, tryCastHotRange - 5, tryCastHotRange - 15, tryCastHotRange + 25); //navigate to them
            }
            else
            if (tryCastSpeedBuffTarget != default)
            { //if have someone we want to cast HoT on but they're out of range
                if (tryCastSpeedBuffTarget != owner && Vector3.Distance(tryCastSpeedBuffTarget.transform.position, transform.position) > tryCastSpeedBuffRange)
                    self.NavigateToRange(tryCastSpeedBuffTarget, tryCastSpeedBuffRange - 5, tryCastSpeedBuffRange - 5, tryCastHotRange + 15); //navigate to them
            }
            else
            if (self.currentTarget != null)
            { //no nearby friends
              //just run
              if(self.currentTarget != owner && GameMode.instance.CheckNotFriendly(self.currentTarget, owner) && Vector3.Distance(self.currentTarget.transform.position, transform.position) < runAwayDistance)
                self.RunAwayFrom(self.currentTarget, runAwayDistance, maintainMinDistanceWhileRunning, maintainMaxDistanceWhileRunning);
            }

        } //end if not navigating

        //every interval loop through all nearby Entities and find lowest friendly
        checkNearbyFriendliesTimer -= Time.fixedDeltaTime;
        if (checkNearbyFriendliesTimer <= 0)
        {
            checkNearbyFriendliesTimer = checkNearbyFriendliesInterval;
            c_nearbyFriendlies = Physics.OverlapSphere(transform.position, checkNearbyFriendliesRange, WorldFunctions.entityMask);
            lowestHealthNearbyFriend = default; //reset
            lowestFriendlyHealth = int.MaxValue; //reset 
            if (c_nearbyFriendlies.Length > 0)
            {
                Array.Resize(ref nearbyFriendlies, c_nearbyFriendlies.Length); //resize to match colliders array
                for (int i = 0; i < c_nearbyFriendlies.Length; i++)
                { //loop through all found Entities, assign the ones that were friendly to array
                    nearbyFriendlyTemp = c_nearbyFriendlies[i].GetComponent<Entity>();
                    if (GameMode.instance.CheckFriendly(nearbyFriendlyTemp, self.e))
                    { //if is friendly
                        nearbyFriendlies[i] = nearbyFriendlyTemp; //save in other array
                        if (nearbyFriendlyTemp != owner && nearbyFriendlyTemp.health < lowestFriendlyHealth)
                        {
                            lowestFriendlyHealth = nearbyFriendlyTemp.health;
                            lowestHealthNearbyFriend = nearbyFriendlyTemp;
                        }
                    }
                    else
                        nearbyFriendlies[i] = null; //clear
                } //end for loop through all nearby
            } //end if found anything
            else
            { //if didn't find anything
                nearbyFriendlies = default; //reset
            }
        } //end timer area
    } //end PreStep


    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        basicHoT = new SpellBehaviour<S_HealerHoT>(UseBasicHoT, CanBasicHoT, 9);
        speedBuff = new SpellBehaviour<S_HealerSpeedBuff>(UseSpeedBuff, CanSpeedBuff, 5);

        behaviours = new SpellBehaviour[]
        {
            //beamHealTotem,
            basicHoT,
            speedBuff,
            basicHeal,
        };
        syncSummons = GetComponent<SyncSummons>(); //init
    }

    static void UseBeamHealTotem(AI self, SpellBehaviour behaviour, Entity target)
    {
        //self.LookAtTarget();
        behaviour.spell.TryStartCast(self.e, self.e, self.transform.position, default);
    }

    static bool CanBeamHealTotem(AI self)
    {
        //return self.self.currentTarget != null && Vector3.Distance(self.transform.position, self.self.currentTarget.transform.position);
        return true;
    }
    static void UseBasicHeal(AI self, SpellBehaviour behaviour, Entity target)
    {
        //self.LookAtTarget();
        behaviour.spell.TryStartCast(target.e, self.e, self.transform.position, default);
    }

    static bool CanBasicHeal(AI self)
    {
        //return self.self.currentTarget != null && Vector3.Distance(self.transform.position, self.self.currentTarget.transform.position);
        //return true;
        return self.currentTarget.health < self.currentTarget.maxHealth;

    }
    void UseBasicHoT(AI self, SpellBehaviour behaviour, Entity target)
    {
        behaviour.spell.TryStartCast(target.e, self.e, self.transform.position, default);
    } //end UseBasicHoT

    bool CanBasicHoT(AI self)
    {
        //return self.self.currentTarget != null && Vector3.Distance(self.transform.position, self.self.currentTarget.transform.position);
        return tryCastHotTarget != default && Vector3.Distance(tryCastHotTarget.transform.position, transform.position) < tryCastHotRange;
    }
    void UseSpeedBuff(AI self, SpellBehaviour behaviour, Entity target)
    {
        behaviour.spell.TryStartCast(target.e, self.e, self.transform.position, default);
    }

    bool CanSpeedBuff(AI self)
    {
        //return self.self.currentTarget != null && Vector3.Distance(self.transform.position, self.self.currentTarget.transform.position);
        return tryCastSpeedBuffTarget != default && Vector3.Distance(tryCastSpeedBuffTarget.transform.position, transform.position) < tryCastSpeedBuffRange;
    }


    public override void CleanRefs(SyncObject self)
    {
        base.CleanRefs(self);

        c_nearbyFriendlies = null;
        nearbyFriendlies = null;
        lowestHealthNearbyFriend = null;
        syncSummons = null;
        tryCastSpeedBuffTarget = null;
        tryCastHotTarget = null;
        basicHoT = null;
        speedBuff = null;

    }

} //end class AI_Healer
