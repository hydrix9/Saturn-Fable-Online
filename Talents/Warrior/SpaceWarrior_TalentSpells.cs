using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CreateFX_Params;


public class S_WarriorBarrier : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Barrier";
    public override string description => "mmvvmmvmvmvmmvvvvvmmmvvvmmmvmvm";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorBarrier>().asTalentItem;

    public S_WarriorBarrier() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


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
        duration = SpellDuration.config.cc_large;
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

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {

    }

    public override void DoAnimation(spellHit spellHit)
    {
        ProjectileCreate.TwoD.XZ.FromArc(spellHit, prop.prefab, prop.length, prop.offset, spellHit.caster.transform.position, prop.startRadius, prop.homingType, prop.rotationType);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {

    }
} //end spell S_WarriorBarrier






public class S_WarriorMoon : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Moon";
    public override string description => "orbits the caster through sheer love";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorMoon>().asTalentItem;


    public S_WarriorMoon() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Reverse_Repeating_Fading_mono";
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
        duration = SpellDuration.config.buff_medium;
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

    }


    public override void DoAnimation(spellHit spellHit)
    {
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {

    }
} //end spell S_WarriorMoon




public class S_WarriorCripplingGaze : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description; //immobilize the target
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;

    public TalentItem talent => Item.From<T_WarriorCripplingGaze>().asTalentItem;

    public static float _duration; //expose to description

    public S_WarriorCripplingGaze() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();

        _duration = SpellDuration.config.cc_medium;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Hit_Bump_Deep_Zap_mono";
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
            new CreateFX_Params("RedShrink", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
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
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.slow_rotation;
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
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //doesn't target anything
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.Immobilize(spellHit, true, false);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {

    }
} //end spell S_WarriorCripplingGaze




public class S_WarriorMoonShatter : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Moon Shatter";
    public override string description => "dokkan!";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorMoonShatter>().asTalentItem;

    public S_WarriorMoonShatter() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Wobble_mono";
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
        return CalcValidTargetSelf(target, caster);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {

    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {

    }
} //end spell S_WarriorMoonShatter



public class S_WarriorRedBadge : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description; //increase max health for duration
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorRedBadge>().asTalentItem;

    public static float _duration;

    public S_WarriorRedBadge() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        _duration = SpellDuration.config.buff_medium;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Flash_Aggressive_mono";
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
            new CreateFX_Params("PowerupIconHeart", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
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
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.fast_buff_rotation;
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
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster); //doesn't target anything
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.MultMaxHealth(spellHit, spellHit.power_nonDamage, true, true, false);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorRedBadge.healthPercent, T_WarriorRedBadge.amountPerlevel);
    }
} //end spell S_WarriorRedBadge




public class S_WarriorNeverBackDown : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description; //increase max health for duration
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorNeverBackDown>().asTalentItem;

    public S_WarriorNeverBackDown() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
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
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.large;
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
        canBeDispelled = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster); //doesn't target anything
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.Aura(spellHit, false, true, false, CalcValidTargetHeal_NotSelf);
    }

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.power = (int) (spellHit.power * selfStatusEffect.power_nonDamage);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_WarriorNeverBackDown.baseAmount, T_WarriorNeverBackDown.amountPerLevel);
    }
} //end spell S_WarriorNeverBackDown




public class S_WarriorInstincts : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted; //dodge the next ability cast on you
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorInstincts>().asTalentItem;

    public S_WarriorInstincts() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Flash_Fading_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "8BIT_RETRO_Powerup_Spawn_Flash_Fading_mono";
        sfxPitch_animationHit = new PitchAnimate.Params(1, 1.2f, 1, 1.2f, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("CheckpointBlueFast", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
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
        duration = SpellDuration.config.none; //will set later..
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.slow_rotation;
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
        return CalcValidTargetSelf(target, caster); //doesn't target anything
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void OnCalcHitChanceDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.dodgeChance < 99 && spellHit.caster != selfStatusEffect.caster)
        { //if don't already have a non-sane dodge chance.... this allows multiple spells of this type to oossibly be used simultaneously
            spellHit.dodgeChance += 999; //dodge this attack no matter what, 1.0F is perfect dodge chance
            selfStatusEffect.caster.StatusEffect_Consume(spellHit, selfStatusEffect); //consume the effect
        }
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorInstincts.baseAmount, T_WarriorInstincts.amountPerLevel);
    }
} //end spell S_WarriorInstincts



