using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using static TeamSpellModifier;

public class T_WarriorBeef : GridTalentBlack
{
    public override string nameFormatted => "Beef";
    public override string description => "more helth";
    public override string iconName => nameFormatted;

    const float increaseAmount = 0.07f;

    public T_WarriorBeef() : base(4, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>();
        permanentStatMods = new List<StatMod>() { new StatMod<int>(SyncHealth.maxHealth, ModMaxHealth, typeof(T_WarriorBeef)) };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    static int ModMaxHealth(int current, int talentLevel)
    {
        return (int)(current * CalcEffectPer(ref talentLevel, increaseAmount));
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "max health increased by " + CalcDescriptionPer(ref currentLevel, increaseAmount);
    }
} //end T_WarriorBeef


public class T_WarriorBarrier : GridTalentBlack
{
    public override string nameFormatted => "Barrier";
    public override string description => "slow moving barrier that can be modified by friends and enemies";
    public override string iconName => nameFormatted;

    const float incrementAmount = 0.1f;

    public T_WarriorBarrier() : base(1, Spell.GetName<S_WarriorBarrier>())
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
        return "increased damage by " + ((currentLevel * (incrementAmount - 1)) * 100).ToString("0") + "%";
    }
} //end class T_WarriorBarrier




public class T_WarriorNutrition : GridTalentBlack
{
    public override string nameFormatted => "Nutrition";
    public override string description => "increases your size";
    public override string iconName => nameFormatted;

    const float incrementAmount = 0.4f;

    public T_WarriorNutrition() : base(4, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>();
        permanentStatMods = new List<StatMod>() { new StatMod<Vector3>(SyncScale.scale, ModScale, typeof(T_WarriorNutrition)) };
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    static Vector3 ModScale(Vector3 current, int talentLevel)
    {
        return current * CalcEffectPer(ref talentLevel, incrementAmount);
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "increased size by " + CalcDescriptionPer(ref currentLevel, incrementAmount);
    }
} //end class T_WarriorNutrition



public class T_RichDiet : GridTalentBlack
{
    public override string nameFormatted => "Rich Diet";
    public override string description => "increases your size and health, but decreases your speed";

    const float incrementSizeAmount = 0.35f;
    const float speedDebuff = 0.4f;
    const float incrementHealthAmount = 0.25f;
    public override string iconName => nameFormatted;

    public T_RichDiet() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>();
        permanentStatMods = new List<StatMod>() {
            new StatMod<Vector3>(SyncScale.scale, ModScale, typeof(T_RichDiet)),
            new StatMod<int>(SyncHealth.maxHealth, ModHealth, typeof(T_RichDiet)),
            new StatMod<float>(SyncSpeed.maxSpeed, ModSpeed, typeof(T_RichDiet)),
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

    static Vector3 ModScale(Vector3 current, int talentLevel)
    {
        return current * CalcEffectPer(ref talentLevel, incrementSizeAmount);
    }
    static float ModSpeed(float current, int talentLevel)
    {
        return current / CalcEffectPer(ref talentLevel, speedDebuff);
    }
    static int ModHealth(int current, int talentLevel)
    {
        return (int)(current * CalcEffectPer(ref talentLevel, incrementHealthAmount));
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "increases size by " + CalcDescriptionPer(ref currentLevel, incrementSizeAmount) + ", increases health by " + CalcDescriptionPer(ref currentLevel, incrementSizeAmount) + ", decreases speed by " + CalcDescriptionPer(ref currentLevel, speedDebuff);
    }
} //end class T_RichDiet



public class T_WarriorMoon : GridTalentBlack
{
    public override string nameFormatted => "Non-gravitational Moon";
    public override string description => "That's a moon";
    public override string iconName => nameFormatted;

    const float sizeIncrement = 0.3f;
    const float healthIncrement = 0.2f;

    public const string moonPrefabName = "WarriorMoon";

    public T_WarriorMoon() : base(8, Spell.GetName<S_WarriorMoon>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnSummonTalentMods.ModStatEntityType(WorldFunctions.GetEntityTypeID(moonPrefabName), new StatMod<Vector3>(SyncScale.scale, ModScale, this.GetType())), //increase moon's size per point based on function
            new OnSummonTalentMods.ModStatEntityType(WorldFunctions.GetEntityTypeID(moonPrefabName), new StatMod<int>(SyncHealth.health, ModHealth, this.GetType())) //increase moon's health per point based on function
        };
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

    static Vector3 ModScale(Vector3 current, int talentLevel)
    {
        return current * CalcEffectPer(ref talentLevel, sizeIncrement); //do talentLevel - 1 because first talent doesn't increase their stats
    }

