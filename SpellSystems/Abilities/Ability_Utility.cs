using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;

/// <summary>
/// common functions like Attack(), get enemies in a cone, teleport behind
/// </summary>
public partial class Ability
{
    public const int knockBackUpwardForce = 40;
    public const float meleeDistance = 8f;
    public static readonly DateTime zeroTicks = new DateTime(0);
    public static readonly Vector3 vector3Zero = Vector3.zero;

    /*
    //spawns an AoE damage or heal prefab at location
    public static void SpawnEffect(spellHit spellHit, Vector3 location, int id)
    {
        GameObject obj;
        if (WorldFunctions.instance.effectPrefabs.ContainsKey(id))
        {
            obj = GameObject.Instantiate(WorldFunctions.instance.effectPrefabs[id], location, Quaternion.identity);
            spellHit.caster.OnCreateObjectOffensive(spellHit, obj);

        }
        //TODO: Align effect to terrain?

    }
    */


    public static Entity GetFirstEnemyArea(Entity from, ref int range)
    {
        //this block of code is optimized

        Collider[] nearby = Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask);

        if (nearby.Length > 0)
        { //if found anything
            Entity target; //temp
            for (int i = 0; i < nearby.Length; i++)
            {
                target = nearby[i].GetComponent<Entity>();
                if (target != null && !GameMode.instance.CheckFriendly(from, target))
                    return target;

            }
        }

        return null; //didn't find anything
    }

    public static bool GetFirstEnemyArea_DoAction(Entity from, ref float range, Action<Entity> onFirst)
    {
        //this block of code is optimized

        Collider[] nearby = Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask);

        //Logger.Log(from.name + "'s GetFirstEnemyAreaAction found " + nearby.Length);

        if (nearby.Length > 0)
        { //if found anything
            Entity target; //temp
            for (int i = 0; i < nearby.Length; i++)
            {
                target = nearby[i].GetComponent<Entity>();
                if (target != null && !GameMode.instance.CheckFriendly(from, target))
                {
                    onFirst(target); //perform action on entity
                    return true; //return success
                }
            }
        }

        return false; //didn't find anything, didn't perform action
    }
    public static void ResetCD(spellHit spellHit)
    {

        //spellHit.caster.cooldowns[spellHit.spell.id] = DateTime.Now;
        if (spellHit.caster.p != null)
        { //if is player...
          //  Server.instance.SendPlayer(new byte[] { netcodes.resetCD, (byte)castable.id, (byte)(castable.id >> 8) }, caster.p.point);
        }
    }

    public static void KnockBack(spellHit spellHit)
    {

        Vector3 newForce = (spellHit.target.transform.position - spellHit.caster.transform.position).normalized * spellHit.power_nonDamage;
        newForce.y = knockBackUpwardForce;
        spellHit.target.GetComponentInChildren<Rigidbody>().AddForce(newForce, ForceMode.Impulse);

    }


    public static bool IsBehindTarget(Transform target, Transform fromEntity)
    {
        return Vector3.Dot(target.position - fromEntity.position, target.forward) > 0;
    }

} //end class Ability




