using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_MinerUpgrades : UpgradeShop
{
    public override string nameFormatted => "Upgrades";

    public Shop_MinerUpgrades() : base(typeof(GridUpgradeShopLayout))
    {

    }

    protected override shopItemInstance[] defaultItems => initItems;

    shopItemInstance[] initItems = new shopItemInstance[0]; //used for init, not runtime

    public override void InitShop()
    {
        initItems = new shopItemInstance[]
        {
            new shopItemInstance(Item.From<U_MiningSpeed>().name, 0, new int[] { Item.From<C_SpaceResearch>().id }, this) //upgrade mining speed costs research
        };
    }



} //end class Shop_Miner