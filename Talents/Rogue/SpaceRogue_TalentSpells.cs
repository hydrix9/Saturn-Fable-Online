using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CreateFX_Params;


public class S_RogueSmokeBomb : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => "dokkan!";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public const float rangeMultiplier = 0.3f;
    SpellMod decreaseRangeEffect = new OnCalcDefensive_BaseTalentMods.DecreaseRange_Percent(rangeMultiplier, rangeMultiplier); //actual status effect SpellMod added to entity

    public TalentItem talent => Item.From<T_RogueSmokeBomb>().asTalentItem;

    public S_RogueSmokeBomb() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Long_Sweep_Down_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("SmokeBomb", TargetType.caster, PersistType.fromSpellHit, false, 4)
        };
        fx_animationHit = new CreateFX_Params[]
        {
            
        };

    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.medium;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.slow_rotation;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        AOEs.Zone_SpellMod(spellHit, decreaseRangeEffect, true);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //make smoke bomb animation
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueSmokeBomb.baseAmount, T_RogueSmokeBomb.incrementAmount);
    }

} //end spell S_SmokeBomb



public class S_RogueEvade : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;


    public TalentItem talent => Item.From<T_RogueEvade>().asTalentItem;

    public S_RogueEvade() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Swoosh_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            //new CreateFX_Params("SmokeBomb", TargetType.target, PersistType.duration, false, 2, 1)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.fast_buff_rotation;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = T_RogueEvade.baseDodgeChance;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void OnCalcHitChanceDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.dodgeChance += selfStatusEffect.dodgeChance; //effects cast on wearer have increased dodge chance
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueEvade.baseAmount, T_RogueEvade.amountPerLevel);
    }

} //end spell S_RogueEvade






public class S_RogueEMP_Mine : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => "mine that disables the target's movement for a short duration";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public const float baseDuration = 2;

    public TalentItem talent => Item.From<T_RogueEMP_Mine>().asTalentItem;
    Spell mineTriggerEffect;

    public S_RogueEMP_Mine() : base() //TODO: add properties
    {
        
        
    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        mineTriggerEffect = Spell.GetSpell<S_RogueEMP_Mine_TriggerEffect>();
        prop.prefab = WorldFunctions.GetEntity("EMP_Mine_Trigger");

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Rumble_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {

        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.trap_medium;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.extremely_slow_rotation;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        
    }
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        MineCreate.Single(spellHit, mineTriggerEffect, prop.prefab.name, null, spellHit.caster.transform.position);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //duration is actually from SpellDuration.config.trap_medium since this is the mine spell, not the Immobilize part
        //selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueEMP_Mine.baseAmount, T_RogueEMP_Mine.amountPerLevel);
    }

} //end spell S_RogueEMP_Mine



public class S_RogueEMP_Mine_TriggerEffect : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => "EMP Mine";
    public override string description => "mine that disables the target's movement for a short duration";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.debuff;

    public TalentItem talent => Item.From<T_RogueEMP_Mine>().asTalentItem;

    //public const float baseDuration = 2;

    public S_RogueEMP_Mine_TriggerEffect() : base() //TODO: add properties
    {


    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Short_Distorted_1_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("EMP_Mine_Effect", TargetType.target, PersistType.duration, false, 1, 15f)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.cc_small;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //doesn't target anything
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }


    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        OnAnimationHit(spellHit);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        AOEs.DoAction(spellHit, OnMyAoE); //do action on all in spherecast
    }

    void OnMyAoE(spellHit spellHit)
    {
        StatusEffects.Immobilize(spellHit, true, false);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueEMP_Mine.baseAmount, T_RogueEMP_Mine.amountPerLevel);
    }

} //end spell S_RogueEMP_Mine_TriggerEffect


