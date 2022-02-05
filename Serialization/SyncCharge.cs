using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncCharge : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    public int startMaxCharge = 100; //for serializing in editor

    //codepoint constants
    public const string
        charge = "charge",
        maxCharge = "maxCharge";

    SyncFloat16Field_8192 chargeField;
    SyncFloat16Field_8192 maxChargeField;

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        chargeField = new SyncFloat16Field_8192(charge, false, 0, parent);
        maxChargeField = new SyncFloat16Field_8192(maxCharge, false, startMaxCharge, parent);
        syncFields = new SyncField[]
        {
            chargeField,
            maxChargeField
        };
    }
} //end class SyncCharge