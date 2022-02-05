using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static CreateFX_Params;



public class S_WarriorCover : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Cover";
    public override string description => "redirect " + (100 * _power_nonDamage).ToString("0.0") + "% damage from the target to you";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public const float _power_nonDamage = 0.25f; //how much damage to redirect
    public static float _duration; //expose to another talent which will make this also increase damage

    public S_WarriorCover() : base() //TODO: add prop
    {
    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();

        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");


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
            new CreateFX_Params("GlowZoneBlue", TargetType.target, PersistType.whileHasSpellEffect, true, 2f),
            new CreateFX_Params("SparkleRed", TargetType.caster, PersistType.whileHasSpellEffect, true, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };


        _duration = SpellDuration.config.buff_short;
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
        power_nonDamage = _power_nonDamage;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal_NotSelf(target, caster); //only castable on friendlies
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true); //add status effect to target
    }

    /// <summary>
    /// called as a status effect on target it is placed
    /// </summary>
    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        //reduce effect of the target ability, make a new attack with the amount
        spellHit newAttack = new spellHit(spellHit, spellHit.caster, selfStatusEffect.caster, spellHit.origin, spellHit.vTarget); //make new attack out of old at warrior
        newAttack.power = (int)(spellHit.power * (selfStatusEffect.power_nonDamage)); //the amount redirected
        spellHit.power -= newAttack.power;
        Damage.Attack(newAttack, false, true); //attack hits caster of buff instead, but doesn't trigger OnHit effects to prevent infinite looping
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }
} //end spell S_WarriorCover



public class S_WarriorCrippleShotgun : Spell
{
    //Vector3 beamRotation = new Vector3(0, 0.1f, 0);
    public override string nameFormatted => "Crippling Shot";
    public override string description => "sprays a volly of crippling energy in a direction, crippling the speed of whatever it hits";
    public override Type spellTargeterType => typeof(TargetShootForward);
    public override int spellGUIType => SpellGUIType.debuff;

    const float slowAmount = 3f; //slow effect applied to shotgun, divides by 1.5

    public S_WarriorCrippleShotgun() : base()
    {
    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {

        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetSpellEffect("BulletTracer");
        prop.length = 10;
        prop.offset = 0;
        prop.startRadius = 10;
        prop.calcStartAnchor = GetSpellAnchor;


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

        };
        fx_animationHit = new CreateFX_Params[]
        {
            new CreateFX_Params("SparksRed", TargetType.target, PersistType.duration, false, 2, 0.5f)
        };


    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.debuff_short;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.extremely_fast_rotation;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.projectiles_low;
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

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
    }

    Transform GetSpellAnchor(spellHit spellHit)
    {
        return spellHit.caster.firePoints[0];
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {

    }

    public override void DoAnimation(spellHit spellHit)
    {
        //WorldFunctions.GetSpellEffect("BeamBlueStart"), WorldFunctions.GetSpellEffect("BeamBlueEnd"), WorldFunctions.GetSpellEffect("BeamBlue"), spellHit.caster.transform, spellHit, spellHit.moveSpeed, 1f, spellHit.target.transform, 3
        GunCast.XZ.ShotgunPoints(spellHit, prop.length, prop.offset, prop.prefab, GetSpellAnchor(spellHit), null);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, true);
        StatusEffects.DivideSpeed(spellHit, slowAmount, true, true, false);
    }

} // S_WarriorCrippleShotgun



public class S_WarriorYoink : Spell
{
    //Vector3 beamRotation = new Vector3(0, 0.1f, 0);
    public override string nameFormatted => "Tractor Beam";
    public override string description => "yoinks the target hither";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    const float yoinkOffset = 1f; //how far away to offset from self when bringing the target to caster

