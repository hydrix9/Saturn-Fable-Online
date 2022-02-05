using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if using //scrapped in favor of using Entities with an AI instead of overloading a spellHit to be able to interact with an Entity object

/// <summary>
/// static class that dictates how and where to place a totem using preset functions
/// </summary>
public class TotemCreate
{
    public static void Single(spellHit spellHit, string entityPrefabName, Vector3 startPos, Transform startAnchor = null)
    {
        //instantiate at position with location
        SyncObject added = Server.instance.AddEntity(entityPrefabName);
        added.transform.position = startPos;
        added.transform.SetParent(startAnchor);
        SyncFaction.CopyTo(spellHit.caster, added.e); //copy faction over
        added.gameObject.AddComponent<Totem>().Set(spellHit, true); //create and set totem component

    }

} //end class TotemCreate
#endif