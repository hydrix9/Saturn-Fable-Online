using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class S_BEES : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //don't add to hotbar, add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Space Bees";
    public override string description => "biting and stinging... yes you read that right, these bees have large TEETH";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_Bees>().asTalentItem;


    public S_BEES() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();
        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");

    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.damage_short;
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
        return CalcValidTargetDamage(target, caster);
    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        spellHit.spellTypes.Add(spellType.nature);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.summonsCount = 
    }

    public override void OnCritDefensive(spellHit spellHit)
    {
        TryStartCast(spellHit.caster, spellHit.target, default, default); //cast this ability on attacker when defending against an attack
    }


    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        ProjectileCreate.TwoD.XZ.FromArc(spellHit, prop.prefab, prop.length, prop.offset, spellHit.caster.transform.position, prop.startRadius, prop.homingType, prop.rotationType);
    }

    public override void OnAnimationHit(spellHit spellHit)
    {
        Damage.Attack(spellHit, true);
    }

} //end spell Space Bees
*/

/*
public class S_BEES_Explode : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //don't add to hotbar, add to entity's spellmods to be triggered by actions like OnCritDefensive
    int targetSpell; //bee spell that procs this

    public override string nameFormatted => "Bee Explosion";
    public override string description => "these bees ate gunpowder and also create explosive honey";
    public override Type spellTargeterType => typeof(TargetSelf);

    public TalentItem talent => Item.From<T_Bees>().asTalentItem;


    public S_BEES_Explode() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties properties)
    {
        properties = new SpellStaticProperties();
        targetSpell = Spell.GetID<S_BEES>();
        soundEffects = new AudioClip[2];
        //soundEffects[0] = WorldFunctions.GetSoundEffect("FIREWORKS_Rocket_Explode_Large_RR5_mono");

    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.na;
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
        return CalcValidTargetDamage(target, caster);
    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        spellHit.spellTypes.Add(spellType.physical);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.radius += TalentItem.CalcEffectFlat(ref TalentLayout, T_Bees.)
    }

    public override void OnCritOffensive(spellHit spellHit)
    {
        if (spellHit.spell.id == targetSpell) //only proc on affected spell, the BEES
            TryStartCast(spellHit.target, spellHit.caster, default, default); //cast on caster's target
    }


    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);

        //AnimationMissile("vfx_lava_blast", spellHit, 1.7f);

        //WorldFunctions.GetSpellEffect("spaceMissiles_001"), spellHit, spellHit.caster.transform.position, circleRadius, arcDegrees, arcOffset, count, Projectiles.HomingType.none, Projectiles.RotationType.facingAwayOrigin
        ProjectileCreate.TwoD.XZ.FromArc(spellHit, prop.prefab, prop.length, prop.offset, spellHit.caster.transform.position, prop.startRadius, prop.homingType, prop.rotationType);
    }

    public override void OnAnimationHit(spellHit spellHit)
    {
        Damage.Attack(spellHit, true);
    }

} //end spell Space Bees

    */