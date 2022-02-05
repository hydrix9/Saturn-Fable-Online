using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncCombatState : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    private SyncDateTimeField_3232 lastInCombat;

    //codepoint constants
    public static readonly string
        lastCombatDate = "lastCombat";


    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        lastInCombat = new SyncDateTimeField_3232(lastCombatDate, false, DateTime.MinValue, parent, OnUpdateLastInCombat);

        syncFields = new SyncField[]
        {
            lastInCombat
        };
    }

    /// <summary>
    /// put a caster and target in combat over an interaction
    /// </summary>
    public static void SetFrom(spellHit spellHit)
    {
        if (spellHit.caster.ContainsSyncField(lastCombatDate))
            spellHit.caster.Set(lastCombatDate, DateTime.UtcNow);

        if (spellHit.target.ContainsSyncField(lastCombatDate))
            spellHit.target.Set(lastCombatDate, DateTime.UtcNow);
    }

    void OnUpdateLastInCombat(ref DateTime newValue)
    {

    }
} //end class SyncCombatState