    static int ModHealth(int current, int talentLevel)
    {
        return (int)(current * CalcEffectPer(ref talentLevel, healthIncrement));
    }


    public override string GetLevelDescription(int currentLevel)
    {
        return "increased size by " + CalcDescriptionPer(ref currentLevel, sizeIncrement) + " and health by " + CalcDescriptionPer(ref currentLevel, healthIncrement);
    }
} //end talent T_WarriorMoon


public class T_WarriorLunarCataclysm : GridTalentBlack {

    public override string nameFormatted => "Lunar Cataclysm";
    public override string description => "causes your moon to explode into shards when destroyed";
    public override string iconName => nameFormatted;

    const string moonPrefabName = T_WarriorMoon.moonPrefabName;
    const int shardsCount = 5;

    public T_WarriorLunarCataclysm() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnSummonTalentMods.AddSpellMod_EntityType(WorldFunctions.GetEntityTypeID(moonPrefabName), new TriggerSpell_TalentMods.OnDeath_TryCastOnSelf(Spell.GetID<S_WarriorMoonShatter>()), this.GetType() )
        };
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
        return shardsCount + " shards";
    }

} //end talent T_WarriorLunarCataclysm


public class T_WarriorNeverBackDown : GridTalentBlack
{

    public override string nameFormatted => "Never Back Down";
    public override string description => "prevents moving backwards, but your Chad aura significantly buffs nearby allies";
    public override string iconName => nameFormatted;

    //TODO- decide what the spell that is applied should actually do

    public const float baseAmount = 0.15f;
    public const float amountPerLevel = 0.15f;

    private Spell effectSpell; //what gets casted to create the actual status effect on talent owner

    public T_WarriorNeverBackDown() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        effectSpell = Spell.GetSpell<S_WarriorNeverBackDown>();

        spellMods = new List<TalentMod>() {
        };
        permanentStatMods = new List<StatMod>()
        {
            new StatMod<float>(SyncSpeed.moveBackwardsModifier, CalcSpeedEffect, typeof(T_WarriorNeverBackDown), false)
        };
        entityMods = new List<EntityMod>()
        {
            new CastOnSelfEntityMod(effectSpell, (entity)=> entity.RemoveAllStatusEffectsOfType_Raw_CastedBySelf(effectSpell.id)) //when reverting talent, this will remove all instances of the otherwise permantent StatusEffect that represents the aura
        };
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    static float CalcSpeedEffect(float current, int talentLevel)
    {
        return 0; //set move backward speed to zero no matter what
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased damage to nearby allies";
    }

} //end talent T_WarriorNeverBackDown




public class T_WarriorRedBadge : GridTalentBlack
{

    public override string nameFormatted => "Red Badge";
    public override string description => "increases max health for duration";
    public override string iconName => nameFormatted;

    public const float healthPercent = 0.4f;
    public const float amountPerlevel = 0;

    public T_WarriorRedBadge() : base(1, Spell.GetName<S_WarriorRedBadge>())
    {
        
    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
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
        return CalcDescriptionPer(ref currentLevel, healthPercent, amountPerlevel) + " increased health for " + S_WarriorRedBadge._duration;
    }

} //end talent T_WarriorRedBadge




public class T_WarriorTopGun : GridTalentBlack
{

    public override string nameFormatted => "Top Gun";
    public override string description => "increases dodge chance";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.04f;
    public const float amountPerLevel = 0.04f;

    public T_WarriorTopGun() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcDefensive_BaseTalentMods.IncreaseDodgeChance_Flat(baseAmount, amountPerLevel)
        };
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " additional chance to dodge";
    }

} //end talent T_WarriorTopGun



public class T_WarriorHeavyPlating : GridTalentBlack
{

    public override string nameFormatted => "Heavy Plating";
    public override string description => "reduces incoming damage, but reduces your speed";
    public override string iconName => nameFormatted;

    const float baseDamageReduction = 0.08f;
    const float damageReductionPerLevel = 0.08f;

    const float amountPerLevelSpeedReduction = 0.1f;
    const float baseSpeedReduction = 0.1f;

    public T_WarriorHeavyPlating() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcDefensive_BaseTalentMods.DecreaseDamage_Percent(baseDamageReduction, damageReductionPerLevel)
        };
        permanentStatMods = new List<StatMod>()
        {
            new StatMod<float>(SyncSpeed.maxSpeed, ModSpeed, typeof(T_WarriorHeavyPlating))
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

    static float ModSpeed(float current, int talentLevel)
    {
        return current / CalcEffectPer(ref talentLevel, baseSpeedReduction, amountPerLevelSpeedReduction);
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, damageReductionPerLevel) + " reduced damage, " + CalcDescriptionPer(ref currentLevel, baseSpeedReduction, amountPerLevelSpeedReduction) + " reduced speed";
    }

} //end talent T_WarriorHeavyPlating



