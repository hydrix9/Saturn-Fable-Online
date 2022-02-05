using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CreateFX_Params;



public class S_EngiMinefield : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public const float baseDuration = 2;

    public TalentItem talent => Item.From<T_EngiMinefield>().asTalentItem;
    Spell mineTriggerEffect;

    public S_EngiMinefield() : base() //TODO: add properties
    {


    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        mineTriggerEffect = Spell.GetSpell<S_EngiMinefield_TriggerEffect>();
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
        //selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, S_EngiMinefield.baseAmount, S_EngiMinefield.amountPerLevel);
    }

} //end spell S_EngiMinefield



public class S_EngiMinefield_TriggerEffect : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiMinefield>().asTalentItem;

    //public const float baseDuration = 2;

    public S_EngiMinefield_TriggerEffect() : base() //TODO: add properties
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
        /*
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("EMP_Mine_Effect", TargetType.target, PersistType.duration, false, 1, 15f)
        };
        */
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("GroundSlamRed", TargetType.vTarget, PersistType.duration, false, 5f, 5)
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
        radius = SpellRadius.config.medium;
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
        //StatusEffects.Immobilize(spellHit, true, false);
        Damage.Attack(spellHit, true, true);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiMinefield.baseAmount, T_EngiMinefield.amountPerLevel);
    }

} //end spell S_EngiMinefield_TriggerEffect




public class S_EngiOnMinefieldHitRoot : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.debuff;

    public TalentItem talent => Item.From<T_EngiOnMinefieldHitRoot>().asTalentItem;

    public S_EngiOnMinefieldHitRoot() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none; //should be set from MultiplyByTalentLevel
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.Immobilize(spellHit, true, false);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.duration = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiOnMinefieldHitRoot.baseAmount, T_EngiOnMinefieldHitRoot.amountPerLevel);
    }

} //end spell S_EngiOnMinefieldHitRoot


public class S_EngiHealZone : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public static float _duration; //expose parameter for talent description
    public static float _interval; //expose parameter for talent description
    public static float _radius; //expose parameter for talent description

    public TalentItem talent => Item.From<T_EngiHealZone>().asTalentItem;


    public S_EngiHealZone() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };

        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";

        _interval = SpellInterval.config.fast;
        _duration = SpellDuration.config.effect_medium;
        _radius = SpellRadius.config.large;
    }


    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.high;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = _interval;
        cooldown = SpellCooldown.config.fast_buff_rotation;
        range = SpellRange.config.na;
        radius = _radius;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }


    public override void DoAnimation(spellHit spellHit)
    {
        AOEs.Zone_Action(spellHit, HealEffect);
    }

    //what gets called on each entity within radius
    void HealEffect(spellHit newAttack)
    {
        Damage.Heal(newAttack, true, true);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiHealZone.baseAmount, T_EngiHealZone.amountPerLevel);
    }

} //end spell S_EngiHealZone




public class S_EngiGunnerDrone : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.summoned;

    int turretID;

    public TalentItem talent => Item.From<T_EngiGunnerDrone>().asTalentItem;

    public S_EngiGunnerDrone() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };
        prop.calcCenter = CalcCenter;


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";

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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

    Vector3 CalcCenter(spellHit spellHit)
    {
        return spellHit.caster.transform.forward * 3;
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //TODO- give the turret spells from this 'params' arg
        SummonCreate.Single(spellHit, prop.prefab.name, spellHit.caster.transform.position + spellHit.caster.transform.forward, null);
    }

} //end spell S_EngiGunnerDrone



public class S_EngiSlowDrone : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.summoned;

    int turretID;

    public TalentItem talent => Item.From<T_EngiSlowDrone>().asTalentItem;

    public S_EngiSlowDrone() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };
        prop.calcCenter = CalcCenter;


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

    Vector3 CalcCenter(spellHit spellHit)
    {
        return spellHit.caster.transform.forward * 3;
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //TODO- give the turret spells from this 'params' arg
        SummonCreate.Single(spellHit, prop.prefab.name, spellHit.caster.transform.position + spellHit.caster.transform.forward, null);
    }

} //end spell S_EngiSlowDrone


