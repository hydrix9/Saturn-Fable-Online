using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//items contained in the "PlayerActions" Shop, like Repair Building



public class Item_RepairBuilding : Spell_ShopItem
{
    public override string nameFormatted => "Repair Building";
    public override string description => spell.description; //same description as spell
    public override string iconName => nameFormatted;
    public override int forcedRarity => itemRarity.common;

    public override GameObject guiPrefab => null;

    public Item_RepairBuilding() : base(Spell.GetName<S_RepairBuilding>())
    {

    }

} //end class Item_RepairBuilding