public class S_WarriorHeavyHanded : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => "immobilized";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;

    public TalentItem talent => Item.From<T_WarriorHeavyHanded>().asTalentItem;

    public S_WarriorHeavyHanded() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Explosion_Short_Dirty_mono";
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
            new CreateFX_Params("PurpleSparks", TargetType.target, PersistType.duration, true, 2, 1)
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
    public override void CalcBasePower(out int newPower)
    {
        base.CalcBasePower(out newPower);
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //doesn't target anything
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.Immobilize(spellHit, true, false);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }
} //end spell S_WarriorHeavyHanded



public class S_WarriorIncreaseTauntRadiusAtCostHealth : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => "hurt self but not in confusion! IN RAGE";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorIncreaseTauntRadiusAtCostHealth>().asTalentItem;

    public S_WarriorIncreaseTauntRadiusAtCostHealth() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Rumble_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 0.9f, 1, 0.9f, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("RedSparks", TargetType.target, PersistType.duration, true, 2, 1)
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
        power_nonDamage = T_WarriorIncreaseTauntRadiusAtCostHealth.costPercentage;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }
    public override void CalcBasePower(out int newPower)
    {
        base.CalcBasePower(out newPower);
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster); //doesn't target anything
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //hurt self for % of max health... the increased radius is actually just a SpellMod on talent
        spellHit.power = (int)(spellHit.caster.Get<int>(SyncHealth.maxHealth) * spellHit.power_nonDamage);
        Damage.Attack(spellHit, false, false);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }
} //end spell S_WarriorIncreaseTauntRadiusAtCostHealth


public class S_WarriorChallengingAura : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted; //redirect damage in area toward yourself
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;


    public TalentItem talent => Item.From<T_WarriorChallengingAura>().asTalentItem;

    public S_WarriorChallengingAura() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


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
            new CreateFX_Params("AuraChargeRed", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
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
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }
    public override void CalcBasePower(out int newPower)
    {
        newPower = 0; //don't initially calculate power...
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster); //doesn't target anything
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.Aura(spellHit, false, true, true, CalcValidTargetHeal_NotSelf);
    }

    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.target != selfStatusEffect.caster)
        { //don't do effect on caster themself
            spellHit newAttack = new spellHit(spellHit, spellHit.caster, selfStatusEffect.caster, spellHit.origin, spellHit.vTarget); //make new attack out of old at warrior
            newAttack.power = (int)(spellHit.power * (selfStatusEffect.power_nonDamage)); //the amount redirected
            spellHit.power -= newAttack.power; //subtract amount redirected
            Damage.Attack(newAttack, false, true); //deal damage to warrior for amount redirected
        }
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_WarriorChallengingAura.baseAmount, T_WarriorChallengingAura.increasePerLevel);
    }
} //end spell S_WarriorChallengingAura


public class S_WarriorShed : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => "Shed";
    public override string description => "removes a debuff from yourself";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorShed>().asTalentItem;

    public S_WarriorShed() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


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
            new CreateFX_Params("BeamupCloudBlue", TargetType.target, PersistType.duration, true, 2, 1)
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
        cooldown = SpellCooldown.config.slow_rotation;
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
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }
    public override void CalcBasePower(out int newPower)
    {
        base.CalcBasePower(out newPower);
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.Cleanse(spellHit, spellHit.numStrikes);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.numStrikes += TalentItem.CalcEffectFlat(ref talentLevel, T_);
    }
} //end spell S_WarriorShed


