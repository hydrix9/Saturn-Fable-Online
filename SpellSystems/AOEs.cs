using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MEC;

/// <summary>
/// functions for calling an action on all entities within an area like a sphere or cone
/// </summary>
public class AOEs
{

    /// <summary>
    /// perform an AoE action on range, only apply a maximum of spellHit.numStrikes
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="startPos"></param>
    /// <param name="action">action to be called after creating a duplicate spellHit instance to be calculated on the target</param>
    public static void OnFriendlies_WithCount(spellHit spellHit, Vector3 startPos, Action<spellHit> action)
    {
        if (spellHit.radius <= 0)
            Logger.LogError("trying to do AoE area action but didn't find any radius on " + spellHit.spell.name);


        Collider[] nearby = Physics.OverlapSphere(startPos, spellHit.radius, WorldFunctions.entityMask);
        int numHitsLeft = spellHit.numStrikes;

        //Logger.Log(from.name + "'s GetFirstEnemyAreaAction found " + nearby.Length);

        if (nearby.Length > 0)
        { //if found anything
            Entity target; //temp
            for (int i = 0; i < nearby.Length && numHitsLeft > 0; i++)
            { //loop through all Entities we found
                target = nearby[i].GetComponent<Entity>();
                if (target != null && GameMode.instance.CheckFriendly(spellHit.caster, target))
                {
                    numHitsLeft--;
                    action(new spellHit(spellHit, spellHit.caster, target, spellHit.origin, spellHit.vTarget)); //perform action on entity with new spellHit instance
                }
            }
        }
    }

    /// <summary>
    /// perform an AoE action on range, only apply a maximum of spellHit.numStrikes
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="startPos"></param>
    /// <param name="action">action to be called after creating a duplicate spellHit instance to be calculated on the target</param>
    public static void OnEnemies_WithCount(spellHit spellHit, Vector3 startPos, Action<spellHit> action)
    {
        if (spellHit.radius <= 0)
            Logger.LogError("trying to do AoE area action but didn't find any radius on " + spellHit.spell.name);


        Collider[] nearby = Physics.OverlapSphere(startPos, spellHit.radius, WorldFunctions.entityMask);
        int numHitsLeft = spellHit.numStrikes;

        //Logger.Log(from.name + "'s GetFirstEnemyAreaAction found " + nearby.Length);

        if (nearby.Length > 0)
        { //if found anything
            Entity target; //temp
            for (int i = 0; i < nearby.Length && numHitsLeft > 0; i++)
            { //loop through all Entities we found
                target = nearby[i].GetComponent<Entity>();
                if (target != null && GameMode.instance.CheckNotFriendly(spellHit.caster, target))
                {
                    numHitsLeft--;
                    action(new spellHit(spellHit, spellHit.caster, target, spellHit.origin, spellHit.vTarget)); //perform action on entity with new spellHit instance
                }
            }
        }
    }

    /// <summary>
    /// chain through enemies up to spellHit.numStrikes times
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="startPos">where to begin calculating the chain</param>
    /// <param name="action">action to be called after creating a duplicate spellHit instance to be calculated on the target</param>
    public static void ChainEnemies(spellHit spellHit, Vector3 startPos, Action<spellHit> action)
    {
        Collider[] nearby = Physics.OverlapSphere(startPos, spellHit.radius, WorldFunctions.entityMask);
        Entity target; //temp

        for (int i = 0; i < spellHit.numStrikes && nearby.Length > 0; i++)
        {

            //loop through all nearby that we found
            if (nearby.Length > 0)
            { //if we found something
                for (int x = 0; x < nearby.Length; i++)
                {
                    target = nearby[x].GetComponent<Entity>();
                    if (target != null && GameMode.instance.CheckNotFriendly(spellHit.caster, target))
                    {
                        startPos = target.transform.position; //begin the chain anew on this entity's position
                        action(new spellHit(spellHit, spellHit.caster, target, spellHit.origin, spellHit.vTarget)); //perform action on entity with new spellHit instance
                    }
                }
            }
            else
            { //chain will end here...

                break;
            }

            nearby = Physics.OverlapSphere(startPos, spellHit.radius, WorldFunctions.entityMask);
        } //end loop through numStrikes

    } //end AoEAction_ChainEnemies



