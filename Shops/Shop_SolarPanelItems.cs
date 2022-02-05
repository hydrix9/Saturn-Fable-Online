using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop_SolarPanelItems_30_50 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T1";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_30_50() : base()
    {

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems


public class Shop_SolarPanelItems_60 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T2";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_60() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems_60



public class Shop_SolarPanelItems_70 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T3";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_70() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems_70



public class Shop_SolarPanelItems_80 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T4";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_80() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems_80



public class Shop_SolarPanelItems_90 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T5";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_90() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems_90



public class Shop_SolarPanelItems_100 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T6";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_100() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems_100



public class Shop_SolarPanelItems_110 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T7";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_110() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems_110



public class Shop_SolarPanelItems_120 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T8";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_120() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems_120



public class Shop_SolarPanelItems_130 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T9";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_130() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems_130



public class Shop_SolarPanelItems_140 : VendorShop_RandomItems
{
    public override string nameFormatted => "Solar Panel's Shop T10";

    protected override GenerateRandomItemsFromList[] randomGeneratedItems => new GenerateRandomItemsFromList[] {
        new GenerateRandomItemsFromList(
            new int[]{ 0, 889, 100, 10, 1, 0, 0 }, //quality chances
            new Type[]{ typeof(ItemType_Core), typeof(ItemType_Generator), typeof(ItemType_Fuel_Cell), typeof(ItemType_Mote) },
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


    public Shop_SolarPanelItems_140() : base()
    {
        //new shopItemInstance(ItemID(typeof(TestItem)), 1, 10, new int[] { ItemID(typeof(C_SpaceScrap)) }, new int[] { 1 }, default, default, 0, this),

    }

    //return true if this item can be put in this shop
    bool CheckValidItem(Item geneated)
    {
        return true;
    } //end func CheckValidItem

} //end Shop_SolarPanelItems_140


