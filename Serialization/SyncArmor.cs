using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncArmor : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    public int startArmor = 0; //for serializing in editor

    //codepoint constants
    public static readonly string
        armor = "armor";

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = new SyncField[]
        {
            new SyncInt16Field(armor, false, startArmor, parent),
        };
    }

    public static void TryCalc(spellHit spellHit, Entity target)
    {
        if (spellHit.power <= 0 || spellHit.isHeal || !target.ContainsSyncField(armor))
            return;

        spellHit.power -= target.Get<int>(armor); //flat reduction... seems cooler
    }

} //end class SyncArmor