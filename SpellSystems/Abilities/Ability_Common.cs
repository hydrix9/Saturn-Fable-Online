using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;
using System;
using UnityEngine.AI;

public partial class Ability
{

    public static void Inturrupt(spellHit spellHit)
    {
        SyncCombatState.SetFrom(spellHit); //put target and caster "in combat"

        int tryInturrupt = spellHit.target.syncCasting.TryInturrupt();
        if (tryInturrupt > 0)
        { //if successful inturrupt
            Logger.LogWarning("successful inturrupt by " + spellHit.caster.name + " on " + spellHit.target.name);

            if (spellHit.target.p != null)
            { //if is player...
                Server.instance.SendPlayer(new byte[] { netcodes.inturruptedYou, (byte)tryInturrupt, (byte)(tryInturrupt >> 8), (byte)(spellHit.duration * 1000), (byte)((int)(spellHit.duration * 1000) >> 8) }, spellHit.target.p.point);
            }
        }
        else
        {
            Logger.LogWarning("failed inturrupt by " + spellHit.caster.name + " on " + spellHit.target.name);
        }
    }


    const float teleportQueryDistance = 10f;
    static NavMeshHit navHit;
    public static void TeleportBehindEntity(spellHit spellHit, float offset)
    {
        if (NavMesh.SamplePosition(spellHit.target.transform.position + ((spellHit.target.transform.position - spellHit.caster.transform.position).normalized * offset), out navHit, teleportQueryDistance, NavMesh.AllAreas)) //if successfully found
        {
            //set them on nav mesh
            spellHit.caster.transform.position = navHit.position;
            spellHit.caster.transform.rotation = spellHit.target.transform.rotation;
        }
        else {
            //was unable to find valid target, do nothing...
            //spellHit.caster.transform.position = spellHit.target.transform.position + ((spellHit.target.transform.position - spellHit.caster.transform.position).normalized * offset);
        }
    }


    public static void TeleportToEntity(spellHit spellHit)
    {
        spellHit.caster.transform.position = spellHit.target.transform.position;
        //spellHit.caster.transform.rotation = spellHit.target.transform.rotation;
    }

    public static void TeleportToVTarget(spellHit spellHit)
    {
        spellHit.caster.transform.position = spellHit.vTarget;
        //spellHit.caster.transform.rotation = spellHit.target.transform.rotation;
    }

    /// <summary>
    /// brings the target to the caster, like Death Knight yoink spell
    /// </summary>
    public static void Yoink(spellHit spellHit, float offset = 0.5f)
    {
        SyncCombatState.SetFrom(spellHit); //put target and caster "in combat"

        spellHit.target.transform.position = spellHit.caster.transform.position + ((spellHit.caster.transform.position - spellHit.target.transform.position).normalized * offset);
        //spellHit.caster.transform.rotation = spellHit.target.transform.rotation;
    }

    public static void Taunt(spellHit spellHit)
    {
        if(spellHit.target.GetComponent<AI>() != null)
            spellHit.target.GetComponent<AI>().currentTarget = spellHit.caster; //go thorugh AI script instead
        else
            spellHit.target.Set<int>(SyncCasting.currentTarget, spellHit.caster.id);
    } //end func Taunt

    public static void AddCharge(Entity target, ref float amount)
    {
        if (target.ContainsSyncField(SyncCharge.charge))
            target.Addition_Clamped<float>(SyncCharge.charge, ref amount, 0, target.Get<float>(SyncCharge.maxCharge));
    } //end AddCharge

    public static void SubtractCharge(Entity target, ref float amount)
    {
        if (target.ContainsSyncField(SyncCharge.charge))
            target.Subtraction_Clamped<float>(SyncCharge.charge, ref amount, 0, target.Get<float>(SyncCharge.maxCharge));
    } //end SubtractCharge
    public static void AddCharge(Entity target, float amount)
    {
        if (target.ContainsSyncField(SyncCharge.charge))
            target.Addition_Clamped<float>(SyncCharge.charge, ref amount, 0, target.Get<float>(SyncCharge.maxCharge));
    } //end AddCharge

    public static void SubtractCharge(Entity target, float amount)
    {
        if (target.ContainsSyncField(SyncCharge.charge))
            target.Subtraction_Clamped<float>(SyncCharge.charge, ref amount, 0, target.Get<int>(SyncCharge.maxCharge));
    } //end SubtractCharge
    
    public static void AddEnergy(Entity target, ref int amount)
    {
        if (target.ContainsSyncField(SyncEnergy.energy))
            target.Addition_Clamped<int>(SyncEnergy.energy, ref amount, 0, target.Get<int>(SyncEnergy.maxEnergy));
    } //end AddEnergy

    public static void SubtractEnergy(Entity target, ref int amount)
    {
        if (target.ContainsSyncField(SyncEnergy.energy))
            target.Subtraction_Clamped<int>(SyncEnergy.energy, ref amount, 0, target.Get<int>(SyncEnergy.maxEnergy));
    } //end SubtractEnergy
    public static void AddEnergy(Entity target, int amount)
    {
        if (target.ContainsSyncField(SyncEnergy.energy))
            target.Addition_Clamped<int>(SyncEnergy.energy, ref amount, 0, target.Get<int>(SyncEnergy.maxEnergy));
    } //end AddEnergy

    public static void SubtractEnergy(Entity target, int amount)
    {
        if (target.ContainsSyncField(SyncEnergy.energy))
            target.Subtraction_Clamped<int>(SyncEnergy.energy, ref amount, 0, target.Get<int>(SyncEnergy.maxEnergy));
    } //end SubtractEnergy




    /// <summary>
    /// call from OnHitDefensive(spellHit spellHit, spellHit selfStatusEffect)
    /// depletes charge from this barrier (selfStatusEffect.power_nonDamage) until completely gone, then removes it (StatusEffect_Consume)
    /// </summary>
    public static void BarrierEffect(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.power > 0 && !spellHit.isHeal)
        {
            spellHit.power -= Mathf.FloorToInt(selfStatusEffect.power_nonDamage);
            if (spellHit.power < 0)
            { //if absorbed more than full amount
                selfStatusEffect.power_nonDamage = -spellHit.power; //continue blocking....
                spellHit.power = 0; //attack is completely blocked
            }
            else
            {
                selfStatusEffect.target.StatusEffect_Consume(spellHit, selfStatusEffect); //remove barrier because we depleted its power
            }
        }
    } //end func BarrierEffect

    /// <summary>
    /// flat subtract healing from a selfStatusEffect using power_nonDamage, presumably called during CalcCastPerDefensive
    /// </summary>
    public static void MortalStrikeEffect(spellHit spellHit, spellHit selfStatusEffect)
    {
        if (spellHit.power > 0 && spellHit.target != null && spellHit.isHeal)
            spellHit.power = (int)(spellHit.power - (spellHit.power * selfStatusEffect.power_nonDamage)); //don't want to do divide or CalcEffectPer, because that would mean -100% only cuts it in half, unlike Mortal Strike calculation
    } //end func MortalStrikeEffect

} //end partial class Ability