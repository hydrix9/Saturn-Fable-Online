using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TeamSpellModifier;
using System.Linq;
using System.Collections.Concurrent;

public class T_EngiEnlightenment : GridTalentBlack
{
    public override string nameFormatted => "Enlightenment";
    public override string description => "allows you to create an additional turret";
    public override string iconName => nameFormatted;


    public T_EngiEnlightenment() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { new StatMod<int>(SyncSummons.maxSentryTurret, ModMaxTurrets, this.GetType()) };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamSpellModifier.TeamMods(
            new List<TeamSpellModifier.SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<TeamSpellModifier.SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<TeamSpellModifier.StatModEntry>(), //faction StatMods
            new Dictionary<int, List<TeamSpellModifier.StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    static int ModMaxTurrets(int current, int talentLevel)
    {
        return current + talentLevel;
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return (int)(currentLevel) + " additional turret";
    }

} //end talent T_EngiEnlightenment




public class T_EngiProgeny : GridTalentBlack
{
    public override string nameFormatted => "Progeny";
    public override string description => "increases max health of built machines";
    public override string iconName => nameFormatted;

    public const float increaseAmount = 0.05f;
    StatMod increaseHealthMod;
    public T_EngiProgeny() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        increaseHealthMod = new StatMod<int>(SyncHealth.maxHealth, ModMaxHealth, typeof(T_EngiProgeny), true); //cache

        spellMods = new List<TalentMod>() {
            new OnSummonTalentMods.ModStatSpellType(spellType.sentry_turret, increaseHealthMod, this.GetType()),
            new OnSummonTalentMods.ModStatSpellType(spellType.sentry_nonTurret, increaseHealthMod, this.GetType()),
            new OnSummonTalentMods.ModStatSpellType(spellType.minion, increaseHealthMod, this.GetType()),
            new OnSummonTalentMods.ModStatSpellType(spellType.portal_entrance, increaseHealthMod, this.GetType()),
            new OnSummonTalentMods.ModStatSpellType(spellType.portal_exit, increaseHealthMod, this.GetType()),
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamSpellModifier.TeamMods(
            new List<TeamSpellModifier.SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<TeamSpellModifier.SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<TeamSpellModifier.StatModEntry>(), //faction StatMods
            new Dictionary<int, List<TeamSpellModifier.StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    static int ModMaxHealth(int current, int talentLevel)
    {
        return (int)(current * (talentLevel * increaseAmount + 1));
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, increaseAmount);
    }
} //end talent T_EngiProgeny


public class T_EngiMinefield : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const int amountPerLevel = 30;
    public const int baseAmount = 40;

    public T_EngiMinefield() : base(3, Spell.GetName<S_EngiMinefield>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() {  };
        permanentStatMods = new List<StatMod>() { };
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
        return "deals " + CalcDescriptionFlat(ref currentLevel, amountPerLevel) + " damage";
    }
} //end talent T_EngiMinefield


public class T_EngiMinefieldCooldown : GridTalentBlack
{
    public override string nameFormatted => "Advanced Minefield Assembly";
    public override string description => "reduces cooldown of " + Spell.GetName<S_EngiMinefield>();
    public override string iconName => nameFormatted;

    public const float baseAmount = 3f;
    public const float amountPerLevel = 3f;

    public T_EngiMinefieldCooldown() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.DecreaseCooldownSpell_Flat(Spell.GetName<S_EngiMinefield>(), baseAmount, amountPerLevel)
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "reduces cooldown by " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_EngiMinefieldCooldown

public class T_EngiOnMinefieldHitRoot : GridTalentBlack
{
    public override string nameFormatted => "EMP Mines";
    public override string description => Spell.GetSpell<S_EngiMinefield>().nameFormatted + " also immobilizes the target";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.5f;
    public const float baseAmount = 1f;

    public T_EngiOnMinefieldHitRoot() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_EngiOnMinefieldHitRoot>(), 1, 0, false, Spell.GetID<S_EngiMinefield_TriggerEffect>())
        };
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
        return "immobilizes for " + CalcDescriptionFlat(ref currentLevel, amountPerLevel) + " seconds";
    }
} //end talent T_EngiOnMinefieldHitRoot



