using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncLevel : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts, and fairly recalcualte using CalcDefault

    //codepoint for name of variables stored on SyncObject
    public static readonly string level = "level";
    public static readonly string experience = "experience";
    /// <summary>
    /// starts at zero, added during ExpConfig.TryAddExp
    /// </summary>
    public static readonly string expBonus = "exp_bonus";

    //public int expValue;
    /// <summary>
    /// store exp like this because it is easier to check whether you have leveled up. Just count down to zero. Otherwise you would have to check the exp needed every time exp is gained
    /// </summary>
    public int expToNextLevel;

    private SyncInt8Field levelField; //cache
    private SyncInt32Field experienceField; //cache

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
        levelField = new SyncInt8Field(level, false, 0, parent); //init to level zero to allow it to update to 1 when we get experience updated
        experienceField = new SyncInt32Field(experience, false, 0, parent, OnUpdateExperience, accountCodes.experienceField);

        syncFields = new SyncField[] {
            levelField,
            experienceField,
            new SyncFloat16Field_1024(expBonus, false, 0, parent)
        };

        if(parent != null)
        {
            
        }

    } //end SetDefaults

    void OnUpdateExperience(ref int newValue)
    {
        int newLevel = ExpConfig.instance.ExpToLevel(newValue);
        if (levelField.value != newLevel) //check first to prevent sending unecessary updates
            levelField.value = newLevel;

        expToNextLevel = ExpConfig.instance.ExpTo_LevelUp(newValue);
    } //end func OnUpdateExperience

} //end class SyncLevel