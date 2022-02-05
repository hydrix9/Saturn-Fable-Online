using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// summoned entity that calls an AOE action when triggered by a valid enemy
/// </summary>
public class Mine : Summoned
{
    Spell castOnTriggered;
    /// <summary>
    /// automatically create and add spherecollider using spellHit.radius
    /// </summary>
    public void Set(spellHit spellHit, Spell castOnTriggered, bool destroyOnCasterDeath = true)
    {
        base.Set(spellHit, destroyOnCasterDeath);
        this.castOnTriggered = castOnTriggered;
        gameObject.layer = LayerMask.NameToLayer("AggroLayer"); //put it on the aggro layer so it only collides with entities
        
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = spellHit.radius;
    }


    SphereCollider sphereCollider;

    void TryActivate(Entity target)
    {
        if (target == null)
            return;

        if (transform != null && spellHit != null)
        {
            if (spellHit.caster != null)
            { //if caster isn't dead/missing
                if (castOnTriggered.CalcValidTarget(target, spellHit.caster) == castFailCodes.success)
                { //if it is a valid target
                    //cause caster to use this spell with vTarget and origin at our position
                    castOnTriggered.ApplyRaw(spellHit.caster, default, transform.position, transform.position);
                    DestroyClean(self); //destroy self
                }
            } else //if caster is null...
                DestroyClean(self); //destroy self since caster is dead/missing
        }
        
    } //end TryActivate

    protected override void Clean(SyncObject owner)
    {
        base.Clean(owner);
        sphereCollider = null;
    }

    public void OnTriggerEnter(Collider other)
    { //called when colliding with other collider, when one is a trigger
        TryActivate(other.GetComponent<Entity>());
    } //end OnTriggerEnter

} //end class Mine
