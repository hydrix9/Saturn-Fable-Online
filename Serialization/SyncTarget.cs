using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTarget : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    //codepoint for name of variables stored on SyncObject
    public static readonly string target = "target";

    public override void ServerInit(SyncObject parent)
    {
        base.ServerInit(parent);

        string faction = parent.Get<string>(SyncFaction.faction);
        if (faction != "")
        {
            GameMode.instance.TryAddFaction(faction, false); //make sure we have a reference...
        }       
    }

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = new SyncField[] {
            new SyncInt16Field(target, false, 1, parent)
        };
    }
}
