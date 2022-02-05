using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectVisuals : MonoBehaviour
{

    /// <summary>
    /// stub, instantiates the prefab on parent
    /// </summary>
    public static void AddTo(SpellStaticProperties prop, spellHit spellHit)
    {
        GameObject newObj = GameObject.Instantiate(prop.prefab, prop.calcStartAnchor(spellHit), false);
    }


    /// <summary>
    /// instantiate the object and add an Aura component which will call spellHit.spell.OnAnimationHit on valid targets that collide
    /// </summary>
    public static void AddAuraZoneTo(SpellStaticProperties prop, spellHit spellHit)
    {
        GameObject newObj = GameObject.Instantiate(prop.prefab, prop.calcStartAnchor(spellHit), false);
        AuraZone.AddTo(newObj, spellHit);
    }

}
