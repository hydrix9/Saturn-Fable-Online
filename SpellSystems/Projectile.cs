using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ISerializableCastAnimationObject defines that this must have an id that SyncUnsentCasts can write to for referencing OnAnimationHit target obj to client, called self_serializableCastID
public class Projectile : MonoBehaviour, ISerializableCastAnimationObject, IObjectPoolable<Projectile>
{
    const float default_projectile_duration = 10f;
    static float default_homing_radius = 30f;
    const float checkHomingTargetInteval = 0.75f;

    //id is used to label self so we can tell client what triggered OnAnimationHit
    public int self_serializableCastID { get; set; } = 0; //reference to own entry in spellHit.serialization_ids, which is for designating what object triggered OnAnimationHit
    public bool destroyOn_OnAnimationHit { get; set; } = true; //whether to destroy this object on first contact automatically

    public SpellStaticProperties.HomingType homingType;

    Entity homingTarget = null;
    public spellHit spellHit;

    bool lockXZ; //whether to use xz as in the case of 3D, or not, as in the case of 2D

    float checkHomingTargetTimer; //check homing performs a SphereCast on every projectile from every mob, so we have to slow it down with this timer..
    //actually set to spellHit.duration if it !=0
    float destructionTimer = default_projectile_duration;
    // Update is called once per frame