public class S_EngiOnSummonBuffSummoned : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiOnSummonBuffSummoned>().asTalentItem;

    public S_EngiOnSummonBuffSummoned() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";

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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //probably calc effects manually after fetching talent level
        //StatusEffects.AddMaxHealth(spellHit, )
        //StatusEffects.AddSpeed(spellHit, )
        //StatusEffects.AddScale(spellHit, )
    }

} //end spell S_EngiOnSummonBuffSummoned




public class S_EngiOnSacrificeRecreate : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiOnSacrificeRecreate>().asTalentItem;

    public S_EngiOnSacrificeRecreate() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {


        // spellHit.target.GetComponent<Summoned>().spellHit.spell.name
        Spell.GetSpell("SummonName").ApplyRaw(spellHit.caster, spellHit.caster, default, default);
    }

} //end spell S_EngiOnSacrificeRecreate


public class S_EngiOnDamagedRobotFrenzy : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiOnDamagedRobotFrenzy>().asTalentItem;

    public S_EngiOnDamagedRobotFrenzy() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";

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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.power = (int)(spellHit.power * selfStatusEffect.power_nonDamage);

        //do other frenzy stuff here
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        spellHit.caster.GetComponent<SyncSummons>().ApplyToAllSummoned(spellHit, AddEffectToRobot);
    }

    void AddEffectToRobot(spellHit newSpellHit)
    {
        StatusEffects.TryAdd(newSpellHit, true, true, false);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

} //end spell S_EngiOnDamagedRobotFrenzy




public class S_EngiOnUseChargeHealAllRobots : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiOnUseChargeHealAllRobots>().asTalentItem;

    public S_EngiOnUseChargeHealAllRobots() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";

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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        spellTypes = new List<int> { spellType.healing };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        spellHit.caster.GetComponent<SyncSummons>().ApplyToAllSummoned(spellHit, AddEffectToRobot);
    }

    void AddEffectToRobot(spellHit newSpellHit)
    {
        Damage.Heal(newSpellHit, true, true);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

} //end spell S_EngiOnUseChargeHealAllRobots



public class S_EngiOnCritRegenEnergy : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiOnCritRegenEnergy>().asTalentItem;

    public S_EngiOnCritRegenEnergy() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        spellHit.target.Addition<int>(SyncEnergy.energy, (int)spellHit.power_nonDamage);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

} //end spell S_EngiOnCritRegenEnergy





public class S_EngiOnHitBurnDamage : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;

    public TalentItem talent => Item.From<T_EngiOnHitBurnDamage>().asTalentItem;

    public S_EngiOnHitBurnDamage() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, false, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, false);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

} //end spell S_EngiOnHitBurnDamage




public class S_EngiCleanse : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;


    ///TODO- SET THIS
    public TalentItem talent => Item.From<T_RoguePurge>().asTalentItem;


    public S_EngiCleanse() : base() //TODO: add properties
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
        StatusEffects.Cleanse(spellHit, spellHit.numStrikes);
    }

    //AoE that happens on expiration
    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //TODO- SET ACTUAL TALENT
        selfCast.numStrikes = TalentItem.CalcEffectFlat(ref talentLevel, T_RoguePurge.baseNumStrikes, T_RoguePurge.numStrikesPerLevel);
    }

} //end spell S_EngiCleanse



public class S_EngiOnCleanseShield : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiOnCleanseShield>().asTalentItem;

    public S_EngiOnCleanseShield() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, false, false);
    }

    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        Ability.BarrierEffect(spellHit, selfStatusEffect);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

} //end spell S_EngiOnCleanseShield



public class S_EngiOnHealShield : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiOnHealShield>().asTalentItem;

    public S_EngiOnHealShield() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, false, false);
    }

    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        Ability.BarrierEffect(spellHit, selfStatusEffect);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

} //end spell S_EngiOnHealShield


public class S_EngiDelayedHeal : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiDelayedHeal>().asTalentItem;

    public static float _duration; //expose to talent description


    public S_EngiDelayedHeal() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";


    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        _duration = SpellDuration.config.buff_short;

        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.medium_rotation;
        range = SpellRange.config.high;
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void OnExpireStatusEffect_Caster(spellHit expired)
    { //when this spell expires
        Damage.Heal(expired, true, false);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiDelayedHeal.baseAmount, T_EngiDelayedHeal.amountPerLevel);
    }

} //end spell S_EngiDelayedHeal



