using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// shop for things like Repair Building 
/// </summary>
public class Shop_PlayerActions : Shop
{
    public override string nameFormatted => "Actions";

    public override Func<Entity, Entity, bool> permission_spendable => Permission_Self;
    public override Func<Entity, Entity, bool> permission_viewable => Permission_Self;
    public override Func<Entity, Entity, bool> permission_movable => Permission_Never;
    public override Func<Entity, Entity, bool> permission_placable => Permission_Never;
    public override Func<Entity, Entity, bool> permission_delete => Permission_Never;

    public Shop_PlayerActions() : base(typeof(GridUpgradeShopLayout))
    {

    }

    protected override shopItemInstance[] defaultItems => initItems;

    shopItemInstance[] initItems = new shopItemInstance[0]; //used for init, not runtime

    public override void InitShop()
    {
        initItems = new shopItemInstance[]
        {
            new shopItemInstance(Item.From<Item_RepairBuilding>().id, 0, new int[] { Item.From<C_SpaceMinerals>().id }, this) //cast Repair Building at the cost of minerals
        };
    }

    public override bool CheckUnlocked(Entity owner, Entity spender, Item item, int count)
    {
        return true; //everything unlocked
    }

    public override void ChargeBuyer(Entity owner, Entity spender, shopItemInstance item, int count)
    {
        ChargeBuyer_FromInventory(owner, spender, item, count); //just take items from inventory
    }

    public override bool HasEnoughMoney(Entity owner, Entity spender, shopItemInstance item, int count)
    {
        return MoneyCheck_InventoryCurrencyTypes(owner, spender, item, count);
    }

    public override bool AllowedItem(shopItemInstance item, ShopInstance shop)
    {
        return false; //don't allow adding to this
    }
} //end class Shop_Miner