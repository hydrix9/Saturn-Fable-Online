using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_BarracksVendor : VendorShop
{
    public override string nameFormatted => "Barracks Shop";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList()
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {
        /*
        new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, 1, this),
        new shopItemInstance(ItemID(typeof(TestItem)), 99, 100, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 10 }, default, default, 0, 1, this),
        new shopItemInstance(ItemID(typeof(WearableItem1)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 50 }, default, default, 1, 1, this),
        new shopItemInstance(ItemID(typeof(WearableItem2)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 100 }, default, default, 1, 1, this)
        */
    };


    public Shop_BarracksVendor() : base(typeof(GrowVendorShopLayout))
    {

    }


    //set cost and currency type for each item generated using some algo
    public override void SetRandomItemsCost(WearableItem target, out int[] costs, out int[] costCurrencies)
    {
        costs = new int[0];
        costCurrencies = new int[0];

    } //end func SetRandomItemCost


} //end Shop_BarracksVendor
