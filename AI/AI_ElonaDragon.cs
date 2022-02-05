using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_ElonaDragon : AI_CombatBehaviour
{

    public SpellBehaviour chargeUpAttack;

    public const float tryBeamAttackRange = 55f;

    const float closestFriendRallyDistance = 15f; //how far to search for nearby friend to rally from
    const float closestFriendRallyToRange = 8f; //how far away to get
    const float runAfterBasicShotDistance = 10f;
    const float maintainMinDistanceWhileRunning = 7f;
    const float maintainMaxDistanceWhileRunning = 55f;
    const float runAwayDistance = 12f; //how close they get before running away

    const float beamsCooldownDuration = 6f; //how long to wait after casting a few beams to start again
    float beamsCooldownTimer = 0f; //timer to check if we can cast beam yet
    const int beamsBeforeBreak = 3;
#pragma warning disable CS0414 // The field 'AI_ElonaDragon.currentBeamsCount' is assigned but its value is never used
    int currentBeamsCount = 0; //counter to check for break
#pragma warning restore CS0414 // The field 'AI_ElonaDragon.currentBeamsCount' is assigned but its value is never used

    protected override void Start_Init()
    {

    }

    float distFromTarget;
    private void FixedUpdate()
    {
        if (self == null || self.e == null || gameObject == null)
            return;

        if (self.currentTarget != null && !self.isNavigating && !isCasting)
        {
            distFromTarget = Vector3.Distance(self.currentTarget.transform.position, transform.position);
            if (distFromTarget <= runAwayDistance)
            {
                allowedToAttack = true;
                self.RunAwayFrom(self.currentTarget, runAfterBasicShotDistance, maintainMinDistanceWhileRunning, maintainMaxDistanceWhileRunning);
            }
            else
            if (distFromTarget > tryBeamAttackRange)
                self.NavigateToRange(self.currentTarget, tryBeamAttackRange - 25, runAwayDistance, tryBeamAttackRange + 15);
            else
                self.NavigateToClosestFriend(closestFriendRallyDistance, closestFriendRallyToRange);
        }

        beamsCooldownTimer -= Time.deltaTime;
    } //end FixedUpdate

    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        chargeUpAttack = new SpellBehaviour<S_DragonChargeUp>(ChargeUp, CanChargeUp, 7);

        behaviours = new SpellBehaviour[]
        {
            chargeUpAttack
        };
    }

    void ChargeUp(AI self, SpellBehaviour behaviour, Entity target)
    {
        //Logger.Log("AI casting beam attack ... " +
        behaviour.spell.TryStartCast(self.e, self.e, default, default);
        //);
    }
    bool CanChargeUp(AI self)
    {
        return self.currentTarget != null;
    }

} //end class AI_ElonaDragon
