using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SummonCreate
{

    /// <summary>
    /// create a generic summoned item like a sentry or barrier which has no script deriving from Summoned which implements specific behaviours like OnTriggerEnter
    /// </summary>
    public static SyncObject Single(spellHit spellHit, string entityPrefabName, Vector3 startPos, Transform startAnchor = null, int[] knownSpells = null, SpellMod[] knownMods = null)
    {
        //instantiate at position with location
        SyncObject added = Server.instance.AddEntity(entityPrefabName, null);
        added.transform.position = startPos;
        added.transform.rotation = spellHit.caster.transform.rotation;
        added.transform.SetParent(startAnchor);
        Set(spellHit, added, knownSpells, knownMods);
        return added;
    }

    /// <summary>
    /// do stuff after instantiating, like add Summoned component, set known spells and talents
    /// </summary>
    static void Set(spellHit spellHit, SyncObject added, int[] knownSpells = null, SpellMod[] knownMods = null)
    {

        added.gameObject.AddComponent<Summoned>().Set(spellHit, true); //create and set totem component

        if (knownSpells != null)
            added.e.syncCasting.spellsKnown.AddRange(knownSpells); //Add spells this entity knows from function parameter
        if (knownMods != null)
            added.e.spellMods.AddRange(knownMods);
    }

    /// <summary>
    /// copy health and energy perecents, remove old entity, and create new, upgraded version
    /// </summary>
    public static bool UpgradeTo(string newPrefab, spellHit selfStatusEffect, out SyncObject newObj, int[] knownSpells = null, SpellMod[] knownMods = null)
    {
        newObj = null; //init out param

        if (selfStatusEffect.caster == null)
            return false; //somehow got destroyed before could remove this obj from the scene

        //copy all relevant data (health, energy, power), destroy entity, then create new
        //in that order, so we don't interfere with summons limit

        Summoned current = selfStatusEffect.target.GetComponent<Summoned>(); //cache
        float healthPercent = current.self.Get<int>(SyncHealth.health) / current.self.Get<int>(SyncHealth.maxHealth);
        float energyPercent = current.self.Get<int>(SyncEnergy.energy) / current.self.Get<int>(SyncEnergy.maxEnergy);


        newObj = SummonCreate.Single(current.spellHit, newPrefab, current.transform.position, current.transform.parent, knownSpells, knownMods);

        current.self.Destroy(); //remove old

        //copy energy percent to new upgraded turret
        int maxHealth = newObj.Get<int>(SyncHealth.maxHealth);
        int maxEnergy = newObj.Get<int>(SyncEnergy.maxEnergy);
        newObj.Set<int>(SyncHealth.health, (int)Mathf.Max(0, Mathf.Min(maxHealth, (healthPercent * maxHealth))));
        newObj.Set<int>(SyncEnergy.energy, (int)Mathf.Max(0, Mathf.Min(maxEnergy, (energyPercent * maxEnergy))));
        
        return true;
    } //end func UpgradeTo

} //end class SummonCreate