    /// <summary>
    /// chain through friendlies up to spellHit.numStrikes times
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="startPos">where to begin calculating the chain</param>
    /// <param name="action">action to be called after creating a duplicate spellHit instance to be calculated on the target</param>
    public static void ChainFriendlies(spellHit spellHit, Vector3 startPos, Action<spellHit> action)
    {
        if (spellHit.radius <= 0)
            Logger.LogError("trying to do AoE chain but didn't find any radius on " + spellHit.spell.name);


        Collider[] nearby = Physics.OverlapSphere(startPos, spellHit.radius, WorldFunctions.entityMask);
        Entity target; //temp

        for (int i = 0; i < spellHit.numStrikes && nearby.Length > 0; i++)
        {

            //loop through all nearby that we found
            if (nearby.Length > 0)
            { //if we found something
                for (int x = 0; x < nearby.Length; i++)
                {
                    target = nearby[x].GetComponent<Entity>();
                    if (target != null && GameMode.instance.CheckFriendly(spellHit.caster, target))
                    {
                        startPos = target.transform.position; //begin the chain anew on this entity's position
                        action(new spellHit(spellHit, spellHit.caster, target, spellHit.origin, spellHit.vTarget)); //perform action on entity with new spellHit instance
                    }
                }
            }
            else
            { //chain will end here...

                break;
            }

            nearby = Physics.OverlapSphere(startPos, spellHit.radius, WorldFunctions.entityMask);
        } //end loop through numStrikes

    } //end AoEAction_ChainFriendlies



    /// <summary>
    /// simply return all entities in Physics.OverlapSphere
    /// </summary>
    public static Entity[] GetAreaEntities(Vector3 location, float range)
    {
        return Physics.OverlapSphere(location, range, WorldFunctions.entityMask).Select(c => c.GetComponent<Entity>()).ToArray();
    }
    /// <summary>
    /// simply return all entities in Physics.OverlapSphere
    /// </summary>
    public static IEnumerable<Entity> GetAreaEntities_Enum(Vector3 location, float range)
    {
        return Physics.OverlapSphere(location, range, WorldFunctions.entityMask).Select(c => c.GetComponent<Entity>());
    }

    /// <summary>
    /// get all local players using Physics.OverlapSphere using the LocalPlayerInteraction layer
    /// useful for GUI stuff, but that should be it
    /// </summary>
    public static Player[] GetAreaLocalPlayers(Vector3 location, float range)
    {
        return Physics.OverlapSphere(location, range, WorldFunctions.localPlayerMask).Select(c => c.GetComponentInParent<Player>()).ToArray();
    }


    /// <summary>
    /// simply return all entities in Physics.OverlapSphere
    /// </summary>
    public static Entity[] GetAreaEntities(spellHit spellHit)
    {
        if (spellHit.radius <= 0)
            Logger.LogError("trying to do AoE area action but didn't find any radius on " + spellHit.spell.name);


        return Physics.OverlapSphere(spellHit.vTarget, spellHit.radius, WorldFunctions.entityMask).Select(c => c.GetComponent<Entity>()).ToArray();
    }


