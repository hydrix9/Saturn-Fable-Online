using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CreateFX_Params;


public class S_EngiTurretUpgrade : Spell
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => "Sentry";
    public override string description => "powers up when upgraded";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;
    public override bool hideStatusEffect => true;

    static readonly int[] levelHealingRequirements = new int[]
    {
        0,
        1000, //1 to 2
        5000, //2 to 3 //could be 6000 if you wan to /truly/ take 5000...
        30000 //3 to 4
    };

    //prefabs for each level of turret
    public static readonly string[] prefabNames = new string[]
    {
        "na",
        "EngiTurret_Lv1 Variant",
        "EngiTurret_Lv2 Variant",
        "EngiTurret_Lv3 Variant"
    };

    static readonly float[] powerUpgrades = new float[]
    {
        1,
        1, //lv 1
        2f, //lv 2
        3f, //lv 3
        4f //lv 4
    };
    static readonly float[] sizeUpgrades = new float[]
    {
        1,
        1, //lv 1
        1.5f, //lv 2
        2f, //lv 3
        2.5f, //lv 4
    };

    static readonly float[] healthUpgrades = new float[]
    {
        1,
        1, //lv 1
        1.5f, //lv 2
        2f, //lv 3
        3f, //lv 4
    };
    const int maxLevel = 3;

    public S_EngiTurretUpgrade() : base() //TODO: add properties
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
        interval = SpellInterval.config.none;
        duration = SpellDuration.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = 0;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none; //calculate in MultiplyByTalentLevel
        summonsCount = 1;
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
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, false, false, false);
    }

    //triggers when healed
    public override void OnHitHealDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        if(selfStatusEffect.summonsCount < maxLevel)
        { //if not max level yet
            selfStatusEffect.power += spellHit.power; //add power to calculation
            //Debug.LogWarning("UpgradeTurret at " + selfStatusEffect.power + " / " + levelHealingRequirements[selfStatusEffect.summonsCount]);
            if(selfStatusEffect.power >= levelHealingRequirements[selfStatusEffect.summonsCount])
            { //if reached the next level
                SyncObject newObj;
                if(SummonCreate.UpgradeTo(prefabNames[selfStatusEffect.summonsCount + 1], selfStatusEffect, out newObj, S_EngiTurret.turretShotSpells, S_EngiTurret.turrentKnownMods))
                { //if successfully upgraded
                    //Debug.LogWarning("upgrading to level " + (selfStatusEffect.summonsCount + 1) +  " and size to " + sizeUpgrades[selfStatusEffect.summonsCount + 1]);
                    selfStatusEffect.target = newObj.e; //update ref

                    selfStatusEffect.summonsCount++; //update var for upgrade level
                    //StatusEffects.TryAdd(new spellHit(selfStatusEffect, selfStatusEffect.caster, newObj.e, default, default), false, true, false); //try copy same status effect to new obj

                    //note- should use StatMods through StatusEffect instead?..
                    //Debug.LogWarning("old scale " + newObj.Get<Vector3>(SyncScale.scale));

                    Logger.LogWarning(castFailCodes.ToString(StatusEffects.MultScale(selfStatusEffect, sizeUpgrades[selfStatusEffect.summonsCount], false, false, false)));
                    //Debug.LogWarning("new scale " + newObj.Get<Vector3>(SyncScale.scale));
                    //newObj.Set<int>(SyncHealth.maxHealth, (int)(newObj.Get<int>(SyncHealth.maxHealth) * 1f * healthUpgrades[selfStatusEffect.summonsCount])); //mod scale by specified size upgrades


                } //end if successfully upgraded
            } //end if reached next level
        } //end if not max level yet

    } //end func OnHitHealDefensive

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.isDamage)
            spellHit.power = (int)(spellHit.power * powerUpgrades[selfStatusEffect.summonsCount]); //mod power based on current level
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

} //end spell S_EngiTurretUpgrade