public class T_EngiHealZone : GridTalentBlack
{
    public override string nameFormatted => "Nano Field";
    public override string description => "heal all allies an area over time";
    public override string iconName => nameFormatted;

    public const int amountPerLevel = 15;
    public const int baseAmount = 15;

    public T_EngiHealZone() : base(5, Spell.GetName<S_EngiHealZone>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " healed every " + S_EngiHealZone._interval + " seconds for " + S_EngiHealZone._duration + " seconds to allies within " + S_EngiHealZone._radius + "m";
    }
} //end talent T_EngiHealZone



public class T_EngiBuffBeam : GridTalentBlack
{
    public override string nameFormatted => "Electrify";
    public override string description => "shoot a beam that briefly increases the damage of all allies it touches";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiBuffBeam() : base(5, Spell.GetName<S_EngiBuffBeam>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
        };
        permanentStatMods = new List<StatMod>() { };
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
        return "damage increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel) + " for " + S_EngiBuffBeam_PowerEffect._duration + " seconds";
    }
} //end talent T_EngiBuffBeam



public class T_EngiLongHot : GridTalentBlack
{
    public override string nameFormatted => "Nano Swarm";
    public override string description => "heals the target over duration, can only be placed on one target";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 20f;
    public const float baseAmount = 20f;

    public T_EngiLongHot() : base(5, Spell.GetName<S_EngiLongHoT>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
        };
        permanentStatMods = new List<StatMod>() { };
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
        return "heal " + CalcDescriptionFlat(ref currentLevel, amountPerLevel) + " every " + S_EngiLongHoT._interval + " seconds for " + S_EngiLongHoT._duration + " seconds";
    }
} //end talent T_EngiLongHot


public class T_EngiBubbleRadius : GridTalentBlack
{
    public override string nameFormatted => "Graviton Field Compression";
    public override string description => "Increases the radius of your " + Spell.GetSpell<S_EngiBubble>().nameFormatted + " ability";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.2f;
    public const float baseAmount = 0.2f;

    public T_EngiBubbleRadius() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
            new OnCalcOffensive_BaseTalentMods.IncreaseRadiusSpell_Percent(Spell.GetName<S_EngiBubble>(), baseAmount, amountPerLevel)
        };
        permanentStatMods = new List<StatMod>() { };
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
        return "radius increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiBubbleRadius


public class T_EngiTurretEnergyPerSecondAura : GridTalentBlack
{
    public override string nameFormatted => "Altonian Resonator Fields";
    public override string description => "turrets also restore energy to nearby allies";
    public override string iconName => nameFormatted;

    public const int amountPerLevel = 5;
    public const int baseAmount = 5;

    Spell auraInitalCast; //what gets casted to apply the initial StatusEffect to talent owner

    public T_EngiTurretEnergyPerSecondAura() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {
        auraInitalCast = Spell.GetSpell<S_EngiTurretEnergyPerSecondAura>();

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
            new OnSummonTalentMods.AddEntityMod_SpellType(spellType.sentry_turret, new CastOnSelfEntityMod(auraInitalCast, (owner)=> { owner.RemoveAllStatusEffectsOfType_Raw_CastedBySelf(auraInitalCast.id); }), this.GetType()),
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamSpellModifier.TeamMods(
            new List<TeamSpellModifier.SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<TeamSpellModifier.SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<TeamSpellModifier.StatModEntry>(), //faction StatMods
            new Dictionary<int, List<TeamSpellModifier.StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    //construct the SpellBehaviour to add to the turret
    AI_CombatBehaviour.SpellBehaviour CreateTaughtBehaviour(Spell taughtSpell)
    {
        return new AI_CombatBehaviour.SpellBehaviour(taughtSpell.GetType(), AI_Turret.SelfBuff, AI_Turret.CanBasicAttack);
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionFlat(ref currentLevel, amountPerLevel) + " energy every " + S_EngiTurretEnergyPerSecondAura._interval + " to allies within " + S_EngiTurretEnergyPerSecondAura._radius + "m ";
    }
} //end talent T_EngiTurretEnergyPerSecondAura


public class T_EngiIncreaseTurretFireRate : GridTalentBlack
{
    public override string nameFormatted => "Alacrity";
    public override string description => "increase the fire rate of turrets";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.2f;
    public const float baseAmount = 0.2f;

    public T_EngiIncreaseTurretFireRate() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
            new OnSummonTalentMods.AddSpellMod_SpellType(spellType.sentry_turret, new OnCalcOffensive_BaseTalentMods.DecreaseCooldownSpell_Percent(Spell.GetName<S_EngiTurretShot>(), baseAmount, amountPerLevel), this.GetType())
        };
        permanentStatMods = new List<StatMod>() { };
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
        return "increased by " + CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel);
    }
} //end talent T_EngiIncreaseTurretFireRate


