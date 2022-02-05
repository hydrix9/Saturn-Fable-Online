using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerMovementType : SerializedComponent_ByID
{
    public override bool resetToDefaultValues_OnGameRestart => false; //dont call base.ResetFieldsToDefaults when game restarts, and fairly recalcualte using CalcDefault

    public SyncPlayerMovementType() : base(typeof(PlayerMovement), true)
    {

    }

    protected override Component Set(Type type)
    {
        Logger.LogWarning("attaching to body " + parent.e.body);

        return parent.e.body.gameObject.AddComponent(type); //add to body object
    }

    public override void SetConfig(string configType)
    {
        switch(configType)
        {
            case "TopDown":
                break;

            default:
                break;
        }
    }



} //end class SyncPlayerMovementType