public class T_WarriorReflectivePlating : GridTalentBlack
{

    public override string nameFormatted => "Reflective Plating";
    public override string description => "increases reflect damage";
    public override string iconName => nameFormatted;


    const int reflectPerLevel = 2;

    public T_WarriorReflectivePlating() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcDefensive_BaseTalentMods.IncreaseReflectedDamageFlat(reflectPerLevel)
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
        return CalcDescriptionFlat(ref currentLevel, reflectPerLevel) + " reflect damage";
    }

} //end talent T_WarriorReflectivePlating



public class T_WarriorDodgeChanceAgainstRanged : GridTalentBlack
{

    public override string nameFormatted => "Vigilance";
    public override string description => "increases dodge chance against long ranged attacks";
    public override string iconName => nameFormatted;


    const int minRange = 35;
    const float amountPerLevel = 0.2f;
    const float baseAmount = 0.2f;
    public T_WarriorDodgeChanceAgainstRanged() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcDefensive_BaseTalentMods.IncreaseDodgeChanceFromRanged(baseAmount, amountPerLevel, minRange)
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " chance from abilites with range >= " + minRange + "m";
    }

} //end talent T_WarriorDodgeChanceAgainstRanged



public class T_WarriorHeavyHanded : GridTalentBlack
{

    public override string nameFormatted => "Heavy Handed";
    public override string description => "chance to immobilize targets on hit";
    public override string iconName => nameFormatted;

    const float baseAmount = 0.02f;
    const float amountPerLevel = 0.02f;
    public T_WarriorHeavyHanded() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
        permanentStatMods = new List<StatMod>()
        {
        };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_WarriorHeavyHanded>(), baseAmount, amountPerLevel, false)
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " chance to immobilize ";
    }

} //end talent T_WarriorHeavyHanded


public class T_WarriorIncreasedDamagevsImmobilized : GridTalentBlack
{

    public override string nameFormatted => "Draconian Measures";
    public override string description => "increased damage vs immobilized targets";
    public override string iconName => nameFormatted;


    const float increasePerLevel = 0.1f;
    public T_WarriorIncreasedDamagevsImmobilized() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseDamage_Percent_VsImmobilized(increasePerLevel, increasePerLevel)
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
        return CalcDescriptionPer(ref currentLevel, increasePerLevel) + " increased damage";
    }

} //end talent T_WarriorIncreasedDamagevsImmobilized



public class T_WarriorCollateralMending : GridTalentBlack
{

    public override string nameFormatted => "Collateral Mending";
    public override string description => "heals a percentage of your health when you remove a debuff";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.04f;
    public const float increasePerLevel = 0.03f;
    public T_WarriorCollateralMending() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new TriggerSpell_TalentMods.OnDispelStatusEffectOffensive_CastOnSelf(Spell.GetID<S_WarriorCollateralMendingEffect>())
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " restored";
    }

} //end talent T_WarriorCollateralMending




public class T_WarriorCripplingGaze : GridTalentBlack
{

    public override string nameFormatted => "Crippling Gaze";
    public override string description => "immobilizes the target for duration";
    public override string iconName => nameFormatted;

    //public const float baseAmount = 2f;
   // public const float increasePerLevel = 0.00f;
    public T_WarriorCripplingGaze() : base(1, Spell.GetName<S_WarriorCripplingGaze>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
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
        return CalcDescription_SingleUnlockable(ref currentLevel);
    }

} //end talent T_WarriorCripplingGaze



public class T_WarriorMoonShatter : GridTalentBlack
{

    public override string nameFormatted => "unused";
    public override string description => ""; //moon shatters when destroyed
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.08f;
    public const float increasePerLevel = 0.02f;
    public T_WarriorMoonShatter() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " restored";
    }

} //end talent T_WarriorMoonShatter



//dodge next ability?
public class T_WarriorInstincts : GridTalentBlack
{

    public override string nameFormatted => "Instincts";
    public override string description => "dodge the next ability cast on you";
    public override string iconName => nameFormatted;

    public const float baseAmount = 1f;
    public const float amountPerLevel = 0.5f;
    public T_WarriorInstincts() : base(3, Spell.GetName<S_WarriorInstincts>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
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
        return "lasts " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }

} //end talent T_WarriorInstincts



public class T_WarriorChallengingAura : GridTalentBlack
{

