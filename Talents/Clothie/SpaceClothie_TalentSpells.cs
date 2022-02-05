using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if using
public class S_ClothieBolt : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Bolt";
    public override string description => "shoot a basic missile at selected target";
    public override Type spellTargeterType => typeof(TargetSelected);

    public S_ClothieBolt() : base() //TODO: add prop
    {

    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        spellHit.spellTypes.Add(spellType.energy);
    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        soundEffects = new AudioClip[1];
        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");
        //soundEffects[1] = WorldFunctions.GetSoundEffect("MAGIC_SPELL_Flame_03_mono");
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int charges, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.fast;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.damage_medium;
        interval = SpellInterval.config.fast;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.small;
        charges = SpellNumCharges.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //only use on healable targets
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.power += TalentItem.CalcEffectFlat(ref talentLevel, T_);
    }

    public override void OnTryHitOffensive(spellHit spellHit)
    {
        Damage.Attack(spellHit, true);
    }

    public override void IntervalEffect(spellHit spellHit)
    {

    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    public override void OnAnimationHit(spellHit spellHit)
    {
    }

} //end spell S_ClothieBolt



public class S_ClothieShortDoT : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Dissolve";
    public override string description => "a field of high-energy particles follows the target, dealing damage over time";
    public override Type spellTargeterType => typeof(TargetSelected);

    public S_ClothieShortDoT() : base() //TODO: add prop
    {

    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        spellHit.spellTypes.Add(spellType.energy);
    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        soundEffects = new AudioClip[1];
        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");
        //soundEffects[1] = WorldFunctions.GetSoundEffect("MAGIC_SPELL_Flame_03_mono");
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int charges, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.damage_medium;
        interval = SpellInterval.config.fast;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.small;
        charges = SpellNumCharges.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //only use on healable targets
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //TODO- override CalcBasePower
        //selfCast.power += TalentItem.CalcEffectFlat(ref talentLevel, );
    }

    public override void OnTryHitOffensive(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Attack(spellHit, true); //will also do leechHealthPercent from spellHit
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    public override void OnAnimationHit(spellHit spellHit)
    {
    }

} //end spell S_ClothieShortDoT


public class S_ClothieLongDoT : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Strike Craft Swarm";
    public override string description => "Swarm the enemy with strike craft, dealing damage over time";
    public override Type spellTargeterType => typeof(TargetSelected);

    public S_ClothieLongDoT() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        soundEffects = new AudioClip[1];
        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");
        //soundEffects[1] = WorldFunctions.GetSoundEffect("MAGIC_SPELL_Flame_03_mono");
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int charges, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.damage_long;
        interval = SpellInterval.config.medium;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.small;
        charges = SpellNumCharges.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //only use on healable targets
    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        spellHit.spellTypes.Add(spellType.physical);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //TODO- override CalcBasePower
        //selfCast.power += TalentItem.CalcEffectFlat(ref talentLevel, T_)
    }

    public override void OnTryHitOffensive(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Attack(spellHit, true); //will also do leechHealthPercent from spellHit
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    public override void OnAnimationHit(spellHit spellHit)
    {
    }

} //end spell S_ClothieShortDoT


public class S_ClothieDamageDebuff : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Onslaught";
    public override string description => "an experimental technology psychically afflicts the target's crew, reducing damage dealt";
    public override Type spellTargeterType => typeof(TargetSelected);

    public S_ClothieDamageDebuff() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        soundEffects = new AudioClip[1];
        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");
        //soundEffects[1] = WorldFunctions.GetSoundEffect("MAGIC_SPELL_Flame_03_mono");
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int charges, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.damage_long;
        interval = SpellInterval.config.medium;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.high;
        radius = SpellRadius.config.small;
        charges = SpellNumCharges.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetDamage(target, caster); //only use on healable targets
    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        spellHit.spellTypes.Add(spellType.darkness);
    }

    public override void OnTryHitOffensive(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.power_nonDamage += TalentItem.CalcEffectFlat();
    }
    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    public override void OnAnimationHit(spellHit spellHit)
    {
    }

} //end spell S_ClothieShortDoT




