using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public abstract class VendorShop_RandomItems : VendorShop
{

    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 6; //every 4 hrs


    const bool generateGemItems = false;
    const float chanceGemItems = 0.15f;

    public VendorShop_RandomItems() : base(typeof(GrowVendorShopLayout))
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }


    //set cost and currency type for each item generated using some algo
    public override void SetRandomItemsCost(WearableItem target, out int[] costs, out int[] costCurrencies)
    {
        if (target.baseItemLevel > 100)
        { //is high level, use crystals
            costs = new int[2] {
                Mathf.RoundToInt(Mathf.Pow(Mathf.Max(1, target.baseItemLevel - 30), 2.7f) * target.itemLevelMultiplier_FromSlot),
                Mathf.RoundToInt(Mathf.Pow(target.baseItemLevel, 1.7f) * target.itemLevelMultiplier_FromSlot)
            };
            costCurrencies = new int[2] {
                Item.From<C_SpaceScrap>().id,
                Item.From<C_SpaceGems>().id
            };
        } else
        { //don't use crystals
            if (generateGemItems && Ext.Roll100(chanceGemItems))
            {
                costs = new int[1] {
                    Mathf.RoundToInt(Mathf.Pow(target.baseItemLevel, 1.7f) * target.itemLevelMultiplier_FromSlot)
                };
                costCurrencies = new int[1] {
                    Item.From<C_SpaceGems>().id
                };
            }
            else
            {
                costs = new int[1] {
                    Mathf.RoundToInt(Mathf.Pow(Mathf.Max(1, target.baseItemLevel - 30), 2.7f) * target.itemLevelMultiplier_FromSlot)
                };
                costCurrencies = new int[1] {
                    Item.From<C_SpaceScrap>().id
                };
            }
        } //end else normal level item
    } //end func SetRandomItemCost


} //end VendorShop_RandomItems