    public override string nameFormatted => "Challenging Aura";
    public override string description => "redirects a percentage of damage in radius to you";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.08f;
    public const float increasePerLevel = 0.04f;
    public T_WarriorChallengingAura() : base(3, Spell.GetName<S_WarriorChallengingAura>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " percent reidirected to you";
    }

} //end talent T_WarriorChallengingAura


public class T_WarriorTaunt : GridTalentBlack
{

    public override string nameFormatted => "Taunt";
    public override string description => "forces all enemies in area to target you";
    public override string iconName => nameFormatted;

    //public const float baseAmount = 0.08f;
    //public const float increasePerLevel = 0.02f;

    public T_WarriorTaunt() : base(1, Spell.GetName<S_WarriorTaunt>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
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
        return CalcDescription_SingleUnlockable(ref currentLevel);
        //return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " restored";
    }

} //end talent T_WarriorTaunt


public class T_WarriorDecreaseTauntCooldown : GridTalentBlack
{

    public override string nameFormatted => "Bravery";
    public override string description => "reduces the cooldown of taunt";
    public override string iconName => nameFormatted;

    public const float baseAmount = 2f;
    public const float increasePerLevel = 2f;
    public T_WarriorDecreaseTauntCooldown() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.DecreaseCooldownSpell_Flat(Spell.GetName<S_WarriorTaunt>(), baseAmount, increasePerLevel)
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
        return "reduced by " + CalcDescriptionFlat(ref currentLevel, baseAmount, increasePerLevel) + " seconds";
    }

} //end talent T_WarriorDecreaseTauntCooldown


public class T_WarriorIncreaseTauntRadius : GridTalentBlack
{

    public override string nameFormatted => "Stout-Hearted";
    public override string description => "increases the radius of taunt";
    public override string iconName => nameFormatted;

    public const float baseAmount = 5f;
    public const float increasePerLevel = 5f;
    public T_WarriorIncreaseTauntRadius() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseRadiusSpell_Flat(Spell.GetName<S_WarriorTaunt>(), baseAmount, increasePerLevel)
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
        return "increased by " + CalcDescriptionFlat(ref currentLevel, baseAmount, increasePerLevel) + " m";
    }

} //end talent T_WarriorIncreaseTauntRadius



public class T_WarriorIncreaseTauntRadiusAtCostHealth : GridTalentBlack
{

    public override string nameFormatted => "Ballsy";
    public override string description => "dramatically increases the radius of taunt, but at the cost of health per use";
    public override string iconName => nameFormatted;

    public const float radiusIncrease = 25f;
    public const float costPercentage = 0.2f;
    //public const float increasePerLevel = 5f;
    public T_WarriorIncreaseTauntRadiusAtCostHealth() : base(1, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseRadiusSpell_Flat(Spell.GetName<S_WarriorTaunt>(), radiusIncrease, 0),
            new TriggerSpell_TalentMods.OnTryHitOffensive_CastSpellOnSelf(Spell.GetID<S_WarriorIncreaseTauntRadiusAtCostHealth>(), Spell.GetID<S_WarriorTaunt>())
        };
        permanentStatMods = new List<StatMod>()
        {
        };
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
        return "increased by " + CalcDescriptionFlat(ref currentLevel, radiusIncrease) + " m, costs " + CalcDescriptionPer(ref currentLevel, costPercentage);
    }

} //end talent T_WarriorIncreaseTauntRadiusAtCostHealth


public class T_WarriorShed : GridTalentBlack
{

    public override string nameFormatted => "Shed";
    public override string description => "remove a debuff from yourself";
    public override string iconName => nameFormatted;

    //public const float baseAmount = 0.08f;
    //public const float increasePerLevel = 0.02f;
    public T_WarriorShed() : base(1, Spell.GetName<S_WarriorShed>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
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
        return CalcDescription_SingleUnlockable(ref currentLevel);
    }

} //end talent T_WarriorShed


public class T_WarriorOnDamagedAddShield : GridTalentBlack
{

    public override string nameFormatted => "Aegis";
    public override string description => "Chance on damaged to add a shield";
    public override string iconName => nameFormatted;

    public const float baseAmountChance = 0.05f;
    public const float amountPerLevelChance = 0.05f;

    public const int baseAmount = 20;
    public const int amountPerLevel = 20;

    public T_WarriorOnDamagedAddShield() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
        permanentStatMods = new List<StatMod>()
        {
        };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageDefensiveChanceMod(this, Spell.GetSpell<S_WarriorOnDamagedAddShield>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + "chance to shield for "+ CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " points for " + S_WarriorOnDamagedAddShield._duration + " seconds";
    }

} //end talent T_WarriorOnDamagedAddShield


public class T_WarriorOnDodgeGainMoveSpeed : GridTalentBlack
{