public class S_WarriorCollateralMendingEffect : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Collateral Mending";
    public override string description => "heals yourself after removing a debuff";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorCollateralMending>().asTalentItem;

    public S_WarriorCollateralMendingEffect() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


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
            new CreateFX_Params("HealOnceLowPlus", TargetType.target, PersistType.duration, true, 2, 1)
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
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { spellType.healing };
    }
    public override void CalcBasePower(out int newPower)
    {
        base.CalcBasePower(out newPower);
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster); //doesn't target anything
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        spellHit.power += (int)(spellHit.power_nonDamage * spellHit.caster.maxHealth);
        Damage.Heal(spellHit, true, true);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_WarriorCollateralMending.baseAmount, T_WarriorCollateralMending.increasePerLevel);
    }
} //end spell S_WarriorCollateralMendingEffect



public class S_WarriorOnDamagedAddShield : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorOnDamagedAddShield>().asTalentItem;

    public static float _duration;

    public S_WarriorOnDamagedAddShield() : base() //TODO: add properties
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
            new CreateFX_Params("SimpleZoneBlue", TargetType.target, PersistType.whileHasSpellEffect, true, 1.2f, 1)
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
        spellTypes = new List<int> {  };
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

    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        Ability.BarrierEffect(spellHit, selfStatusEffect);
    }

    //AoE that happens on expiratoin
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorOnDamagedAddShield.baseAmount, T_WarriorOnDamagedAddShield.amountPerLevel);
    }

} //end spell S_WarriorOnDamagedAddShield


public class S_WarriorOnDodgeGainMoveSpeed : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorOnDodgeGainMoveSpeed>().asTalentItem;

    public S_WarriorOnDodgeGainMoveSpeed() : base() //TODO: add properties
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
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.buff_short;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = 1;
        maxStacks = 1; //calc from talent level
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
        spellTypes = new List<int> {};
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
        StatusEffects.MultSpeed(spellHit, spellHit.power_nonDamage, true, true, false);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectPer(ref talentLevel, T_WarriorOnDodgeGainMoveSpeed.baseAmount, T_WarriorOnDodgeGainMoveSpeed.amountPerLevel);
        selfCast.maxStacks = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorOnDodgeGainMoveSpeed.baseMaxStacks, T_WarriorOnDodgeGainMoveSpeed.maxStacksPerLevel);
    }

} //end spell S_WarriorOnDodgeGainMoveSpeed



public class S_WarriorTaunt : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorTaunt>().asTalentItem;

    public S_WarriorTaunt() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Rumble_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1.1f, 1.1f, 1, 1.1f, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("SpawnRed", TargetType.target, PersistType.duration, true, 2, 1)
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
        range = SpellRange.config.na; //uses radius not range
        radius = SpellRadius.config.large;
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
        spellTypes = new List<int> {  };
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
        //StatusEffects.AddSpeed(spellHit, spellHit.power_nonDamage, true, true, false);
        AOEs.DoAction(spellHit, Ability.Taunt, CalcValidTargetDamage);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.duration += TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorTaunt.baseAmount, T_WarriorTaunt.amountPerLevel);
    }

} //end spell S_WarriorTaunt



public class S_WarriorBuffFriendliesBehind : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorBuffFriendliesBehind>().asTalentItem;

    public S_WarriorBuffFriendliesBehind() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
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
        canBeDispelled = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> {  };
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
        //TODO- implement calculating power from power_nonDamage
        //StatusEffects.AddSpeed(spellHit, spellHit.power_nonDamage, true, true, false);
        StatusEffects.Aura(spellHit, false, true, true, CalcValidTargetHeal_NotSelf);
    }

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (Ability.IsBehindTarget(selfStatusEffect.caster.transform, selfStatusEffect.target.transform))
            spellHit.power = (int)(spellHit.power * spellHit.power_nonDamage);
    }   

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectPer(ref talentLevel, T_WarriorBuffFriendliesBehind.baseAmount, T_WarriorBuffFriendliesBehind.amountPerLevel);
    }

} //end spell S_WarriorBuffFriendliesBehind



