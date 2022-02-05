using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPosition : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = new SyncField[] {
            new SyncVector3Field("position", false, Vector3.zero, parent),
            new SyncQuaternionField("rotation", false, Quaternion.identity, parent)
        };
    }
}
