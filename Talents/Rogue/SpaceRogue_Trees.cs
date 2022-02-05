using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tree_RogueGuile : GridTalentTree
{
    public override string nameFormatted => "Guile";

    public Tree_RogueGuile() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(4, 6);

        //row 0
        internalGrid[0][0] = typeof(T_RogueIncreaseSpeed);

        internalGrid[1][0] = typeof(T_RogueMiniaturization);

        internalGrid[2][0] = typeof(T_RogueIncreaseEXP);

        //row 1
        internalGrid[0][1] = typeof(T_RogueOnDodgeMoveSpeed);
        requiredTalentMap.Add(typeof(T_RogueOnDodgeMoveSpeed), new Type[] { typeof(T_RogueIncreaseSpeed) });

        internalGrid[1][1] = typeof(T_RogueShadowMastery);

        internalGrid[2][1] = typeof(T_RogueIncreaseEnergy);

        //row 2
        internalGrid[0][2] = typeof(T_RogueOnHitIncreasedRange);

        internalGrid[1][2] = typeof(T_RogueSmokeBomb);

        internalGrid[2][2] = typeof(T_RogueOnKillTeleport);

        internalGrid[3][2] = typeof(T_RogueConsumeChargeIncreaseDodge);

        //row 3
        internalGrid[0][3] = typeof(T_RogueIncreaseOnHitRangeAmount);
        requiredTalentMap.Add(typeof(T_RogueIncreaseOnHitRangeAmount), new Type[] { typeof(T_RogueOnHitIncreasedRange) });

        internalGrid[2][3] = typeof(T_RogueIncreaseTeamRogueSpeed);

        internalGrid[3][3] = typeof(T_RogueRangedAttackWithCharge);



        //row 4
        internalGrid[1][4] = typeof(T_RogueIllusionaryProjection);

        internalGrid[2][4] = typeof(T_RogueGainChargeStandingStill);



        //row 5
        internalGrid[1][5] = typeof(T_RogueReduceIllusionaryProjectionCD);
        requiredTalentMap.Add(typeof(T_RogueReduceIllusionaryProjectionCD), new Type[] { typeof(T_RogueIllusionaryProjection) });


    } //end InitShop

} //end Tree_RogueGuile




public class Tree_RogueInsurrection : GridTalentTree
{
    public override string nameFormatted => "Insurrection";

    public Tree_RogueInsurrection() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(4, 6);

        //row 0
        internalGrid[0][0] = typeof(T_RogueIncreaseCastTimeAndDamageBackstab);

        internalGrid[2][0] = typeof(T_RogueOnHitExtraEnergyConsumption);

        internalGrid[3][0] = typeof(T_RogueShadowstepGainCharge);

        //row 1
        internalGrid[0][1] = typeof(T_RogueAssimilation);

        internalGrid[1][1] = typeof(T_RogueGunsBlazing);

        internalGrid[2][1] = typeof(T_RogueIncreaseConsumeExtraEnergyAmount);
        requiredTalentMap.Add(typeof(T_RogueIncreaseConsumeExtraEnergyAmount), new Type[] { typeof(T_RogueOnHitExtraEnergyConsumption) });

        internalGrid[3][1] = typeof(T_RogueRecouperation);
        
        //row 2
        internalGrid[0][2] = typeof(T_RoguePurge);

        internalGrid[1][2] = typeof(T_RogueIncreaseShadowstepRange);

        internalGrid[2][2] = typeof(T_RogueIncreaseDamagePerCharge);

        internalGrid[3][2] = typeof(T_RogueIncreaseRecouperationStacks);
        requiredTalentMap.Add(typeof(T_RogueIncreaseRecouperationStacks), new Type[] { typeof(T_RogueRecouperation) });


        //row 3
        internalGrid[1][3] = typeof(T_RogueSuicideBomb);

        internalGrid[2][3] = typeof(T_RogueConsumeChargeMortalStrike);

        internalGrid[3][3] = typeof(T_RogueIncreaseOnKillRecoupDuration);
        requiredTalentMap.Add(typeof(T_RogueIncreaseOnKillRecoupDuration), new Type[] { typeof(T_RogueIncreaseRecouperationStacks) });

        //row 4
        internalGrid[1][4] = typeof(T_RogueIncreaseOnHitSuicideBombDamage);
        requiredTalentMap.Add(typeof(T_RogueIncreaseOnHitSuicideBombDamage), new Type[] { typeof(T_RogueSuicideBomb) });

        internalGrid[2][4] = typeof(T_RogueSeedOfCorruption);

        //row 5
        internalGrid[1][5] = typeof(T_RogueDecreaseSuicideBombCountdown);
        requiredTalentMap.Add(typeof(T_RogueDecreaseSuicideBombCountdown), new Type[] { typeof(T_RogueIncreaseOnHitSuicideBombDamage) });

    } //end InitShop

} //end Tree_RogueInsurrection




public class Tree_RogueDrifter : GridTalentTree
{
    public override string nameFormatted => "Drifter";

    public Tree_RogueDrifter() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(4, 6);

        //row 0
        internalGrid[0][0] = typeof(T_RogueChanceOnHitRegenCharge);

        //row 1

        //row 2

        //row 3

        //row 4

        //row 5


    } //end InitShop

} //end Tree_RogueDrifter