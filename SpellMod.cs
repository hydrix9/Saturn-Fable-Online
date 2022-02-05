using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//SpellMods are static-like functions that are called when something is cast. They individually manage to check if they apply to defense, offense, crits, parries, etc
public interface SpellMod
{
    //the below are used for ALL SPELL LOGIC. These allow entry points into modifying the outcome of a cast, the spelLHit. This is from bonuses from talents, armor, buffs, etc. It could be because of the spell ID, spell type, or enemy type. The spellMods look at this data
    //and decides to modify the outcome ON THEIR OWN. This is EXTREMELY FLEXABLE and MODULAR. A spellmod can be a override function which runs MULTIPLE OTHER SPELLMODS. HOLY GRAIL OF SPELL SYSTEMS.

    //looping through all the bonuses like this and calling possibly empty virtual functions actually takes up almost zero overhead. Millions of calls in a loop should only take a fraction of a millisecond.
    //there's 2 of each, Offensive and Defensive. This is just to tell the SpellMod whether it's being triggered offensively, or defensively without using any if statements in each one



    /// <summary>
    /// called after passing all sanity checks on a spell and finishing the cast time
    /// </summary>
    void OnTryHitOffensive(spellHit spellHit);


    //successfully landed hit. Do damage, remove a charge of this buff, or anything a spell would do. You can even take the damage from the spellHit and do an AoE, which will call further SpellMods ofc.
    //this is pretty much the last step before OnKill. We already calculated the enemy's effects too, and determined a hit.
    void OnHitDamageOffensive(spellHit spellHit);
    void OnHitDamageDefensive(spellHit spellHit);
    void OnHitHealOffensive(spellHit spellHit);
    void OnHitHealDefensive(spellHit spellHit);


    /// <summary>
    /// called on offender when an ability misses them for any reason other than evade (used for meta errors like mob de-aggroing) this should be either miss or dodge, not to be confused with OnMissOffensive
    /// </summary>
    void OnNotHitOffensive(spellHit spellHit);
    /// <summary>
    /// called on defender when their attack misses for any reason other than evade (used for meta errors like mob de-aggroing) this should be either miss or dodge, not to be confused with OnMissDefensive
    /// </summary>
    void OnNotHitDefensive(spellHit spellHit);

    void OnCritOffensive(spellHit spellHit);
    void OnCritDefensive(spellHit spellHit);

    //calculate range, speed of cast
    //flat bonuses are calculated first, then percentage bonuses, as you would expect.
    //this is where you would define power to the spellhit. A "base" Spell SpellMod would add it's damage here too.
    void CalcCastFlatOffensive_PreHit(spellHit spellHit);

    //calculate further effects in form of percentages on range, speed, and stuff needed to know before cast
    void CalcCastPerOffensive_PreHit(spellHit spellHit);

    //calculate range, speed of cast
    //flat bonuses are calculated first, then percentage bonuses, as you would expect.
    //this is where you would define power to the spellhit. A "base" Spell SpellMod would add it's damage here too.
    void CalcCastFlatOffensive_PostHit(spellHit spellHit);

    //calculate further effects in form of percentages on range, speed, and stuff needed to know before cast
    void CalcCastPerOffensive_PostHit(spellHit spellHit);



    //what to do when you kill an enemy
    void OnKillOffensive(spellHit spellHit);
    /// <summary>
    /// what to do when killed
    /// </summary>
    /// <param name="spellHit"></param>
    void OnKillDefensive(spellHit spellHit);

    //calculate spell type based on talents
    void CalcSpellTypeOffensive(spellHit spellHit);

    void OnCalcHitChanceOffensive(spellHit spellHit);


    //defensive versions of the above
    //void OnTryHitDefensive(spellHit spellHit);

    /// <summary>
    /// called on attacker when their own attack misses due to hit chance
    /// </summary>
    void OnMissOffensive(spellHit spellHit);
    /// <summary>
    /// called on defender when an attack misses due to hit chance
    /// </summary>
    void OnMissDefensive(spellHit spellHit);
    void OnParryOffensive(spellHit spellHit);
    void OnParryDefensive(spellHit spellHit);
    void OnDodgeOffensive(spellHit spellHit);
    void OnDodgeDefensive(spellHit spellHit);
    void OnBlockOffensive(spellHit spellHit);
    void OnBlockDefensive(spellHit spellHit);

    void CalcCastFlatDefensive(spellHit spellHit);
    void CalcCastPerDefensive(spellHit spellHit);
    void OnCalcHitChanceDefensive(spellHit spellHit);

    void CalcMinChargeUsed(spellHit spellHit); //called when spell is first cast
    void CalcMaxChargeUsed(spellHit spellHit); //called when spell is first cast
    void OnUseCharge(spellHit spellHit); //called when spell is finished casting and consumes charge
    void CalcChargeGain(spellHit spellHit); //called when spell is finished casting to add charge to caster
    void OnGainCharge(spellHit spellHit);

    /// <summary>
    /// called on dispeller when forcisbly dispelled, as with a Cleanse or Purge (not naturally, as with a shield wearing off)
    /// </summary>
    void OnDispelStatusEffect_Offensive(spellHit spellHit, spellHit statusEffect);

