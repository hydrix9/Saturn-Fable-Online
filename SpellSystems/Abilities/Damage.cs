using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;
using System;

//contains Attack() and Heal()
public class Damage
{

    public const int numRandomRolls = 2;

    /// <summary>
    /// entry point for ALL raw healing, handles death, triggers, updates and combat data
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="performHitTriggers">should be true if this is a normal heal, false if it is a on-hit effect or trigger so as not to trigger other things</param>
    /// <param name="performPostHitCalc">true if this is the final target. Generally true for direct attacks, OnAnimationHit, or AoEs, false for StatusEffects (already calculated final target) and true-damage</param>
    /// <returns></returns>
    public static int Heal(spellHit spellHit, bool performHitTriggers, bool performPostHitCalc)
    {
        if (spellHit.target.health <= 0 || spellHit.caster.health <= 0)
            return castFailCodes.dead;

        //spellHit.SetCalculatedTarget(spellHit.target); //do OnCalcDefensive calculations in a locked, safe way, to prevent double calculations due to jumping targets on spellHit or something

        if (performPostHitCalc)
        {
            CalcPostHit(spellHit); //perform final calculations, likely on .power, now that final target is certain (some bonuses only apply to certain targets)

            DefensiveCalc(spellHit); //perform bonuses from defender's mods, like Armor
        }

        if (performHitTriggers)
        {
            ///success! now actually do stuff
            spellHit.spell.OnHitHealOffensive(spellHit); //do procs
            spellHit.caster.OnHitHealOffensive(spellHit); //do procs
            TeamSpellModifier.instance.OnHitHealOffensive(spellHit); //perform team bonus calculations on caster
            spellHit.target.OnHitHealDefensive(spellHit); //do procs
            TeamSpellModifier.instance.OnHitHealDefensive(spellHit); //perform team bonus calculations on target
        }

        CalcCrit(spellHit, ref performHitTriggers);


        spellHit.target.health = Mathf.Min(spellHit.target.health + spellHit.power, spellHit.target.maxHealth); //take damage... more if crit

        // target.TriggerOnUpdate();


        spellHit.target.syncCasting.syncCombatData.AddEntry(ref spellHit.caster.id, ref spellHit.power, SyncCombatData.castTextCodes.heal);

        OnlineGUI.TryMakeCombatText(spellHit.target, ref spellHit.caster.id, spellHit.power, SyncCombatData.castTextCodes.heal); //fake receivign combat data on Server local player if exists


        return spellHit.power;
    }

    //modifiers which twist each attack around a random roll
    const float randomModiferMin = 0.9f;
    const float randomModifierMax = 1.1f;

    /// <summary>
    /// entry point for ALL raw damage, handles death, triggers, updates and combat data
    /// </summary>
    /// <param name="spellHit"></param>
    /// <param name="performHitTriggers">should be true if this is a normal attack, false if it is a hit-effect or trigger so as not to trigger other things (possibly looping infinitely)</param>
    /// <param name="putTargetInCombat">should be true for direct attacks, false for interval effects</param>
    /// <param name="performPostHitCalc">true for direct attacks, false for interval effects and true-damage like effects</param>
    /// <returns></returns>
    public static int Attack(spellHit spellHit, bool performHitTriggers, bool performPostHitCalc)
    {
        //Logger.Log("attack from " + spellHit.caster + " to " + spellHit.target + " with ability " + spellHit.spell.name);

        //spellHit.SetCalculatedTarget(spellHit.target); //do OnCalcDefensive calculations in a locked, safe way, to prevent double calculations due to jumping targets on spellHit or something

        if (spellHit.target.health <= 0 || spellHit.caster.health <= 0)
            return castFailCodes.dead;

        if (performPostHitCalc)
        {
            CalcPostHit(spellHit); //perform final calculations, likely on .power, now that final target is certain (some bonuses only apply to certain targets)

            DefensiveCalc(spellHit); //perform bonuses from defender's mods, like Armor
        }
        int blocked = CalcHitChance(spellHit);
        if (blocked != castFailCodes.success)
            return blocked;


        ///success! now actually do stuff
        if (performHitTriggers)
        {
            spellHit.spell.OnHitDamageOffensive(spellHit); //proc
            spellHit.caster.OnHitDamageOffensive(spellHit); //proc
            TeamSpellModifier.instance.OnHitDamageOffensive(spellHit); //perform team bonus calculations on caster
            spellHit.target.OnHitDamageDefensive(spellHit); //proc
            TeamSpellModifier.instance.OnHitDamageDefensive(spellHit); //perform team bonus calculations on castertarget
        }

        if(spellHit.interval <= 0 && spellHit.duration <= 0)
        { //if this is a direct attack, not a DoT (note- adding debuffs will also put "in combat" from StatusEffect)
            SyncCombatState.SetFrom(spellHit); //put target and caster "in combat"
        }


        CalcCrit(spellHit, ref performHitTriggers);

        ApplyDamage(spellHit); //remove health, possibly kill target, add combat data, trigger GUI updates

        if (spellHit.leechHealthPercent > 0)
        { //do leech effect if it has one
            spellHit healEffect = new spellHit(spellHit, spellHit.caster, spellHit.caster, spellHit.origin, spellHit.vTarget);
            healEffect.power = (int)(spellHit.power * spellHit.leechHealthPercent);
            Heal(healEffect, false, false);
        }

        if(spellHit.damageReflected > 0)
        { //apply reflect effect if it has one
            spellHit reflectEffect = new spellHit(S_Utility_ReflectDamage.empty, spellHit.target, spellHit.caster, default, default);
            reflectEffect.power = spellHit.damageReflected;
            ApplyDamage(reflectEffect);
        }

        return spellHit.power;
    }

