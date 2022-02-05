using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SyncFaction))]
public class SyncTeamStats : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    //codepoint constants
    public static readonly string
        score = "score",
        kills = "kills"
    ;

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = new SyncField[]
        {
            new SyncInt16Field(score, false, 0, parent, OnUpdateScore)
        };

        if(parent != null)
        {

        }
    } //end SetDefaults

    void OnUpdateScore(ref int newValue)
    {
        //don't really need to do anything since GameMode is subscribed to onUpdate
    }

} //end class SyncTeamStats