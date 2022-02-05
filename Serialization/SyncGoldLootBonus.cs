using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncGoldLootBonus : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    //codepoint constants
    public static readonly string
        goldBonusPercent = "goldBonus",
        lootBonusPercent = "lootBonusPercent";

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = new SyncField[]
        {
            new SyncFloat16Field_1024(goldBonusPercent, false, 1, parent),
            new SyncFloat16Field_1024(lootBonusPercent, false, 1, parent)
        };
    }
} //end class SyncEnergy