public class S_EngiDelayedBomb : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiDelayedBomb>().asTalentItem;

    public S_EngiDelayedBomb() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
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
    }

} //end spell S_EngiDelayedBomb



public class S_EngiUniqueRobotBuff : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiUniqueRobotBuff>().asTalentItem;

    public S_EngiUniqueRobotBuff() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, true);
    }

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        //spellHit.power = (int)(spellHit.power * (selfStatusEffect.power_nonDamage));
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

} //end spell S_EngiUniqueRobotBuff



public class S_EngiRobotBuffFromDistance : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiRobotBuffFromDistance>().asTalentItem;

    public S_EngiRobotBuffFromDistance() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
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
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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
        return CalcValidTargetHeal(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.power = (int)(spellHit.power * (selfStatusEffect.power_nonDamage * Vector3.Distance(selfStatusEffect.caster.transform.position, selfStatusEffect.target.transform.position)));
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

} //end spell S_EngiRobotBuffFromDistance


public class S_EngiBuffBeam : Spell, SpellTalent
{
    public bool PassivelyTriggered => false;

    //Vector3 beamRotation = new Vector3(0, 0.1f, 0);
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    Spell powerBuffEffect;

    public TalentItem talent => Item.From<T_EngiBuffBeam>().asTalentItem;


    public S_EngiBuffBeam() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetSpellEffect("BeamGreenStart");
        prop.secondaryPrefabs = new GameObject[]
        {
            WorldFunctions.GetSpellEffect("BeamGreenEnd"),
            WorldFunctions.GetSpellEffect("BeamGreen"),
        };
        prop.calcStartAnchor = (cast) => { return cast.caster.transform; };
        prop.speed = 4.5f;


        sfx_castStarted = "WEAPON_Energy_Beam_05_loop_mono";
        sfxPitch_castStarted = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, true);
        sfxChannel_castStarted = "spells_default";
        sfx_castFinished = "8BIT_RETRO_Explosion_Long_Distant_Fade_mono";
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

        powerBuffEffect = Spell.GetSpell<S_EngiBuffBeam_PowerEffect>();

    } //end func InitDefaults

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.effect_medium;
        interval = 0.1f;
        cooldown = SpellCooldown.config.fast_rotation;
        range = SpellRange.config.unfair;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = 15;
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
        return CalcValidTargetHeal_NotSelf(target, caster);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {

    }

    public override void DoAnimation(spellHit spellHit)
    {
        BeamCreate.Single(spellHit, prop.prefab, prop.secondaryPrefabs[0], prop.secondaryPrefabs[1], spellHit.caster.transform, spellHit.target.transform.position, ref prop.speed, ref prop.scale, spellHit.target.transform, ref spellHit.moveSpeed, CalcValidTargetHeal);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        //Damage.Attack(spellHit, true);
        if(!spellHit.target.ContainsStatusEffectID(ref spellHit.spell.id)) //don't upsert to avoid spamming during beam Update
            powerBuffEffect.ApplyRaw(spellHit.caster, spellHit.target, default, default);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_EngiBuffBeam.baseAmount, T_EngiBuffBeam.amountPerLevel);
    }


} // S_EngiBuffBeam



public class S_EngiBuffBeam_PowerEffect : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;


    public TalentItem talent => Item.From<T_EngiBuffBeam>().asTalentItem;

    public static float _duration; //expose to talent description

    public S_EngiBuffBeam_PowerEffect() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Effect_Reverse_Zoom_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        //sfxChannel_castFinished = "spells_default";
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
        _duration = SpellDuration.config.effect_short;

        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.high;
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
        power_nonDamage = 1.5f; // will be set if they have talents during MultiplyByTalentLevel

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster); //only use on healable targets
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);

    }
    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.target != null && spellHit.power > 0 && !spellHit.isHeal) //if is an attack with damage
            spellHit.power = (int)(spellHit.power * selfStatusEffect.power_nonDamage); //apply mod
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_EngiBuffBeam.baseAmount, T_EngiBuffBeam.amountPerLevel);
    }

} //end spell S_EngiBuffBeam_PowerEffect



