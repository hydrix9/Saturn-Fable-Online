using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Shotgunner : AI_CombatBehaviour
{
    public static readonly SpellBehaviour shotgunAttack = new SpellBehaviour<SpaceShotgun>(ShotgunAttack, CanShotgunAttack, 5);

    public const float tryShotgunRange = 18f;

    float beamTimer;

    const float closestFriendRallyDistance = 25; //how far to search for nearby friend to rally from
    const float closestFriendRallyToRange = 3f; //how far away to get

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
            _distToTarget = distToCurrentTarget;
            if(!self.isNavigating && distToCurrentTarget > 10f)
                self.NavigateToRange(self.currentTarget, tryShotgunRange - 5, 5, tryShotgunRange + 35);
        } else
        { 
            //if no current target, try to rally close to a nearby friend
            if(isStepping && !self.isNavigating)
                self.NavigateToClosestFriend(closestFriendRallyDistance, closestFriendRallyToRange);
        }
    }


    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        behaviours = new SpellBehaviour[]
        {
            shotgunAttack
        };

    }

    static void ShotgunAttack(AI self, SpellBehaviour behaviour, Entity target)
    {
        self.LookAtTarget();
        behaviour.spell.TryStartCast(target, self.e, self.transform.position, default);
    }

    static bool CanShotgunAttack(AI self)
    {
        return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position) < tryShotgunRange;
    }


} //end class AI_Shotgunner
