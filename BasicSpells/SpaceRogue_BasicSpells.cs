using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CreateFX_Params;



public class S_RogueCloak : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Cloak";
    public override string description => "make your craft invisible to enemies beyond a certain range. Must be out of combat";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public S_RogueCloak() : base()
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
        sfxChannel_castFinished = "8BIT_RETRO_Effect_Reverse_Zoom_mono";
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
        duration = SpellDuration.config.buff_medium;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.fast_buff_rotation;
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

        usableInCombat = false;
        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { spellType.stealth };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        spellHit.caster.Stealth(spellHit, true);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
    }

} //end spell S_RogueCloak



public class S_RogueShadowStep : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Shadow Warp";
    public override string description => "teleports behind a target and creates an explosion at the target";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    Spell gunsBlazingEffect; //AoE effect at location

    public S_RogueShadowStep() : base()
    {
    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        gunsBlazingEffect = Spell.GetSpell<S_RogueGunsBlazingEffect>();

        prop = new SpellStaticProperties();
        //prop.prefab = WorldFunctions.GetEntity("EngiTurret");     //TODO- create turret
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this

        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Reverse_Repeating_Fading_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0.0f, false, 0.5f);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {
            new CreateFX_Params("LightningFloorTrail", TargetType.caster, PersistType.duration, true, 3, 5f),
            new CreateFX_Params("Purple Spawn", TargetType.caster, PersistType.duration, false, 3, 5f)
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
        cooldown = SpellCooldown.config.slow_rotation;
        range = SpellRange.config.medium;
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
        return CalcValidTargetDamage(target, caster); //doesn't target anything
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    const float behindTargetOffset = 10;
    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        Ability.TeleportBehindEntity(spellHit, behindTargetOffset);

        gunsBlazingEffect.ApplyRaw(spellHit.caster, null, spellHit.target.transform.position, spellHit.caster.transform.position); //do AoE effect
    }

} //end spell S_RogueShadowStep



public class S_RogueGunsBlazingEffect : Spell
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => "Guns Blazing (Effect)";
    public override string description => "deals damage in radius after casting Warp";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_RogueGunsBlazing>().asTalentItem;

    public S_RogueGunsBlazingEffect() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();


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
            new CreateFX_Params("GroundSlamRed", TargetType.vTarget, PersistType.duration, false, 5f, 5)
        };
        fx_animationHit = new CreateFX_Params[]
        {

        };

    }

    public override void CalcBasePower(out int newPower)
    {
        //base.CalcBasePower(out newPower);
        newPower = 75; //dont modify power except from talent level
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
        Damage.Attack(spellHit, true, true);
    }

    /*
    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power += TalentItem.CalcEffectFlat(ref talentLevel, T_RogueGunsBlazing.baseAmount, T_RogueGunsBlazing.amountPerLevel);
    }
    */
} //end spell S_RogueGunsBlazingEffect



public class S_RogueInturrupt : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Lockdown";
    public override string description => "inturrupts the target when they are casting an ability";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public S_RogueInturrupt() : base()
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
        sfxPitch_castFinished = new PitchAnimate.Params();
        sfxChannel_castFinished = "8BIT_RETRO_Powerup_Spawn_Flash_Fading_mono";
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
        return CalcValidTargetDamage(target, caster); //doesn't target anything
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
        Ability.Inturrupt(spellHit);
    }

} //end spell S_RogueInturrupt


public class S_RogueBackstab : Spell
{
    public const float turnSpeed = 2.25f;

    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Assassin Shell";
    public override string description => "short range, agile missile that deals bonus damage from behind";
    public override Type spellTargeterType => typeof(TargetShootForward);
    public override int spellGUIType => SpellGUIType.none;

    float baseBonusDamage = 1.3f;