public class T_EngiIncreaseHealthTurrets : GridTalentBlack
{
    public override string nameFormatted => "Adamantine Ramparts";
    public override string description => "increase the health and size of turrets";
    public override string iconName => nameFormatted;

    const float incrementSizeAmount = 0.1f;
    const float incrementHealthAmount = 0.1f;

    public T_EngiIncreaseHealthTurrets() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
            new OnSummonTalentMods.ModStatSpellType(spellType.sentry_turret, new StatMod<int>(SyncHealth.maxHealth, ModHealth, this.GetType())),
            new OnSummonTalentMods.ModStatSpellType(spellType.sentry_turret, new StatMod<Vector3>(SyncScale.scale, ModScale, this.GetType()))
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamSpellModifier.TeamMods(
            new List<TeamSpellModifier.SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<TeamSpellModifier.SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<TeamSpellModifier.StatModEntry>(), //faction StatMods
            new Dictionary<int, List<TeamSpellModifier.StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    static Vector3 ModScale(Vector3 current, int talentLevel)
    {
        return current * CalcEffectPer(ref talentLevel, incrementSizeAmount);
    }

    static int ModHealth(int current, int talentLevel)
    {
        return (int)(current * CalcEffectPer(ref talentLevel, incrementHealthAmount));
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "increases health by " + CalcDescriptionPer(ref currentLevel, incrementSizeAmount) + ", increases size by " + CalcDescriptionPer(ref currentLevel, incrementSizeAmount);
    }

} //end talent T_EngiIncreaseHealthTurrets



public class T_EngiIncreaseTurretsRange : GridTalentBlack
{
    public override string nameFormatted => "Radical Ballistics";
    public override string description => "increase the range of turrets";
    public override string iconName => nameFormatted;

    const float baseAmount = 0.2f;
    const float amountPerLevel = 0.2f;

    public T_EngiIncreaseTurretsRange() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
            new OnSummonTalentMods.AddSpellMod_SpellType(spellType.sentry_turret, new OnCalcOffensive_BaseTalentMods.IncreaseSpellDuration_Percent(Spell.GetName<S_EngiTurretShot>(), baseAmount, amountPerLevel), this.GetType())
        };
        permanentStatMods = new List<StatMod>() { };
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
        return "increases range by " + CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel);
    }

} //end talent T_EngiIncreaseTurretsRange


public class T_EngiOnTeleporterSpeed : GridTalentBlack
{
    public override string nameFormatted => "Transwarp Field Fusion";
    public override string description => "after using a teleporter, increases the target's speed for duration";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.15f;
    public const float baseAmount = 0.15f;

    public T_EngiOnTeleporterSpeed() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
            new OnSummonTalentMods.AddEntityMod_SpellType(spellType.portal_entrance, new OnTryHitOffensiveChanceMod(this, Spell.GetSpell<S_EngiOnTeleporterSpeed>(), 1, 1, false, Spell.GetID<S_EngiTeleporter_Teleport>()), this.GetType()),
        };
        permanentStatMods = new List<StatMod>() { };
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
        return "speed increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel) + " for " + S_EngiOnTeleporterSpeed._duration;
    }
} //end talent T_EngiOnTeleporterSpeed


