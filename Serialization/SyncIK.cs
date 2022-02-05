using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SyncIK : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts, and fairly recalcualte using CalcDefault

    /// <summary>
    /// points that will be synced. The IK will solve afterwards
    /// </summary>
    public Transform[] syncPoints = new Transform[0];


    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = syncPoints.Select(entry => new SyncTransformField("", true, false, entry, parent)).ToArray(); //create SyncFields from transforms
    }

}
