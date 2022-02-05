using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CreateFX_Params;

public class S_EngiTurret : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Sentry";
    public override string description => "build a turret that automatically fires at enemies. Heal above max HP to upgrade";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.summoned;

    int turretID;
    public static int[] turretShotSpells;
    public static SpellMod[] turrentKnownMods;

    Spell upgradeSpell; //what allows it to be upgraded with healing

    public S_EngiTurret() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetEntity("EngiTurret_Lv1 Variant");
        prop.secondaryPrefabs = new GameObject[]
        {

        };

        turretID = prop.prefab.GetComponent<Entity>().type;

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
        castTime = SpellCastTime.config.unreasonable_wind_up;
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
        spellTypes = new List<int> { spellType.sentry_turret };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
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

    const float forwardOffset = 1;
    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        SyncObject newObj = SummonCreate.Single(spellHit, prop.prefab.name, spellHit.caster.transform.position + (spellHit.caster.transform.forward.normalized * forwardOffset), null, turretShotSpells, turrentKnownMods);
        upgradeSpell.ApplyRaw(spellHit.caster, spellHit.target, default, default);

        //TODO- apply all gear mods to turrets?...
    }

} //end spell S_EngiTurret



public class S_EngiHealShot : Spell
{
    public const float turnSpeed = 3f;

    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Nano Shot";
    public override string description => "shoot a smart projectile that damages foes and heals allies";
    public override Type spellTargeterType => typeof(TargetShootForward);
    public override int spellGUIType => SpellGUIType.none;

#pragma warning disable CS0414 // The field 'S_EngiHealShot.baseBonusDamage' is assigned but its value is never used
    float baseBonusDamage = 1.3f;
#pragma warning restore CS0414 // The field 'S_EngiHealShot.baseBonusDamage' is assigned but its value is never used

    public S_EngiHealShot() : base()
    {
    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetSpellEffect("nanoshot Variant");
        prop.rotationType = SpellStaticProperties.RotationType.facingAwayOrigin;
        prop.homingType = SpellStaticProperties.HomingType.fromSpell;

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
        cooldown = 0.5f;
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
        spellBaseScore = SpellBaseScore.config.boring;
        power_nonDamage = 0;

        inturruptedByMovement = false;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { spellType.healing };
    }

    public override void CalcBasePower(out int newPower)
    {
        newPower = 15;
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        //if is friendly, has to be healable, if not, will succeed with a damaging attack
        return target != caster ? (GameMode.instance.CheckFriendly(caster, target) ? (target.canBeHealed ? castFailCodes.success : castFailCodes.invalidTarget) : castFailCodes.success) : castFailCodes.invalidTarget;
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
        if (GameMode.instance.CheckFriendly(spellHit.caster, spellHit.target))
        {
            Damage.Heal(spellHit, true, true);
        }
        else
        {
            spellHit.spellTypes.Remove(spellType.healing); //make it not count as a healing spell
            Damage.Attack(spellHit, true, true);
        }
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //Damage.Attack(spellHit, true);
    }

} //end spell S_EngiHealShot


public class S_EngiTeleporterEntrance : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Teleporter Entrance";
    public override string description => "create a teleporter that leads to Teleporter Exit. Heal above max HP to upgrade";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.summoned;

    int turretID;
    Spell upgradeSpell; //what allows it to be upgraded with healing

    public S_EngiTeleporterEntrance() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetEntity("Engi_PortalEntrance Variant");
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

        upgradeSpell = Spell.GetSpell<S_EngiPortalEntranceUpgrade>();
    }


    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.high;
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
        spellTypes = new List<int> { spellType.portal_entrance };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
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

    float forwardOffset = 1;
    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //TODO- give the turret spells from this 'params' arg
        spellHit.caster.syncSummons.RemoveAllOfType(spellType.portal_entrance); //remove old
        SummonCreate.Single(spellHit, prop.prefab.name, spellHit.caster.transform.position + (spellHit.caster.transform.forward.normalized * forwardOffset), null);
        upgradeSpell.ApplyRaw(spellHit.caster, spellHit.target, default, default);

    }

} //end spell S_EngiTeleporterEntrance


public class S_EngiTeleporterExit : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Teleporter Exit";
    public override string description => "create the exit for Teleporter Entrance. Heal above max HP to upgrade";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.summoned;

    int turretID;
    Spell upgradeSpell; //what allows it to be upgraded with healing

    public S_EngiTeleporterExit() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetEntity("Engi_PortalExit Variant");
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

        upgradeSpell = Spell.GetSpell<S_EngiPortalExitUpgrade>();
    }


    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.high;
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
        spellTypes = new List<int> { spellType.portal_exit };
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetSelf(target, caster);
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

    const float forwardOffset = 1f;
    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //TODO- give the turret spells from this 'params' arg
        spellHit.caster.syncSummons.RemoveAllOfType(spellType.portal_exit); //remove old
        SummonCreate.Single(spellHit, prop.prefab.name, spellHit.caster.transform.position + (spellHit.caster.transform.forward.normalized * forwardOffset), null);
        upgradeSpell.ApplyRaw(spellHit.caster, spellHit.target, default, default);

    }

} //end spell S_EngiTeleporterExit


public class S_EngiBubble : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Bubble Shield";
    //public override string description => "deploy a shield that blocks projectiles and absorbs damage. Increases absorbtion by " + healthPerCharge + " per charge used";
    public override string description => "deploy a shield that blocks projectiles. Increases absorbtion by " + healthPerCharge + " per charge used";

    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.summoned;

    int turretID;
    Spell absorbAuraEffect; //effect that gets applied to enhabitants

    const int healthPerCharge = 100;

    public S_EngiBubble() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetEntity("Engi_Bubble Variant");     //TODO- create turret
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
        fx_castStarted = new CreateFX_Params[]
        {

        };
        fx_castFinished = new CreateFX_Params[]
        {
        };
        fx_animationHit = new CreateFX_Params[]
        {
            //new CreateFX_Params("PurpleOrange SmallFireball", TargetType.target, PersistType.duration, false, 2, 1f)
        };

        absorbAuraEffect = Spell.GetSpell<S_EngiBubbleAbsorbEffect>();
    }

    const float baseRadius = 4.8f; //real world width of the bubble

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.effect_long;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.slow_rotation;
        range = SpellRange.config.na;
        radius = baseRadius;
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
        spellHit.power_nonDamage = spellHit.chargeUsed * healthPerCharge; //shield per charge used
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

    const float forwardOffset = 0f;
    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        Entity newObj = SummonCreate.Single(spellHit, prop.prefab.name, spellHit.caster.transform.position + (spellHit.caster.transform.forward * forwardOffset), null).e;
        newObj.Set<Vector3>(SyncScale.scale, Vector3.one * (spellHit.radius / baseRadius)); //increase size by radius
        StatusEffects.AddMaxHealth(spellHit, (int)spellHit.power_nonDamage, false, true, false);
        //absorbAuraEffect.ApplyRaw(newObj, newObj, default, default);
    }

} //end spell S_EngiBubble