public class S_RogueIllusionaryProjection : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => "Illusionary Projection";
    public override string description => "cloaks the friendly target for a limited duration. Must be out of combat";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueIllusionaryProjection>().asTalentItem;

    public S_RogueIllusionaryProjection() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Effect_Reverse_Zoom_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.fast_buff_rotation;
        range = SpellRange.config.high;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        usableInCombat = false;
        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { spellType.stealth };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal_NotSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        spellHit.target.Stealth(spellHit); //turn invisible and add status effect

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
    }

    public override void OnExpireStatusEffect_Caster(spellHit expired)
    {
        if(!expired.target.isStealthed)
        { //if not still stealthed
            expired.target.Unstealth_Raw(null); //remove effects of stealth
        }
    } //end OnExpireStatusEffect_Caster

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.duration += TalentItem.CalcEffectFlat(ref talentLevel, T_RogueIllusionaryProjection.baseAmount, T_RogueIllusionaryProjection.amountPerLevel);
    }

} //end spell S_RogueIllusionaryProjection



public class S_RogueRecouperation : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueRecouperation>().asTalentItem;

    public const int _power = 15;
    public static float _interval;

    public S_RogueRecouperation() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Quick_Climbing_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("SparkleGreen", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

        _interval = SpellInterval.config.fast;

    } //end InitDefaults

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = _power;
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none; //set during MultiplyByTalentLevel
        interval = _interval;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = 1;
        maxStacks = 1; //will calculate power with this b/c another talent increases the num stacks...
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { spellType.healing };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        //TODO: also spawn prefab
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        for(int i = 0; i < spellHit.stacks; i++)
            Damage.Heal(spellHit, true, false);
    }


    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueRecouperation.baseAmount, T_RogueRecouperation.amountPerLevel);
    }

} //end spell S_RogueRecouperation



public class S_RogueAssimilation : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_RogueAssimilation>().asTalentItem;

    public S_RogueAssimilation() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Quick_Climbing_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(0.9f, 0.9f, 1, 0.9f, 1, 0.04f, 0.04f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("HealOnceLowPlus", TargetType.target, PersistType.whileHasSpellEffect, false, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.fast_buff_rotation;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { spellType.healing };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        //TODO: also spawn prefab
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        Damage.Heal(spellHit, true, true);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueAssimilation.baseAmount, T_RogueAssimilation.amountPerLevel);
    }

} //end spell S_RogueAssimilation



public class S_RogueOnDodgeMoveSpeed : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueOnDodgeMoveSpeed>().asTalentItem;

    public static float _duration; //exposed field for TalentItem description

    public S_RogueOnDodgeMoveSpeed() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Swoosh_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {

        };
        fx_animationHit = new CreateFX_Params[]
        {

        };


        _duration = SpellDuration.config.buff_short;
    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.fast_buff_rotation;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };

    } //end InitSpellBaseValues

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        //TODO: also spawn prefab
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.MultSpeed(spellHit, spellHit.power_nonDamage, true, true, false);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_RogueOnDodgeMoveSpeed.baseAmount, T_RogueOnDodgeMoveSpeed.amountPerLevel);
    }

} //end spell S_RogueOnDodgeMoveSpeed





public class S_RogueOnHitExtraEnergyConsumption : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueOnHitExtraEnergyConsumption>().asTalentItem;

    public static float _duration; //expose to talent description

    public S_RogueOnHitExtraEnergyConsumption() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Reverse_Repeating_Fading_Zaps_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("FairiesRed", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };


        _duration = SpellDuration.config.buff_short;
    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void CalcCastPerOffensive_PreHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        if(spellHit.power > 0 && !spellHit.isHeal)
        {
            spellHit.cost = (int)(spellHit.cost * selfStatusEffect.power_nonDamage); //also increase cost
        }
    }

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.power > 0 && !spellHit.isHeal)
        {
            spellHit.power = (int)(spellHit.power * selfStatusEffect.power_nonDamage); //increase damage
        }
    }


    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectPer(ref talentLevel, T_RogueOnHitExtraEnergyConsumption.baseAmount, T_RogueOnHitExtraEnergyConsumption.amountPerLevel);
    }

} //end spell S_RogueOnHitExtraEnergyConsumption


