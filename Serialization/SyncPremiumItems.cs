using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SyncPremiumItems : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    public Transform skinAddonArea; //where to get child skin addons, name each child .name to its "true name" to ensure proper synchronization

    //codepoint constants
    public static readonly string
        skinAddons = "skinAddons"
        //maxEnergy = "maxEnergy"
    ;

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        Dictionary<string, int> skinAddon_nameToChildIndex = new Dictionary<string, int>(skinAddonArea.childCount); //pass this to SyncChildrenEnabled so it knows self index by name
        for (int i = 0, l = skinAddonArea.childCount; i < l; i++)
            skinAddon_nameToChildIndex.Add(skinAddonArea.GetChild(i).name, i);

        syncFields = new SyncField[]
        {
            new SyncChildrenEnabled(skinAddons, parent, GetSkinAddons, false, skinAddon_nameToChildIndex)
        };
    }


    //get dots from target to supply to SyncField, used as parameter func during SyncChildrenEnabled constructor
    GameObject[] GetSkinAddons(SyncObject parent)
    {
        //get all dots in all dotAreas
        return Enumerable.Range(0, skinAddonArea.childCount).Select(entry => skinAddonArea.GetChild(entry).gameObject).ToArray();
    } //end func GetSkinAddons

} //end class SyncPremiumItems