public class S_WarriorShotgunMortalStrikeEffect : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;

    public TalentItem talent => Item.From<T_WarriorShotgunMortalStrikeEffect>().asTalentItem;

    public static float _duration;

    public S_WarriorShotgunMortalStrikeEffect() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        _duration = SpellDuration.config.debuff_short;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
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
        power_nonDamage = 0; //will calc from talent

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
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorShotgunMortalStrikeEffect.baseAmount, T_WarriorShotgunMortalStrikeEffect.amountPerLevel);
    }

} //end spell S_WarriorShotgunMortalStrikeEffect



public class S_WarriorOnDamagedGenerateChargeEffect : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorOnDamagedGenerateCharge>().asTalentItem;

    public S_WarriorOnDamagedGenerateChargeEffect() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Coin_Collect_Two_Note_Twinkle_Fast_mono";
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
        power_nonDamage = 0; //calculated from talent level

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> {  };
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
        Ability.AddCharge(spellHit.caster, ref spellHit.power_nonDamage);
    }


    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorOnDamagedGenerateCharge.baseAmount, T_WarriorOnDamagedGenerateCharge.amountPerLevel);
    }

} //end spell S_WarriorOnDamagedGenerateChargeEffect


public class S_WarriorOnUseChargeGainMoveSpeed : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorOnUseChargeGainMoveSpeed>().asTalentItem;

    public S_WarriorOnUseChargeGainMoveSpeed() : base() //TODO: add properties
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
        spellTypes = new List<int> {  };
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
        StatusEffects.MultSpeed(spellHit, spellHit.power_nonDamage, true, true, false);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_WarriorOnUseChargeGainMoveSpeed.baseAmount, T_WarriorOnUseChargeGainMoveSpeed.amountPerLevel);
    }
} //end spell S_WarriorOnUseChargeGainMoveSpeed


    //separate spell for making cover also increase damage
public class S_WarriorCoverSpellIncreaseDamageEffect : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorCoverSpellIncreasesDamage>().asTalentItem;

    public S_WarriorCoverSpellIncreaseDamageEffect() : base() //TODO: add properties
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
        duration = S_WarriorCover._duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.very_high; //technically don't need this since TriggerSpell_TalentMods will cast this using ApplyRaw
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
        spellTypes = new List<int> {  };
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

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.power = (int)(spellHit.power * selfStatusEffect.power_nonDamage);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_WarriorCoverSpellIncreasesDamage.baseAmount, T_WarriorCoverSpellIncreasesDamage.amountPerLevel);
    }

} //end spell S_WarriorCoverSpellIncreaseDamageEffect




//separate spell for making cover also increase damage
public class S_WarriorYoinkDelayedAoEEffect : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorYoinkDelayedAoE>().asTalentItem;

    public static float _duration; //expose to talent description

    public S_WarriorYoinkDelayedAoEEffect() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
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


        _duration = SpellDuration.config.damage_short; //delay before AoE explodes
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
        spellTypes = new List<int> {  };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster); //will calculate again during AoE.DoAction
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void OnExpireStatusEffect_Caster(spellHit expired)
    { //when this spell expires
        OnAnimationHit(expired);
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
        //selfCast.duration += TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorYoinkDelayedAoEEffect.baseAmount, T_WarriorYoinkDelayedAoEEffect.amountPerLevel);
    }

} //end spell S_WarriorYoinkDelayedAoEEffect

