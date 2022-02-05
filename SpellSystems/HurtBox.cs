using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MEC;

public class HurtBox : MonoBehaviour, ISerializableCastAnimationObject, IObjectPoolable<HurtBox>
{
    public DateTime expiresOn = DateTime.MinValue; //make sure it is always expired on first run
    public spellHit spellHit;
    Transform parent;

    //id is used to label self so we can tell client what triggered OnAnimationHit
    public int self_serializableCastID { get; set; } = 0; //reference to own entry in spellHit.serialization_ids, which is for designating what object triggered OnAnimationHit
    public bool destroyOn_OnAnimationHit { get; set; } = false; //whether to destroy this object on first contact automatically

    /// <summary>
    /// populate relevant data from spellHit
    /// </summary>
    public HurtBox Set(spellHit spellHit, DateTime expiresOn, Transform currentParent)
    {
        this.spellHit = spellHit;
        this.expiresOn = expiresOn;
        this.parent = currentParent;

        Timing.CallDelayed(spellHit.duration, CleanDestroy);

        return this;
    } //end func Set

    Entity targetTemp;
    private void OnTriggerEnter(Collider other)
    {

        if (DateTime.UtcNow > expiresOn)
            return; //HurtBox already expired

        targetTemp = other.GetComponent<Entity>(); //target them...

        //NOTE- won't be able to calculate an attack unless caster isn't destroyed yet... 
        //to fix you would need to delay GameObject.Destroy on caster as long as any projectiles remain
        if (spellHit != null && targetTemp != null && spellHit.caster != null && !spellHit.caster.destroyed && spellHit.spell.CalcValidTarget(targetTemp, spellHit.caster) == castFailCodes.success)
        {
            spellHit.spell.OnAnimationHit(new spellHit(spellHit, spellHit.caster, targetTemp, spellHit.origin, spellHit.vTarget), this);

        }
    } //end func OnTriggerEnter



    void OnCasterDeath(Entity died, spellHit killingBlow = null)
    {
        spellHit.spell.OnAnimationHit(new spellHit(spellHit, spellHit.caster, null, spellHit.origin, spellHit.vTarget), this); //will cause this to be destroyed and synced to clients
    } //end func OnCasterDeath

    public void CleanDestroy()
    { //clean refs then self destruct
        if (this != null && spellHit != null && spellHit.caster != null)
            spellHit.caster.onDeath -= OnCasterDeath;
        spellHit = null;
        expiresOn = DateTime.MinValue; //make automatically expired without explicit value set
        parent = null;

        if (this != null && gameObject != null)
            ObjectPool.Reclaim(gameObject); //"destroy" self
    }

    public void SetReclaimValues(GameObject original, IObjectPoolable<HurtBox> self)
    {
        HurtBox hurtBox = self as HurtBox;
        hurtBox.spellHit = null;
        hurtBox.expiresOn = DateTime.MinValue; //make automatically expired without explicit value set
        hurtBox.parent = null;
        hurtBox.transform.parent = null; //set back in to world with no parent

    }
} //end class HurtBox