using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncParty : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    public const int maxPartyMembers = 5; //for serializing in editor

    private SyncInt16MultiField partyMembersField;

    //codepoint constants
    public static readonly string
        partyMembers = "partyMembers"
    ;

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        partyMembersField = new SyncInt16MultiField(partyMembers, false, new int[maxPartyMembers], parent);

        syncFields = new SyncField[]
        {
            partyMembersField
        };

        if (parent != null)
        {
            partyMembersField.Set(parent.id, 0); //set self to be in a party of 1
        }
    }
} //end class SyncParty