    public S_RogueBackstab() : base()
    {
    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetSpellEffect("spaceMissiles_001_backstab");
        prop.rotationType = SpellStaticProperties.RotationType.facingAwayOrigin;
        prop.homingType = SpellStaticProperties.HomingType.enemy;

        prop.length = 60;
        prop.startRadius = 0.1f;
        prop.offset = 180 + 45;
        prop.secondaryPrefabs = new GameObject[]
        {

        };


        //turretID = prop.prefab.GetComponent<Entity>().type; //TODO- also uncomment this


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        //sfx_castFinished = "MAGIC_SPELL_Bright_Sci-Fi_Subtle_stereo";
        sfx_castFinished = "spells_default";
        sfx_castFinished = "8BIT_RETRO_Fire_Blaster_Short_Glide_mono";
        sfxPitch_castFinished = new PitchAnimate.Params(0.9f, 0.9f, 1, 0.9f, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "8BIT_RETRO_Explosion_Short_Bright_mono";
        sfxPitch_animationHit = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_animationHit = "";
        fx_castStarted = new CreateFX_Params[]
        {
            
        };
        fx_castFinished = new CreateFX_Params[]
        {
        };
        fx_animationHit = new CreateFX_Params[]
        {
            new CreateFX_Params("PurpleOrange SmallFireball", TargetType.target, PersistType.duration, false, 2, 1f)
        };

    }


    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.projectile_short;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.extremely_fast_rotation;
        range = SpellRange.config.melee;
        radius = SpellRadius.config.medium;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.low;
        summonsCount = SpellSummonsCount.config.single;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.fast;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;
        power_nonDamage = 0;

        inturruptedByMovement = false;
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
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
        ProjectileCreate.TwoD.XZ.Single(spellHit, prop.prefab, spellHit.caster.firePoints[0].position, prop.homingType, prop.rotationType, turnSpeed, true);

    }

    //called after creating the projectile
    void ModProjectile(spellHit spellHit, GameObject newObj)
    {
        //newObj.transform.localScale = spellHit.caster.GetOrDefault<Vector3>(SyncScale.scale);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        if (spellHit.target != null && Ability.IsBehindTarget(spellHit.target.transform, spellHit.caster.transform))
            spellHit.power = (int)(spellHit.power * baseBonusDamage);
        Damage.Attack(spellHit, true, true);

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //Damage.Attack(spellHit, true);
    }

} //end spell S_RogueBackstab


public class S_RogueConsumeChargeHeal : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Guzzle";
    public override string description => "use a minimum of 1 charge to heal " + healedPerCharge + " damage per charge";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    int healedPerCharge = 50;

    public S_RogueConsumeChargeHeal() : base()
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
        sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Quick_Climbing_mono";
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


    public override void CalcMinChargeUsed(spellHit spellHit)
    {
        spellHit.minChargeUsed = 1f;
    }

    public override void CalcMaxChargeUsed(spellHit spellHit)
    {
        spellHit.maxChargeUsed = Mathf.FloorToInt(spellHit.caster.Get<float>(SyncCharge.charge)); //use all max charge available as multiple of 1
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //doesn't target anything
    }

    public override void CalcFlatEffectsFromCharge(spellHit spellHit)
    {
        spellHit.power = Mathf.RoundToInt(spellHit.chargeUsed * healedPerCharge); //calculate power based on final charge used
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
        Damage.Heal(spellHit, true, true);
    }

} //end spell S_RogueConsumeChargeHeal





public class S_RogueStackingDoT : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "";
    public override string description => "deals damage over time, stacking up to 3 times";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;


    public S_RogueStackingDoT() : base() //TODO: add properties
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
        duration = SpellDuration.config.damage_short;
        interval = SpellInterval.config.fast;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.medium;
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

    /*
    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power = TalentItem.CalcEffectFlat(ref talentLevel, T_RogueOnHitDoT.baseAmount, T_RogueOnHitDoT.amountPerLevel);
    }
    */

} //end spell S_RogueOnHitDoT