    public S_WarriorYoink() : base()
    {
    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {

        prop = new SpellStaticProperties();

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
            new CreateFX_Params("IceFloorTrail", TargetType.target, PersistType.duration, true, 2, 1f)
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
        cooldown = SpellCooldown.config.slow_rotation;
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

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetEnemyNonBuilding(target, caster); //fail if either a non-moving entity or not attackable
    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
    }

    Transform GetSpellAnchor(spellHit spellHit)
    {
        return spellHit.caster.transform;
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        Ability.Yoink(spellHit, yoinkOffset);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end S_WarriorYoink



public class S_WarriorHoldTheLine : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Hold the Line";
    public override string description => "reduces incoming damage for duration";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public float baseReduction = 0.3f;

    public static float _duration; //expose to other talent which also gives charge regen to nearby allies

    public S_WarriorHoldTheLine() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();

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
            new CreateFX_Params("AuraChargeBlue", TargetType.target, PersistType.whileHasSpellEffect, true, 2)

        };
        fx_animationHit = new CreateFX_Params[]
        {

        };


        _duration = SpellDuration.config.buff_short;
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
        power_nonDamage = baseReduction;

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
        //StatusEffects.MultMaxHealth(spellHit, spellHit.power_nonDamage, true, false, false);
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void CalcCastPerDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.power = Mathf.RoundToInt(spellHit.power * 1f / (1 + selfStatusEffect.power_nonDamage));
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_WarriorHoldTheLine



public class S_WarriorDoT : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Bombard";
    public override string description => "shell the target for a brief period, causing damage over time";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;

    //public TalentItem talent => Item.From<T_WarriorDoT>().asTalentItem;

    public static float _duration;

    public S_WarriorDoT() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        _duration = SpellDuration.config.damage_medium;

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
            new CreateFX_Params("RedSparksLong", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
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
        interval = SpellInterval.config.medium;
        cooldown = SpellCooldown.config.slow_rotation;
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

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        //spellHit.spellTypes.Add(spellType.physical);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, false);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    /*
    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_WarriorDoT.baseAmount, T_WarriorDoT.increasePerLevel);
    }
    */
} //end spell S_WarriorDoT



public class S_WarriorBackHomingProjectiles : Spell
{
    public const float circleRadius = 3f;
    public const int count = 5;
    public const float moveSpeed = 0.1f;
    public const int arcDegrees = 90;
    public float arcOffset = 0;
    public const float turnSpeed = 0.6f;

    public override string nameFormatted => "Pigeon Bombs";
    public override string description => "shoot 3 homing missiles from the back of your ship";
    public override Type spellTargeterType => typeof(TargetShootForward);
    public override int spellGUIType => SpellGUIType.none;

    public S_WarriorBackHomingProjectiles() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetSpellEffect("spaceMissiles_001");
        prop.rotationType = SpellStaticProperties.RotationType.facingAwayOrigin;
        prop.homingType = SpellStaticProperties.HomingType.enemy;

        prop.length = 60;
        prop.startRadius = 0.1f;
        prop.offset = 180 + 45;

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "spells_default";
        sfx_castFinished = "8BIT_RETRO_Fire_Blaster_Short_Glide_mono";
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
            new CreateFX_Params("RedFireballShort", TargetType.target, PersistType.duration, true, 2)
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
        cooldown = SpellCooldown.config.extremely_fast_rotation;
        range = SpellRange.config.na;
        radius = SpellRadius.config.large;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.projectiles_low;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.slow;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.dull;
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

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {

    }

    //called after creating the projectile
    void ModProjectile(spellHit spellHit, GameObject newObj)
    {
        newObj.transform.localScale = spellHit.caster.GetOrDefault<Vector3>(SyncScale.scale);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        ProjectileCreate.TwoD.XZ.FromArc(spellHit, prop.prefab, prop.length, prop.offset, spellHit.caster.transform.position, prop.startRadius, prop.homingType, prop.rotationType, turnSpeed, ModProjectile);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, true);
    }

} //end spell S_WarriorBackHomingProjectiles