//separate spell for making cover also increase damage
public class S_WarriorOnHitChanceGiveChargeAlly : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorOnHitChanceGiveChargeAlly>().asTalentItem;

    public S_WarriorOnHitChanceGiveChargeAlly() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
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
            new CreateFX_Params("BeamupCloudPink", TargetType.target, PersistType.duration, true, 0.5f, 1)
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
        radius = SpellRadius.config.large;
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
        spellTypes = new List<int> {  };
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
        Entity[] targets = AOEs.GetAreaFriendlyEntities_NotSelf(spellHit.caster, spellHit.range);
        if (targets.Length > 0) //if found nearby target
            OnAnimationHit(new spellHit(spellHit, spellHit.caster, targets.Random(), spellHit.origin, spellHit.vTarget)); //cast ability on them
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        Ability.AddCharge(spellHit.target, ref spellHit.power_nonDamage);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorOnHitChanceGiveChargeAlly.baseAmount, T_WarriorOnHitChanceGiveChargeAlly.amountPerLevel);
    }

} //end spell S_WarriorOnHitChanceGiveChargeAlly


public class S_WarriorGiftChargeAlly : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_WarriorGiftChargeAlly>().asTalentItem;

    public S_WarriorGiftChargeAlly() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Flash_Fading_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1.1f, 1.1f, 1, 1.1f, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("BeamupCloudPink", TargetType.target, PersistType.duration, true, 2f, 1)
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
        cooldown = SpellCooldown.config.fast_rotation;
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
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> {  };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal_NotSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }


    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {

        Ability.AddCharge(spellHit.target, spellHit.chargeUsed); //add charge to target

        //it should do this automatically..
        //Ability.SubtractCharge(spellHit.caster, spellHit.chargeUsed);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void CalcMinChargeUsed(spellHit spellHit)
    {
        //something based off config from talent
        spellHit.minChargeUsed = T_WarriorGiftChargeAlly.baseAmount;
    }

    public override void CalcMaxChargeUsed(spellHit spellHit)
    {
        spellHit.maxChargeUsed = spellHit.power_nonDamage;
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorGiftChargeAlly.baseAmount, T_WarriorGiftChargeAlly.amountPerLevel);
    }

} //end spell S_WarriorGiftChargeAlly



public class S_WarriorUseChargeShieldOthers : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorUseChargeShieldOthers>().asTalentItem;

    public static float _duration; //expose to talent description

    public S_WarriorUseChargeShieldOthers() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Reverse_Repeating_Fading_mono";
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
            new CreateFX_Params("SimpleZoneBlueRed", TargetType.target, PersistType.whileHasSpellEffect, true, 1.2f, 1)
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
        cooldown = SpellCooldown.config.slow_rotation;
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
        spellTypes = new List<int> {  };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal_NotSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void CalcMaxChargeUsed(spellHit spellHit)
    {
        //round to nearest one?...
        spellHit.maxChargeUsed =  Mathf.FloorToInt(spellHit.caster.Get<float>(SyncCharge.charge));
    }

    public override void CalcMinChargeUsed(spellHit spellHit)
    {
        //something based off config from talent
        spellHit.minChargeUsed = 1;
    }

    //use this special override to ensure proper calculation order
    public override void CalcFlatEffectsFromCharge(spellHit spellHit)
    {
        spellHit.power_nonDamage *= spellHit.chargeUsed; //shield per charge used
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
        //will automatically subtract charge...
        //Ability.SubtractCharge(spellHit.caster, spellHit.chargeUsed);  //subtract charge from warrior
    }

    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        Ability.BarrierEffect(spellHit, selfStatusEffect);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorUseChargeShieldOthers.baseAmount, T_WarriorUseChargeShieldOthers.amountPerLevel);

    }

} //end spell S_WarriorUseChargeShieldOthers



