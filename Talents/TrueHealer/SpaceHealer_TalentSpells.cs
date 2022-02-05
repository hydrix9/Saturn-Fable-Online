using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CreateFX_Params;

public class S_HealerBeamHealTotem : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Beam Robots";
    public override string description => "creates robots that shoot healing beams between themselves";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    int totemID;
    int totemChainHealID;

    public TalentItem talent => Item.From<T_HealerBeamHealTotem>().asTalentItem;


    public S_HealerBeamHealTotem() : base()
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.prefab = WorldFunctions.GetEntity("BeamTotem");
        prop.secondaryPrefabs = new GameObject[]
        {
            WorldFunctions.GetSpellEffect("BeamBlueStart"),
            WorldFunctions.GetSpellEffect("BeamBlueEnd"),
            WorldFunctions.GetSpellEffect("BeamBlue")

        };
        prop.calcCenter = CalcCenter;


        totemID = prop.prefab.GetComponent<Entity>().type;
        totemChainHealID = Spell.GetSpellID(Spell.GetName<S_HealerBeamHealTotem_ChainHeal>());

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
        duration = SpellDuration.config.buff_short;
        interval = SpellInterval.config.fast;
        cooldown = SpellCooldown.config.slow_rotation;
        range = SpellRange.config.very_high;
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
        return castFailCodes.success; //doesn't target anything
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        selfCast.summonsCount += talentLevel;
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
        SummonCreate.Single(spellHit, prop.prefab.name, spellHit.caster.transform.position + spellHit.caster.transform.forward, null, new int[] { totemChainHealID });
    }

} //end spell S_HealerBeamHealTotem




public class S_HealerBeamHealTotem_ChainHeal : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Beam Robots Chainheal";
    public override string description => "creates robots that shoot healing beams between themselves";
    public override Type spellTargeterType => typeof(TargetSelf);
    public override int spellGUIType => SpellGUIType.none;

    int totemID;
    public TalentItem talent => Item.From<T_HealerBeamHealTotem>().asTalentItem;

    public S_HealerBeamHealTotem_ChainHeal() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();
        prop.secondaryPrefabs = new GameObject[]
        {
            WorldFunctions.GetSpellEffect("BeamBlueStart"),
            WorldFunctions.GetSpellEffect("BeamBlueEnd"),
            WorldFunctions.GetSpellEffect("BeamBlue")

        };



        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "my_sound_here";
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
        duration = SpellDuration.config.none;
        interval = 0.1f;
        cooldown = SpellCooldown.config.fast_rotation;
        range = SpellRange.config.medium;
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
        breaksStealth = true;
        spellAvailableMode = SpellAvailableMode.all;
        spellTypes = new List<int> { spellType.healing };
    }

    public override void CalcBasePower(out int newPower)
    {
        newPower = 20;
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster);
    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        spellHit.spellTypes.Add(spellType.healing);
    }

    Vector3 beamRotationSpeed = new Vector3(0, 0, 0);
    float beamScale = 2;
    float beamSpeed = 30;
    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
        Entity closest;
        if(AOEs.GetClosestFriendly_Type(spellHit.caster, ref spellHit.range, spellHit.caster.type, out closest))
            BeamCreate.Single(spellHit, prop.secondaryPrefabs[0], prop.secondaryPrefabs[1], prop.secondaryPrefabs[2], null, spellHit.caster.transform.position, ref beamSpeed, ref beamScale, closest.transform, ref beamSpeed, CalcValidTargetDamage);
    }


    public override void IntervalEffect(spellHit spellHit)
    {

    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
        Damage.Heal(spellHit, true, true);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {

    }

} //end spell S_HealerBeamHealTotem_ChainHeal





