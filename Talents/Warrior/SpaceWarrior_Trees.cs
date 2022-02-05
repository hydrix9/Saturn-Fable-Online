using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tree_WarriorChampion : GridTalentTree
{
    public override string nameFormatted => "Champion";

    public Tree_WarriorChampion() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(4, 6);

        //row 0
        internalGrid[0][0] = typeof(T_WarriorReflectivePlating); //increase reflection damage

        internalGrid[1][0] = typeof(T_WarriorIncreaseTeamEnergy); //increase max energy of team

        internalGrid[2][0] = typeof(T_WarriorOnDamagedGenerateCharge); //chance on damaged to gain charge

        internalGrid[3][0] = typeof(T_WarriorYoinkGiveCharge); //yoink 


        //row 1
        internalGrid[0][1] = typeof(T_WarriorNeverBackDown); //unbind s key

        internalGrid[1][1] = typeof(T_WarriorShotgunLeechEffect); //shotgun also adds health

        internalGrid[2][1] = typeof(T_WarriorHeavyHanded); //chance to immobilize on hit

        internalGrid[3][1] = typeof(T_WarriorOnHitChanceGiveChargeAlly); //chance to immobilize on hit

        //row 2
        internalGrid[0][2] = typeof(T_WarriorIncreaseTeamSpeed);

        //replace with something more interesting soon
        internalGrid[2][2] = typeof(T_WarriorIncreasedDamagevsImmobilized); //increased damage vs immobilized
        requiredTalentMap.Add(typeof(T_WarriorIncreasedDamagevsImmobilized), new Type[] { typeof(T_WarriorHeavyHanded) });

        //place here- when you strike (or maybe yoink) a (immobilized) target you also add a marker to them...

        //row 3
        internalGrid[0][3] = typeof(T_WarriorIncreaseTeamExp);

        internalGrid[1][3] = typeof(T_WarriorUseChargeShieldOthers); //use your charge to put a shield on another

        internalGrid[2][3] = typeof(T_WarriorShotgunMortalStrikeEffect);

        internalGrid[3][3] = typeof(T_WarriorGiftChargeAlly); //use ability- gift charge to ally

        //row 4
        internalGrid[1][4] = typeof(T_ShieldOthersGiveCharge); //when you put a shield on others you also give them charge
        requiredTalentMap.Add(typeof(T_ShieldOthersGiveCharge), new Type[] { typeof(T_WarriorUseChargeShieldOthers) });

        //row 5

        internalGrid[1][5] = typeof(T_WarriorHoldTheLineChargePerSecondAllies); //hold the line also gives charge/sec to allies


    } //end InitShop

} //end Warrior Talent Tree Champion


public class Tree_WarriorCommando : GridTalentTree
{
    public override string nameFormatted => "Commando";

    public Tree_WarriorCommando() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(4, 6);

        //row 0
        internalGrid[3][1] = typeof(T_WarriorIncreaseShotgunDistance);

        //row 1
        internalGrid[1][1] = typeof(T_WarriorInstincts);

        internalGrid[2][1] = typeof(T_WarriorOnDodgeGainMoveSpeed);


        //row 2
        internalGrid[2][1] = typeof(T_WarriorCripplingGaze);


        //row 3

        //<- chance to AoE for % of current health when you take healing
    }

} //end Warrior Talent Tree Commando



public class Tree_WarriorBulwark : GridTalentTree
{
    public override string nameFormatted => "Bulwark";

    public Tree_WarriorBulwark() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(4, 6);

        //row 0
        internalGrid[0][0] = typeof(T_RichDiet); //increase size and health

        internalGrid[1][0] = typeof(T_WarriorBeef);

        internalGrid[2][0] = typeof(T_WarriorTopGun); //dodge chance

        internalGrid[3][0] = typeof(T_WarriorNutrition); //increase size

        //row 1
        internalGrid[0][1] = typeof(T_WarriorInstincts);

        internalGrid[1][1] = typeof(T_WarriorTaunt);

        internalGrid[2][1] = typeof(T_WarriorChallengingAura); //redirect damage in area toward you


        //row 2
        internalGrid[0][2] = typeof(T_WarriorHeavyPlating); //reduce damage taken and speed

        //T_WarriorDecreaseTauntCooldown
        internalGrid[1][2] = typeof(T_WarriorDecreaseTauntCooldown); //reduce damage taken and speed
        requiredTalentMap.Add(typeof(T_WarriorDecreaseTauntCooldown), new Type[] { typeof(T_WarriorTaunt) });

        internalGrid[3][2] = typeof(T_WarriorShed); //remove debuff from self

        //NOTE- add a team shield of some sort

        //row 3
        internalGrid[0][3] = typeof(T_WarriorIncreaseTeamHealth); //increase health of entire team

        internalGrid[1][3] = typeof(T_WarriorIncreaseTauntRadiusAtCostHealth); //increase radius at cost of health
        requiredTalentMap.Add(typeof(T_WarriorIncreaseTauntRadiusAtCostHealth), new Type[] { typeof(T_WarriorDecreaseTauntCooldown) });

        internalGrid[2][3] = typeof(T_WarriorDodgeChanceAgainstRanged);

        internalGrid[3][3] = typeof(T_WarriorCollateralMending); //heal self when remove debuff
        requiredTalentMap.Add(typeof(T_WarriorCollateralMending), new Type[] { typeof(T_WarriorShed) });

        //row 4
        internalGrid[2][4] = typeof(T_WarriorOnDamagedAddShield); //on damaged add shield

        //row 5
        internalGrid[2][5] = typeof(T_WarriorIncreaseOnDamagedShield); //increase shield amount
        requiredTalentMap.Add(typeof(T_WarriorIncreaseOnDamagedShield), new Type[] { typeof(T_WarriorOnDamagedAddShield) });




    } //end InitShop

} //end Warrior Talent Tree Bulwark





public class Bees : GridTalentTree
{
    public override string nameFormatted => "Bees";

    public Bees() : base(typeof(GridTalentTreeLayout))
    {
    }

    public override void InitShop()
    {
        base.InitShop();

        Resize(3, 5);

        /*
        internalGrid[0][0] = typeof(T_Bees);

        internalGrid[0][1] = typeof(T_KillerBees);
        requiredTalentMap.Add(typeof(T_KillerBees), new Type[] { typeof(T_Bees) });

        internalGrid[0][2] = typeof(T_KillererBees);
        requiredTalentMap.Add(typeof(T_KillererBees), new Type[] { typeof(T_KillerBees) });

        internalGrid[1][1] = typeof(T_Bombees);
        requiredTalentMap.Add(typeof(T_Bombees), new Type[] { typeof(T_Bees) });
        */
    } //end InitShop

} //end Warrior Talent Tree