public class S_RogueSuicideBomb : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueSuicideBomb>().asTalentItem;

    public const int power = 150;

    public S_RogueSuicideBomb() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Long_Sweep_Down_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(0.9f, 0.9f, 1, 0.9f, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("SlowSkull", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {
            new CreateFX_Params("GroundSlamRed", TargetType.target, PersistType.duration, false, 8, 3f)
        };

    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = false;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = power; //dont modify power except from talent level
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //can't do these at the same time, otherwise might accidentally damage self first, destroy self, then fail to damage area enemies
        StatusEffects.TryAdd(spellHit, true, true, false); //attack all nearby valid targets
        MyAoE_Action(new spellHit(spellHit, spellHit.caster, spellHit.target, spellHit.origin, spellHit.vTarget)); //damage to self
    }

    //called when this status effect expires
    public override void OnExpireStatusEffect_Caster(spellHit expired)
    {
        if(expired.caster != null)
            expired.vTarget = expired.caster.transform.position; //do AoE at caster's position

        OnAnimationHit(expired); //do AoE, tell clients animation hit, create fx/sfx
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        AOEs.DoAction(spellHit, MyAoE_Action); //do damage AoE
    }

    //AoE that happens on expiration
    public void MyAoE_Action(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, true);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueSuicideBomb.baseAmount, T_RogueSuicideBomb.amountPerLevel);
    }

} //end spell S_RogueSuicideBomb


public class S_RogueCritRegenHealth : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueCritRegen>().asTalentItem;

    public static float _interval; //expose parameter to talent description

    public S_RogueCritRegenHealth() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        _interval = SpellInterval.config.medium;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Coin_Collect_Two_Note_Off_Tune_Twinkle_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none; //calculated from talent
        interval = _interval;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> {spellType.healing };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        //TODO: also spawn prefab
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Heal(spellHit, true, false);
    }
    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power += TalentItem.CalcEffectFlat(ref talentLevel, T_RogueCritRegen.baseAmount, T_RogueCritRegen.amountPerLevel);
        selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueCritRegen.baseDuration, T_RogueCritRegen.amountPerLevel);

    }

} //end spell S_RogueCritRegenHealth



public class S_RogueOnKillTeleport : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_RogueOnKillTeleport>().asTalentItem;

    public S_RogueOnKillTeleport() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Reverse_Repeating_Fading_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(0.9f, 0.9f, 1, 0.9f, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {

        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        //TODO: also spawn prefab
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        spellHit.vTarget = spellHit.caster.transform.position + (spellHit.caster.transform.forward * spellHit.power_nonDamage);
        Ability.TeleportToVTarget(spellHit);
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectFlat(ref talentLevel, T_RogueOnKillTeleport.baseAmount, T_RogueOnKillTeleport.amountPerLevel);
    }

} //end spell S_RogueOnKillTeleport



public class S_RogueOnHitDoT : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;

    public TalentItem talent => Item.From<T_RogueOnHitDoT>().asTalentItem;

    public static float _duration; //expose parameter for talent description
    public static float _interval; //expose parameter for talent description

    public S_RogueOnHitDoT() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Short_Bright_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("RedSparksLong", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };


        _duration = SpellDuration.config.damage_short;
        _interval = SpellInterval.config.fast;
    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = _interval;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        //TODO: also spawn prefab
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        StatusEffects.TryAdd(spellHit, true, false, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, false);
    }
    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueOnHitDoT.baseAmount, T_RogueOnHitDoT.amountPerLevel);
    }

} //end spell S_RogueOnHitDoT


public class S_RogueOnHitIncreasedRange : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueOnHitIncreasedRange>().asTalentItem;

    public static float _duration; //expose field for TalentItem description

    public S_RogueOnHitIncreasedRange() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Flash_Fading_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("FairiesPurple", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };


        _duration = SpellDuration.config.damage_short;
    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };

    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void CalcCastFlatOffensive_PreHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.range += selfStatusEffect.power_nonDamage;
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectFlat(ref talentLevel, T_RogueOnHitIncreasedRange.baseAmount, T_RogueOnHitIncreasedRange.amountPerLevel);
    }

} //end spell S_RogueOnHitIncreasedRange