public class S_EngiLongHoT : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;

    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiLongHot>().asTalentItem;

    public static float _duration; //expose to talent description
    public static float _interval; //expose to talent description


    public S_EngiLongHoT() : base() //TODO: add prop
    {

    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        _interval = SpellInterval.config.fast;
        _duration = SpellDuration.config.buff_long;

        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = _interval;
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
        spellBaseScore = SpellBaseScore.config.powerful;
        power_nonDamage = 0;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { spellType.healing };

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        //sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Quick_Climbing_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        //sfxPitch_castFinished = new PitchAnimate.Params(1f, 0.73f, 0.5f, 0.73f, 0.5f, 0.15f, 0.2f, 0.5f, false);
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
            new CreateFX_Params("HealOnceLowPlus", TargetType.caster, PersistType.duration, false, 2, 1)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster); //only use on healable targets
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //TODO- override CalcBasePower
        selfCast.power += (int)TalentItem.CalcEffectFlat(ref talentLevel, T_EngiLongHot.baseAmount, T_EngiLongHot.amountPerLevel);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, true);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Heal(spellHit, true, false);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_EngiLongHoT



public class S_EngiTurretEnergyPerSecondAura : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiTurretEnergyPerSecondAura>().asTalentItem;

    public static float _interval; //expose to talent description
    public static float _radius; //expose to talent description

    public S_EngiTurretEnergyPerSecondAura() : base() //TODO: add properties
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
            //new CreateFX_Params("Electricity", TargetType.caster, PersistType.whileHasSpellEffect, true, 3f, 2)
        };
        fx_animationHit = new CreateFX_Params[]
        {
            //new CreateFX_Params("Electricity", TargetType.caster, PersistType.duration, true, 2f, 1)

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 0; //dont modify power except from talent level
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        _interval = SpellCooldown.config.medium_rotation;
        _radius = SpellRadius.config.large;

        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = _interval;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = _radius;
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
        canBeDispelled = false;
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
        //AOEs.Zone_Action(spellHit, OnMyAoE);
        //Ability.SubtractCharge(spellHit.caster, spellHit.chargeUsed);  //subtract charge from self
        StatusEffects.TryAdd(spellHit, false, true, false);
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
        //Debug.LogWarning("add " + spellHit.power_nonDamage + "E  to " + spellHit.target);
        Ability.AddEnergy(spellHit.target, (int)spellHit.power_nonDamage); //add charge to target
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiTurretEnergyPerSecondAura.baseAmount, T_EngiTurretEnergyPerSecondAura.amountPerLevel);
    }

} //end spell S_EngiTurretEnergyPerSecondAura



public class S_EngiOnTeleporterNextInstantCast : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    //float baseSpeedIncrease = 0.4f;
    //float additionalSpeedPerLevel = 0.5f;

    public TalentItem talent => Item.From<T_EngiOnTeleporterNextInstantCast>().asTalentItem;

    public S_EngiOnTeleporterNextInstantCast() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Effect_Reverse_Zoom_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        //sfxChannel_castFinished = "spells_default";
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
        duration = SpellDuration.config.buff_short;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.high;
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
        power_nonDamage = 1.8f; // will be set if they have talents during MultiplyByTalentLevel

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster); //only use on healable targets
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.stacks = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiOnTeleporterNextInstantCast.baseAmount, T_EngiOnTeleporterNextInstantCast.amountPerLevel);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //StatusEffects.MultSpeed(spellHit, spellHit.power_nonDamage, true, true, false);
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void CalcCastPerOffensive_PreHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.castTime > 0)
        {
            spellHit.castTime = 0; //make instant cast

            //consume stacks left
            selfStatusEffect.stacks--;
            if (selfStatusEffect.stacks <= 0)
                selfStatusEffect.target.StatusEffect_Consume(spellHit, selfStatusEffect); //effect is consumed
        }
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_EngiOnTeleporterNextInstantCast



