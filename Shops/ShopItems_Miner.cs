using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;


public class U_MiningSpeed : HorizontalUpgradeItem
{
    public override string nameFormatted => "Mining Speed";
    public override string description => "increases the rate of mineral production";
    public override string iconName => nameFormatted;

    public const float incrementAmount = 0.1f;

    public U_MiningSpeed() : base(5, null, false)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { new OnCalcOffensive_BaseTalentMods.IncreaseCurrencyGeneratedSpell_Percent(Spell.GetName<S_MineMinerals>(), incrementAmount, incrementAmount) }; //increase amount genearted by S_MineMinerals spell by percent
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>() { };
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
        return (currentLevel + 100) + "% mining speed";
    }
}

