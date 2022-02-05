using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_EngiGizmos : GridTalentTree
{
    public override string nameFormatted => "Gizmos";

    public Tree_EngiGizmos() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(4, 6);

        //internalGrid[0][0] = typeof(T_EngiDelayedBomb);

        //row 0
        internalGrid[0][0] = typeof(T_EngiOnTeleporterSpeed);
        internalGrid[1][0] = typeof(T_EngiProgeny);
        internalGrid[2][0] = typeof(T_EngiIncreaseHealthTurrets);

        //row 1
        internalGrid[0][1] = typeof(T_EngiBubbleRadius);
        internalGrid[1][1] = typeof(T_EngiOnTeleporterGainShield);
        internalGrid[2][1] = typeof(T_EngiIncreaseBubbleAbsorption);
        internalGrid[3][1] = typeof(T_EngiMinefield);

        //row 2
        internalGrid[0][2] = typeof(T_EngiIncreaseTurretFireRate);
        internalGrid[2][2] = typeof(T_EngiOnTeleporterNextInstantCast);
        internalGrid[3][2] = typeof(T_EngiOnMinefieldHitRoot);
        requiredTalentMap.Add(typeof(T_EngiOnMinefieldHitRoot), new Type[] { typeof(T_EngiMinefield) });


        //row 3
        internalGrid[3][3] = typeof(T_EngiMinefieldCooldown);
        requiredTalentMap.Add(typeof(T_EngiMinefieldCooldown), new Type[] { typeof(T_EngiOnMinefieldHitRoot) });

        //row 4
        internalGrid[0][4] = typeof(T_EngiIncreaseTeamTurretHealth);
        internalGrid[2][4] = typeof(T_EngiIncreaseTurretsRange);

        //row 5
        internalGrid[1][5] = typeof(T_EngiEnlightenment);


    } //end InitShop

} //end Tree_EngiGizmos


public class Tree_EngiHenchman : GridTalentTree
{
    public override string nameFormatted => "Henchman";

    public Tree_EngiHenchman() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(4, 6);

        internalGrid[0][0] = typeof(T_EngiRobotBuffFromDistance);
        internalGrid[0][0] = typeof(T_EngiUniqueRobotBuff);
        internalGrid[0][0] = typeof(T_EngiOnUseChargeHealAllRobots);
        internalGrid[0][0] = typeof(T_EngiOnDamagedRobotFrenzy);
        internalGrid[0][0] = typeof(T_EngiOnSacrificeRecreate);
        internalGrid[0][0] = typeof(T_EngiOnSummonBuffSummoned);
        internalGrid[0][0] = typeof(T_EngiSlowDrone);
        internalGrid[0][0] = typeof(T_EngiGunnerDrone);
        //internalGrid[0][0] = typeof(T_EngiStuff);

        //row 0

        //row 1

        //row 2

        //row 3

        //row 4

        //row 5


    } //end InitShop

} //end Tree_EngiHenchman


public class Tree_EngiReformation : GridTalentTree
{
    public override string nameFormatted => "Reformation";

    public Tree_EngiReformation() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(4, 6);

        //internalGrid[0][0] = typeof(T_EngiOnHealShield);
        //internalGrid[0][0] = typeof(T_EngiOnCleanseShield);

        //row 0
        internalGrid[0][0] = typeof(T_EngiEnergyIncrease);
        internalGrid[1][0] = typeof(T_EngiBuffBeam);
        internalGrid[2][0] = typeof(T_EngiIncreasedHealingOnTurrets);

        //row 1
        internalGrid[0][1] = typeof(T_EngiGiveReflectDamage);
        internalGrid[2][1] = typeof(T_EngiOnHitHealAlly);

        //row 2
        internalGrid[0][2] = typeof(T_EngiDrainLife);
        internalGrid[1][2] = typeof(T_EngiGiftHealthPercent);
        internalGrid[2][2] = typeof(T_EngiYoink);

        //row 3
        internalGrid[0][3] = typeof(T_EngiDelayedHeal);
        internalGrid[2][3] = typeof(T_EngiTurretEnergyPerSecondAura);

        //row 4
        internalGrid[0][4] = typeof(T_EngiHealZone);
        internalGrid[2][4] = typeof(T_EngiRestoreEnergyOverTime);

        //row 5
        internalGrid[0][5] = typeof(T_EngiLongHot);
        internalGrid[2][5] = typeof(T_EngiRestorePowerOverTime);
        requiredTalentMap.Add(typeof(T_EngiRestorePowerOverTime), new Type[] { typeof(T_EngiRestoreEnergyOverTime) });


    } //end InitShop

} //end Tree_EngiReformation