    private void Awake()
    {
        //put this projectile on a layer that can interact with entities
        if (Server.isServer)
            gameObject.layer = LayerMask.NameToLayer(WorldFunctions.entityInteractionLayerName);
        else
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); //client should not do physics on this object...
    }

    /*
    private void Start()
    {

    } //end func Start
    */

    void Update()
    {
        if (this == null || transform == null || spellHit == null || transform == null)
            return; //do nothing, reference not set yet

        if (checkHomingTargetTimer <= 0)
        {
            checkHomingTargetTimer = checkHomingTargetInteval;
            CalcEffects();
        }

        checkHomingTargetTimer -= Time.deltaTime;
        destructionTimer -= Time.deltaTime; //count down so projectiles don't go infinitely
        if (destructionTimer <= 0)
            CleanDestroy();

        #region movement
        if (homingTarget != null)
        {
            //calculate LookAt, then interpoliate to that
            homingRotationTemp = transform.rotation;
            transform.LookAt(homingTarget.transform);
            transform.rotation = Quaternion.LerpUnclamped(homingRotationTemp, transform.rotation, turnSpeed * Time.deltaTime);
            if (lockXZ)
            { //if lockXZ will reset XZ rotation back to normal
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
        }

        if (spellHit == null)
            return; //do nothing, reference not set yet
        transform.position += transform.forward * spellHit.moveSpeed * Time.deltaTime;
        #endregion
    }

    public float turnSpeed = 0.1f;

    Quaternion homingRotationTemp;

    /* fixed update isn't smooth enough, and don't want to lerp these manually..
    private void FixedUpdate()
    {

    } //end FixedUpdate
    */
    void CalcEffects()
    { //do all possible projectile movement
        CalcHoming();
    }

    const bool alwaysHomeClosest = true;

    void CalcHoming()
    { //move toward target we are homing at
        if (alwaysHomeClosest || homingTarget == null)
        {
            if (spellHit.caster == null)
                return; //entity was destroyed...

            if (homingType == SpellStaticProperties.HomingType.enemy)
            {
                if(spellHit.radius != 0)
                    AOEs.GetClosestEnemy(spellHit.caster, transform, ref spellHit.radius, out homingTarget);
                else
                    AOEs.GetClosestEnemy(spellHit.caster, transform, ref default_homing_radius, out homingTarget);
            }
            else
            if (homingType == SpellStaticProperties.HomingType.friendly)
            {
                if (spellHit.radius != 0)
                    AOEs.GetClosestFriendly(spellHit.caster, transform, ref spellHit.radius, out homingTarget);
                else
                    AOEs.GetClosestFriendly(spellHit.caster, transform, ref default_homing_radius, out homingTarget);
            }
            else
            if (homingType == SpellStaticProperties.HomingType.fromSpell)
            {
                if (spellHit.radius != 0)
                    AOEs.GetClosestValidTarget(spellHit.caster, transform, ref spellHit.radius, out homingTarget, spellHit.spell.CalcValidTarget);
                else
                    AOEs.GetClosestValidTarget(spellHit.caster, transform, ref default_homing_radius, out homingTarget, spellHit.spell.CalcValidTarget);
            }
        }
    }

    public static Projectile AddTo(GameObject target, spellHit spellHit, SpellStaticProperties.HomingType homingType, bool lockXZ, float turnSpeed = 0.1f, bool destroyOnHit = true)
    { //called from Projectiles, will add this script to a GameObject and set references
        Projectile returns = target.AddComponent<Projectile>();
        returns.homingType = homingType;
        returns.spellHit = spellHit;
        returns.lockXZ = lockXZ;
        returns.destroyOn_OnAnimationHit = destroyOnHit;
        returns.turnSpeed = turnSpeed;
        spellHit.caster.onDeath += returns.OnCasterDeath;
        if (spellHit.duration != 0)
            returns.destructionTimer = spellHit.duration;

        if(!Server.isServer && Client.instance.snapProjectilesToPingPosition && Client.instance.initPing)
        { //if configured to predict actual projectile location based on ping and have enough ping to form a baseline average
            returns.transform.position += returns.transform.forward * spellHit.moveSpeed * (float)(Client.instance.averageFromServerPing / 1000); //predict actual position based on ping
        }

        return returns;
    }

    Entity targetTemp;


    private void OnTriggerEnter(Collider other)
    {

        targetTemp = other.GetComponent<Entity>(); //target them...

        //NOTE- won't be able to calculate an attack unless caster isn't destroyed yet... 
        //to fix you would need to delay GameObject.Destroy on caster as long as any projectiles remain
        if (spellHit != null && targetTemp != null && spellHit.caster != null && !spellHit.caster.destroyed && spellHit.spell.CalcValidTarget(targetTemp, spellHit.caster) == castFailCodes.success)
        {
            spellHit.spell.OnAnimationHit(new spellHit(spellHit, spellHit.caster, targetTemp, spellHit.origin, spellHit.vTarget), this);

        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (!destroyOn_OnAnimationHit)
        { //only do OnTriggerStay if we don't expect to be destroyed on first hit
            targetTemp = other.GetComponent<Entity>(); //target them...

            if (spellHit != null && targetTemp != null && spellHit.caster != null && spellHit.spell.CalcValidTarget(targetTemp, spellHit.caster) == castFailCodes.success)
            {
                spellHit.spell.OnAnimationHit(new spellHit(spellHit, spellHit.caster, targetTemp, spellHit.origin, spellHit.vTarget));
            }
        }
    }

    void OnCasterDeath(Entity died, spellHit killingBlow = null)
    {
        spellHit.spell.OnAnimationHit(new spellHit(spellHit, spellHit.caster, null, spellHit.origin, spellHit.vTarget), this); //will cause this to be destroyed and synced to clients
    } //end func OnCasterDeath

    public void CleanDestroy()
    { //clean refs then self destruct
        if (this != null && spellHit != null && spellHit.caster != null)
            spellHit.caster.onDeath -= OnCasterDeath;
        spellHit = null;
        homingTarget = null;
        homingType = default;

        if(this != null && gameObject != null)
            ObjectPool.Reclaim(gameObject); //"destroy" self
    }

    public void SetReclaimValues(GameObject original, IObjectPoolable<Projectile> self)
    {
        Projectile proj = self as Projectile; //pretty sure we don't even need this parameter since this?. is the instance
        GameObject.Destroy(this); //remove Projectile component
    } //end func SetReclaimValues

} //end class Projectile