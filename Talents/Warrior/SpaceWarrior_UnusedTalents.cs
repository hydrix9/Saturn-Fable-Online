using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
public class T_Bees : GridTalentBlack
{
    public override string nameFormatted => "BEES";
    public override string description => "go my pretties";
    public override string iconName => nameFormatted;

    public T_Bees() : base(3, Spell.GetName<S_BEES>())
    {
    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>();
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamSpellModifier.TeamMods(
            new List<TeamSpellModifier.SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<TeamSpellModifier.SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<TeamSpellModifier.StatModEntry>(), //faction StatMods
            new Dictionary<int, List<TeamSpellModifier.StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        //return S_BEES.CalcNumBees(currentLevel) + " bees";
        return "";
    }
}

public class T_Bombees : GridTalentBlack
{
    public override string nameFormatted => "BomBees";
    public override string description => "makes the bees explode when they crit";
    public override string iconName => nameFormatted;

    public T_Bombees() : base(3, Spell.GetName<S_BEES>())
    {
    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>();
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamSpellModifier.TeamMods(
            new List<TeamSpellModifier.SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<TeamSpellModifier.SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<TeamSpellModifier.StatModEntry>(), //faction StatMods
            new Dictionary<int, List<TeamSpellModifier.StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "explosion radius: " + (currentLevel * 5);
    }

}

public class T_KillerBees : GridTalentBlack
{
    public override string nameFormatted => "Killer Bees";
    public override string description => "makes the bees last longer";
    public override string iconName => nameFormatted;

    const float incrementDuration = 5f;

    public T_KillerBees() : base(3)
    {
    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { new OnCalcOffensive_BaseTalentMods.IncreaseDurationSpell_Flat(Spell.GetName<S_BEES>(), incrementDuration, incrementDuration) };
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamSpellModifier.TeamMods(
            new List<TeamSpellModifier.SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<TeamSpellModifier.SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<TeamSpellModifier.StatModEntry>(), //faction StatMods
            new Dictionary<int, List<TeamSpellModifier.StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "increased by " + (currentLevel * incrementDuration).ToString("0.0") + " seconds";
    }
}



public class T_KillererBees : GridTalentBlack
{
    public override string nameFormatted => "Killerer Bees";
    public override string description => "makes the bees do more damage";
    public override string iconName => nameFormatted;

    const float incrementAmount = 1.1f;

    public T_KillererBees() : base(3)
    {
    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>();
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamSpellModifier.TeamMods(
            new List<TeamSpellModifier.SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<TeamSpellModifier.SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<TeamSpellModifier.StatModEntry>(), //faction StatMods
            new Dictionary<int, List<TeamSpellModifier.StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "increased damage by " + ((currentLevel * (incrementAmount - 1)) * 100).ToString("0") + "%";
    }
}
*/