public class S_EngiPortalEntranceUpgrade : Spell
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => "Teleporter Entrance";
    public override string description => "powers up when upgraded";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;
    public override bool hideStatusEffect => true;

    static readonly int[] levelHealingRequirements = new int[]
    {
        0,
        1000,
        6000,
        27000
    };

    //prefabs for each level of turret
    static readonly string[] prefabNames = new string[]
    {
        "na",
        "Engi_PortalEntrance Variant",
        "Engi_PortalEntrance Variant",
        "Engi_PortalEntrance Variant"
    };

    static readonly float[] cooldownUpgrades = new float[]
    {
        1,
        1,
        0.66f,
        0.33f
    };
    static readonly float[] sizeUpgrades = new float[]
    {
        1,
        1, //lv 1
        2.5f, //lv 2
        4f, //lv 3
        6f, //lv 4
    };

    static readonly float[] healthUpgrades = new float[]
    {
        1,
        1, //lv 1
        1.5f, //lv 2
        2f, //lv 3
        3f, //lv 4
    };

    const int maxLevel = 3;

    public S_EngiPortalEntranceUpgrade() : base() //TODO: add properties
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
        interval = SpellInterval.config.none;
        duration = SpellDuration.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = 0;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none; //calculate in MultiplyByTalentLevel
        summonsCount = 1;
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
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, false, true, false);
    }

    //triggers when healed
    public override void OnHitHealDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (selfStatusEffect.summonsCount < maxLevel)
        { //if not max level yet
            selfStatusEffect.power += spellHit.power; //add power to calculation
            if (selfStatusEffect.power >= levelHealingRequirements[selfStatusEffect.summonsCount])
            { //if reached the next level
                SyncObject newObj;
                if (SummonCreate.UpgradeTo(prefabNames[selfStatusEffect.summonsCount + 1], selfStatusEffect, out newObj))
                { //if successfully upgraded

                    selfStatusEffect.summonsCount++; //update var for upgrade level
                    StatusEffects.TryAdd(new spellHit(selfStatusEffect, selfStatusEffect.caster, newObj.e, default, default), false, true, false); //try copy same status effect to new obj
                    StatusEffects.MultScale(selfStatusEffect, sizeUpgrades[selfStatusEffect.summonsCount], false, false, false);
                    StatusEffects.MultMaxHealth(selfStatusEffect, healthUpgrades[selfStatusEffect.summonsCount], false, false, false);

                } //end if successfully upgraded
            } //end if reached next level
        } //end if not max level yet

    } //end func OnHitHealDefensive

    public override void CalcCastPerOffensive_PreHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.cooldown = spellHit.cooldown * cooldownUpgrades[selfStatusEffect.summonsCount]; //mod power based on current level

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

} //end spell S_EngiPortalEntranceUpgrade



public class S_EngiPortalExitUpgrade : Spell
{
    public bool PassivelyTriggered => true; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive
    public override string nameFormatted => "Teleporter Exit";
    public override string description => "powers up when upgraded";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;
    public override bool hideStatusEffect => true;

    static readonly int[] levelHealingRequirements = new int[]
    {
        0,
        1000,
        6000,
        27000
    };
    static readonly float[] sizeUpgrades = new float[]
    {
        1,
        1, //lv 1
        2.5f, //lv 2
        4f, //lv 3
        6f, //lv 4
    };

    static readonly float[] healthUpgrades = new float[]
    {
        1,
        1, //lv 1
        1.5f, //lv 2
        2f, //lv 3
        3f, //lv 4
    };

    //prefabs for each level of turret
    static readonly string[] prefabNames = new string[]
    {
        "na",
        "Engi_PortalExit Variant",
        "Engi_PortalExit Variant",
        "Engi_PortalExit Variant"
    };

    const int maxLevel = 3;

    public S_EngiPortalExitUpgrade() : base() //TODO: add properties
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
        interval = SpellInterval.config.none;
        duration = SpellDuration.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
        radius = SpellRadius.config.none;
        stacks = 0;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none; //calculate in MultiplyByTalentLevel
        summonsCount = 1;
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
        return CalcValidTargetSelf(target, caster);
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, false, true, false);
    }

    //triggers when healed
    public override void OnHitHealDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (selfStatusEffect.summonsCount < maxLevel)
        { //if not max level yet
            selfStatusEffect.power += spellHit.power; //add power to calculation
            if (selfStatusEffect.power >= levelHealingRequirements[selfStatusEffect.summonsCount])
            { //if reached the next level
                SyncObject newObj;
                if (SummonCreate.UpgradeTo(prefabNames[selfStatusEffect.summonsCount + 1], selfStatusEffect, out newObj))
                { //if successfully upgraded

                    selfStatusEffect.summonsCount++; //update var for upgrade level
                    StatusEffects.TryAdd(selfStatusEffect, false, true, false); //try copy same status effect to new obj
                    StatusEffects.MultScale(selfStatusEffect, sizeUpgrades[selfStatusEffect.summonsCount], false, false, false);
                    StatusEffects.MultMaxHealth(selfStatusEffect, healthUpgrades[selfStatusEffect.summonsCount], false, false, false);

                } //end if successfully upgraded
            } //end if reached next level
        } //end if not max level yet

    } //end func OnHitHealDefensive

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {

    }

} //end spell S_EngiPortalExitUpgrade