public class S_RogueOnDodgeIncreaseHitChance : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueOnDodgeIncreaseHitChance>().asTalentItem;

    public static float _duration;

    public S_RogueOnDodgeIncreaseHitChance() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        _duration = SpellDuration.config.buff_short;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Quick_Climbing_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("FairiesYellow", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = false;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        //TODO: also spawn prefab
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void OnCalcHitChanceOffensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.hitChance += selfStatusEffect.power_nonDamage;
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectFlat(ref talentLevel, T_RogueOnDodgeIncreaseHitChance.baseAmount, T_RogueOnDodgeIncreaseHitChance.amountPerLevel);
    }

} //end spell S_RogueOnDodgeIncreaseHitChance




public class S_RogueOnHitAddMaxEnergy : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_RogueOnHitAddMaxEnergy>().asTalentItem;

    public static float _duration; //expose to talent description

    public S_RogueOnHitAddMaxEnergy() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Coin_Collect_Two_Note_Bright_Twinkle_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("FairiesCyan", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };


        _duration = SpellDuration.config.buff_short;
    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = 1;
        maxStacks = T_RogueOnHitAddMaxEnergy.maxStacks;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = false;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.AddMaxEnergy(spellHit, Mathf.FloorToInt(spellHit.power_nonDamage), true, false, false);
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueOnHitAddMaxEnergy.baseAmount, T_RogueOnHitAddMaxEnergy.amountPerLevel);
    }

} //end spell S_RogueOnHitAddMaxEnergy



public class S_RogueOnHitBootsOfBlindingSpeed : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueOnHitBootsOfBlindingSpeed>().asTalentItem;

    public S_RogueOnHitBootsOfBlindingSpeed() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Bright_Repeating_Fading_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {

        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.buff_short;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.fast_buff_rotation;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        //TODO: also spawn prefab
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        StatusEffects.MultSpeed(spellHit, spellHit.power_nonDamage, true, true, false);
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.duration += TalentItem.CalcEffectFlat(ref talentLevel, S_RogueOnHitBootsOfBlindingSpeed.baseAmount, S_RogueOnHitBootsOfBlindingSpeed.amountPerLevel);
    }

} //end spell S_RogueOnHitBootsOfBlindingSpeed



public class S_RogueOnKillGainCharge : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_RogueOnKillGainCharge>().asTalentItem;

    public S_RogueOnKillGainCharge() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Coin_Collect_Two_Note_Bright_Fast_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("BeamupCloudPink", TargetType.caster, PersistType.duration, true, 0.7f, 2f)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        Ability.AddCharge(spellHit.caster, ref spellHit.chargeGained);
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectFlat(ref talentLevel, T_RogueOnKillGainCharge.baseAmount, T_RogueOnKillGainCharge.amountPerLevel);
    }

} //end spell S_RogueOnKillGainCharge



public class S_RogueRangedAttackWithCharge : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted; //Hawk Shot
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_RogueRangedAttackWithCharge>().asTalentItem;

    public S_RogueRangedAttackWithCharge() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Woosh_Down_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("SmallPurpleFireball Variant", TargetType.caster, PersistType.duration, false, 0.7f, 2f)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.high;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override void CalcMinChargeUsed(spellHit spellHit)
    {
        spellHit.minChargeUsed = 1;
    }

    public override void CalcMaxChargeUsed(spellHit spellHit)
    {
        spellHit.maxChargeUsed = 1;
    }

    //use this special override to ensure proper calculation order
    public override void CalcFlatEffectsFromCharge(spellHit spellHit)
    {
        spellHit.power = (int)(spellHit.power * spellHit.chargeUsed); //in case you want to consume more charge later, or?..
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, true);
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueRangedAttackWithCharge.baseAmount, T_RogueRangedAttackWithCharge.amountPerLevel);
    }

} //end spell S_RogueRangedAttackWithCharge



