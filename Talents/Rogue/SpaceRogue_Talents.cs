using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using static TeamSpellModifier;

public class T_RogueSmokeBomb : GridTalentBlack
{
    public override string nameFormatted => "Smoke Bomb";
    public override string description => "create a smoke cloud like a ninja that reduces the range of incoming attacks";
    public override string iconName => nameFormatted;

    public const int incrementAmount = 1;
    public const int baseAmount = 4;
    public T_RogueSmokeBomb() : base(5, Spell.GetName<S_RogueSmokeBomb>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>();
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
        return "duration: " + CalcDescriptionFlat(ref currentLevel, baseAmount, incrementAmount);
    }
}



public class T_RogueGunsBlazing : GridTalentBlack
{ //T_RogueShadowStepAoE
    public override string nameFormatted => "Guns Blazing";
    //public override string description => "creates an explosion at your destination when you use Warp";
    public override string description => "increases the AoE damage effect of Warp";

    public override string iconName => nameFormatted;

    public const int baseAmount = 8;
    public const int amountPerLevel = 8;

    public T_RogueGunsBlazing() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        //spellMods = new List<TalentMod>() { new TriggerSpell_TalentMods.OnTryHitOffensive_CastSpellOnTarget(Spell.GetID<S_RogueGunsBlazingEffect>(), Spell.GetID<S_RogueShadowStep>()) };
        spellMods = new List<TalentMod>() { new OnCalcOffensive_BaseTalentMods.IncreaseDamageSpell_Flat(Spell.GetName<S_RogueGunsBlazingEffect>(), baseAmount, amountPerLevel) };
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

    /*
    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) +  " damage";
    }*/

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " additional damage";
    }

} //end T_RogueGunsBlazing


public class T_RogueMiniaturization : GridTalentBlack
{
    public override string nameFormatted => "Miniaturization";
    public override string description => "makes your ship smaller and more cute";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.5f;

    public T_RogueMiniaturization() : base(2)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { new StatMod<Vector3>(SyncScale.scale, CalcEffectFromLevel, typeof(T_RogueMiniaturization), false)};
        entityMods = new List<EntityMod>();
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }

    Vector3 CalcEffectFromLevel(Vector3 currentValue, int currentLevel)
    {
        return currentValue / ((currentLevel * amountPerLevel) + 1);
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return "reduced size by " + CalcDescriptionPer(ref currentLevel, amountPerLevel);
    }
} //end T_RogueMiniaturization



public class T_RogueShadowMastery : GridTalentBlack
{
    public override string nameFormatted => "Shadow Mastery";
    public override string description => "increases chance to dodge while stealthed";
    public override string iconName => nameFormatted;

    public const float amountPerLevel = 0.25f;
    public const float baseAmount = 0.25f;

    public T_RogueShadowMastery() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { new OnCalcDefensive_BaseTalentMods.IncreaseDodgeChanceIfStealthed(baseAmount, amountPerLevel) };
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " additional dodge chance";
    }
} //end T_RogueShadowMastery


public class T_RogueIllusionaryProjection : GridTalentBlack
{
    public override string nameFormatted => "Illusionary Projection";
    public override string description => "grants friendly target cloaking for a limited time";
    public override string iconName => nameFormatted;

    public const float baseAmount = 15f;
    public const float amountPerLevel = 5f;

