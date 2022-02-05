using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_BeamWall : AI_CombatBehaviour
{

    public static readonly SpellBehaviour beamWallAttack = new SpellBehaviour<S_BeamWall_4>(BeamWallAttack, CanBeamWallAttack, 5);

    public const float tryBeamWallRange = 40f;

    float beamTimer;

    const float closestFriendRallyDistance = 15; //how far to search for nearby friend to rally from
    const float closestFriendRallyToRange = 3f; //how far away to get

    const float runAwayRange = 10f;

    protected override void Start_Init()
    {

    }

    float _distToTarget;
    private void FixedUpdate()
    {
        if (self == null || self.e == null || gameObject == null)
            return;

        if (self.currentTarget != null)
        {
            self.LookAtTarget();

            _distToTarget = distToCurrentTarget;
            if (!self.isNavigating && distToCurrentTarget < runAwayRange)
                self.RunAwayFrom(self.currentTarget, runAwayRange + 5, runAwayRange + 10);
        }
        else
        {
            //if no current target, try to rally close to a nearby friend
            if (isStepping && !self.isNavigating)
                self.NavigateToClosestFriend(closestFriendRallyDistance, closestFriendRallyToRange);
        }
    }


    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        behaviours = new SpellBehaviour[]
        {
            beamWallAttack
        };
    }

    static void BeamWallAttack(AI self, SpellBehaviour behaviour, Entity target)
    {
        behaviour.spell.TryStartCast(self.e, self.e, self.transform.position, default);
    }

    static bool CanBeamWallAttack(AI self)
    {
        return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position) < tryBeamWallRange;
    }


} //end class AI_BeamWall