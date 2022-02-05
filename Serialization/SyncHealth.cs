using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncHealth : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts, and fairly recalcualte using CalcDefault

    public int startMaxHealth = 100; //for serializing in editor

    /// <summary>
    /// whether to add more health to starting amount based on LevelStatsConfig and SyncLevel.level
    /// </summary>
    [SerializeField]
    bool addMaxHealthFromLevel = false;

    //codepoint constants
    public static readonly string 
        health = "health",
        maxHealth = "maxHealth";

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {

        syncFields = new SyncField[]
        {
            new SyncInt16Field(health, false, startMaxHealth, parent),
            new SyncInt16Field(maxHealth, false, startMaxHealth, parent)
        };

        if (parent != null)
        {
            if(addMaxHealthFromLevel)
            { //if configured to calculate max health based on level
                /* will throw an error if this loads before SyncLevel.level
                if (!parent.ContainsSyncField(SyncLevel.level))
                    throw new System.Exception("unable to find level but config'd to calc health from level on " + name);
                */
                parent.GetSyncField<int>(SyncHealth.maxHealth).AddMod_Permanent(LevelStatsConfig.CalcHealthFromLevel); //mod max health from level when it is calculated
            }

        } //end if parent exists

    } //end SetDefaults

    void OnUpdateHealth(ref int newValue)
    {

    }

} //end class SyncLevel