    public override string nameFormatted => "Storm";
    public override string description => "chance on dodge to gain move speed";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.2f;
    public const float amountPerLevel = 0.2f;

    public const float baseAmountChance = 0.1f;
    public const float amountPerLevelChance = 0.1f;

    public const int baseMaxStacks = 1;
    public const int maxStacksPerLevel = 1;

    public T_WarriorOnDodgeGainMoveSpeed() : base(5, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
        };
        permanentStatMods = new List<StatMod>()
        {
        };
        entityMods = new List<EntityMod>()
        {
            new OnDodgeDefensiveChanceMod(this, Spell.GetSpell<S_WarriorOnDodgeGainMoveSpeed>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance to increase move speed by " + CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + ", stacks up to " + CalcDescriptionFlat(ref currentLevel, baseMaxStacks, amountPerLevel) + " times";
    }

} //end talent T_WarriorOnDodgeGainMoveSpeed



public class T_WarriorIncreaseHealthRegen : GridTalentBlack
{

    public override string nameFormatted => "empty";
    public override string description => "increased health regen";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.08f;
    public const float increasePerLevel = 0.02f;
    public T_WarriorIncreaseHealthRegen() : base(3, null)
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
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " restored";
    }

} //end talent T_WarriorIncreaseHealthRegen



public class T_WarriorIncreaseTeamHealth : GridTalentBlack
{

    public override string nameFormatted => "Esprit de Corps";
    public override string description => "increased max health for entire team";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.02f;
    public const float increasePerLevel = 0.02f;
    public T_WarriorIncreaseTeamHealth() : base(3, null)
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
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>()
            {
                new StatModEntry(new StatMod<int>(SyncHealth.maxHealth, ModMaxHealth, typeof(T_WarriorIncreaseTeamHealth), false), false, this, -1)
            }, //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    int ModMaxHealth(int current, int talentLevel)
    {
        return (int)(current * CalcEffectPer(ref talentLevel, baseAmount, increasePerLevel));
    }


    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " increase";
    }

} //end talent T_WarriorIncreaseTeamHealth


public class T_WarriorIncreaseTeamSpeed : GridTalentBlack
{

    public override string nameFormatted => "Footslog";
    public override string description => "increase speed for entire team";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.02f;
    public const float increasePerLevel = 0.02f;
    public T_WarriorIncreaseTeamSpeed() : base(3, null)
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
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>()
            {
                new StatModEntry(new StatMod<float>(SyncSpeed.maxSpeed, ModSpeed, typeof(T_WarriorIncreaseTeamSpeed), false), false, this, -1)
            }, //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    float ModSpeed(float current, int talentLevel)
    {
        return (current * CalcEffectPer(ref talentLevel, baseAmount, increasePerLevel));
    }


    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " increased speed";
    }

} //end talent T_WarriorIncreaseTeamSpeed

public class T_WarriorIncreaseTeamExp : GridTalentBlack
{

    public override string nameFormatted => "Amity";
    public override string description => "increase experience gain for entire team";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.02f;
    public const float increasePerLevel = 0.02f;
    public T_WarriorIncreaseTeamExp() : base(3, null)
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
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>()
            {
                new StatModEntry(new StatMod<float>(SyncLevel.expBonus, ModExp, typeof(T_WarriorIncreaseTeamExp), false), false, this, -1)
            }, //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    float ModExp(float current, int talentLevel)
    {
        //would add it flat, because we're actually adding to the expBonus modifer which itself gets multiplied to exp
        //if you multiply it, that would be an exponential effect
        return (current + CalcEffectFlat(ref talentLevel, baseAmount, increasePerLevel));
    }


    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " increased experience gain";
    }

} //end talent T_WarriorIncreaseTeamExp


public class T_WarriorIncreaseTeamWarriorHealth : GridTalentBlack
{
    const string warrior_enitity_name = "Player_Vanguard Variant";

    public override string nameFormatted => "Tovarich";
    public override string description => "increases max health of all warriors on the team";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.02f;
    public const float increasePerLevel = 0.02f;
    public T_WarriorIncreaseTeamWarriorHealth() : base(3, null)
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
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>()
            {
            }, //faction StatMods
            new Dictionary<int, List<StatModEntry>>(
                /*
                new List<KeyValuePair<int, List<StatModEntry>>>() {
                    new KeyValuePair<int, List<StatModEntry>>(
                        //warrier_entity_name has be a creatable Entity or will throw an error
                        WorldFunctions.GetEntityTypeID(warrior_enitity_name), new List<StatModEntry>() { new StatModEntry(new StatMod<int>(SyncHealth.maxHealth, ModMaxHealth, typeof(T_WarriorIncreaseTeamWarriorHealth), false), false, this, -1) }
                    ) //faction StatMods for entity typeID
                } */
            ) //faction StatMods for entity typeID
            {  {
                WorldFunctions.GetEntityTypeID(warrior_enitity_name), new List<StatModEntry>() { new StatModEntry(new StatMod<int>(SyncHealth.maxHealth, ModMaxHealth, typeof(T_WarriorIncreaseTeamWarriorHealth), false), false, this, -1) }
            } }
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

} //end talent T_WarriorIncreaseTeamWarriorHealth


