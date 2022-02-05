using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Sniper : AI_CombatBehaviour
{
    public SpellBehaviour beamAttack;
    public static readonly SpellBehaviour empMine = new SpellBehaviour<S_RogueEMP_Mine>(EMP_Mine, CanEMP_Mine, 7); //doesn't interface with instance so can be static 

    public const float tryBeamAttackRange = 55f;

    const float closestFriendRallyDistance = 30f; //how far to search for nearby friend to rally from
    const float closestFriendRallyToRange = 8f; //how far away to get
    const float runAfterBasicShotDistance = 10f;
    const float maintainMinDistanceWhileRunning = 7f;
    const float maintainMaxDistanceWhileRunning = 55f;
    const float runAwayDistance = 12f; //how close they get before running away

    const float beamsCooldownDuration = 6f; //how long to wait after casting a few beams to start again
    float beamsCooldownTimer = 0f; //timer to check if we can cast beam yet
    const int beamsBeforeBreak = 3;
    int currentBeamsCount = 0; //counter to check for break

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
                self.NavigateToRange(self.currentTarget, tryBeamAttackRange - 10, runAwayDistance, tryBeamAttackRange + 8);
            else
                self.NavigateToClosestFriend(closestFriendRallyDistance, closestFriendRallyToRange);
        }

        beamsCooldownTimer -= Time.deltaTime;
    } //end FixedUpdate

    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        beamAttack = new SpellBehaviour<SpaceBeam>(BeamAttack, CanBeamAttack, 5);
        behaviours = new SpellBehaviour[]
        {
            beamAttack,
            //empMine
        };
    }

    void BeamAttack(AI self, SpellBehaviour behaviour, Entity target)
    {
        
        //Logger.Log("AI casting beam attack ... " +
        behaviour.spell.TryStartCast(target, self.e, default, default); //will track toward target
        //);
        currentBeamsCount++;
        if (currentBeamsCount >= beamsBeforeBreak) //if reached number of beams before taking a break
            beamsCooldownTimer = beamsCooldownDuration; //start counting..
    }
    bool CanBeamAttack(AI self)
    {
        return beamsCooldownTimer <= 0 && self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position) < tryBeamAttackRange;
    }

    static void EMP_Mine(AI self, SpellBehaviour behaviour, Entity target)
    {
        //Logger.Log("AI casting beam attack ... " +
        behaviour.spell.TryStartCast(target, self.e, default, default);
        //);
    }
    static bool CanEMP_Mine(AI self)
    {
        return self.currentTarget != null && !self.isNavigating && Vector3.Distance(self.transform.position, self.currentTarget.transform.position) < tryBeamAttackRange;
    }

} //end class AI_Sniper
