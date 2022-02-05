using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncInventory : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts, and fairly recalcualte using CalcDefault

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        throw new System.NotImplementedException();
    }
}
