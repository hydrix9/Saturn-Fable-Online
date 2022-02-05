using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Goon_DoubleShot : AI_CombatBehaviour
{
    SpellBehaviour basicPewPew;

    SpellBehaviour gravityShot;

    const float closestFriendRallyDistance = 10f; //how far to search for nearby friend to rally from
    const float closestFriendRallyToRange = 8f; //how far away to get
    const float runAfterBasicShotDistance = 10f;
    const float maintainMinDistanceWhileRunning = 10f;
    const float maintainMaxDistanceWhileRunning = 55f;

    const float startChaseDistance = 30f;
    const float chaseToDistance = 20f;

    const float runAwayThresholdBelow = 10; //run away when current target is this close
    const float runAwayThrsholdAbove = 5; //must be at least this far away to keep running away
    public int circleShotsBeforeSwitch = 1;
#pragma warning disable CS0414 // The field 'AI_Goon_DoubleShot.circleShotCount' is assigned but its value is never used
    int circleShotCount = 0;
#pragma warning restore CS0414 // The field 'AI_Goon_DoubleShot.circleShotCount' is assigned but its value is never used

    //chance that this mob knows grav shot AND double shot
    public float knowsBothAbilitiesChance = 0.3f;
    public float gravShotPercent = 0.3f; //chance that this mob knows grav shot or double shot

    protected override void Start_Init()
    {

    }

    float _distToCurrentTarget;
    private void FixedUpdate()
    {
        if (self == null || self.e == null || gameObject == null)
            return;

        if (self.currentTarget != null && !self.isNavigating && !isCasting)
        {
            _distToCurrentTarget = distToCurrentTarget;
            //if target is close but not extremely close, focus on running away
            if (allowedToAttack && _distToCurrentTarget < runAwayThresholdBelow && _distToCurrentTarget > runAwayThrsholdAbove)
                allowedToAttack = false;
            else
                if (_distToCurrentTarget > startChaseDistance)
                    self.NavigateToRange(self.currentTarget, chaseToDistance, maintainMinDistanceWhileRunning, maintainMaxDistanceWhileRunning);

            if (!allowedToAttack)
            {
                allowedToAttack = true;
                circleShotCount = 0; //reset to allow circle shot
                self.RunAwayFrom(self.currentTarget, runAfterBasicShotDistance, maintainMinDistanceWhileRunning, maintainMaxDistanceWhileRunning);
            }
        }
    }


    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        basicPewPew = new SpellBehaviour<DoublePew>(BasicPewpewsAttack, CanPewPew, 5);
        gravityShot = new SpellBehaviour<GravityShot>(BasicPewpewsAttack, CanPewPew, 6);

        if (Ext.Roll100(knowsBothAbilitiesChance))
        { //successfully rolled to know both behaviours
            behaviours = new SpellBehaviour[]
            {
                basicPewPew,
                gravityShot
            };

        } else
        if(Ext.Roll100(gravShotPercent))
        { //successfully rolled to know grav shot
            behaviours = new SpellBehaviour[]
            {
                gravityShot
            };
        } else
        { //rolled to only know basicPewPew
            behaviours = new SpellBehaviour[]
            {
                basicPewPew
            };
        }



    } //end SetDefaults

    void BasicPewpewsAttack(AI self, SpellBehaviour behaviour, Entity target)
    {
        //circleShotCount++; //counter before have to switch
        //cycle priorities
        behaviour.priority = 5; //set this one to bottom

        self.LookAtTarget();
        //Logger.Log("AI casting circle pew pews attack ... " + castFailCodes.ToString());
        behaviour.spell.TryStartCast(target, self.e, default, default);
    }

    bool CanPewPew(AI self)
    {
        //return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position);
        //return circleShotCount < circleShotsBeforeSwitch;
        return true;
    }

} //end class AI_Goon_DoubleShot