public class T_WarriorIncreaseTeamEnergy : GridTalentBlack
{

    public override string nameFormatted => "Mob Squad";
    public override string description => "increased max energy for entire team";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.02f;
    public const float increasePerLevel = 0.02f;
    public T_WarriorIncreaseTeamEnergy() : base(3, null)
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
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>()
            {
                new StatModEntry(new StatMod<int>(SyncEnergy.maxEnergy, ModMaxEnergy, typeof(T_WarriorIncreaseTeamEnergy), false), false, this, -1)
            }, //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    int ModMaxEnergy(int current, int talentLevel)
    {
        return (int)(current * CalcEffectPer(ref talentLevel, baseAmount, increasePerLevel));
    }


    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, increasePerLevel) + " increase";
    }

} //end talent T_WarriorIncreaseTeamEnergy

public class T_WarriorBuffFriendliesBehind : GridTalentBlack
{

    public override string nameFormatted => "Commanding Presence";

    public override string description => "increases damage of friendly targets behind you";
    public override string iconName => nameFormatted;

    Spell auraInitalCast; //what gets casted to apply the initial StatusEffect to talent owner

    public const float baseAmount = 0.04f;
    public const float amountPerLevel = 0.03f;
    public T_WarriorBuffFriendliesBehind() : base(3, null)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        auraInitalCast = Spell.GetSpell<S_WarriorBuffFriendliesBehind>();

        spellMods = new List<TalentMod>()
        {
            //new OnCalcDefensive_BaseTalentMods.IncreaseDodgeChanceFromRanged(increasePerLevel, minRange)
        };
        permanentStatMods = new List<StatMod>()
        {
        };
        entityMods = new List<EntityMod>()
        {
            new CastOnSelfEntityMod(auraInitalCast, (owner)=> { owner.RemoveAllStatusEffectsOfType_Raw_CastedBySelf(auraInitalCast.id); })
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased damage";
    }

} //end talent T_WarriorBuffFriendliesBehind



public class T_WarriorIncreaseOnDamagedShield : GridTalentBlack
{
    public override string nameFormatted => "Advanced Defensive Capacitance";
    public override string description => "increases the amount absorbed by Aegis";
    public override string iconName => nameFormatted;

    public const float baseAmount = 20f;
    public const float amountPerLevel = 20f;

    public T_WarriorIncreaseOnDamagedShield() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseSpellPower_NonDamage_Flat(Spell.GetName<S_WarriorOnDamagedAddShield>(), baseAmount, amountPerLevel)
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
} //end T_WarriorIncreaseOnDamagedShield



public class T_WarriorOnDamagedGenerateCharge : GridTalentBlack
{
    public override string nameFormatted => "Nuclear Alchemization";
    public override string description => "chance on damaged to generate charge";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.5f;
    public const float amountPerLevel = 0.5f;

    public const float baseAmountChance = 0.02f;
    public const float amountPerLevelChance = 0.02f;

    public T_WarriorOnDamagedGenerateCharge() : base(4, Spell.GetName<S_WarriorOnDamagedGenerateChargeEffect>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_WarriorOnDamagedGenerateChargeEffect>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance on damaged to generate " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " charge";
    }
} //end T_WarriorOnDamagedGenerateCharge



public class T_WarriorOnUseChargeGainMoveSpeed : GridTalentBlack
{
    public override string nameFormatted => "Nuclear Overdrive";
    public override string description => "chance when using charge to gain move speed";
    public override string iconName => nameFormatted;

    public const float baseAmountChance = 0.1f;
    public const float amountPerLevelChance = 0.1f;

    public const float baseAmount = 0.2f;
    public const float amountPerLevel = 0.2f;