    //apply damage after calculating everything, definitely shouldn't be used outside Damage class. Separted for purpose of doing "reflect" damage
    private static void ApplyDamage(spellHit spellHit)
    {

        spellHit.target.health = Mathf.Max(spellHit.target.health - spellHit.power, 0); //take damage... more if crit

        // target.TriggerOnUpdate();

        if(spellHit.power > 0) //if power is 0, wasn't technically damaged...
            spellHit.target.TriggerOnDamaged(spellHit); //should unstealth, trigger AI and maybe others

        if (Server.instance.player != null && Server.instance.player.id == spellHit.target.id && Server.instance.onlineGUI != null && Server.instance.onlineGUI.wowTargetingControls.currentTarget == null && spellHit.target != spellHit.caster) //if we don't have a target, they attacked us, and not casting at self (this is done here for server alone because it is done on client during SyncCasting.Write)
            Server.instance.onlineGUI.wowTargetingControls.AssignTarget(spellHit.caster); //auto target them because they casted at us...

        spellHit.target.syncCasting.syncCombatData.AddEntry(ref spellHit.caster.id, ref spellHit.power, SyncCombatData.castTextCodes.damage);

        OnlineGUI.TryMakeCombatText(spellHit.target, ref spellHit.caster.id, spellHit.power, SyncCombatData.castTextCodes.damage); //fake receivign combat data on Server local player if exists

        if (spellHit.target.health <= 0)
        {
            Kill(spellHit.target, spellHit);
        }

        spellHit.target.TriggerOnUpdate(); //for possibly local player on server
    }

    /// <summary>
    /// sets health to zero, calls "combat triggers" like OnKill, and adds EXP, but DOES NOT destroy the entity automatically- destroying should be configured based on isPlayer/it has loot/etc...
    /// </summary>
    public static void Kill(Entity target, spellHit killingBlow = null)
    {
        target.health = 0; //safety

        target.Death(killingBlow); //wont necessarily destroy it..

        if (killingBlow != null)
        {   //do "combat" triggers
            killingBlow.spell.OnKillOffensive(killingBlow);
            killingBlow.caster.OnKillOffensive(killingBlow);
            TeamSpellModifier.instance.OnKillOffensive(killingBlow); //perform team bonus calculations on caster
            target.OnKillDefensive(killingBlow);
            TeamSpellModifier.instance.OnKillDefensive(killingBlow); //perform team bonus calculations on target

            ExpConfig.instance.TryAddExp(killingBlow.caster, target);
        }

        if (target is Player)
            GameMode.instance.OnPlayerDeath_Server(target.p, killingBlow);
        else
        if (target.GetComponent<AI>() != null)
            GameMode.instance.OnAI_Death(target.GetComponent<AI>());

    }