    public T_RogueIllusionaryProjection() : base(3, Spell.GetName<S_RogueIllusionaryProjection>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_RogueIllusionaryProjection


public class T_RogueReduceIllusionaryProjectionCD : GridTalentBlack
{
    public override string nameFormatted => "Abyss Sequencing";
    public override string description => "reduces cooldown of Illusionary Projection";
    public override string iconName => nameFormatted;

    public const float baseAmount = 3f;
    public const float amountPerLevel = 3f;

    public T_RogueReduceIllusionaryProjectionCD() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.DecreaseCooldownSpell_Flat(Spell.GetName<S_RogueIllusionaryProjection>(), baseAmount, amountPerLevel)
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
} //end T_RogueReduceIllusionaryProjectionCD



public class T_RogueEMP_Mine : GridTalentBlack
{
    public override string nameFormatted => "EMP Mine";
    public override string description => "mine that disables the target's movement for a short duration";
    public override string iconName => nameFormatted;

    public const float baseAmount = 2f;
    public const float amountPerLevel = 1f;

    public T_RogueEMP_Mine() : base(3, Spell.GetName<S_RogueEMP_Mine>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
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
        return "effect lasts " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_RogueEMP_Mine


public class T_RogueEvade : GridTalentBlack
{
    public override string nameFormatted => "Absquatulate";
    public override string description => "make scarce, hit the bricks, sally forth, dig out, execute barrel rolls";
    public override string iconName => nameFormatted;

    public const float baseAmount = 4f;
    public const float amountPerLevel = 2f;

     public const float baseDodgeChance = 0.4f;
    // public const float dodgePerLevel = 0.15f;

    public T_RogueEvade() : base(3, Spell.GetName<S_RogueEvade>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
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
        return "increased dodge chance by " + baseDodgeChance + " for " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_RogueEvade


public class T_RogueRecouperation : GridTalentBlack
{
    public override string nameFormatted => "Recouperate";
    public override string description => "regenerate health on kill over time";
    public override string iconName => nameFormatted;

    public const float baseAmount = 30f;
    public const float amountPerLevel = 10f;

    public T_RogueRecouperation() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new TriggerSpell_TalentMods.OnKillOffensive_CastOnSelf(Spell.GetID<S_RogueRecouperation>())
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
        return "heals " + S_RogueRecouperation._power + " every " + S_RogueRecouperation._interval + " for " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_RogueRecouperation


public class T_RogueIncreaseRecouperationStacks : GridTalentBlack
{
    public override string nameFormatted => "Anarchy Fuel";
    public override string description => "increase the max stacks of Recouperate";
    public override string iconName => nameFormatted;

    public const int baseAmount = 1;
    public const int amountPerLevel = 1;

    public T_RogueIncreaseRecouperationStacks() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseMaxStacksSpell_Flat(Spell.GetName<S_RogueRecouperation>(), baseAmount, amountPerLevel)
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " possible stacks";
    }
} //end T_RogueIncreaseRecouperationStacks


public class T_RogueAssimilation : GridTalentBlack
{
    public override string nameFormatted => "Assimilate";
    public override string description => "gain health on kill instantly";
    public override string iconName => nameFormatted;

    public const int baseAmount = 50;
    public const int amountPerLevel = 50;

    public T_RogueAssimilation() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnKillOffensiveChanceMod(this, Spell.GetSpell<S_RogueAssimilation>(), 1, 0, true)
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " health";
    }
} //end T_RogueAssimilation


public class T_RogueIncreaseEnergy : GridTalentBlack
{
    public override string nameFormatted => "Reaper of Opportunity";
    public override string description => "increased max power";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.5f;
    public const float amountPerLevel = 0.5f;

    public T_RogueIncreaseEnergy() : base(4)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() {
            new StatMod<int>(SyncEnergy.maxEnergy, CalcEnergyIncrease, typeof(T_RogueIncreaseEnergy), false)
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

    int CalcEnergyIncrease(int currentEnergy, int talentLevel)
    {
        return currentEnergy + (int)CalcEffectFlat(ref talentLevel, baseAmount, amountPerLevel);
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " additional max energy";
    }
} //end T_RogueIncreaseSpeed



public class T_RogueIncreaseEXP : GridTalentBlack
{
    public override string nameFormatted => "Infamous Wit";
    public override string description => "increase experience gain";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.04f;
    public const float amountPerLevel = 0.04f;

    public T_RogueIncreaseEXP() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() {
            new StatMod<float>(SyncLevel.expBonus, CalcExpIncrease, typeof(T_RogueIncreaseEXP), false)
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

    float CalcExpIncrease(float currentExpBonus, int talentLevel)
    {
        return currentExpBonus + CalcEffectFlat(ref talentLevel, baseAmount, amountPerLevel);
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased exp gain";
    }
} //end T_RogueIncreaseEXP



public class T_RogueIncreaseSpeed : GridTalentBlack
{
    public override string nameFormatted => "Nimble";
    public override string description => "increased move speed";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.06f;
    public const float amountPerLevel = 0.07f;

    public T_RogueIncreaseSpeed() : base(3)
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
} //end T_RogueIncreaseSpeed


public class T_RogueIncreaseCrit : GridTalentBlack
{
    public override string nameFormatted => "Calculation";
    public override string description => "increase crit chance";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.04f;
    public const float amountPerLevel = 0.03f;

    public T_RogueIncreaseCrit() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseCritChance_Flat(baseAmount, amountPerLevel)
        };
        permanentStatMods = new List<StatMod>() {
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
        return currentSpeed * CalcEffectPer(ref talentLevel, baseAmount, amountPerLevel);
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " additional crit chance";
    }
} //end T_RogueIncreaseSpeed


public class T_RogueOnDodgeMoveSpeed : GridTalentBlack
{
    public override string nameFormatted => "Onrush";
    public override string description => "on dodge gain move speed";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.1f;
    public const float amountPerLevel = 0.1f;

    public T_RogueOnDodgeMoveSpeed() : base(3, Spell.GetName<S_RogueOnDodgeMoveSpeed>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new TriggerSpell_TalentMods.OnDodgeDefensive_CastOnSelf(Spell.GetID<S_RogueOnDodgeMoveSpeed>())
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>() {
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increase for " + S_RogueOnDodgeMoveSpeed._duration + " seconds";
    }
} //end T_RogueOnDodgeMoveSpeed


public class T_RogueOnHitExtraEnergyConsumption : GridTalentBlack
{
    public override string nameFormatted => "Altruism";
    public override string description => "chance on hit to increase damage and energy cost of abilities";
    public override string iconName => nameFormatted;

    public const float baseAmountChance = 0.05f;
    public const float amountPerLevelChance = 0.05f;

    public const float baseAmount = 0.05f;
    public const float amountPerLevel = 0.05f;

    public T_RogueOnHitExtraEnergyConsumption() : base(4, Spell.GetName<S_RogueOnHitExtraEnergyConsumption>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_RogueOnHitExtraEnergyConsumption>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " chance to increase by " + CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " for " + S_RogueOnHitExtraEnergyConsumption._duration + " seconds";
    }
} //end T_RogueOnHitExtraEnergyConsumption


public class T_RogueSuicideBomb : GridTalentBlack
{
    public override string nameFormatted => "Seppuku";
    public override string description => "chance on hit to attach a bomb to yourself that will explode after duration";
    public override string iconName => nameFormatted;

    public const float baseAmountChance = 0.02f;
    public const float amountPerLevelChance = 0.02f;

    public const float baseAmount = 12f;
    public const float amountPerLevel = -2f; //will reduce cooldown for points after first one

    public T_RogueSuicideBomb() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_RogueSuicideBomb>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance, " + "deals " + S_RogueSuicideBomb.power + " damage, "+ " and lasts " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_RogueSuicideBomb


public class T_RogueCritRegen : GridTalentBlack
{
    public override string nameFormatted => "Incitation";
    public override string description => "regenerate health on crit";
    public override string iconName => nameFormatted;

    public const int baseAmount = 10;
    public const int amountPerLevel = 10;

    public const float baseDuration = 6f;
    public const float durationPerlevel = 4f;

    public T_RogueCritRegen() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnCritOffensiveChanceMod(this, Spell.GetSpell<S_RogueCritRegenHealth>(), 1, 0, true) //100% chance
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " restored every " + S_RogueCritRegenHealth._interval + " seconds for " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " seconds";
    }
} //end T_RogueCritRegen


public class T_RogueOnKillTeleport : GridTalentBlack
{
    public override string nameFormatted => "Cut and Run";
    public override string description => "teleport after killing an enemy";
    public override string iconName => nameFormatted;

    public const float baseAmount = 10f;
    public const float amountPerLevel = 10f;

    public T_RogueOnKillTeleport() : base(3, Spell.GetName<S_RogueOnKillTeleport>())
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() {
            new TriggerSpell_TalentMods.OnKillOffensive_CastOnSelf(Spell.GetID<S_RogueOnKillTeleport>())
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
        return "teleport " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + "m";
    }
} //end T_RogueOnKillTeleport


public class T_RogueOnHitDoT : GridTalentBlack
{
    public override string nameFormatted => "Corrosive Shells";
    public override string description => "chance on hit to apply a damage over time effect";
    public override string iconName => nameFormatted;

    public const int baseAmount = 10;
    public const int amountPerLevel = 10;

    public const float baseAmountChance = 0.02f;
    public const float amountPerLevelChance = 0.02f;

    public T_RogueOnHitDoT() : base(5)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_RogueOnHitDoT>(), baseAmountChance, amountPerLevelChance, false)
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " chance to deal " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " damage every " + S_RogueOnHitDoT._interval + " for "  + S_RogueOnHitDoT._duration + " seconds";
    }
} //end T_RogueOnHitDoT


public class T_RogueOnHitIncreasedRange : GridTalentBlack
{
    public override string nameFormatted => "Clutch";
    public override string description => "chance on hit to increase the range of your abilities";
    public override string iconName => nameFormatted;

    public const float baseAmount = 4f;
    public const float amountPerLevel = 3f;

    public const float baseAmountChance = 0.04f;
    public const float amountPerLevelChance = 0.03f;

    public T_RogueOnHitIncreasedRange() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>() {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_RogueOnHitIncreasedRange>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance to increase range by " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + "m for " + S_RogueOnHitIncreasedRange._duration + " seconds";
    }
} //end T_RogueOnHitIncreasedRange


public class T_RogueOnDodgeIncreaseHitChance : GridTalentBlack
{
    public override string nameFormatted => "Reprisal";
    public override string description => "increase hit chance after dodging";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.1f;
    public const float amountPerLevel = 0.1f;

    public T_RogueOnDodgeIncreaseHitChance() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {

        spellMods = new List<TalentMod>() {
            new TriggerSpell_TalentMods.OnDodgeDefensive_CastOnSelf(Spell.GetID<S_RogueOnDodgeIncreaseHitChance>())
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " additional chance to hit for " + S_RogueOnDodgeIncreaseHitChance._duration;
    }
} //end T_RogueOnDodgeIncreaseHitChance


public class T_RogueOnHitAddMaxEnergy : GridTalentBlack
{
    public override string nameFormatted => "Lucid Vibrations";
    public override string description => "chance on hit to increase max energy, stacks up to " + maxStacks + " times";
    public override string iconName => nameFormatted;

    public const float baseAmount = 10f;
    public const float amountPerLevel = 10f;

    public const float baseAmountChance = 0.09f;
    public const float amountPerLevelChance = 0.08f;

    public const int maxStacks = 5;

    public T_RogueOnHitAddMaxEnergy() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() { };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_RogueOnHitAddMaxEnergy>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance to increase max energy by " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " for " + S_RogueOnHitAddMaxEnergy._duration;
    }
} //end T_RogueOnHitAddMaxEnergy


public class T_RogueIncreaseOnKillRecoupDuration : GridTalentBlack
{
    public override string nameFormatted => "Vitalizing Flow";
    public override string description => "increases the duration of recouperate";
    public override string iconName => nameFormatted;

    public const float baseAmount = 4f;
    public const float amountPerLevel = 4f;

    public T_RogueIncreaseOnKillRecoupDuration() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseDurationSpell_Flat(Spell.GetName<S_RogueRecouperation>(), baseAmount, amountPerLevel)
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
} //end T_RogueIncreaseOnKillRecoupDuration


public class T_RogueIncreaseConsumeExtraEnergyAmount : GridTalentBlack
{
    public override string nameFormatted => "Ascetic";
    public override string description => "further increases damage and energy consumtion";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.08f;
    public const float amountPerLevel = 0.08f;

    public T_RogueIncreaseConsumeExtraEnergyAmount() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseSpellPower_NonDamage_Flat(Spell.GetName<S_RogueOnHitExtraEnergyConsumption>(), baseAmount, amountPerLevel) //would actually be flat because we're "adding" to the power_nonDamage which is a percent
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " additional increase";
    }
} //end T_RogueIncreaseConsumeExtraEnergyAmount



public class T_RogueIncreaseOnHitSuicideBombDamage : GridTalentBlack
{
    public override string nameFormatted => "Rite of Passage";
    public override string description => "increases Seppeku's damage further";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.1f;
    public const float amountPerLevel = 0.2f;

    public T_RogueIncreaseOnHitSuicideBombDamage() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseDamageSpell_Percent(Spell.GetName<S_RogueSuicideBomb>(), baseAmount, amountPerLevel)
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased damage";
    }
} //end T_RogueIncreaseOnHitSuicideBombDamage



public class T_RogueDecreaseSuicideBombCountdown : GridTalentBlack
{
    public override string nameFormatted => "Obsession";
    public override string description => "decreases the countdown duration of Sepekku and increases damage";
    public override string iconName => nameFormatted;

    public const float baseAmount_countdown = 2f;
    public const float amountPerLevel_countdown = 2f;

    //TODO- change to decrease damage done to self?...
    //need to separate the spell into two- one for self-damage another for damage to targets?...
    //maybe even cast one from the other
    public const float baseAmount_damage = 0.1f;
    public const float amountPerLevel_damage = 0.1f;

    public T_RogueDecreaseSuicideBombCountdown() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.DecreaseDurationSpellFlat(Spell.GetName<S_RogueSuicideBomb>(), baseAmount_countdown, amountPerLevel_countdown),
            new OnCalcOffensive_BaseTalentMods.IncreaseDamageSpell_Percent(Spell.GetName<S_RogueSuicideBomb>(), baseAmount_damage, amountPerLevel_damage)
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
        return "reduced by " + CalcDescriptionFlat(ref currentLevel, baseAmount_countdown, amountPerLevel_countdown) + " seconds and " + CalcDescriptionPer(ref currentLevel, baseAmount_damage, amountPerLevel_damage) + " increased damage";
    }
} //end T_RogueDecreaseSuicideBombCountdown



public class T_RogueIncreaseOnCritRegenHealthAmount : GridTalentBlack
{
    public override string nameFormatted => "Radical Stimulus";
    public override string description => "increases healing from " + Item.From<T_RogueCritRegen>().name;
    public override string iconName => nameFormatted;

    public const float baseAmount = .1f;
    public const float amountPerLevel = .2f;

    public T_RogueIncreaseOnCritRegenHealthAmount() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseDamageSpell_Percent(Spell.GetName<S_RogueCritRegenHealth>(), baseAmount, amountPerLevel)
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased healing";
    }
} //end T_RogueIncreaseOnCritRegenHealthAmount



public class T_RogueIncraseOnHitDotDamage : GridTalentBlack
{
    public override string nameFormatted => "";
    public override string description => "";
    public override string iconName => nameFormatted;