public class S_WarriorUseChargeShieldSelf : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From <T_WarriorUseChargeShieldSelf>().asTalentItem;

    public static float _duration; //expose to talent description

    public S_WarriorUseChargeShieldSelf() : base() //TODO: add properties
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
            new CreateFX_Params("SimpleZoneBluePurple", TargetType.target, PersistType.whileHasSpellEffect, true, 1.2f, 1)

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
        cooldown = SpellCooldown.config.slow_rotation;
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
        power_nonDamage = 0; //calculated from talent

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> {  };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal_NotSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    public override void CalcMaxChargeUsed(spellHit spellHit)
    {
        //round to nearest one?...
        spellHit.maxChargeUsed = Mathf.FloorToInt(spellHit.caster.Get<float>(SyncCharge.charge));
    }

    public override void CalcMinChargeUsed(spellHit spellHit)
    {
        //something based off config from talent
        spellHit.minChargeUsed = 1;
    }

    //use this special override to ensure proper calculation order
    public override void CalcFlatEffectsFromCharge(spellHit spellHit)
    {
        spellHit.power_nonDamage *= spellHit.chargeUsed; //shield per charge used
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
        //will automatically subtract charge...
        //Ability.SubtractCharge(spellHit.caster, spellHit.chargeUsed);  //subtract charge from warrior
    }

    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        Ability.BarrierEffect(spellHit, selfStatusEffect);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorUseChargeShieldSelf.baseAmount, T_WarriorUseChargeShieldSelf.amountPerLevel);
    }

} //end spell S_WarriorUseChargeShieldSelf

public class S_ShieldOthersGiveChargeEffect : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_ShieldOthersGiveCharge>().asTalentItem;

    public S_ShieldOthersGiveChargeEffect() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
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
        spellTypes = new List<int> {  };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal_NotSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }


    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //technically wouldn't be spellHit.chargeGained since that is for the caster
        Ability.AddCharge(spellHit.target, spellHit.power_nonDamage); //add charge to target

        //Ability.SubtractCharge(spellHit.caster, spellHit.chargeUsed);  //subtract charge from warrior
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_ShieldOthersGiveCharge.baseAmount, T_ShieldOthersGiveCharge.amountPerLevel);
    }

} //end spell S_ShieldOthersGiveChargeEffect


public class S_WarriorHoldTheLineChargePerSecondAllies : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_WarriorHoldTheLineChargePerSecondAllies>().asTalentItem;

    public S_WarriorHoldTheLineChargePerSecondAllies() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
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
            new CreateFX_Params("Electricity", TargetType.target, PersistType.whileHasSpellEffect, true, 2f, 1)
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
        duration = S_WarriorHoldTheLine._duration;
        interval = T_WarriorHoldTheLineChargePerSecondAllies.interval; //should be 1
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.large;
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
        spellTypes = new List<int> {  };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster); //still uses a different one when duing AoE.DoAction
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }


    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
        //Ability.SubtractCharge(spellHit.caster, spellHit.chargeUsed);  //subtract charge from warrior
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        OnAnimationHit(spellHit);
        
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        AOEs.DoAction(spellHit, OnMyAoE, CalcValidTargetHeal);
    }

    void OnMyAoE(spellHit spellHit)
    {
        Ability.AddCharge(spellHit.target, spellHit.power_nonDamage); //add charge to target
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorHoldTheLineChargePerSecondAllies.baseAmount, T_WarriorHoldTheLineChargePerSecondAllies.amountPerLevel);

        //allow this other talent which increases the duration of Hold the Line to also increase the effect of this...
        //might be able to call spell.GetFullStats on S_WarriorHoldTheLine if you need to extend this behaviour
        int increasedDurationLevel = selfCast.caster.GetTalentLevel(Item.From<T_WarriorHoldTheLineDuration>().id);
        selfCast.duration = S_WarriorHoldTheLine._duration + TalentItem.CalcEffectFlat(ref increasedDurationLevel, T_WarriorHoldTheLineDuration.baseAmount, T_WarriorHoldTheLineDuration.amountPerLevel);
    }

} //end spell S_WarriorHoldTheLineChargePerSecondAllies

