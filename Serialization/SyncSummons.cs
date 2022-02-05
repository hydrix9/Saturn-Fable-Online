using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SyncSummons : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false;


    [HideInInspector]
    public List<Summoned> cast_summons = new List<Summoned>(); //everything we've summoned so far
    [HideInInspector]
    public List<Summoned> sentryTurrets = new List<Summoned>(); //everything we've summoned so far
    [HideInInspector]
    public List<Summoned> sentryNonTurrets = new List<Summoned>(); //everything we've summoned so far
    [HideInInspector]
    public List<Summoned> minions = new List<Summoned>(); //everything we've summoned so far
    [HideInInspector]
    public List<Summoned> pets_nonCombat = new List<Summoned>(); //everything we've summoned so far

    Entity entity;

    //codepoint constants
    public static readonly string
        maxSentryTurret = "maxTurret",
        maxSentryNonTurret = "maxNonTurret",
        maxMinions = "maxMinions"
    ;

    SyncInt8Field maxSentryTurretField;
    SyncInt8Field maxSentryNonTurretField;
    SyncInt8Field maxMinionsField;

    #region params
    const int maxNonCombatPets = 1;

    #endregion

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entity.onClean += Clean; //clean refs on death
    }

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        maxSentryTurretField = new SyncInt8Field(maxSentryTurret, false, 1, parent);
        maxSentryNonTurretField = new SyncInt8Field(maxSentryNonTurret, false, 1, parent);
        maxMinionsField = new SyncInt8Field(maxMinions, false, 1, parent);

        syncFields = new SyncField[]
        {
            maxSentryTurretField,
            maxSentryNonTurretField,
            maxMinionsField
        };

    } //end func SetDefaults

    /// <summary>
    /// return number of summoned by entity name
    /// </summary>
    public int CountByEntityName(string name)
    {
        int type = WorldFunctions.GetEntityTypeID(name); //find entity type 
        int returns = 0;

        for (int i = 0; i < cast_summons.Count; i++)
        {
            if (cast_summons[i] != null && cast_summons[i].spellHit.target.type == type) //if entity type id matches
                returns++;
        }
        return returns;
    }

    public void Add(Summoned summoned)
    {
        PurgeList(); //remove null entries so we can keep proper count

        cast_summons.Add(summoned);
        entity.OnAddSummoned(summoned.spellHit, summoned); //possibly modify the summoned obj/creature with higher HP, damage, etc
        TeamSpellModifier.instance.OnAddSummoned(summoned.spellHit, summoned); //perform team bonus calculations on caster

        List<int> spellTypes = summoned.spellHit.spellTypes; //cache
        if (spellTypes.Contains(spellType.sentry_turret))
            sentryTurrets.Add(summoned);
        else
        if (spellTypes.Contains(spellType.sentry_nonTurret))
            sentryNonTurrets.Add(summoned);
        else
        if (spellTypes.Contains(spellType.minion))
            minions.Add(summoned);
        else
        if (spellTypes.Contains(spellType.pet_nonCombat))
            pets_nonCombat.Add(summoned);

        //remove oldest if went OOB on any max counts

        while (sentryTurrets.Count > maxSentryTurretField.value)
        {
            RemoveOldest(spellType.sentry_turret);
        }
        while (sentryNonTurrets.Count > maxSentryNonTurretField.value)
        {
            RemoveOldest(spellType.sentry_nonTurret);
        }
        while (minions.Count > maxMinionsField.value)
        {
            RemoveOldest(spellType.minion);
        }
        while (pets_nonCombat.Count > maxNonCombatPets)
        {
            RemoveOldest(spellType.pet_nonCombat);
        }

    } //end func Add

    /// <summary>
    /// destroy all but the non-combat pets
    /// </summary>
    public void DestroyAll_Combat()
    {
        for(int i = 0; i < cast_summons.Count; i++)
        {
            if (cast_summons[i] != null && cast_summons[i].spellHit != null && cast_summons[i].spellHit.spellTypes.Contains(spellType.pet_nonCombat))
                continue; //skip the non-combat pets
            RemoveDestroy(cast_summons[i]);
        }

    } //end func DestroyAll

    //remove oldest Summoned by spell type
    public void RemoveOldest(int spellType)
    {
        DateTime oldestDate = default;
        Summoned oldest = null;
        for(int i = 0; i < cast_summons.Count; i++)
        {
            if(cast_summons[i] != null && cast_summons[i].spellHit.spellTypes.Contains(spellType) && (oldestDate == default || cast_summons[i].spellHit.castStarted < oldestDate))
            { //if is same time and is older
                oldestDate = cast_summons[i].spellHit.castStarted;
                oldest = cast_summons[i];
            }
        }

        if (oldest != null) //if found something
            RemoveDestroy(oldest);
    } //end func RemoveOldest

    /// <summary>
    /// remove by summons spellType
    /// </summary>
    public void RemoveAllOfType(int spellType)
    {
        for (int i = 0; i < cast_summons.Count; i++)
        {
            if (cast_summons[i] != null && cast_summons[i].spellHit.spellTypes.Contains(spellType))
            {
                RemoveDestroy(cast_summons[i]);
            }
        }
    } //end func RemoveAllOfType

    //remove and destroy target
    public void RemoveDestroy(Summoned summoned)
    {
        Remove(summoned);
        if (summoned != null)
        {
            if (summoned.self != null)
                summoned.self.Destroy();
            else
            if (summoned.GetComponent<SyncObject>() != null) //wasn't loaded in yet or something, try destroy through the SyncObject
                summoned.GetComponent<SyncObject>().Destroy();
        }

    } //end func RemoveDestroy

    public void Remove(Summoned summoned)
    {
        cast_summons.Remove(summoned);
        sentryTurrets.Remove(summoned);
        sentryNonTurrets.Remove(summoned);
        pets_nonCombat.Remove(summoned);
    } //end func Remove

    public bool TryGet(string name, out Summoned first)
    {
        int type = WorldFunctions.GetEntityTypeID(name); //find entity type 

        for (int i = 0; i < cast_summons.Count; i++)
        {
            if (cast_summons[i] != null && cast_summons[i].spellHit.target.type == type)
            {
                first = cast_summons[i];
                return true; //fouund it, so assign and return true
            }
        }

        //didn't find, assign null and return false
        first = default;
        return false;
    }

    /// <summary>
    /// apply a new spellHit to all this.cast_summons
    /// </summary>
    /// <param name="original"></param>
    /// <param name="action"></param>
    public void ApplyToAllSummoned(spellHit original, Action<spellHit> action)
    {
        for (int i = 0; i < cast_summons.Count; i++)
            if(cast_summons[i].self.e != null)
                action(new spellHit(original, original.caster, cast_summons[i].self.e, original.origin, original.vTarget));
    }

    /// <summary>
    /// apply a new spellHit to all this.cast_summons of spellType spellType
    /// </summary>
    /// <param name="original"></param>
    /// <param name="action"></param>
    public void ApplyToAllSummonedType(spellHit original, Action<spellHit> action, int spellType)
    {
        for (int i = 0; i < cast_summons.Count; i++)
            if(cast_summons[i].spellHit.spellTypes.Contains(spellType) && cast_summons[i].self.e != null)
               action(new spellHit(original, original.caster, cast_summons[i].self.e, original.origin, original.vTarget));
    }


    void Clean(SyncObject owner)
    { //remove refs on death
        cast_summons.Clear();
        sentryTurrets.Clear();
        sentryNonTurrets.Clear();
        pets_nonCombat.Clear();

        entity.onClean -= Clean;
        entity = null;

    }

    //remove null entries
    void PurgeList()
    {
        cast_summons.RemoveAll(entry => entry == null || entry.self == null || entry.self.destroyed);
        sentryTurrets.RemoveAll(entry => entry == null || entry.self == null || entry.self.destroyed);
        sentryNonTurrets.RemoveAll(entry => entry == null || entry.self == null || entry.self.destroyed);
        minions.RemoveAll(entry => entry == null || entry.self == null || entry.self.destroyed);
        pets_nonCombat.RemoveAll(entry => entry == null || entry.self == null || entry.self.destroyed);

    } //end func PurgeList

} //end class SyncSentries