    public const float baseAmount = .1f;
    public const float amountPerLevel = .2f;

    public T_RogueIncraseOnHitDotDamage() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseSpellPower_NonDamage_Percent(Spell.GetName<S_RogueOnHitDoT>(), baseAmount, amountPerLevel)
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
} //end T_RogueIncraseOnHitDotDamage



public class T_RogueIncreaseOnHitRangeAmount : GridTalentBlack
{
    public override string nameFormatted => "Phenomenon";
    public override string description => "increases the range of Clutch even further";
    public override string iconName => nameFormatted;

    public const float baseAmount = 4f;
    public const float amountPerLevel = 3f;

    public T_RogueIncreaseOnHitRangeAmount() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseSpellPower_NonDamage_Flat(Spell.GetName<S_RogueOnHitIncreasedRange>(), baseAmount, amountPerLevel)
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " additional m";
    }
} //end T_RogueIncreaseOnHitRangeAmount




public class T_RogueIncreaseOnDodgeHitChance : GridTalentBlack
{
    public override string nameFormatted => "";
    public override string description => "";
    public override string iconName => nameFormatted;

    public const float baseAmount = 1f;
    public const float amountPerLevel = 2f;

    public T_RogueIncreaseOnDodgeHitChance() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseSpellPower_NonDamage_Flat(Spell.GetName<S_RogueOnDodgeIncreaseHitChance>(), baseAmount, amountPerLevel)
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
} //end T_RogueIncreaseOnDodgeHitChance




