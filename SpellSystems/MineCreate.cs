using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// dictates how and where to place mines with predetermined configs like walls or fields
/// </summary>
public class MineCreate
{

    public static void Single(spellHit spellHit, Spell castOnTriggered, string entityPrefabName, Transform startAnchor, Vector3 startPos)
    {
        //instantiate at position with location
        SyncObject added = Server.instance.AddEntity(entityPrefabName, null);
        added.transform.position = startPos;
        added.transform.SetParent(startAnchor);

        added.gameObject.AddComponent<Mine>().Set(spellHit, castOnTriggered, true); //create and set Mine component

    }

    /// <summary>
    /// create a wall facing perpendicular to the caster
    /// </summary>
    public static void WallPerpendicular_XZ(SpellStaticProperties prop, spellHit spellHit)
    {
        SyncObject added;

        float spacing = prop.length / spellHit.summonsCount;
        float offset = spellHit.summonsCount / 2 * spacing; //make it centered
        for (int i = spellHit.summonsCount; i > 0; i--)
        {
            added = Server.instance.AddEntity(prop.prefab.name, null);
            added.transform.position = spellHit.vTarget;
            added.transform.forward = added.transform.position - spellHit.caster.transform.position; //make it face away from caster
            added.transform.position += added.transform.right * (i * spacing + offset); //set offset position to create a row
        }

    }

} //end class Mines