public class S_HealerCleanse : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Discharge";
    public override string description => "remove debuffs from target";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_HealerCleanse>().asTalentItem;

    public S_HealerCleanse() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "my_sound_here";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.medium;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
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
        //selfCast.numStrikes = TalentItem.CalcEffectFlat(ref talentLevel, );
    }

    public override void DoAnimation(spellHit spellHit)
    {
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.Cleanse(spellHit, spellHit.numStrikes);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_HealerCleanse


public class S_HealerWard : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Ward";
    public override string description => "shields the target from damage until depleted";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;
    public const int powerPerLevel = 3;

    public TalentItem talent => Item.From<T_HealerWard>().asTalentItem;

    public S_HealerWard() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "my_sound_here";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.high;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.buff_short;
        interval = SpellInterval.config.none;
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
        //selfCast.power_nonDamage += TalentItem.CalcEffectFlat(ref talentLevel, T_);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true);
    }

    public override void OnHitDamageDefensive(spellHit spellHit, spellHit selfStatusEffect)
    {
        Ability.BarrierEffect(spellHit, selfStatusEffect); //reduces damage then remoevs self when depleted
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_HealerWard




public class S_HealerHoT : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Repairing Swarm";
    public override string description => "invisible robots that repair damage over time for duration";

    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;
    public TalentItem talent => Item.From<T_HealerHoT>().asTalentItem;

    public S_HealerHoT() : base() //TODO: add prop
    {

    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        //carefully fill out these fields, because they WILL cause bugs if something is wrong
        cost = SpellCost_energy.config.none;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.buff_medium;
        interval = SpellInterval.config.fast;
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
        //selfCast.power += TalentItem.CalcEffectFlat(ref talentLevel, )
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true, false);
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

} //end spell S_HealerHoT





public class S_HealerSpeedBuff : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Overdrive";
    public override string description => "invisible robots increase target's speed for duration";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    //float baseSpeedIncrease = 0.4f;
#pragma warning disable CS0414 // The field 'S_HealerSpeedBuff.additionalSpeedPerLevel' is assigned but its value is never used
    float additionalSpeedPerLevel = 0.5f;
#pragma warning restore CS0414 // The field 'S_HealerSpeedBuff.additionalSpeedPerLevel' is assigned but its value is never used

    public TalentItem talent => Item.From<T_HealerSpeedBuff>().asTalentItem;

    public S_HealerSpeedBuff() : base() //TODO: add prop
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
        //selfCast.power_nonDamage =  TalentItem.CalcEffectPer(ref talentLevel, T_);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.MultSpeed(spellHit, spellHit.power_nonDamage, true, true, false);

    }
    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_HealerSpeedBuff



public class S_HealerLeech : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Scuttle";
    public override string description => "dissasembles the target over time, dealing damage and healing you for a percent";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.debuff;

    public static readonly List<int> spellTypes = new List<int> { spellType.energy, spellType.healing };

    public TalentItem talent => Item.From<T_HealerLeech>().asTalentItem;

    public S_HealerLeech() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "my_sound_here";
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
        duration = SpellDuration.config.damage_medium;
        interval = SpellInterval.config.medium;
        cooldown = SpellCooldown.config.none;
        range = SpellRange.config.medium;
        radius = SpellRadius.config.small;
        stacks = SpellNumStacks.config.none;
        maxStacks = SpellMaxStacks.config.none;
        chargeGained = SpellChargeGained.config.none;
        summonsCount = SpellSummonsCount.config.none;
        numStrikes = SpellNumStrikes.config.single;
        leechHealthPercent = SpellLeechHealthPercent.config.main_feature_medium;
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
        return CalcValidTargetHeal(target, caster); //only use on healable targets
    }

    public override void CalcSpellTypeOffensive(spellHit spellHit)
    {
        spellHit.spellTypes.AddRange(spellTypes);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.leechHealthPercent += TalentItem.CalcEffectFlat(ref talentLevel, T_);
    }

    public override void IntervalEffect(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, false); //will also do leechHealthPercent from spellHit
    }

    public override void DoAnimation(spellHit spellHit)
    {

    }
    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {

    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_HealerLeech




public class S_HealerAimedShot : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Aimed Shot";
    public override string description => "charge shot that deals physical damage";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_HealerAimedShot>().asTalentItem;

    public S_HealerAimedShot() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "my_sound_here";
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
        castTime = SpellCastTime.config.medium;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.none;
        interval = SpellInterval.config.none;
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
        //selfCast.leechHealthPercent += TalentItem.CalcEffectFlat(ref talentLevel, );
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        Damage.Attack(spellHit, true, true); //will also do leechHealthPercent from spellHit
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_HealerAimedShot




public class S_HealerOvercharge : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Overcharge";
    public override string description => "increases target's damage for duration";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public TalentItem talent => Item.From<T_HealerOvercharge>().asTalentItem;

    public float increasePerLevel = 0.02f;

    public S_HealerOvercharge() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "my_sound_here";
        sfxPitch_castFinished = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.05f, 0.05f, 0, false);
        sfxChannel_castFinished = "spells_default";
        sfx_animationHit = "";
        sfxPitch_animationHit = new PitchAnimate.Params();
        sfxChannel_animationHit = "";
    }

    public override void InitSpellBaseValues(out int cost, out float castTime, out float critChance, out float duration, out float interval, out float cooldown, out float range, out float radius, out int stacks, out int maxStacks, out float chargeGained, out bool inturruptedByMovement, out List<int> spellTypes, out float speed, out int summonsCount, out int numStrikes, out float leechHealthPercent, out string spellAvailableMode, out bool requiresFreeAnimation, out int animationTimeMS, out bool breaksStealth, out int generateCurrencyAmount, out int spellBaseScore, out float power_nonDamage)
    {
        cost = SpellCost_energy.config.low;
        castTime = SpellCastTime.config.instant;
        critChance = SpellCritChance.config.normal;
        duration = SpellDuration.config.buff_short;
        interval = SpellInterval.config.none;
        cooldown = SpellCooldown.config.fast_buff_rotation;
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
        return CalcValidTargetHeal(target, caster); //only use on healable targets
    }

    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.power = (int)(spellHit.power * selfStatusEffect.power_nonDamage);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
       //selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, T_);
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, true, true);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_HealerOvercharge