    public T_WarriorOnUseChargeGainMoveSpeed() : base(3, Spell.GetName<S_WarriorOnUseChargeGainMoveSpeed>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {

        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnUseChargeChanceMod(this, Spell.GetSpell<S_WarriorOnUseChargeGainMoveSpeed>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance on damaged to gain " + CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased speed";
    }
} //end T_WarriorOnUseChargeGainMoveSpeed



public class T_WarriorCoverSpellIncreasesDamage : GridTalentBlack
{
    public override string nameFormatted => "Buddy System";
    public override string description => "Cover also increases damage of target";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.04f;
    public const float amountPerLevel = 0.03f;

    public T_WarriorCoverSpellIncreasesDamage() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new TriggerSpell_TalentMods.OnTryHitOffensive_CastSpellOnTarget(Spell.GetID<S_WarriorCoverSpellIncreaseDamageEffect>(), Spell.GetID<S_WarriorCover>())
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
        return "increases damage by " + CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel);
    }
} //end T_WarriorCoverSpellIncreasesDamage



public class T_WarriorIncreaseShotgunDistance : GridTalentBlack
{
    //Gilt-Edge Armanents
    public override string nameFormatted => "Long Arm of the Law";
    public override string description => "increases the range of Shotgun";
    public override string iconName => nameFormatted;

    public const float baseAmount = 2f;
    public const float amountPerLevel = 2f;

    public T_WarriorIncreaseShotgunDistance() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseRangeSpell_Flat(Spell.GetName<S_WarriorCrippleShotgun>(), baseAmount, amountPerLevel)
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
        return "increased by " +  CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " m";
    }
} //end T_WarriorIncreaseShotgunDistance



public class T_WarriorShotgunLeechEffect : GridTalentBlack
{
    public override string nameFormatted => "Shark Mana";
    public override string description => "heals you for a percent of Shotgun ability's damage";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.1f;
    public const float amountPerLevel = 0.1f;

    public T_WarriorShotgunLeechEffect() : base(4)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseLeechPercentSpellFlat(Spell.GetName<S_WarriorCrippleShotgun>(), baseAmount, amountPerLevel)
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " percent of damage";
    }
} //end T_WarriorShotgunLeechEffect



public class T_WarriorYoinkDelayedAoE : GridTalentBlack
{
    public override string nameFormatted => "";
    public override string description => "";
    public override string iconName => nameFormatted;

    public const float baseAmount = 1f;
    public const float amountPerLevel = 2f;

    public T_WarriorYoinkDelayedAoE() : base(3, Spell.GetName<S_WarriorYoinkDelayedAoEEffect>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            //chance on use "Yoink" spell.. 100% chance
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_WarriorYoinkDelayedAoE



public class T_WarriorIncreaseYoinkRange : GridTalentBlack
{
    public override string nameFormatted => "";
    public override string description => "";
    public override string iconName => nameFormatted;

    public const float baseAmount = 1f;
    public const float amountPerLevel = 2f;

    public T_WarriorIncreaseYoinkRange() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseRangeSpell_Flat(Spell.GetName<S_WarriorYoink>(), baseAmount, amountPerLevel)
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_WarriorIncreaseYoinkRange



public class T_WarriorShotgunMortalStrikeEffect : GridTalentBlack
{
    public override string nameFormatted => "Sunder Vellum";
    public override string description => "Shotgun reduces healing target receives";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.2f;
    public const float amountPerLevel = 0.15f;

    public T_WarriorShotgunMortalStrikeEffect() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_WarriorShotgunMortalStrikeEffect>(), 1, 1, false, Spell.GetID<S_WarriorCrippleShotgun>())
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " reduced healing received";
    }
} //end T_WarriorShotgunMortalStrikeEffect



public class T_WarriorOnHitChanceGiveChargeAlly : GridTalentBlack
{
    public override string nameFormatted => "Inspiration";
    public override string description => "chance on hit to give charge to a nearby ally";
    public override string iconName => nameFormatted;

    public const float baseAmountChance = 0.05f;
    public const float amountPerLevelChance = 0;
    
    public const float baseAmount = 0.5f;
    public const float amountPerLevel = 0.5f;

    public T_WarriorOnHitChanceGiveChargeAlly() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_WarriorOnHitChanceGiveChargeAlly>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance to give " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " charge to nearby ally";
    }
} //end T_WarriorOnHitChanceGiveChargeAlly



public class T_WarriorIncreaseCoverTargets : GridTalentBlack
{
    public override string nameFormatted => "";
    public override string description => "";
    public override string iconName => nameFormatted;

    public const float baseAmount = 1f;
    public const float amountPerLevel = 2f;


    //to implement-
    //probably need to implement having multiple charges available for cast like engi RoR mines, then increase # available
    //only show the # in GUI if there is > 1 max
    //or just reduce the cooldown to the extent that you can cast on multiple targets