    /// <summary>
    /// not only kills target, but also destroys them (useful for mobs). Merely calls Damage.Kill(target) then target.Destroy(). note- you won't be able to loot this entity after destroyed if configured that way
    /// </summary>
    public static void KillAndDestroy(Entity target, spellHit killingBlow = null)
    {
        Kill(target, killingBlow);
        target.Destroy();
    }

    //perform some hit calculations here due to the uncertainty of having a target in some situations (like projectiles) until the very end
    public static void CalcPostHit(spellHit spellHit)
    {
        //from spell
        spellHit.spell.CalcCastFlatOffensive_PostHit(spellHit); //populate with base info on range, cast time, damage, etc
        spellHit.spell.CalcCastPerOffensive_PostHit(spellHit); //populate with further info on range, cast time, etc

        //bonuses from caster on everything, importantly cast time and range
        spellHit.caster.CalcCastFlatOffensive_PostHit(spellHit);
        spellHit.caster.CalcCastPerOffensive_PostHit(spellHit);

        TeamSpellModifier.instance.CalcCastFlatOffensive_PostHit(spellHit); //perform team bonus calculations on caster
        TeamSpellModifier.instance.CalcCastPerOffensive_PostHit(spellHit); //perform team bonus calculations on caster

    } //end func CalcPostHit

    //perform bonuses from the target's defensive stats
    public static void DefensiveCalc(spellHit spellHit)
    {
        //need to sanitize the code from calculating defensive bonuses twice, like on a StatusEffect+AoE
        //best way to do this is to only call this from Damage.Attack, Damage.Heal, StatusEffects.TryAdd, or similar funnel points

        spellHit.target.CalcCastFlatDefensive(spellHit);
        SyncArmor.TryCalc(spellHit, spellHit.target); //do armor damage reduction if exists
        spellHit.target.CalcCastPerDefensive(spellHit);

        TeamSpellModifier.instance.CalcCastFlatDefensive(spellHit); //perform team bonus calculations on this.target
        TeamSpellModifier.instance.CalcCastPerDefensive(spellHit); //perform team bonus calculations on this.target
    } //end func DefensiveCalc

    public static void CalcCrit(spellHit spellHit, ref bool performHitTriggers)
    {
        Ext.ModExponentially10(ref spellHit.power, randomModiferMin, randomModifierMax, numRandomRolls); //mod number with a far less chance to be extreme in either pole

        if (Ext.Roll100(spellHit.critChance))
        {
            spellHit.power = (int)(spellHit.power * spellHit.critBonus); //crit chance

            if (performHitTriggers)
            {
                spellHit.spell.OnCritOffensive(spellHit); //do special move, if any

                spellHit.caster.OnCritOffensive(spellHit);
                TeamSpellModifier.instance.OnCritOffensive(spellHit); //perform team bonus calculations on caster
                spellHit.target.OnCritDefensive(spellHit);
                TeamSpellModifier.instance.OnCritDefensive(spellHit); //perform team bonus calculations on target
            }

        }

    }