public class T_EngiOnTeleporterNextInstantCast : GridTalentBlack
{
    public override string nameFormatted => "Quantum Phase Initiators";
    public override string description => "after using a teleporter, the target's next ability is instant cast";
    public override string iconName => nameFormatted;

    public const int amountPerLevel = 1;
    public const int baseAmount = 1;

    public T_EngiOnTeleporterNextInstantCast() : base(1)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
            new OnSummonTalentMods.AddEntityMod_SpellType(spellType.portal_entrance, new OnTryHitOffensiveChanceMod(this, Spell.GetSpell<S_EngiOnTeleporterNextInstantCast>(), 1, 1, false, Spell.GetID<S_EngiTeleporter_Teleport>()), this.GetType()),
        };
        permanentStatMods = new List<StatMod>() { };
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
        return "next " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " cast is instant";
    }
} //end talent T_EngiOnTeleporterNextInstantCast


public class T_EngiOnTeleporterGainShield : GridTalentBlack
{
    public override string nameFormatted => "Transwarp Graviton Atmosphere";
    public override string description => "after using a teleporter, the target gains a shield";
    public override string iconName => nameFormatted;

    public const int amountPerLevel = 30;
    public const int baseAmount = 40;

    public T_EngiOnTeleporterGainShield() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
            new OnSummonTalentMods.AddEntityMod_SpellType(spellType.portal_entrance, new OnTryHitOffensiveChanceMod(this, Spell.GetSpell<S_EngiOnTeleporterGainShield>(), 1, 1, false, Spell.GetID<S_EngiTeleporter_Teleport>()), this.GetType()),
        };
        permanentStatMods = new List<StatMod>() { };
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
        return "absorbs " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + "  damage and lasts " + S_EngiOnTeleporterGainShield._duration + " seconds";
    }
} //end talent T_EngiOnTeleporterGainShield



public class T_EngiIncreaseBubbleAbsorption : GridTalentBlack
{
    public override string nameFormatted => "Bubble Shield Hardening";
    public override string description => "increases the amount absorbed by " + Spell.GetName<S_EngiBubble>();
    public override string iconName => nameFormatted;

    public const float baseAmount = 20f;
    public const float amountPerLevel = 20f;

    public T_EngiIncreaseBubbleAbsorption() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseSpellPower_NonDamage_Flat(Spell.GetName<S_EngiBubble>(), baseAmount, amountPerLevel)
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
        };
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " additional points of damage";
    }
} //end T_EngiIncreaseBubbleAbsorption


public class T_EngiIncreaseSpeed : GridTalentBlack
{
    public override string nameFormatted => "Amped";
    public override string description => "increased move speed";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.06f;
    public const float amountPerLevel = 0.06f;

    public T_EngiIncreaseSpeed() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() {
            new StatMod<float>(SyncSpeed.maxSpeed, CalcSpeedIncrease, typeof(T_RogueIncreaseSpeed), false)
        };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    float CalcSpeedIncrease(float currentSpeed, int talentLevel)
    {
        //float newSpeed = currentSpeed * CalcEffectPer(ref talentLevel, baseAmount, amountPerLevel);
        return currentSpeed * CalcEffectPer(ref talentLevel, baseAmount, amountPerLevel);
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased speed";
    }
} //end T_EngiIncreaseSpeed


public class T_EngiIncreaseTeamTurretHealth : GridTalentBlack
{
    static readonly string[] turret_entity_names = S_EngiTurretUpgrade.prefabNames.Except(new List<string> { "na" }).ToArray();

    public override string nameFormatted => "Consortium";
    public override string description => "increases max health of all turrets on the team";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.02f;
    public const float increasePerLevel = 0.02f;
    public T_EngiIncreaseTeamTurretHealth() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
        permanentStatMods = new List<StatMod>()
        {
            //change health regen here
        };
        entityMods = new List<EntityMod>()
        {
        };
        //mods applied to entire team