public class S_EngiTurretShot : Spell
{
    public const float turnSpeed = 1.5f;

    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Turret Shot";
    public override string description => "basic missile";
    public override Type spellTargeterType => typeof(TargetShootForward);
    public override int spellGUIType => SpellGUIType.none;

#pragma warning disable CS0414 // The field 'S_EngiTurretShot.baseBonusDamage' is assigned but its value is never used
    float baseBonusDamage = 1.3f;
#pragma warning restore CS0414 // The field 'S_EngiTurretShot.baseBonusDamage' is assigned but its value is never used

    public S_EngiTurretShot() : base()
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
        cost = 0;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.projectile_medium;
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

    public override void CalcBasePower(out int newPower)
    {
        newPower = 10;
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //doesn't target anything
    }


    //called after creating the projectile
    void ModProjectile(spellHit spellHit, GameObject newObj)
    {
        newObj.transform.localScale = spellHit.caster.GetOrDefault<Vector3>(SyncScale.scale);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
        ProjectileCreate.TwoD.XZ.Single(spellHit, prop.prefab, spellHit.caster.firePoints[0].position, prop.homingType, prop.rotationType, turnSpeed, true, ModProjectile);

    }


    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, true);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        //Damage.Attack(spellHit, true);
    }

} //end spell S_EngiTurretShot




public class S_EngiTeleporter_Teleport : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Teleport";
    public override string description => "brrrrr";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    Spell gunsBlazingEffect; //AoE effect at location

    public S_EngiTeleporter_Teleport() : base()
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
        cooldown = 15f;
        range = SpellRange.config.medium;
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
        return !target.canMove ? castFailCodes.invalidTarget : CalcValidTargetHeal_NotSelf(target, caster);
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
        //  Ability.TeleportToVTarget(spellHit, behindTargetOffset);
        if (spellHit.caster != null && spellHit.caster.summoned && spellHit.caster.summoned.spellHit != null && spellHit.caster.summoned.spellHit.caster != null)
        {
            //find the exit portal through the caster's (this portal's) owner
            Summoned exitPortal = spellHit.caster.summoned.spellHit.caster.syncSummons.cast_summons.FirstOrDefault(entry => entry != null && entry.spellHit != null && entry.spellHit.spellTypes.Contains(spellType.portal_exit));
            if (exitPortal != null)
                spellHit.target.transform.position = exitPortal.transform.position;
        }
    }

} //end spell S_EngiTeleporter_Teleport


public class S_EngiBubbleAbsorbEffect : Spell
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Bubble Shield"; //redirect damage in area toward yourself
    public override string description => "Absorbs a part of the damage to targets within the shield";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.buff;

    public const float absorbPercent = 0.5f;

    public S_EngiBubbleAbsorbEffect() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        //sfx_castFinished = "8BIT_RETRO_Powerup_Spawn_Rumble_mono";
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
            //new CreateFX_Params("AuraChargeRed", TargetType.target, PersistType.whileHasSpellEffect, true, 2)
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
        power_nonDamage = absorbPercent;

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
        //TODO- change radius based on self scale

        StatusEffects.Aura(spellHit, true, true, false, CalcValidTargetHeal_NotSelf);
    }

    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.target != selfStatusEffect.caster)
        { //don't do effect on caster themself
            spellHit newAttack = new spellHit(spellHit, spellHit.caster, selfStatusEffect.caster, spellHit.origin, spellHit.vTarget); //make new attack out of old at warrior
            newAttack.power = (int)(spellHit.power * (selfStatusEffect.power_nonDamage)); //the amount redirected
            spellHit.power -= newAttack.power; //subtract amount redirected
            Damage.Attack(newAttack, false, true); //deal damage to bubble for amount redirected
        }
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_EngiBubbleAbsorbEffect