public class S_EngiOnTeleporterSpeed : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    //float baseSpeedIncrease = 0.4f;
    //float additionalSpeedPerLevel = 0.5f;

    public TalentItem talent => Item.From<T_EngiOnTeleporterSpeed>().asTalentItem;

    public static float _duration; //expose to talent description

    public S_EngiOnTeleporterSpeed() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Effect_Reverse_Zoom_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        //sfxChannel_castFinished = "spells_default";
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

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = _duration;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.high;
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
        power_nonDamage = 1.8f; // will be set if they have talents during MultiplyByTalentLevel

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster); //only use on healable targets
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_EngiOnTeleporterSpeed.baseAmount, T_EngiOnTeleporterSpeed.amountPerLevel);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //StatusEffects.MultSpeed(spellHit, spellHit.power_nonDamage, true, true, false);
        StatusEffects.MultSpeed(spellHit, spellHit.power_nonDamage, true, true, false);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_EngiOnTeleporterSpeed


public class S_EngiOnTeleporterGainShield : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiOnTeleporterGainShield>().asTalentItem;

    public static float _duration;

    public S_EngiOnTeleporterGainShield() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();

        _duration = SpellDuration.config.buff_medium;

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
            new CreateFX_Params("SimpleZoneBlue", TargetType.target, PersistType.whileHasSpellEffect, true, 1.75f, 1)
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
        selfCast.power_nonDamage += TalentItem.CalcEffectFlat(ref talentLevel, T_EngiOnTeleporterGainShield.baseAmount, T_EngiOnTeleporterGainShield.amountPerLevel);
    }
    

} //end spell S_EngiOnTeleporterGainShield


public class S_EngiMirrorImage : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;

    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.summoned;

    public TalentItem talent => Item.From<T_EngiMirrorImage>().asTalentItem;

    int turretID;
    public static int[] turretShotSpells;
    public static SpellMod[] turrentKnownMods;

    Spell upgradeSpell; //what allows it to be upgraded with healing

    public const string prefabName = "Engi_MirrorImage Variant";

    public S_EngiMirrorImage() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetEntity(prefabName);     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };
        prop.calcCenter = CalcCenter;


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";

    }


    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        turretShotSpells = new int[] { Spell.GetID<S_EngiTurretShot>() };
        turrentKnownMods = new SpellMod[] { };
        upgradeSpell = Spell.GetSpell<S_EngiTurretUpgrade>();

        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.high;
        castTime = SpellCastTime.config.slow;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.slow_rotation;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.single;
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

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
    }

    Vector3 CalcCenter(spellHit spellHit)
    {
        return spellHit.caster.transform.forward * 3;
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        SyncObject newObj = SummonCreate.Single(spellHit, prop.prefab.name, spellHit.caster.transform.position + spellHit.caster.transform.forward, null, turretShotSpells, turrentKnownMods);
        upgradeSpell.ApplyRaw(spellHit.caster, spellHit.target, default, default);

        //TODO- apply all gear mods to turrets?...
    }

} //end spell S_EngiMirrorImage


//separate spell for making cover also increase damage
public class S_EngiOnHitHealAlly : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiOnHitHealAlly>().asTalentItem;

    public S_EngiOnHitHealAlly() : base() //TODO: add properties
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
        range = SpellRange.config.unfair;
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
        Entity[] targets = AOEs.GetAreaFriendlyEntities_NotSelf(spellHit.caster, spellHit.range);
        if (targets.Length > 0) //if found nearby target
            OnAnimationHit(new spellHit(spellHit, spellHit.caster, targets.Random(), spellHit.origin, spellHit.vTarget)); //cast ability on them
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        Damage.Heal(spellHit, false, true);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiOnHitHealAlly.baseAmount, T_EngiOnHitHealAlly.amountPerLevel);
    }

} //end spell S_EngiOnHitHealAlly


