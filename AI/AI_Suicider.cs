using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Suicider : AI_CombatBehaviour
{
    public static readonly SpellBehaviour suicideAttack = new SpellBehaviour<S_MobSuicideBomb>(SuicideAttack, CanSuicideAttack, 5);

    public const float trySuicideAttack = 25;
    public SyncSummons syncSummons;

    const float closestFriendRallyDistance = 50; //how far to search for nearby friend to rally from
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
            if (!self.isNavigating && distToCurrentTarget > 3f)
                self.NavigateTo(self.currentTarget.transform.position, 0, 10);
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
            suicideAttack
        };
        syncSummons = GetComponent<SyncSummons>(); //init
    }

    static void SuicideAttack(AI self, SpellBehaviour behaviour, Entity target)
    {
        self.LookAtTarget();
        behaviour.spell.TryStartCast(self.e, self.e, self.transform.position, default);
    }

    static bool CanSuicideAttack(AI self)
    {
        return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position) < trySuicideAttack;
    }


} //end class AI_Suicider
