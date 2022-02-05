using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Tank : AI_CombatBehaviour
{
    public static readonly SpellBehaviour shotgunAttack = new SpellBehaviour<S_MobCrippleShotgun>(ShotgunAttack, CanShotgunAttack, 5);
    public static readonly SpellBehaviour yoinkAttack = new SpellBehaviour<S_TankYoink>(YoinkAttack, CanYoinkAttack, 6);



    const float closestFriendRallyDistance = 50; //how far to search for nearby friend to rally from
    const float closestFriendRallyToRange = 3f; //how far away to get

    const float closeDistanceAmount = 1f;

    const float yoinkMinDistance = 7f;

    protected override void Start_Init()
    {

    }


    private void FixedUpdate()
    {
        if (self == null || self.e == null || gameObject == null)
            return;

        //TODO: try selecting different targets that are targeting ally so we can yoink them

        if (self.currentTarget != null)
        {
            if (!self.isNavigating && Vector3.Distance(transform.position, self.currentTarget.transform.position) > 1f)
                self.NavigateToRange(self.currentTarget, 10, 5, 35);
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
            shotgunAttack,
            yoinkAttack
        };
    }

    static void ShotgunAttack(AI self, SpellBehaviour behaviour, Entity target)
    {
        self.LookAtTarget();
        //Logger.Log("AI tank casting shotgun attack ... " +
        behaviour.spell.TryStartCast(default, self.e, default, default);
        //);
    }

    static bool CanShotgunAttack(AI self)
    {
        return self.currentTarget != null;
    }

    static void YoinkAttack(AI self, SpellBehaviour behaviour, Entity target)
    {

        //Logger.Log("AI tank casting yoink attack ... " +
        behaviour.spell.TryStartCast(target, self.e, default, default);
        //);
    }

    static bool CanYoinkAttack(AI self)
    {
        return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position) > yoinkMinDistance;
    }


} //end class AI_Tank
