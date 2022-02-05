using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;


public class T_HealingBeamWire : GridTalentBlack
{
    public override string nameFormatted => "Healing Beam Robot";
    public override string description => "shoots robots that create healing beams between them";
    public override string iconName => nameFormatted;

    const int incrementAmount = 1;

    public T_HealingBeamWire() : base(5, Spell.GetName<S_HealerBeamHealTotem>())
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
        return "number of robots: "+ currentLevel;
    }
} //end T_HealingBeamWire



public class T_HealerCleanse : GridTalentBlack
{
    public override string nameFormatted => "Cleanse";
    public override string description => "removes a harmful effect from a friendly target";
    public override string iconName => nameFormatted;

    public T_HealerCleanse() : base(2, Spell.GetName<S_HealerCleanse>())
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
        return "number of effects cleansed: " + currentLevel;
    }
} //end T_HealerCleanse



public class T_HealerWard : GridTalentBlack
{

    public override string nameFormatted => "Ward";
    public override string description => "unlock ability: Ward";
    public override string iconName => nameFormatted;

    const int perLevel = 3;

    public T_HealerWard() : base(7, Spell.GetName<S_HealerWard>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseDamageSpell_Flat(Spell.GetName<S_HealerWard>(), perLevel, perLevel)
        };
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
        return CalcDescriptionFlat(ref currentLevel, perLevel) + " additional damage blocked";
    }

} //end talent T_HealerWard



public class  T_HealerWardDuration : GridTalentBlack
{

    public override string nameFormatted => "Ward: Onboard Proton Farm";
    public override string description => "increases the duration of Ward";
    public override string iconName => nameFormatted;

    const float perLevel = 2f;

    public T_HealerWardDuration() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseDurationSpell_Flat(Spell.GetName<S_HealerWard>(), perLevel, perLevel)
        };
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
        return CalcDescriptionFlat(ref currentLevel, perLevel) + " additional seconds";
    }

} //end talent T_HealerWardDuration



public class T_HealerWardExpireHeal : GridTalentBlack
{

    public override string nameFormatted => "Ward: Tachyon Containment Engine";
    public override string description => "heals the target when Ward is consumed";
    public override string iconName => nameFormatted;

    const float perLevel = 2f;

    public T_HealerWardExpireHeal() : base(5, Spell.GetName<S_HealerWardConsumed>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new TriggerSpell_TalentMods.OnConsumeStatusEffect_CastOnWearer(Spell.GetID<S_HealerWardConsumed>())
        };
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
        return CalcDescriptionFlat(ref currentLevel, perLevel) + " damage healed";
    }

} //end talent T_HealerWardExpireHeal


public class T_HealerOvercharge : GridTalentBlack
{

    public override string nameFormatted => "Overcharge";
    public override string description => "increase target's damage for duration";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 2f;
    const float baseAmount = 2f;

    public T_HealerOvercharge() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
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
        return "";
    }

} //end talent T_HealerOvercharge


public class T_HealerBeamHealTotem : GridTalentBlack
{

    public override string nameFormatted => "Beam Heal Totem";
    public override string description => " ";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 2f;
    const float baseAmount = 2f;

    public T_HealerBeamHealTotem() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
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
        return "";
    }

} //end talent T_HealerBeamHealTotem


public class T_HealerHoT : GridTalentBlack
{

    public override string nameFormatted => "HoT";
    public override string description => " ";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 2f;
    const float baseAmount = 2f;

    public T_HealerHoT() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
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
        return "";
    }

} //end talent T_HealerHoT


public class T_HealerSpeedBuff : GridTalentBlack
{

    public override string nameFormatted => "Speed Buff";
    public override string description => " ";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 2f;
    const float baseAmount = 2f;

    public T_HealerSpeedBuff() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
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
        return "";
    }

} //end talent T_HealerSpeedBuff


public class T_HealerLeech : GridTalentBlack
{

    public override string nameFormatted => "Scuttle";
    public override string description => " ";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 2f;
    const float baseAmount = 2f;

    public T_HealerLeech() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
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
        return "";
    }

} //end talent T_HealerLeech

public class T_HealerAimedShot : GridTalentBlack
{

    public override string nameFormatted => "Aimed Shot";
    public override string description => " ";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 2f;
    const float baseAmount = 2f;

    public T_HealerAimedShot() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
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
        return "";
    }

} //end talent T_HealerAimedShot


public class T_HealerEncouragingGaze : GridTalentBlack
{

    public override string nameFormatted => "Aimed Shot";
    public override string description => " ";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 2f;
    const float baseAmount = 2f;

    public T_HealerEncouragingGaze() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
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
        return "";
    }

} //end talent T_HealerEncouragingGaze



public class T_HealerWardConsumed : GridTalentBlack
{

    public override string nameFormatted => "Ward Consumed Heal";
    public override string description => " ";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 2f;
    const float baseAmount = 2f;

    public T_HealerWardConsumed() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
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
        return "";
    }

} //end talent T_HealerWardConsumed