    public T_WarriorIncreaseCoverTargets() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {

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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_WarriorIncreaseCoverTargets



public class T_WarriorYoinkGiveCharge : GridTalentBlack
{
    public override string nameFormatted => "Revolutionary Yoink";
    public override string description => "groundbreaking Yoink technology generates charge on use";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.4f;
    public const float amountPerLevel = 0.3f;

    public T_WarriorYoinkGiveCharge() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseChargeGainedSpell_Flat(Spell.GetName<S_WarriorYoink>(), baseAmount, amountPerLevel)
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " charge gained";
    }
} //end T_WarriorYoinkGiveCharge


public class T_WarriorReduceYoinkCooldown : GridTalentBlack
{
    public override string nameFormatted => "Volatile Yoink Distillation";
    public override string description => "experimental tincture added to physics chamber reduces cooldown of Yoink ability";
    public override string iconName => nameFormatted;

    public const float baseAmount = 1f;
    public const float amountPerLevel = 2f;

    public T_WarriorReduceYoinkCooldown() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.DecreaseCooldownSpell_Flat(Spell.GetName<S_WarriorYoink>(), baseAmount, amountPerLevel)
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
        return "reduced by " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_WarriorReduceYoinkCooldown



public class T_WarriorGiftChargeAlly : GridTalentBlack
{
    public override string nameFormatted => "Delegate";
    public override string description => "gift charge to an ally";
    public override string iconName => nameFormatted;


    public const float baseAmount = 1f;
    public const float amountPerLevel = 1f;

    public T_WarriorGiftChargeAlly() : base(3, Spell.GetName<S_WarriorGiftChargeAlly>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {

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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " charge given";
    }
} //end T_WarriorGiftChargeAlly




public class T_WarriorUseChargeShieldSelf : GridTalentBlack
{
    public override string nameFormatted => "Mantle";
    public override string description => "creates a shield on yourself when you use charge";
    public override string iconName => nameFormatted;

    public const float baseAmount = 15f;
    public const float amountPerLevel = 15f;

    public T_WarriorUseChargeShieldSelf() : base(3)
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
            new OnUseChargeChanceMod(this, Spell.GetSpell<S_WarriorUseChargeShieldSelf>(), 1, 0, true)
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " damage absorbed";
    }
} //end T_WarriorUseChargeShieldSelf



public class T_WarriorUseChargeShieldOthers : GridTalentBlack
{
    public override string nameFormatted => "Warden";
    public override string description => "consume charge to put a shield on a friendly target";
    public override string iconName => nameFormatted;

    public const float baseAmount = 10f;
    public const float amountPerLevel = 10f;

    public T_WarriorUseChargeShieldOthers() : base(3, Spell.GetName<S_WarriorUseChargeShieldOthers>())
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " damage absorbed per charge";
    }
} //end T_WarriorUseChargeShieldOthers

public class T_ShieldOthersGiveCharge : GridTalentBlack
{
    public override string nameFormatted => "Galvanizing Refuge";
    public override string description => Spell.GetSpell<S_WarriorUseChargeShieldOthers>().nameFormatted + " also gives the target charge";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.5f;
    public const float amountPerLevel = 0.5f;

    public T_ShieldOthersGiveCharge() : base(3)
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
            new OnTryHitOffensiveChanceMod(this, Spell.GetSpell<S_ShieldOthersGiveChargeEffect>(), 1, 1, false, Spell.GetID<S_WarriorUseChargeShieldOthers>()) //chance to give others charge when you shield them
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " charge given";
    }
} //end T_ShieldOthersGiveCharge


public class T_WarriorHoldTheLineDuration : GridTalentBlack
{
    public override string nameFormatted => "Resolve";
    public override string description => "Increases the duration of Hold the Line";
    public override string iconName => nameFormatted;

    public const float baseAmount = 3f;
    public const float amountPerLevel = 4f;

    public T_WarriorHoldTheLineDuration() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseDurationSpell_Flat(Spell.GetName<S_WarriorHoldTheLine>(), baseAmount, amountPerLevel)
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_WarriorHoldTheLineDuration


public class T_WarriorHoldTheLineChargePerSecondAllies : GridTalentBlack
{
    public override string nameFormatted => "Heroical Speech";
    public override string description => "hold the line grants charge/sec to allies";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.2f;
    public const float amountPerLevel = 0.2f;

    public const float interval = 1;

    public T_WarriorHoldTheLineChargePerSecondAllies() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new TriggerSpell_TalentMods.OnTryHitOffensive_CastSpellOnSelf(Spell.GetID<S_WarriorHoldTheLineChargePerSecondAllies>(), Spell.GetID<S_WarriorHoldTheLine>())
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " charge per second";
    }
} //end T_WarriorHoldTheLineChargePerSecondAllies