public class T_RogueIncreaseOnHitEnergyAmount : GridTalentBlack
{
    public override string nameFormatted => "";
    public override string description => "";
    public override string iconName => nameFormatted;

    public const float baseAmount = 1f;
    public const float amountPerLevel = 2f;

    public T_RogueIncreaseOnHitEnergyAmount() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
            new OnCalcOffensive_BaseTalentMods.IncreaseSpellPower_NonDamage_Flat(Spell.GetName<S_RogueOnHitAddMaxEnergy>(), baseAmount, amountPerLevel)
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
} //end T_RogueIncreaseOnHitEnergyAmount



public class T_RogueOnHitBootsOfBlindingSpeed : GridTalentBlack
{
    public override string nameFormatted => "";
    public override string description => "";
    public override string iconName => nameFormatted;

    public const float baseAmount = 1f;
    public const float amountPerLevel = 2f;

    public T_RogueOnHitBootsOfBlindingSpeed() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>() {
        };
        permanentStatMods = new List<StatMod>() { };
        entityMods = new List<EntityMod>()
        {
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_RogueOnHitBootsOfBlindingSpeed>(), baseAmount, amountPerLevel, true)
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " chance on hit to give you the boosters of blinding speed";
    }
} //end T_RogueOnHitBootsOfBlindingSpeed



