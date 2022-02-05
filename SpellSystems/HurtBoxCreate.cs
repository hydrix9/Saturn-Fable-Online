using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// entry point for creating and populating HurtBoxes during a Spell cast from an Entity
/// </summary>
public class HurtBoxCreate : MonoBehaviour
{

    public void Single(GameObject hurtBoxPrefab, spellHit spellHit, Quaternion rotation, string hurtBoxPointName)
    {
        //find anchor for HurtBox
        Transform startPoint = spellHit.caster.GetComponent<SyncHurtBoxes>().GetHurtBoxPoint(hurtBoxPointName);

        if (startPoint == null)
            Logger.LogWarning($"HurtBox start point { hurtBoxPointName } not found");

        //create new instance on specified start point with specified rotation
        GameObject created = ObjectPool.GetInstance(hurtBoxPrefab, startPoint.position, rotation, startPoint);

        //assign references and expiration date
        HurtBox hurtBox = created.GetComponent<HurtBox>();

        //set refs and create serialization id for possibly referencing to clients later
        SetParams(hurtBox, spellHit, startPoint, 0);

    } //end func CreateFromDuration

    //common function between all hurtbox creations
    void SetParams(HurtBox hurtBox, spellHit spellHit, Transform startPoint, int index)
    {
        spellHit.caster.syncCasting.syncUnsentCasts.SetObjectSerializationID(index, spellHit,
            hurtBox.Set(spellHit, DateTime.UtcNow.AddSeconds(spellHit.duration), startPoint)
        );
    }

} //end class HurtBoxCreate