    public static int CalcHitChance(spellHit spellHit)
    {
        if (spellHit.caster.id == spellHit.target.id || GameMode.instance.CheckFriendly(spellHit.caster, spellHit.target))
            return castFailCodes.success; //if we're targeting ourself, can't miss

        //run all calculations that change the hit chance on caster and target
        CalcHitChanceFromLevel(spellHit);

        spellHit.spell.OnCalcHitChanceOffensive(spellHit);
        spellHit.caster.OnCalcHitChanceOffensive(spellHit);
        TeamSpellModifier.instance.OnCalcHitChanceOffensive(spellHit); //perform team bonus calculations on caster
        spellHit.target.OnCalcHitChanceDefensive(spellHit);
        TeamSpellModifier.instance.OnCalcHitChanceDefensive(spellHit); //perform team bonus calculations on target

        if (Ext.Roll100(spellHit.evadeChance))
        {

            spellHit.target.syncCasting.syncCombatData.AddEntry(ref spellHit.caster.id, ref spellHit.power, SyncCombatData.castTextCodes.evaded);
            OnlineGUI.TryMakeCombatText(spellHit.target, ref spellHit.caster.id, 0, SyncCombatData.castTextCodes.evaded);
            return castFailCodes.evaded;
        }

        if (!Ext.Roll100(spellHit.hitChance))
        {
            spellHit.caster.OnMissOffensive(spellHit);
            TeamSpellModifier.instance.OnMissOffensive(spellHit); //perform team bonus calculations on caster
            spellHit.target.OnMissDefensive(spellHit);
            TeamSpellModifier.instance.OnMissDefensive(spellHit); //perform team bonus calculations on target

            //also do OnNotHit since miss avoids damage completely
            spellHit.caster.OnNotHitOffensive(spellHit); //run effects for "on any ability not hit," this is run on a miss and dodge
            TeamSpellModifier.instance.OnNotHitOffensive(spellHit); //perform team bonus calculations on caster
            spellHit.caster.OnNotHitDefensive(spellHit); //run effects for "on any ability not hit," this is run on a miss and dodge
            TeamSpellModifier.instance.OnNotHitDefensive(spellHit); //perform team bonus calculations on caster

            spellHit.target.syncCasting.syncCombatData.AddEntry(ref spellHit.caster.id, ref spellHit.power, SyncCombatData.castTextCodes.miss);
            OnlineGUI.TryMakeCombatText(spellHit.target, ref spellHit.caster.id, 0, SyncCombatData.castTextCodes.miss);
            return castFailCodes.miss;
        }

        if (Ext.Roll100(spellHit.dodgeChance))
        {
            spellHit.caster.OnDodgeOffensive(spellHit);
            TeamSpellModifier.instance.OnDodgeOffensive(spellHit); //perform team bonus calculations on caster
            spellHit.target.OnDodgeDefensive(spellHit);
            TeamSpellModifier.instance.OnDodgeDefensive(spellHit); //perform team bonus calculations on target

            //also do OnNotHit since a dodge avoids damage completely
            spellHit.caster.OnNotHitOffensive(spellHit); //run effects for "on any ability not hit," this is run on a miss and dodge
            TeamSpellModifier.instance.OnNotHitOffensive(spellHit); //perform team bonus calculations on caster
            spellHit.caster.OnNotHitDefensive(spellHit); //run effects for "on any ability not hit," this is run on a miss and dodge
            TeamSpellModifier.instance.OnNotHitDefensive(spellHit); //perform team bonus calculations on target

            spellHit.target.syncCasting.syncCombatData.AddEntry(ref spellHit.caster.id, ref spellHit.power, SyncCombatData.castTextCodes.dodged);
            OnlineGUI.TryMakeCombatText(spellHit.target, ref spellHit.caster.id, 0, SyncCombatData.castTextCodes.dodged);
            return castFailCodes.dodged;
        }

        if (Ext.Roll100(spellHit.parryChance))
        {
            spellHit.caster.OnParryOffensive(spellHit);
            TeamSpellModifier.instance.OnParryOffensive(spellHit); //perform team bonus calculations on caster
            spellHit.target.OnParryDefensive(spellHit);
            TeamSpellModifier.instance.OnParryDefensive(spellHit); //perform team bonus calculations on target

            spellHit.target.syncCasting.syncCombatData.AddEntry(ref spellHit.caster.id, ref spellHit.power, SyncCombatData.castTextCodes.parry);
            OnlineGUI.TryMakeCombatText(spellHit.target, ref spellHit.caster.id, 0, SyncCombatData.castTextCodes.parry);
            return castFailCodes.parry;
        }

        if (Ext.Roll100(spellHit.blockChance))
        {
            spellHit.caster.OnBlockOffensive(spellHit);
            TeamSpellModifier.instance.OnBlockOffensive(spellHit); //perform team bonus calculations on caster
            spellHit.target.OnBlockDefensive(spellHit);
            TeamSpellModifier.instance.OnBlockDefensive(spellHit); //perform team bonus calculations on target
            //TODO: run calculations to block, which means run OnCalcBlockDefensive on the spell and on the defender to modify the damage

            //spellHit.caster.syncCasting.syncCombatData.AddEntry(ref spellHit.caster.id, ref spellHit.power, SyncCombatData.castTextCodes.dodged);
            //OnlineGUI.TryMakeCombatText(spellHit.target, spellHit.caster.id, "dodge");
        }


        return castFailCodes.success;
    }

    public static int CalcHitChanceFromLevel(spellHit spellHit)
    {
        return castFailCodes.success;
    }

} //end class Damage