public class T_RogueIncreaseTeamRogueSpeed : GridTalentBlack
{
    const string rogue_entity_name = "Player_Falcon Variant";

    public override string nameFormatted => "Brothers in the Craft";
    public override string description => "increases speed of all Falcons on the team";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.02f;
    public const float increasePerLevel = 0.02f;
    public T_RogueIncreaseTeamRogueSpeed() : base(3, null)
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
                    //rogue_entity_name has to be a creatable Entity or will throw an error
                    WorldFunctions.GetEntityTypeID(rogue_entity_name), new List<StatModEntry>() { new StatModEntry(new StatMod<float>(SyncSpeed.maxSpeed, ModSpeed, typeof(T_RogueIncreaseTeamRogueSpeed), false), false, this, -1) }
                
            ) //faction StatMods for entity typeID
            }
            */
            ) //team StatModEntries 
            {  {
                                WorldFunctions.GetEntityTypeID(rogue_entity_name), new List<StatModEntry>() { new StatModEntry(new StatMod<float>(SyncSpeed.maxSpeed, ModSpeed, typeof(T_RogueIncreaseTeamRogueSpeed), false), false, this, -1) }
            } }
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

} //end talent T_RogueIncreaseTeamRogueSpeed



public class T_RogueOnKillGainCharge : GridTalentBlack
{
    public override string nameFormatted => "";
    public override string description => "";
    public override string iconName => nameFormatted;