    public static Entity[] GetAreaEnemies(Entity from, float range)
    {
        return Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask).Where(c => c != null && GameMode.instance.CheckNotFriendly(from, c.GetComponent<Entity>())).Select(entry => entry.GetComponent<Entity>()).ToArray();
    }

    public static IEnumerable<Collider> GetAreaEnemies_Colliders(Entity from, float range)
    {
        return Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask).Where(c => c != from && c != null && GameMode.instance.CheckNotFriendly(from, c.GetComponent<Entity>()));
    }

    public static IEnumerable<Collider> GetAreaFriendlies_Colliders(Entity from, float range)
    {
        return Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask).Where(c => c != null && GameMode.instance.CheckFriendly(from, c.GetComponent<Entity>()));
    }
    public static IEnumerable<Collider> GetAreaFriendlies_Colliders_NotSelf(Entity from, float range)
    {
        return Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask).Where(c => c != from && c != null && GameMode.instance.CheckFriendly(from, c.GetComponent<Entity>()));
    }

    public static List<Entity> GetAreaEnemiesListed(Entity from, float range)
    {
        return Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask).Where(c => c != null && GameMode.instance.CheckNotFriendly(from, c.GetComponent<Entity>())).Select(entry => entry.GetComponent<Entity>()).ToList<Entity>();
    }

    public static Entity[] GetAreaFriendlyEntities(Entity from, float range)
    {
        return Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask).Select(c => { if (c != null && GameMode.instance.CheckFriendly(from, c.GetComponent<Entity>())) return c.GetComponent<Entity>(); return null; }).Where(entry => entry != null).ToArray();
    }

    /// <summary>
    /// return friendlies in area other than self
    /// </summary>
    public static Entity[] GetAreaFriendlyEntities_NotSelf(Entity from, float range)
    {
        return Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask).Select(c => { if (c != null && GameMode.instance.CheckFriendly(from, c.GetComponent<Entity>())) return c.GetComponent<Entity>(); return null; }).Where(entry => entry != null && entry != from).ToArray();
    }

    public static bool GetClosestFriendly(Entity from, float range, out Entity closest)
    {
        Entity returns = default;
        Ext.RunTaskThread(() => {
            float distance = range + 1; //temp, make larger than max range to make any return shorter
            returns = null;
            Collider[] nearby = Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask); //get all nearby entities
            Entity entityTemp;
            for (int i = 0; i < nearby.Length; i++)
            {
                entityTemp = nearby[i].GetComponent<Entity>();
                if (GameMode.instance.CheckFriendly(from, entityTemp) && entityTemp.id != from.id && Vector3.Distance(from.transform.position, nearby[i].transform.position) < distance) //if closer than current closest
                    returns = entityTemp;
            }
        });

        closest = returns;
        return closest != null; //return true if found
    }

    public static bool GetClosestFriendly(Entity caster, Transform from, ref float range, out Entity closest)
    {
        float distance = range + 1; //temp, make larger than max range to make any return shorter
        closest = null;
        Collider[] nearby = Physics.OverlapSphere(from.position, range, WorldFunctions.entityMask); //get all nearby entities
        Entity entityTemp;
        for (int i = 0; i < nearby.Length; i++)
        {
            entityTemp = nearby[i].GetComponent<Entity>();
            if (GameMode.instance.CheckFriendly(caster, entityTemp) && entityTemp.id != caster.id && Vector3.Distance(from.position, nearby[i].transform.position) < distance) //if closer than current closest
                closest = entityTemp;
        }
        return closest != null; //return true if found
    }

    public static bool GetClosestValidTarget(Entity caster, Transform from, ref float range, out Entity closest, Func<Entity, Entity, int> checkValid)
    {
        float distance = range + 1; //temp, make larger than max range to make any return shorter
        closest = null;
        Collider[] nearby = Physics.OverlapSphere(from.position, range, WorldFunctions.entityMask); //get all nearby entities
        Entity entityTemp;
        for (int i = 0; i < nearby.Length; i++)
        {
            entityTemp = nearby[i].GetComponent<Entity>();
            if (checkValid(entityTemp, caster) == castFailCodes.success && entityTemp.id != caster.id && Vector3.Distance(from.position, nearby[i].transform.position) < distance) //if closer than current closest
                closest = entityTemp;
        }
        return closest != null; //return true if found
    }

    public static bool GetClosestEnemy(Entity from, ref float range, out Entity closest)
    {
        float distance = range + 1; //temp, make larger than max range to make any return shorter
        closest = null;
        Collider[] nearby = Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask); //get all nearby entities
        Entity entityTemp;
        for (int i = 0; i < nearby.Length; i++)
        {
            entityTemp = nearby[i].GetComponent<Entity>();
            if (GameMode.instance.CheckNotFriendly(from, entityTemp) && Vector3.Distance(from.transform.position, nearby[i].transform.position) < distance) //if closer than current closest
                closest = entityTemp;
        }
        return closest != null; //return true if found
    }
    public static bool GetClosestEnemy(Entity caster, Transform from, ref float range, out Entity closest)
    {
        float distance = range + 1; //temp, make larger than max range to make any return shorter
        closest = null;
        Collider[] nearby = Physics.OverlapSphere(from.position, range, WorldFunctions.entityMask); //get all nearby entities
        Entity entityTemp;
        for (int i = 0; i < nearby.Length; i++)
        {
            entityTemp = nearby[i].GetComponent<Entity>();
            if (GameMode.instance.CheckNotFriendly(caster, entityTemp) && Vector3.Distance(from.position, nearby[i].transform.position) < distance) //if closer than current closest
                closest = entityTemp;
        }
        return closest != null; //return true if found
    }
    public static bool GetClosestFriendly_Type(Entity from, ref float range, int entityType, out Entity closest)
    {
        float distance = range + 1; //make larger than max range to make any return shorter
        closest = null;
        Entity entityTemp;
        Collider[] nearby = Physics.OverlapSphere(from.transform.position, range, WorldFunctions.entityMask); //get all nearby entities
        for (int i = 0; i < nearby.Length; i++)
        {
            entityTemp = nearby[i].GetComponent<Entity>();
            if (GameMode.instance.CheckFriendly(from, entityTemp) && entityTemp.id != from.id && entityTemp.type == entityType && Vector3.Distance(from.transform.position, nearby[i].transform.position) < distance) //if closer than current closest
                closest = entityTemp;
        }
        return closest != null; //return true if found
    }


    /// <summary>
    /// calls an action on all Entities in a spherecast using all spellActions tagged isAoE
    /// </summary>
    /// <param name="onHitTarget">probably spellHit.spell.OnAnimationHit</param>
    public static void Cone(Vector3 location, Entity caster, spellHit spellHit, Action<Entity, Entity> onHitTarget, int coneAngle = 30)
    {
        if (spellHit.range <= 0)
            Logger.LogError("trying to do AoE cone but didn't find any range on " + spellHit.spell.name);


        Collider[] hits = Array.FindAll(Physics.OverlapSphere(location, spellHit.range, WorldFunctions.entityMask), collider => Vector3.Dot((collider.transform.position - caster.transform.position).normalized, caster.transform.forward) > coneAngle);

        /*
        Entity[] hits = Physics.OverlapSphere(location, castable.range, WorldFunctions.entityMask).Select(c => c.GetComponent<Entity>()).ToArray();
        for(int i = 0; i < hits.Length; i++)
        {
            Logger.Log(hits[i].name + " is " + (Vector3.Dot((hits[i].transform.position - caster.transform.position), caster.transform.forward) > castable.coneAngle) + " because angle is " + Vector3.Dot((hits[i].transform.position - caster.transform.position).normalized, Vector3.forward));
        }*/

        Logger.Log("cone cast hit " + hits.Length);

        if (hits.Length > 0)
            for (int i = 0; i < hits.Length; i++)
            {
                //shouldn't need to check for Entity component because everything on this layer should have one, right?..
                onHitTarget(caster, hits[i].GetComponent<Entity>()); //call action on entity
            }
    }
    /// <summary>
    /// calls an action on all Entities in a spherecast using all spellActions tagged isAoE
    /// </summary>
    /// <param name="onHitTarget">probably spellHit.spell.OnAnimationHit</param>
    public static void Cone(Vector3 location, Entity caster, spellHit spellHit, Action<spellHit> onHitTarget, int coneAngle = 30)
    {
        if (spellHit.range <= 0)
            Logger.LogError("trying to do AoE cone but didn't find any range on " + spellHit.spell.name);


        Collider[] hits = Array.FindAll(Physics.OverlapSphere(location, spellHit.range, WorldFunctions.entityMask), collider => Vector3.Dot((collider.transform.position - caster.transform.position).normalized, caster.transform.forward) > coneAngle);

        /*
        Entity[] hits = Physics.OverlapSphere(location, castable.range, WorldFunctions.entityMask).Select(c => c.GetComponent<Entity>()).ToArray();
        for(int i = 0; i < hits.Length; i++)
        {
            Logger.Log(hits[i].name + " is " + (Vector3.Dot((hits[i].transform.position - caster.transform.position), caster.transform.forward) > castable.coneAngle) + " because angle is " + Vector3.Dot((hits[i].transform.position - caster.transform.position).normalized, Vector3.forward));
        }*/

        Logger.Log("cone cast hit " + hits.Length);
        if (hits.Length > 0)
        {
            Entity e;
            for (int i = 0; i < hits.Length; i++)
            {
                e = hits[i].GetComponent<Entity>();
                //shouldn't need to check for Entity component because everything on this layer should have one, right?..
                if (spellHit.spell.CalcValidTarget(e, spellHit.caster) == castFailCodes.success)
                {
                    spellHit newHit = new spellHit(spellHit, spellHit.caster, e, spellHit.origin, spellHit.vTarget); //duplicate spellhit which contains offensive calculations from caster so it can be passed to all targets and calculated against their defensive bonuses
                    onHitTarget(newHit); //call action on entity
                }
            }
        }
    }


    public static Entity[] GetAoECone(spellHit spellHit, float angle, float offSet = 4)
    {
        if (spellHit.range <= 0)
            Logger.LogError("trying to do AoE cone but didn't find any range on " + spellHit.spell.name);


        return Physics.OverlapSphere(spellHit.caster.transform.position, spellHit.range, WorldFunctions.entityMask).Where(collider => collider.GetComponent<Entity>() != null && Vector3.Dot((collider.transform.position - (spellHit.caster.transform.position + (-spellHit.caster.transform.forward * offSet))).normalized, spellHit.caster.transform.forward) > angle).Select(entry => entry.GetComponent<Entity>()).ToArray();

    }


    public static void Periodic(spellHit spellHit, Action<spellHit> action)
    {
        CoroutineHandle routine = Timing.CallPeriodically(spellHit.duration, spellHit.interval,
            () => { DoAction(spellHit, action); }, //do AoEAction on interval
            () => { routine = default; } //on done... gc
        );
        //spellHit.caster.onDeath += () => { EndPeriodicAoE(spellHit, routine); }; //if caster dies, stop

    }
    //also unsubscribes from caster death
    /* not sure how to implement this so the Coroutine can unsubscribe from caster death
    static void EndPeriodicAoE(spellHit spellHit, CoroutineHandle routine) {
        spellHit.caster.onDeath -= () => { EndPeriodicAoE(spellHit, routine); }; //also remove self from 
        Timing.KillCoroutines(routine);
    }
    */

    /// <summary>
    /// calls an action on all Entities in a spherecast, automatically uses CalcValidTarget, and creates a new spellHit for each target (to prevent double defensive calculations)
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="action">probably spellHit.spell.OnAnimationHit</param>
    public static void DoAction(spellHit spellHit, Func<spellHit, int> action)
    {
        if (spellHit.radius <= 0)
            Logger.LogError("trying to do AoE action but didn't find any radius on " + spellHit.spell.name);


        Collider[] hits = Physics.OverlapSphere(spellHit.vTarget, spellHit.radius, WorldFunctions.entityMask);
        Entity e = null;
        for (int i = 0; i < hits.Length; i++)
        {
            e = hits[i].GetComponent<Entity>();
            if (spellHit.spell.CalcValidTarget(e, spellHit.caster) == castFailCodes.success)
            {
                action(new spellHit(spellHit, spellHit.caster, e, spellHit.origin, spellHit.vTarget)); //create a new spellHit for each entity hit, because it affects them in different ways
            }
        }
    }
    /// <summary>
    /// calls an action on all Entities in a spherecast, automatically uses CalcValidTarget, and creates a new spellHit for each target (to prevent double defensive calculations) 
    /// </summary>
    public static void DoAction(spellHit spellHit, Action<spellHit> action)
    {
        if (spellHit.radius <= 0)
            Logger.LogError("trying to do AoE action but didn't find any radius on " + spellHit.spell.name);

        Collider[] hits = Physics.OverlapSphere(spellHit.vTarget, spellHit.radius, WorldFunctions.entityMask);
        Entity e = null;
        for (int i = 0; i < hits.Length; i++)
        {
            e = hits[i].GetComponent<Entity>();
            if (spellHit.spell.CalcValidTarget(e, spellHit.caster) == castFailCodes.success)
            {
                action(new spellHit(spellHit, spellHit.caster, e, spellHit.origin, spellHit.vTarget)); //create a new spellHit for each entity hit, because it affects them in different ways
            }
        }
    }

    /// <summary>
    /// calls an action on all Entities in a spherecast, automatically uses CalcValidTarget
    /// </summary>
    public static void DoAction(spellHit spellHit, Action<spellHit> action, Func<Entity, Entity, int> calcValidTarget)
    {
        if (spellHit.radius <= 0)
            Logger.LogError("trying to do AoE action but didn't find any radius on " + spellHit.spell.name);

        Collider[] hits = Physics.OverlapSphere(spellHit.vTarget, spellHit.radius, WorldFunctions.entityMask);
        Entity e = null;
        for (int i = 0; i < hits.Length; i++)
        {
            e = hits[i].GetComponent<Entity>();
            if (calcValidTarget(e, spellHit.caster) == castFailCodes.success)
            {
                action(new spellHit(spellHit, spellHit.caster, e, spellHit.origin, spellHit.vTarget)); //create a new spellHit for each entity hit, because it affects them in different ways
            }
        }
    }

    /// <summary>
    /// makes all valid entities within spherecast have effect, re-checking if they remain in AoE area every 0.3f with spellHit.radius
    /// </summary>
    public static void Zone_SpellMod(spellHit spellHit, SpellMod effect, bool ensureUnique)
    {
        Timing.RunCoroutine(IContinuous_SpellMod(spellHit, effect, ensureUnique)); //continuously apply status effects and check entities in the area

    } //end function Zone_SpellMod


    static IEnumerator<float> IContinuous_SpellMod(spellHit spellHit, SpellMod effect, bool ensureUnique)
    {

        if (spellHit.radius <= 0)
            Logger.LogError("trying to do AoE action but didn't find any radius on " + spellHit.spell.name);


        Collider[] hitsNew;
        Collider[] hitsOld = new Collider[0];

        Entity e = null;
        float timer = spellHit.duration; //countdown

        while (timer >= 0)
        {
            hitsNew = Physics.OverlapSphere(spellHit.vTarget, spellHit.radius, WorldFunctions.entityMask);

            //try add new effects for valid targets
            for (int i = 0; i < hitsNew.Length; i++)
            { //loop through all hit colliders
                e = hitsNew[i].GetComponent<Entity>();
                if (!hitsOld.Contains(hitsNew[i]) && spellHit.spell.CalcValidTarget(e, spellHit.caster) == castFailCodes.success && (!ensureUnique || !e.spellMods.Contains(effect)))
                { //if they're not seen before and a valid target and (doesn't contain effect already or doesn't matter)
                    e.spellMods.Add(effect); //add effect
                }
            } //end loop

            //try remove effects for targets that are no longer in area
            for (int i = 0; i < hitsOld.Length; i++)
            {
                e = hitsOld[i].GetComponent<Entity>();
                if (e != null && !hitsNew.Contains(hitsOld[i]))
                { //if not found in area
                    e.spellMods.Remove(effect); //remove effect
                }
            }

            hitsOld = hitsNew; //new are now old

            timer -= 0.3f;
            yield return 0.3f;
        }
    } //end function IContinuous_SpellMod



    /// <summary>
    /// calls action with a deep copy of spellHit on all entities within spellHit.radius of spellHit.vTarget every spellHit.interval for spellHit.duration
    /// </summary>
    public static void Zone_Action(spellHit spellHit, Action<spellHit> action)
    {
        Timing.RunCoroutine(IContinuous_Action(spellHit, action)); //continuously apply status effects and check entities in the area

    } //end function Zone_SpellMod



    static IEnumerator<float> IContinuous_Action(spellHit spellHit, Action<spellHit> action)
    {

        Collider[] hitsNew;
        //Collider[] hitsOld = new Collider[0];

        Entity e = null;
        float timer = spellHit.duration; //countdown

        while (timer >= 0)
        {
            hitsNew = Physics.OverlapSphere(spellHit.vTarget, spellHit.radius, WorldFunctions.entityMask);

            //try add new effects for valid targets
            for (int i = 0; i < hitsNew.Length; i++)
            { //loop through all hit colliders
                e = hitsNew[i].GetComponent<Entity>();
                if (spellHit.spell.CalcValidTarget(e, spellHit.caster) == castFailCodes.success)
                { //if they're not seen before and a valid target and (doesn't contain effect already or doesn't matter)
                    action(new spellHit(spellHit, spellHit.caster, e, spellHit.origin, spellHit.vTarget));
                }
            } //end loop
            timer -= spellHit.interval;
            yield return Timing.WaitForSeconds(spellHit.interval);
        }
    } //end function IContinuous_Action



} //end class AOEs