public class S_RogueChanceOnHitRegenCharge : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueChanceOnHitRegenCharge>().asTalentItem;

    public static float _interval;
    public static float _duration;

    public S_RogueChanceOnHitRegenCharge() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        _interval = SpellInterval.config.medium;
        _duration = SpellDuration.config.buff_short;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Quick_Deep_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("RegenerateRed Minor", TargetType.target, PersistType.whileHasSpellEffect, true, 2, 1)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        interval = _interval;
        duration = _duration;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none; //calculate in MultiplyByTalentLevel
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //Ability.AddCharge(spellHit.caster, spellHit.chargeGained); ///spell should do this automatically on cast, only need to do during IntervalEffect
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Ability.AddCharge(spellHit.caster, spellHit.power_nonDamage);
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectFlat(ref talentLevel, T_RogueChanceOnHitRegenCharge.baseAmount, T_RogueChanceOnHitRegenCharge.amountPerLevel);

    }

} //end spell S_RogueChanceOnHitRegenCharge


public class S_RogueChanceOnHitRegenEnergy : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueChanceOnHitRegenEnergy>().asTalentItem;

    public static float _interval;
    public static float _duration;

    public S_RogueChanceOnHitRegenEnergy() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        _interval = SpellInterval.config.medium;
        _duration = SpellDuration.config.buff_short;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Quick_Deep_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            //new CreateFX_Params("RegenerateRed Minor", TargetType.target, PersistType.whileHasSpellEffect, false, 2, 1)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        interval = _interval;
        duration = _duration;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none; //calculate in MultiplyByTalentLevel
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Ability.AddEnergy(spellHit.caster, (int)spellHit.power_nonDamage);
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueChanceOnHitRegenEnergy.baseAmount, T_RogueChanceOnHitRegenEnergy.amountPerLevel);

    }

} //end spell S_RogueChanceOnHitRegenEnergy



public class S_RogueConsumeChargeIncreaseDodge : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_RogueConsumeChargeIncreaseDodge>().asTalentItem;

    public static float _duration;

    public S_RogueConsumeChargeIncreaseDodge() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        _duration = SpellDuration.config.buff_short;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Appear_Fading_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("BeamupCloudPurple", TargetType.target, PersistType.whileHasSpellEffect, true, 2, 1)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        //TODO: also spawn prefab
    }

    public override void CalcMinChargeUsed(spellHit spellHit)
    {
        spellHit.minChargeUsed = 1;
    }

    public override void CalcMaxChargeUsed(spellHit spellHit)
    {
        spellHit.maxChargeUsed = Mathf.FloorToInt(spellHit.caster.Get<float>(SyncCharge.charge)); //use all max charge available as multiple of 1
    }

    //use this special override to ensure proper calculation order
    public override void CalcFlatEffectsFromCharge(spellHit spellHit)
    {
        spellHit.power_nonDamage *= spellHit.chargeUsed; //multiply ability for every charge used
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void CalcCastFlatDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.dodgeChance += selfStatusEffect.power_nonDamage;
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //would be flat because we're not multiplying dodge chance, only adding it
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueConsumeChargeIncreaseDodge.baseAmount, T_RogueConsumeChargeIncreaseDodge.amountPerLevel);
    }

} //end spell S_RogueConsumeChargeIncreaseDodge



public class S_RogueConsumeChargeMortalStrike : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;

    public TalentItem talent => Item.From<T_RogueConsumeChargeMortalStrike>().asTalentItem;

    public static float _duration;

    public S_RogueConsumeChargeMortalStrike() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        _duration = SpellDuration.config.debuff_long;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Short_Bright_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("BeamDownCloudGreen", TargetType.target, PersistType.duration, false, 2, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void CalcMinChargeUsed(spellHit spellHit)
    {
        spellHit.minChargeUsed = 1;
    }

    public override void CalcMaxChargeUsed(spellHit spellHit)
    {
        spellHit.maxChargeUsed = Mathf.FloorToInt(spellHit.caster.Get<float>(SyncCharge.charge)); //use all max charge available as multiple of 1
    }

    //use this special override to ensure proper calculation order
    public override void CalcFlatEffectsFromCharge(spellHit spellHit)
    {
        spellHit.power_nonDamage *= spellHit.chargeUsed; //multiply ability for every charge used
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        //Ability.SubtractCharge(spellHit.caster, spellHit.chargeUsed); ///spell will do this automatically...
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void CalcCastPerDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        Ability.MortalStrikeEffect(spellHit, selfStatusEffect);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueConsumeChargeMortalStrike.baseAmount, T_RogueConsumeChargeMortalStrike.amountPerLevel);
    }

} //end spell S_RogueConsumeChargeMortalStrike