public class S_ClothieLeech : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Scuttle";
    public override string description => "dissasembles the target over time, dealing damage and healing you for a percent";
    public override Type spellTargeterType => typeof(TargetSelected);

    public S_ClothieLeech() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        soundEffects = new AudioClip[1];
        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");
        //soundEffects[1] = WorldFunctions.GetSoundEffect("MAGIC_SPELL_Flame_03_mono");
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int charges, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.damage_medium;
        interval = SpellInterval.config.medium;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.small;
        charges = SpellNumCharges.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.main_feature_medium;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        spellHit.spellTypes.Add(spellType.energy);
    }


    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster); //only use on healable targets
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.leechHealthPercent += TalentItem.CalcEffectFlat(ref talentLevel, );
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Attack(spellHit, true); //will also do leechHealthPercent from spellHit
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    public override void OnAnimationHit(spellHit spellHit)
    {
    }

} //end spell S_ClothieLeech



public class S_ClothieEMP : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "EMP";
    public override string description => "a pulse of energy disables all nearby enemy craft for duration";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_RogueConsumeChargeIncreaseDodge>().asTalentItem;

    public S_ClothieEMP() : base() //TODO: add properties
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();



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
        spellTypes = new List<int> { spellType.stealth };
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
        //spellHit.maxChargeUsed = 5; ///is this already capped at caster.maxCharge?
    }

    public override void OnTryHitOffensive(spellHit spellHit)
    {
        //calc vTarget from transform.forward * power_nonDamage in MultiplyByTalentLevel...
        spellHit.power_nonDamage *= spellHit.chargeUsed; //multiply ability for every charge used
        Ability.SubtractCharge(spellHit.caster, spellHit.chargeUsed);
    }

    //AoE that happens on expiratoin
    public override void OnAnimationHit(spellHit spellHit)
    {
        Damage.Attack(spellHit, true);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.duration += TalentItem.CalcEffectFlat(ref talentLevel, T_RogueIllusionaryProjection.baseAmount, T_RogueIllusionaryProjection.amountPerLevel);
        //set power_nonDamage
    }

} //end spell S_ClothieEMP



public class S_ClothieWarp : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Warp";
    public override string description => "jump through spacetime to target direction";
    public override Type spellTargeterType => typeof(TargetSelf);

    public S_ClothieWarp() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        soundEffects = new AudioClip[1];
        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");
        //soundEffects[1] = WorldFunctions.GetSoundEffect("MAGIC_SPELL_Flame_03_mono");
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int charges, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore)
    {
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.low;
        radius = SpellRadius.config.small;
        charges = SpellNumCharges.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;

        inturruptedByMovement = true;
        requiresFreeAnimation = false;
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return castFailCodes.success; //only use on healable targets
    }

    public override void OnTryHitOffensive(spellHit spellHit)
    {
        Ability.TeleportToVTarget(spellHit);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    public override void OnAnimationHit(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.range += TalentItem.CalcEffectFlat(ref talentLevel, );
    }
} //end spell S_ClothieWarp




public class S_ClothieAmplifyHealing : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Chief Medical Robot";
    public override string description => "deploy robot to a target to passively increase healing taken, maximum of 1 active";
    public override Type spellTargeterType => typeof(TargetSelected);

    public float increasePerlevel = 0.1f;

    public S_ClothieAmplifyHealing() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        soundEffects = new AudioClip[1];
        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");
        //soundEffects[1] = WorldFunctions.GetSoundEffect("MAGIC_SPELL_Flame_03_mono");
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int charges, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.low;
        radius = SpellRadius.config.small;
        charges = SpellNumCharges.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.none;
        speed = SpellMoveSpeed.config.none;
        animationTimeMS = SpellAnimationTimeMS.config.none;
        generateCurrencyAmount = 0;
        spellBaseScore = SpellBaseScore.config.normal;

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

    public override void OnHitDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.isHeal) {
            spellHit.power = (int)(spellHit.power * selfStatusEffect.power_nonDamage);
        }
    }

    public override void OnTryHitOffensive(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, false, true, true);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    public override void OnAnimationHit(spellHit spellHit)
    {
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.power_nonDamage += TalentItem.CalcEffectPer(ref talentLevel, increasePerlevel);
    }
} //end spell S_ClothieAmplifyHealing


#endif