public class S_HealerEncouragingGaze : Spell, SpellTalent
{
    public bool PassivelyTriggered => false; //add to hotbar, not add to entity's spellmods to be triggered by actions like OnCritDefensive

    public override string nameFormatted => "Encouraging Gaze";
    public override string description => "passively increases the target damage, but unique to one target";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.buff;

    public float increasePerLevel = 0.08f;

    public TalentItem talent => Item.From<T_HealerEncouragingGaze>().asTalentItem;

    public S_HealerEncouragingGaze() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "my_sound_here";
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
        cooldown = SpellCooldown.config.fast_buff_rotation;
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
        return CalcValidTargetHeal(target, caster); //only use on healable targets
    }


    public override void CalcCastPerOffensive_PostHit(spellHit spellHit, spellHit selfStatusEffect)
    {
        spellHit.power = (int)(spellHit.power * selfStatusEffect.power_nonDamage);
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.power_nonDamage = TalentItem.CalcEffectPer(ref talentLevel, );
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
        StatusEffects.TryAdd(spellHit, false, true, true);
    }

    public override void DoAnimation(spellHit spellHit)
    {
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[0], 0.5f);
        //spellHit.caster.audioSource.PlayOneShot(soundEffects[1], 1f);
    }

    protected override void _OnAnimationHit_Server(spellHit spellHit)
    {
    }

} //end spell S_HealerEncouragingGaze



public class S_HealerWardConsumed : Spell, SpellTalent
{
    public bool PassivelyTriggered => true; //dont add to hotbar

    public override string nameFormatted => "Ward Consumed Heal";
    public override string description => "heal triggered from ward being consumed";
    public override Type spellTargeterType => typeof(TargetSelected);
    public override int spellGUIType => SpellGUIType.none;

    public TalentItem talent => Item.From<T_HealerWardConsumed>().asTalentItem;

    public S_HealerWardConsumed() : base() //TODO: add prop
    {

    }

    public override void InitDefaults(out SpellStaticProperties prop)
    {
        prop = new SpellStaticProperties();


        sfx_castStarted = "";
        sfxPitch_castStarted = new PitchAnimate.Params();
        sfxChannel_castStarted = "";
        sfx_castFinished = "my_sound_here";
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
        spellTypes = new List<int> { spellType.healing };
    }

    public override int CalcValidTarget(Entity target, Entity caster)
    {
        return CalcValidTargetHeal(target, caster); //only use on healable targets
    }

    public void MultiplyByTalentLevel(spellHit selfCast, int talentLevel)
    {
        //selfCast.power += TalentItem.CalcEffectFlat(ref talentLevel, );
    }

    public override void OnSuccessfullyFinishCast(spellHit spellHit)
    {
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

} //end spell S_HealerWardConsumed