    /// <summary>
    /// called on status effect caster when their status effect is forcisbly dispelled, as with a Cleanse or Purge (not naturally, as with a shield wearing off)
    /// </summary>
    void OnDispelStatusEffect_Defensive(spellHit spellHit, spellHit statusEffect);


    /// <summary>
    /// when naturally wears off, as in the duration expiring, and nothing else
    /// </summary>
    void OnExpireStatusEffect_Caster(spellHit expired);

    /// <summary>
    /// called on caster when caster's status effect is consumed, as with a Conflagrate from same-caster or a shield being eaten through
    /// </summary>
    void OnConsumeStatusEffect_Caster(spellHit spellHit, spellHit statusEffect);



    void OnAddSummoned(spellHit spellHit, Summoned obj);
}




public abstract class TalentMod : SpellMod
{
    //(unused) would have been used for Spell.CalcEffectFromLevel
    //we could have used this field, instead we just use entity.GetTalentLevel if the Spell is a SpellTalent interface
    private int selfLevel; 

    public TalentMod(int currentLevel)
    {

    }

    /// <summary>
    /// second "constructor" that should create a new instance of self with the current level
    /// </summary>
    public abstract TalentMod Constructor(TalentMod self, int currentLevel);

    //public Entity owner;
    #region functions
    public virtual void OnCalcHitChanceDefensive(spellHit spellHit)
    {
    }
    public virtual void CalcCastFlatDefensive(spellHit spellHit)
    {
    }
    public virtual void CalcCastFlatOffensive_PreHit(spellHit spellHit)
    {
    }
    public virtual void CalcCastFlatOffensive_PostHit(spellHit spellHit)
    {
    }
    public virtual void CalcCastPerDefensive(spellHit spellHit)
    {
    }
    public virtual void CalcCastPerOffensive_PreHit(spellHit spellHit)
    {
    }
    public virtual void CalcCastPerOffensive_PostHit(spellHit spellHit)
    {
    }
    public virtual void CalcSpellTypeDefensive(spellHit spellHit)
    {
    }
    public virtual void CalcSpellTypeOffensive(spellHit spellHit)
    {
    }
    public virtual void OnCritDefensive(spellHit spellHit)
    {
    }
    public virtual void OnCritOffensive(spellHit spellHit)
    {
    }
    public virtual void OnHitDamageDefensive(spellHit spellHit)
    {
    }
    public virtual void OnHitDamageOffensive(spellHit spellHit)
    {
    }
    public virtual void OnHitHealDefensive(spellHit spellHit)
    {
    }
    public virtual void OnHitHealOffensive(spellHit spellHit)
    {
    }
    public virtual void OnKillDefensive(spellHit spellHit)
    {
    }
    public virtual void OnKillOffensive(spellHit spellHit)
    {
    }
    public virtual void OnNotHitDefensive(spellHit spellHit)
    {
    }
    public virtual void OnNotHitOffensive(spellHit spellHit)
    {
    }
    /* isn't a thing... when we try to hit defender shouldn't necessarily do anything...
    public void OnTryHitDefensive(spellHit spellHit)
    {
    } */
    public virtual void OnTryHitOffensive(spellHit spellHit)
    {
    }
    public virtual void OnCalcHitChanceOffensive(spellHit spellHit)
    {
    }
    public virtual void CalcMinChargeUsed(spellHit spellHit)
    {
    }
    public virtual void CalcMaxChargeUsed(spellHit spellHit)
    {
    }
    public virtual void CalcChargeGain(spellHit spellHit)
    {
    }
    public virtual void OnUseCharge(spellHit spellHit)
    {
    }
    public virtual void OnGainCharge(spellHit spellHit)
    {
    }
    public virtual void OnAddSummoned(spellHit spellHit, Summoned obj)
    {
    }

    public virtual void OnMissOffensive(spellHit spellHit)
    {
    }

    public virtual void OnMissDefensive(spellHit spellHit)
    {
    }

    public virtual void OnParryOffensive(spellHit spellHit)
    {
    }

    public virtual void OnParryDefensive(spellHit spellHit)
    {
    }

    public virtual void OnDodgeOffensive(spellHit spellHit)
    {
    }

    public virtual void OnDodgeDefensive(spellHit spellHit)
    {
    }

    public virtual void OnBlockOffensive(spellHit spellHit)
    {
    }

    public virtual void OnBlockDefensive(spellHit spellHit)
    {
    }
    /// <summary>
    /// called on dispeller when they dispel a status effect
    /// </summary>
    public virtual void OnDispelStatusEffect_Offensive(spellHit spellHit, spellHit statusEffect)
    {
    }
    /// <summary>
    /// called on caster when their status effect is dispelled
    /// </summary>
    public virtual void OnDispelStatusEffect_Defensive(spellHit spellHit, spellHit statusEffect)
    {
    }
    /// <summary>
    /// called on caster when their status effect expires
    /// </summary>
    public virtual void OnExpireStatusEffect_Caster(spellHit statusEffect)
    {
    }
    /// <summary>
    /// called on status effect caster when their status effect is consumed
    /// </summary>
    public virtual void OnConsumeStatusEffect_Caster(spellHit spellHit, spellHit statusEffect)
    {
    }

    #endregion
} //end class TalentMod