    public const float baseAmount = 1f;
    public const float amountPerLevel = 2f;

    public T_RogueOnKillGainCharge() : base(3)
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
            new OnKillOffensiveChanceMod(this, Spell.GetSpell<S_RogueOnKillGainCharge>(), 1, 0, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased speed";
    }
} //end T_RogueOnKillGainCharge



public class T_RogueShadowstepGainCharge : GridTalentBlack
{
    public override string nameFormatted => "Inaugeration";
    public override string description => "gain charge after using " + Spell.GetSpell<S_RogueShadowStep>().nameFormatted;
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.5f;
    public const float amountPerLevel = 0.5f;

    public T_RogueShadowstepGainCharge() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseChargeGainedSpell_Flat(Spell.GetName<S_RogueShadowStep>(), baseAmount, amountPerLevel)
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " charge on use";
    }
} //end T_RogueShadowstepGainCharge



public class T_RogueIncreaseDamagePerCharge : GridTalentBlack
{
    public override string nameFormatted => "Catalyzing Force";
    public override string description => "increases damage per charge";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.03f;
    public const float amountPerLevel = 0.02f;

    public T_RogueIncreaseDamagePerCharge() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseDamagePercentPerCharge_Caster(baseAmount, amountPerLevel)
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased damage per charge";
    }
} //end T_RogueIncreaseDamagePerCharge



public class T_RogueRangedAttackWithCharge : GridTalentBlack
{
    public override string nameFormatted => "Hawk Shot";
    public override string description => "long ranged attack, consumes charge";
    public override string iconName => nameFormatted;

    public const int baseAmount = 20;
    public const int amountPerLevel = 20;

    public T_RogueRangedAttackWithCharge() : base(3, Spell.GetName<S_RogueRangedAttackWithCharge>())
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
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " damage, consumes 1 charge";
    }
} //end T_RogueRangedAttackWithCharge