        List<StatModEntry> appliedMods = new List<StatModEntry>() { new StatModEntry(new StatMod<int>(SyncHealth.maxHealth, ModMaxHealth, typeof(T_EngiIncreaseTeamTurretHealth), false), false, this, -1) };

        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>()
            {
            }, //faction StatMods

            //entity type StatMods
            new Dictionary<int, List<StatModEntry>>(turret_entity_names.Select(entry => WorldFunctions.GetEntityTypeID(entry)).ToDictionary(entry => entry, entry => appliedMods)) //create dictionary where entity type id is the key and value is list of mods we want to apply
        );
    }

    int ModMaxHealth(int current, int talentLevel)
    {
        return (int)(current * CalcEffectPer(ref talentLevel, baseAmount, increasePerLevel));
    }


    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " increased max health";
    }

} //end talent T_EngiIncreaseTeamTurretHealth


public class T_EngiEnergyIncrease : GridTalentBlack
{
    public override string nameFormatted => "Powerhouse";
    public override string description => "increases max energy";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 0.1f;

    public T_EngiEnergyIncrease() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>();
        permanentStatMods = new List<StatMod>() { new StatMod<int>(SyncEnergy.maxEnergy, ModMaxEnergy, this.GetType()) };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    static int ModMaxEnergy(int current, int talentLevel)
    {
        return (int)(current * CalcEffectPer(ref talentLevel, amountPerLevel));
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "max energy increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end T_EngiEnergyIncrease


public class T_EngiMirrorImage : GridTalentBlack
{
    public override string nameFormatted => "Mirror Image";
    public override string description => "create a clone of yourself that doesn't attack";
    public override string iconName => nameFormatted;

    public const int amountPerLevel = 30;
    public const int baseAmount = 40;


    public T_EngiMirrorImage() : base(3, Spell.GetName<S_EngiMirrorImage>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() {
            new OnSummonTalentMods.ModStatEntityType(WorldFunctions.GetEntityTypeID(S_EngiMirrorImage.prefabName), new StatMod<int>(SyncHealth.maxHealth, ModHealth, this.GetType()))
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamSpellModifier.TeamMods(
            new List<TeamSpellModifier.SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<TeamSpellModifier.SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<TeamSpellModifier.StatModEntry>(), //faction StatMods
            new Dictionary<int, List<TeamSpellModifier.StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    static int ModHealth(int current, int talentLevel)
    {
        return CalcEffectFlat(ref talentLevel, baseAmount, amountPerLevel);
    }


    public override string GetLevelDescription(int currentLevel)
    {
        return "image has " + CalcDescriptionFlat(ref currentLevel, amountPerLevel) + " health";
    }
} //end talent T_EngiMirrorImage


public class T_EngiOnHitHealAlly : GridTalentBlack
{
    public override string nameFormatted => "Symphony";
    public override string description => "chance on damage to heal a nearby ally";
    public override string iconName => nameFormatted;

    public const float baseAmountChance = 0.1f;
    public const float amountPerLevelChance = 0.1f;

    public const int baseAmount = 10;
    public const int amountPerLevel = 10;

    public T_EngiOnHitHealAlly() : base(5, Spell.GetName<S_EngiOnHitHealAlly>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_EngiOnHitHealAlly>(), baseAmountChance, amountPerLevelChance, true)
        };
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance to heal " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " damage on nearby ally";
    }
} //end T_EngiOnHitHealAlly


public class T_EngiIncreasedHealingOnTurrets : GridTalentBlack
{

    public override string nameFormatted => "Artificer";
    public override string description => "increased healing on turrets";
    public override string iconName => nameFormatted;


    const float increasePerLevel = 0.2f;
    const float baseAmount = 0.2f;

    public T_EngiIncreasedHealingOnTurrets() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseHealingPercentOnTurrets(baseAmount, increasePerLevel)
        };
        permanentStatMods = new List<StatMod>()
        {
        };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " increased healing";
    }

} //end talent T_EngiIncreasedHealingOnTurrets


public class T_EngiGiftHealthPercent : GridTalentBlack
{
    public override string nameFormatted => "Reconstruct";
    public override string description => "heal the target by a percent, and damage yourself by a percent";
    public override string iconName => nameFormatted;