public class S_EngiGiftHealthPercent : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiGiftHealthPercent>().asTalentItem;


    public S_EngiGiftHealthPercent() : base() //TODO: add properties
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
        spellTypes = new List<int> { };
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
        //deal % damage to self, heal target by %

        //heal target by %
        spellHit.power = (int)(spellHit.target.Get<int>(SyncHealth.maxHealth) * spellHit.power_nonDamage);
        Damage.Heal(spellHit, true, true);

        //damage self by %
        spellHit attackSelf = new spellHit(spellHit, spellHit.caster, spellHit.caster, default, default);
        attackSelf.power = (int)(spellHit.caster.Get<int>(SyncHealth.maxHealth) * spellHit.power_nonDamage);
        Damage.Attack(attackSelf, true, true);

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiGiftHealthPercent.baseAmount, T_EngiGiftHealthPercent.amountPerLevel);
    }

} //end spell S_EngiGiftHealthPercent


public class S_EngiDrainLife : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiDrainLife>().asTalentItem;


    public S_EngiDrainLife() : base() //TODO: add properties
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
        //deal % damage to target, heal self by %

        //damage target by %
        spellHit.power = (int)(spellHit.target.Get<int>(SyncHealth.maxHealth) * spellHit.power_nonDamage);
        Damage.Attack(spellHit, true, true);

        //heal self by %
        spellHit healSelf = new spellHit(spellHit, spellHit.caster, spellHit.caster, default, default);
        healSelf.power = (int)(spellHit.caster.Get<int>(SyncHealth.maxHealth) * spellHit.power_nonDamage);
        Damage.Heal(healSelf, true, true);

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiDrainLife.baseAmount, T_EngiDrainLife.amountPerLevel);
    }

} //end spell S_EngiDrainLife


public class S_EngiRestorePowerOverTime : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiRestorePowerOverTime>().asTalentItem;

    public static float _duration; //expose parameter for talent description
    public static float _interval; //expose parameter for talent description

    public S_EngiRestorePowerOverTime() : base() //TODO: add properties
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

        inturruptedByMovement = false;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster);
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
        //Damage.Attack(spellHit, true);
        Ability.AddCharge(spellHit.target, ref spellHit.power_nonDamage);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiRestorePowerOverTime.baseAmount, T_EngiRestorePowerOverTime.amountPerLevel);
    }

} //end spell S_EngiRestorePowerOverTime


public class S_EngiRestoreEnergyOverTime : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiRestoreEnergyOverTime>().asTalentItem;

    public static float _duration; //expose parameter for talent description
    public static float _interval; //expose parameter for talent description

    public S_EngiRestoreEnergyOverTime() : base() //TODO: add properties
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

        inturruptedByMovement = false;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster);
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
        //Damage.Attack(spellHit, true);
        int added = (int)(spellHit.power_nonDamage);
        Ability.AddEnergy(spellHit.target, ref added);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiRestoreEnergyOverTime.baseAmount, T_EngiRestoreEnergyOverTime.amountPerLevel);
    }

} //end spell S_EngiRestoreEnergyOverTime


public class S_EngiYoink : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    //Vector3 beamRotation = new Vector3(0, 0.1f, 0);
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_EngiRestoreEnergyOverTime>().asTalentItem;
    

    public static float _range; //expose parameter for talent description

    const float yoinkOffset = 1f; //how far away to offset from self when bringing the target to caster

    public S_EngiYoink() : base()
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
        _range = SpellRange.config.high;
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.slow_rotation;
        range = _range;
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
        return CalcValidTargetHeal_IsPlayer(target, caster); //fail if either isn't a player or isn't healable
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

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
       // selfCast.power_nonDamage = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiRestoreEnergyOverTime.baseAmount, T_EngiRestoreEnergyOverTime.amountPerLevel);
    }

} //end S_EngiYoink


public class S_EngiGiveReflectDamage : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => talent.nameFormatted;
    public override string description => talent.description;
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_EngiGiveReflectDamage>().asTalentItem;

    public static float _duration; //expose parameter for talent description

    public S_EngiGiveReflectDamage() : base() //TODO: add properties
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

        inturruptedByMovement = false;
        requiresFreeAnimation = false;
        breaksStealth = false;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.damageReflected += selfStatusEffect.power;
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_EngiGiveReflectDamage.baseAmount, T_EngiGiveReflectDamage.amountPerLevel);
    }

} //end spell S_EngiGiveReflectDamage