public class T_RogueChanceOnHitRegenCharge : GridTalentBlack
{
    public override string nameFormatted => "Adrenalized";
    public override string description => "chance on hit to regenerate charge";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.2f;
    public const float amountPerLevel = 0.2f;


    public const float baseAmountChance = 0.04f;
    public const float amountPerLevelChance = 0.03f;


    public T_RogueChanceOnHitRegenCharge() : base(3)
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
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_RogueChanceOnHitRegenCharge>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance to regen " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " every " + S_RogueChanceOnHitRegenCharge._interval + " for " + S_RogueChanceOnHitRegenCharge._duration + " seconds";
    }
} //end T_RogueChanceOnHitRegenCharge




public class T_RogueChanceOnHitRegenEnergy : GridTalentBlack
{
    public override string nameFormatted => "unused";
    public override string description => "chance on hit to regenerate energy";
    public override string iconName => nameFormatted;

    public const int baseAmount = 5;
    public const int amountPerLevel = 5;


    public const float baseAmountChance = 0.04f;
    public const float amountPerLevelChance = 0.03f;


    public T_RogueChanceOnHitRegenEnergy() : base(3)
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
            new OnHitDamageOffensiveChanceMod(this, Spell.GetSpell<S_RogueChanceOnHitRegenCharge>(), baseAmountChance, amountPerLevelChance, true)
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
        return CalcDescriptionPer(ref currentLevel, baseAmountChance, amountPerLevelChance) + " chance to regen " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " every " + S_RogueChanceOnHitRegenEnergy._interval + " for " + S_RogueChanceOnHitRegenEnergy._duration + " seconds";
    }
} //end T_RogueChanceOnHitRegenEnergy




public class T_RogueGainChargeStandingStill : GridTalentBlack
{
    public override string nameFormatted => "Premeditation";
    public override string description => "gain charge from remaining still";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.1f;
    public const float amountPerLevel = 0.1f;

    const float interval = 2f;
    const float minStillnessTime = 3f;
    public T_RogueGainChargeStandingStill() : base(4)
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
            new EntityMod(ApplyTalent, RemoveTalent) //generic EntityMod
        };
        //mods applied to entire team
        teamMods = new TeamMods(
            new List<SpellModEntry>() { }, //faction SpellMods
            new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
            new List<StatModEntry>(), //faction StatMods
            new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
        );
    }


    void ApplyTalent(Entity target)
    {
        RemoveTalent(target); //try remove old first just in case
        target.gameObject.AddComponent<GainChargeStandingStill>().Set(
            baseAmount + ((target.GetTalentLevel(this.id) - 1) * amountPerLevel),
            interval,
            minStillnessTime
            ); //add the target script
    }
    void RemoveTalent(Entity target)
    {
        //remove all instances of the target script, just in case
        Array.ForEach(target.GetComponents<GainChargeStandingStill>(), entry =>
        {
            UnityEngine.Object.Destroy(entry);
        });
    }

    public override string GetLevelDescription(int currentLevel)
    {
        return CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + " charge every " + interval.ToString("0.00") + " seconds after standing still for " + minStillnessTime.ToString("0.00") + " seconds";
    }
} //end T_RogueGainChargeStandingStill



public class T_RogueIncreaseShadowstepRange : GridTalentBlack
{
    public override string nameFormatted => "Grasp of the Void";
    public override string description => "increases the range of " + Spell.GetSpell<S_RogueShadowStep>().nameFormatted;
    public override string iconName => nameFormatted;

    public const float baseAmount = 4f;
    public const float amountPerLevel = 3f;

    public T_RogueIncreaseShadowstepRange() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseRangeSpell_Flat(Spell.GetName<S_RogueShadowStep>(), baseAmount, amountPerLevel)
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
        return "increased by " + CalcDescriptionFlat(ref currentLevel, baseAmount, amountPerLevel) + "m";
    }
} //end T_RogueIncreaseShadowstepRange


public class T_RogueIncreaseCastTimeAndDamageBackstab : GridTalentBlack
{
    public override string nameFormatted => "Trigger Discipline";
    public override string description => "increases the damage of " + Spell.GetSpell<S_RogueBackstab>().nameFormatted + " but also increases the cast time";
    public override string iconName => nameFormatted;

