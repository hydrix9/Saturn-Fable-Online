using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Rocketeer : AI_CombatBehaviour
{
    SpellBehaviour circlePewPewsAttack;
    SpellBehaviour circlePewPewsAttack_10;
    SpellBehaviour circlePewPewsAttack_20;

    [HideInInspector]
    public SpellBehaviour basicShot;

    const float closestFriendRallyDistance = 50; //how far to search for nearby friend to rally from
    const float closestFriendRallyToRange = 8f; //how far away to get
    const float runAfterBasicShotDistance = 20f;
    const float maintainMinDistanceWhileRunning = 5f;
    const float maintainMaxDistanceWhileRunning = 35f;

    public int circleShotsBeforeSwitch = 20;
    int circleShotCount = 0;

    protected override void Start_Init()
    {

    }

    private void FixedUpdate()
    {
        if (self == null || self.e == null || gameObject == null)
            return;

        if (self.currentTarget != null && !self.isNavigating && !isCasting)
        {
            if (!allowedToAttack)
            {
                allowedToAttack = true;
                circleShotCount = 0; //reset to allow circle shot
                self.RunAwayFrom(self.currentTarget, runAfterBasicShotDistance, maintainMinDistanceWhileRunning, maintainMaxDistanceWhileRunning);
            }
            else
                self.NavigateToClosestFriend(closestFriendRallyDistance, closestFriendRallyToRange);
        }
    }


    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        circlePewPewsAttack = new SpellBehaviour<SpaceCirclePewPews>(CirclePewpewsAttack, CanCirclePewPew, 5);
        circlePewPewsAttack_10 = new SpellBehaviour<SpaceCirclePewPews_offset10>(CirclePewpewsAttack, CanCirclePewPew, 6);
        circlePewPewsAttack_20 = new SpellBehaviour<SpaceCirclePewPews_offset20>(CirclePewpewsAttack, CanCirclePewPew, 7);
        basicShot = new SpellBehaviour<S_GoonBasicShot>(BasicShotAttack, CanBasicShot, 2);
        behaviours = new SpellBehaviour[]
        {
            circlePewPewsAttack,
            circlePewPewsAttack_10,
            circlePewPewsAttack_20,
            basicShot
        };
    }

    void CirclePewpewsAttack(AI self, SpellBehaviour behaviour, Entity target)
    {
        circleShotCount++; //counter before have to switch
        //cycle priorities
        behaviour.priority = 5; //set this one to bottom
        circlePewPewsAttack.priority++;
        circlePewPewsAttack_10.priority++;
        circlePewPewsAttack_20.priority++;

        //Logger.Log("AI casting circle pew pews attack ... " + castFailCodes.ToString());
        behaviour.spell.TryStartCast(self.e, self.e, default, default);
    }

    bool CanCirclePewPew(AI self)
    {
        //return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position);
        return circleShotCount < circleShotsBeforeSwitch;
    }


    void BasicShotAttack(AI self, SpellBehaviour behaviour, Entity target)
    {
        circleShotCount = 0; //reset
        //allowedToAttack = false; //not allowed to attack now until we start running away

        self.LookAtTarget();
        Logger.Log("AI casting basic attack ... " + castFailCodes.ToString(
        behaviour.spell.TryStartCast(target, self.e, default, default)
        ));
    }

    static bool CanBasicShot(AI self)
    {
        //return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position);
        return true;
    }

} //end class AI_Rocketeer
