using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SyncSkinAddons : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts

    //where to find index by name
    //could possibly just use a list of string, and that would mean you have to do Array.IndexOf when syncing...
    public Dictionary<string, int> _nameToIndex = new Dictionary<string, int>()
    {
        { "MyWingsSkin", 0 }
    };

    //codepoint constants
    public static readonly string
        skins = "skin_addons"
        ;

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        //init SyncFields
        syncFields = new SyncField[]
        {
            new SyncChildrenEnabled(skins, parent, FindTargets, false, _nameToIndex)
        };

        if(parent != null)
        {


        }

    } //end func SetDefaults

    //get the list of objects to sync off of parent
    GameObject[] FindTargets(SyncObject parent)
    {
        Transform root = parent.transform.Find("Body").Find("SkinAddons");
        return Enumerable.Range(0, root.childCount).Select(i => root.GetChild(i).gameObject).ToArray(); //get all child of skin root obj
    } //end func FindTargets


} //end class SyncSkinAddons