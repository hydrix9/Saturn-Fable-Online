using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop_MainStationItems_30_50 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T1";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            20, //min base ilevel
            50, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    };


    public Shop_MainStationItems_30_50() : base()
    {

    }


    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem


} //end Shop_MainStationItems


public class Shop_MainStationItems_60 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T2";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            60, //min base ilevel
            60, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {

    };


    public Shop_MainStationItems_60() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_MainStationItems_60



public class Shop_MainStationItems_70 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T3";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            70, //min base ilevel
            70, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {

    };


    public Shop_MainStationItems_70() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_MainStationItems_70



public class Shop_MainStationItems_80 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T4";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            80, //min base ilevel
            80, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {

    };


    public Shop_MainStationItems_80() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_MainStationItems_80



public class Shop_MainStationItems_90 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T5";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            90, //min base ilevel
            90, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {

    };


    public Shop_MainStationItems_90() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_MainStationItems_90



public class Shop_MainStationItems_100 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T6";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            100, //min base ilevel
            100, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {

    };


    public Shop_MainStationItems_100() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_MainStationItems_100



public class Shop_MainStationItems_110 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T7";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            110, //min base ilevel
            110, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {

    };


    public Shop_MainStationItems_110() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_MainStationItems_110



public class Shop_MainStationItems_120 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T8";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            120, //min base ilevel
            120, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {

    };


    public Shop_MainStationItems_120() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_MainStationItems_120



public class Shop_MainStationItems_130 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T9";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            130, //min base ilevel
            130, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {

    };


    public Shop_MainStationItems_130() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_MainStationItems_130



public class Shop_MainStationItems_140 : VendorShop_RandomItems
{
    public override string nameFormatted => "Station's Shop T10";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Chassis), typeof(ItemType_Stern), typeof(ItemType_BowArmor), typeof(ItemType_StarboardPort) },
            1, 1, //min, max count
            20, //amount to generate
            140, //min base ilevel
            140, //max base ilevel
            CheckValidItem
            )
    };
    public override float refreshRandomGeneratedEverySeconds => 60 * 60 * 12; //twice once a day...



    protected override shopItemInstance[] defaultItems =>
    new shopItemInstance[]
    {

    };


    public Shop_MainStationItems_140() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_MainStationItems_140