    //public const int baseAmount = 10;
    //public const int amountPerLevel = 8;

    public const float baseAmount = 0.02f;
    public const float amountPerLevel = 0.02f;

    public T_EngiGiftHealthPercent() : base(3, Spell.GetName<S_EngiGiftHealthPercent>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " healed and damaged";
    }
} //end T_EngiGiftHealthPercent


public class T_EngiDrainLife : GridTalentBlack
{
    public override string nameFormatted => "Syphon";
    public override string description => "damage the target by a percent and heal yourself by a percent";
    public override string iconName => nameFormatted;

    //public const int baseAmount = 10;
    //public const int amountPerLevel = 8;

    public const float baseAmount = 0.01f;
    public const float amountPerLevel = 0.01f;

    public T_EngiDrainLife() : base(5, Spell.GetName<S_EngiDrainLife>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " damaged and healed";
    }
} //end T_EngiDrainLife


public class T_EngiRestorePowerOverTime : GridTalentBlack
{
    public override string nameFormatted => "Surge";
    public override string description => Spell.GetSpell<S_EngiRestoreEnergyOverTime>().nameFormatted + " also restores power over time to target";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.1f;
    public const float amountPerLevel = 0.1f;

    public T_EngiRestorePowerOverTime() : base(5, Spell.GetName<S_EngiRestorePowerOverTime>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
        };
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>() {
            new OnTryHitOffensiveChanceMod(this, Spell.GetSpell<S_EngiRestorePowerOverTime>(), 1, 0, false, Spell.GetID<S_EngiRestoreEnergyOverTime>())
        };
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " power every " + S_EngiRestorePowerOverTime._interval + " seconds for " + S_EngiRestorePowerOverTime._duration + " seconds";
    }
} //end T_EngiRestorePowerOverTime



public class T_EngiRestoreEnergyOverTime : GridTalentBlack
{
    public override string nameFormatted => "Galvanize";
    public override string description => "give energy over time to target";
    public override string iconName => nameFormatted;

    public const int baseAmount = 5;
    public const int amountPerLevel = 5;

    public T_EngiRestoreEnergyOverTime() : base(5, Spell.GetName<S_EngiRestoreEnergyOverTime>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " energy every " + S_EngiRestoreEnergyOverTime._interval + " seconds for " + S_EngiRestoreEnergyOverTime._duration + " seconds";
    }
} //end T_EngiRestoreEnergyOverTime


public class T_EngiYoink : GridTalentBlack
{
    public override string nameFormatted => "Tractor Beam";
    public override string description => "yoinks the friendly player target hither";
    public override string iconName => nameFormatted;

    //public const int baseAmount = 10;
    //public const int amountPerLevel = 8;

    public const int baseNumStrikes = 1;
    public const int numStrikesPerLevel = 1;

    public T_EngiYoink() : base(1, Spell.GetName<S_EngiYoink>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescription_SingleUnlockable(ref currentLevel);
    }
} //end T_EngiYoink


public class T_EngiGiveReflectDamage : GridTalentBlack
{
    public override string nameFormatted => "Reflective Barrier";
    public override string description => "give target reflect damage for duration";
    public override string iconName => nameFormatted;

    public const int baseAmount = 4;
    public const int amountPerLevel = 3;

    public T_EngiGiveReflectDamage() : base(3, Spell.GetName<S_EngiGiveReflectDamage>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>();
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " reflect damage for " + S_EngiGiveReflectDamage._duration + " seconds";
    }
} //end T_EngiGiveReflectDamage






public class T_EngiGunnerDrone : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiGunnerDrone() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiGunnerDrone




public class T_EngiSlowDrone : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiSlowDrone() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiSlowDrone



public class T_EngiOnSummonBuffSummoned : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiOnSummonBuffSummoned() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnAddSummonedChanceMod(this, Spell.GetSpell<S_EngiOnSummonBuffSummoned>(), baseAmount, amountPerLevel, false)
        };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiOnSummonBuffSummoned




public class T_EngiOnSacrificeRecreate : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiOnSacrificeRecreate() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            //on cast specific spell chance
            //new On(this, Spell.GetSpell<S_RogueOnHitExtraEnergyConsumption>(), baseAmount, amountPerLevel, true)
        };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiOnSacrificeRecreate


public class T_EngiOnDamagedRobotFrenzy : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiOnDamagedRobotFrenzy() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageDefensiveChanceMod(this, Spell.GetSpell<S_EngiOnDamagedRobotFrenzy>(), baseAmount, amountPerLevel, true)
        };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiOnDamagedRobotFrenzy




public class T_EngiOnUseChargeHealAllRobots : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiOnUseChargeHealAllRobots() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            //on use charge chance mod..
            //new OnHitOffensiveChanceMod(this, Spell.GetSpell<S_RogueOnHitExtraEnergyConsumption>(), baseAmount, amountPerLevel, true)
        };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiOnUseChargeHealAllRobots




public class T_EngiOnCritRegenEnergy : GridTalentBlack
{
    public override string nameFormatted => "Unused";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiOnCritRegenEnergy() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnCritOffensiveChanceMod(this, Spell.GetSpell<S_EngiOnCritRegenEnergy>(), baseAmount, amountPerLevel, true)
        };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiOnCritRegenEnergy




public class T_EngiOnHitBurnDamage : GridTalentBlack
{
    public override string nameFormatted => "Unused";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiOnHitBurnDamage() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_EngiOnHitBurnDamage>(), baseAmount, amountPerLevel, true)
        };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiOnHitBurnDamage




public class T_EngiOnCleanseShield : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiOnCleanseShield() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnDispelStatusEffectOffensive_ChanceMod(this, Spell.GetSpell<S_EngiOnCleanseShield>(), baseAmount, amountPerLevel, false)
        };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiOnCleanseShield




public class T_EngiOnHealShield : GridTalentBlack
{
    public override string nameFormatted => "undecided";
    public override string description => "chance on heal to place a shield on the target";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiOnHealShield() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            //make sure it checks if it is a heal first..
            new OnHitHealOffensiveChanceMod(this, Spell.GetSpell<S_EngiOnHealShield>(), baseAmount, amountPerLevel, true)
        };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiOnHealShield


public class T_EngiDelayedHeal : GridTalentBlack
{
    public override string nameFormatted => "Nano Egg";
    public override string description => "heal target after delay";
    public override string iconName => nameFormatted;

    public const int amountPerLevel = 40;
    public const int baseAmount = 40;

    public T_EngiDelayedHeal() : base(3, Spell.GetName<S_EngiDelayedHeal>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
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
        return CalcDescriptionFlat(ref currentLevel, amountPerLevel) + " healed after " + S_EngiDelayedHeal._duration + " seconds";
    }
} //end talent T_EngiDelayedHeal



public class T_EngiDelayedBomb : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.1f;
    public const float baseAmount = 0.1f;

    public T_EngiDelayedBomb() : base(1, Spell.GetName<S_EngiDelayedBomb>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiDelayedBomb



public class T_EngiUniqueRobotBuff : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 0.1f;
    const float baseAmount = 0.1f;

    public T_EngiUniqueRobotBuff() : base(1, Spell.GetName<S_EngiUniqueRobotBuff>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiUniqueRobotBuff



public class T_EngiRobotBuffFromDistance : GridTalentBlack
{
    public override string nameFormatted => "Minefield";
    public override string description => "deploy mines that explode on contact with enemies";
    public override string iconName => nameFormatted;

    const float amountPerLevel = 0.1f;
    const float baseAmount = 0.1f;

    public T_EngiRobotBuffFromDistance() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamSpellModifier.TeamMods teamMods)
    {

        spellMods = new List<TalentMod>()
        {
            //mod that increases damage and cast speed or w/e for summoned
            //new OnCalcOffensive_BaseTalentMods.
        };
        permanentStatMods = new List<StatMod>() { };
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
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end talent T_EngiRobotBuffFromDistance

