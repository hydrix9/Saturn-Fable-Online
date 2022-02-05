using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// has OnTriggerEnter and will call OnAnimationHit on valid targets
/// </summary>
public class AuraZone : Summoned
{

    public static AuraZone AddTo(GameObject target, spellHit spellHit)
    { //called from Auras, will add this script to a GameObject and set references
        AuraZone returns = target.AddComponent<AuraZone>();
        return returns;
    }

    Entity targetTemp;
    private void OnTriggerEnter(Collider other)
    {
        targetTemp = other.GetComponent<Entity>(); //target them...
        if (targetTemp != null && spellHit.spell.CalcValidTarget(targetTemp, spellHit.caster) == castFailCodes.success)
        {
            spellHit.spell.OnAnimationHit(new spellHit(spellHit, spellHit.caster, targetTemp, spellHit.origin, spellHit.vTarget));
        }
    }

    public void CleanDestroy()
    { //clean refs then self destruct
        spellHit = null;
        Destroy(gameObject); //destroy self
    }
}
