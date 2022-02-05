using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncNames : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts

    public string defaultDisplayName = "anon"; //for serializing in editor

    //codepoint constants
    public static readonly string
        displayName = "displayName",
        guildName = "guildName";

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = new SyncField[]
        {
            new SyncStringFixedField(displayName, false, defaultDisplayName, 32, parent),
            new SyncStringFixedField(guildName, false, "", 32, parent),
        };
    }

} //end class SyncNames