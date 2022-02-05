using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncEnergy : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    public int startMaxEnergy = 100; //for serializing in editor

    //codepoint constants
    public static readonly string
        energy = "energy",
        maxEnergy = "maxEnergy";

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = new SyncField[]
        {
            new SyncInt16Field(energy, false, startMaxEnergy, parent),
            new SyncInt16Field(maxEnergy, false, startMaxEnergy, parent)
        };
    } //end func SetDefaults

} //end class SyncEnergy