public class S_RoguePurge : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_RoguePurge>().asTalentItem;


    public S_RoguePurge() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Swoosh_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {

        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        interval = SpellInterval.config.none;
        duration = SpellDuration.config.none;
        cooldown = SpellCooldown.config.fast_rotation;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        StatusEffects.Purge(spellHit, spellHit.numStrikes);
    }

    //AoE that happens on expiration
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.numStrikes = TalentItem.CalcEffectFlat(ref talentLevel, T_RoguePurge.baseNumStrikes, T_RoguePurge.numStrikesPerLevel);
    }

} //end spell S_RoguePurge




public class S_RogueSeedOfCorruption : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;

    public TalentItem talent => Item.From<T_RogueSeedOfCorruption>().asTalentItem;

    public static float _duration; //expose parameter for talent description
    public static float _interval; //expose parameter for talent description

    public Spell aoeSpell; //actually cast/ApplyRaw this when explodes, so we don't thrash the .power variable

    public S_RogueSeedOfCorruption() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Short_Bright_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("RedArcs", TargetType.target, PersistType.whileHasSpellEffect, true, 2, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {
        };


        _duration = SpellDuration.config.damage_medium;
        _interval = SpellInterval.config.medium;
        aoeSpell = Spell.GetSpell<S_RogueSeedOfCorruptionEffectAoE>();
    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.fast;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = _interval;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = false;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, false);
    }

    public override void OnHitDamageOffensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        selfStatusEffect.power_nonDamage -= spellHit.power; //reduce threshold "countdown"
        if (selfStatusEffect.power_nonDamage <= 0)
        { //if finally reached the end of the "threshold"
            TriggerAoE(selfStatusEffect); //create the explosion and remove self from this target
        }
    }

    public override void OnHostDied(spellHit selfStatusEffect)
    {
        TriggerAoE(selfStatusEffect);
    }

    void TriggerAoE(spellHit selfStatusEffect)
    {
        //create the AoE effect and remove this status effect
        //should probably remove it first, then do AoE just in case it starts somehow looping...
        selfStatusEffect.target.StatusEffect_Expire(selfStatusEffect); //remove status effect and call anything possibly listening for this to OnExpire
        aoeSpell.ApplyRaw(selfStatusEffect.caster, selfStatusEffect.target, selfStatusEffect.target.transform.position, selfStatusEffect.target.transform.position);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueSeedOfCorruption.baseAmountDoT, T_RogueSeedOfCorruption.amountPerLevelDoT);
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueSeedOfCorruption.baseAmountThreshold, T_RogueSeedOfCorruption.amountPerLevelThreshold);
    }

} //end spell S_RogueSeedOfCorruption




public class S_RogueSeedOfCorruptionEffectAoE : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => Spell.GetName<S_RogueSeedOfCorruption>() + " Effect (AoE)";
    public override string description => "explode when the target takes a certain amount of damage";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    //public const float baseDuration = 2;
    public TalentItem talent => Item.From<T_RogueSeedOfCorruption>().asTalentItem;

    public S_RogueSeedOfCorruptionEffectAoE() : base() //TODO: add properties
    {


    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        //prop.prefab = WorldFunctions.GetSpellEffect("EMP_Mine_Effect");

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Short_Distorted_1_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("FireExplosion", TargetType.vTarget, PersistType.duration, false, 2, 5)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //doesn't target anything
    }

    public override void CalcBasePower(out int newPower)
    {
        newPower = 0; //will be calculated from MultiplyByTalentLevel
    }

    public override void DoAnimation(spellHit spellHit)
    {

        //TODO: also spawn prefab
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        OnAnimationHit(spellHit);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        AOEs.DoAction(spellHit, OnMyAoE); //do action on all in spherecast
    }

    void OnMyAoE(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, true);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueSeedOfCorruption.baseAmountAoE, T_RogueSeedOfCorruption.amountPerLevelAoE);
    }

} //end spell S_RogueSeedOfCorruptionEffectAoE