    public const int baseAmountDamage = 20;
    public const int amountPerLevelDamage = 20;

    public const float baseAmountCastTime = 0.34f;
    public const float amountPerLevelCastTime = 0.33f;

    public T_RogueIncreaseCastTimeAndDamageBackstab() : base(3)
    {

    }

    protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
    {
        spellMods = new List<TalentMod>()
        {
            new OnCalcOffensive_BaseTalentMods.IncreaseDamageSpell_Flat(Spell.GetName<S_RogueBackstab>(), baseAmountDamage, amountPerLevelDamage),
            new OnCalcOffensive_BaseTalentMods.IncreaseCastTimeSpell_Flat(Spell.GetName<S_RogueBackstab>(), baseAmountCastTime, amountPerLevelCastTime),
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
        return "increase damage by " + CalcDescriptionFlat(ref currentLevel, baseAmountDamage, amountPerLevelDamage) + ", increase cast time by " + CalcDescriptionFlat(ref currentLevel, baseAmountCastTime, amountPerLevelCastTime);
    }
} //end T_RogueIncreaseCastTimeAndDamageBackstab


public class T_RogueConsumeChargeIncreaseDodge : GridTalentBlack
{
    public override string nameFormatted => "Dramatic Hoax";
    public override string description => "consumes charge to increase dodge chance";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.02f;
    public const float amountPerLevel = 0.02f;

    public T_RogueConsumeChargeIncreaseDodge() : base(3, Spell.GetName<S_RogueConsumeChargeIncreaseDodge>())
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
        return CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " increased chance to dodge per charge for " + S_RogueConsumeChargeIncreaseDodge._duration + " seconds";
    }
} //end T_RogueConsumeChargeIncreaseDodge



public class T_RogueConsumeChargeMortalStrike : GridTalentBlack
{
    public override string nameFormatted => "Rive";
    public override string description => "consume charge to decrease healing the target receives";
    public override string iconName => nameFormatted;

    public const float baseAmount = 0.04f;
    public const float amountPerLevel = 0.04f;

    public T_RogueConsumeChargeMortalStrike() : base(4, Spell.GetName<S_RogueConsumeChargeMortalStrike>())
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
        return "reduces healing received by " + CalcDescriptionPer(ref currentLevel, baseAmount, amountPerLevel) + " per charge for " + S_RogueConsumeChargeMortalStrike._duration + " seconds";
    }
} //end T_RogueConsumeChargeMortalStrike


public class T_RoguePurge : GridTalentBlack
{
    public override string nameFormatted => "Purge";
    public override string description => "removes a beneficial status effect from the target";
    public override string iconName => nameFormatted;

    //public const int baseAmount = 10;
    //public const int amountPerLevel = 8;

    public const int baseNumStrikes = 1;
    public const int numStrikesPerLevel = 1;

    public T_RoguePurge() : base(1, Spell.GetName<S_RoguePurge>())
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
} //end T_RoguePurge


public class T_RogueSeedOfCorruption : GridTalentBlack
{
    public override string nameFormatted => "Seed of Justice";
    public override string description => "deal damage over time to target, if target takes damage beyond a certain amount or dies, they explode, dealing damage to nearby targets";
    public override string iconName => nameFormatted;

    //thrshold is how much damage must be taken before it "explodes"
    public const int baseAmountThreshold = 150;
    public const int amountPerLevelThreshold = 50;

    public const int baseAmountDoT = 10;
    public const int amountPerLevelDoT = 5;

    public const int baseAmountAoE = 75;
    public const int amountPerLevelAoE = 25;


    public T_RogueSeedOfCorruption() : base(3, Spell.GetName<S_RogueSeedOfCorruption>())
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
        return CalcDescriptionFlat(ref currentLevel, baseAmountDoT, amountPerLevelDoT) + " damage every " + S_RogueSeedOfCorruption._interval + " seconds for " + S_RogueSeedOfCorruption._duration + " seconds, " + CalcDescriptionFlat(ref currentLevel, baseAmountAoE, amountPerLevelAoE) + " AoE effect damage, " + CalcDescriptionFlat(ref currentLevel, baseAmountThreshold, amountPerLevelThreshold) + " damage to trigger explosion";
    }
} //end T_RogueSeedOfCorruption
