using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AI_Turret : AI_CombatBehaviour
{

    const float tryTargetCloserInterval = 3f; //will try to get a closer target if our current target exists

    float tryTargetCloserTimer;
    Entity closestEnemy; //temp

    protected override void PreStep()
    {
        if (self != null && self.currentTarget != null)
        {
            //if targeting a friendly unit, clear target
            if (GameMode.instance.CheckFriendly(self.e, self.currentTarget))
                self.currentTarget = null;

            //try targeting a closer target
            tryTargetCloserTimer -= stepInterval;
            if (tryTargetCloserTimer <= 0)
            {
                tryTargetCloserTimer = tryTargetCloserInterval; //reset timer
                AOEs.GetClosestEnemy(self.e, ref self.aggroDistance, out closestEnemy);
                if (closestEnemy != null)
                    self.currentTarget = closestEnemy;
            }


        } //end if currentTarget != null
    } //end func PreStep

    protected override void Start_Init()
    {
        self.UpdateAggroDistance(FindLongestProjectileRange() + 3); //calculate how far our longest projectile can reach and set aggro distance to that

    } //end func Start_Init

    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        behaviours = new SpellBehaviour[0];

        if(self != null && self.e != null)
        {
            //just create behaviours that will try and cast at the current target
            behaviours = self.e.syncCasting.spellsKnown.Select(entry =>
                new SpellBehaviour(Spell.GetSpell(entry).GetType(), BasicAttack)
            ).ToArray();
        }

    } //end func SetDefaults

    //calculate how long all projectiles can reach
    float FindLongestProjectileRange()
    {
        spellHit fullStats;
        float longestDistance = 0;
        float distance = 0;
        for(int i = 0; i < spellBehaviours.Length; i++)
        {
            fullStats = spellBehaviours[i].spell.GetFullStats(self.e, self.e, default, default);
            if(fullStats.isProjectile)
            { //if appears to be a projectile
                distance = fullStats.moveSpeed * fullStats.duration;
                if (distance > longestDistance) //if found new record
                    longestDistance = distance; //asign record
            }
        } //end loop over spellBehaviours

        return longestDistance;
    } //end func FindLongestProjectileRange

    public static void BasicAttack(AI self, SpellBehaviour behaviour, Entity target)
    {
        self.LookAtTarget();
        //Logger.Log("AI casting basic attack ... " + castFailCodes.ToString(
        behaviour.spell.TryStartCast(target, self.e, default, default);
        //));
    }

    public static void SelfBuff(AI self, SpellBehaviour behaviour, Entity target)
    {
        //Logger.Log("AI casting basic attack ... " + castFailCodes.ToString(
        behaviour.spell.TryStartCast(self.e, self.e, default, default);
        //));
    }

    public static bool CanBasicAttack(AI self)
    {
        //return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position);
        return true;
    }

} //end class AI_Shotgunner
