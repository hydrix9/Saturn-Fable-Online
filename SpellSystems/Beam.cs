using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    const float beamEndOffset = 1f;
    const int rayDefaultLength = 10;
    const float raycastWidthMultiplier = 1.5f; //when checking for collisions with beam, raycast use beam width multiplied by this

    LineRenderer line;
    float textureScrollSpeed; //how fast to scroll beam LineRender texture
    spellHit spellHit;
    Transform beamStart;
    Transform beamEnd;
    Vector3 rotateMovement = default;
    Transform trackTarget;
    Vector3 moveToPos; //used when want to just fire and forget move to a position
    float textureLengthScale;
    float trackSpeed = 0;
    Func<Entity, Entity, int> calcValidTarget;

    public bool isTrackingType => trackTarget != null;
    public bool isRotateType => rotateMovement != default;
    public bool isMoveToPosType => moveToPos != null;

    /// <summary>
    /// create beam, and use spellHit.target as tracking target for beam end
    /// </summary>
    /// <param name="trackTarget">what to follow at beam end</param>
    /// <param name="trackSpeed">how fast, zero will anchor itself exactly on it</param>
    /// <returns></returns>
    public Beam Set(GameObject startPrefab, GameObject endPrefab, GameObject lineRenderPrefab, Transform startAnchor, Vector3 startPos, spellHit spellHit, ref float textureScrollSpeed, ref float textureLengthScale, Transform trackTarget, ref float trackSpeed, Func<Entity, Entity, int> calcValidTarget)
    {
        this.trackTarget = trackTarget;
        this.trackSpeed = trackSpeed;
        this.calcValidTarget = calcValidTarget;
        Init(ref startPrefab, ref endPrefab, ref lineRenderPrefab, startAnchor, startPos, spellHit, ref textureScrollSpeed, ref textureLengthScale);

        if (trackTarget != null && trackSpeed != 0)
        { //if tracking something and we're following it slowly, not instantly
            ///don't want to use SmoothFollowAndRot any more, gives less control over the exact end pos/offset (beam should track slightly through target to ensure collision)
            //beamEnd.gameObject.AddComponent<SmoothFollowAndRot>().Set(trackTarget, false, null, -1, trackSpeed, false, false); //smoothly follow target slowly
        }

        return this;
    }

    /// <summary>
    /// create beam, assign a rotateMovement which will cause the beam to move
    /// </summary>
    public Beam Set(GameObject startPrefab, GameObject endPrefab, GameObject lineRenderPrefab, Transform startAnchor, Vector3 startPos, spellHit spellHit, ref float textureScrollSpeed, ref float textureLengthScale, ref Vector3 rotateMovement, Func<Entity, Entity, int> calcValidTarget)
    {
        this.rotateMovement = rotateMovement;
        this.calcValidTarget = calcValidTarget;

        Init(ref startPrefab, ref endPrefab, ref lineRenderPrefab, startAnchor, startPos, spellHit, ref textureScrollSpeed, ref textureLengthScale);
        return this;
    }
    /// <summary>
    /// create beam, assign a moveToPos which will cause the beam to move
    /// </summary>
    public Beam Set_vMoveToPos(GameObject startPrefab, GameObject endPrefab, GameObject lineRenderPrefab, Transform startAnchor, Vector3 startPos, spellHit spellHit, ref float textureScrollSpeed, ref float textureLengthScale, ref Vector3 moveToPos, Func<Entity, Entity, int> calcValidTarget)
    {
        this.moveToPos = moveToPos;
        this.calcValidTarget = calcValidTarget;

        Init(ref startPrefab, ref endPrefab, ref lineRenderPrefab, startAnchor, startPos, spellHit, ref textureScrollSpeed, ref textureLengthScale);
        return this;
    }


    /// <summary>
    /// does common tasks between beam types from Set()
    /// </summary>
    private void Init(ref GameObject startPrefab, ref GameObject endPrefab, ref GameObject lineRenderPrefab, Transform startAnchor, Vector3 startPos, spellHit spellHit, ref float textureScrollSpeed, ref float textureLengthScale)
    {
        this.spellHit = spellHit;
        this.textureScrollSpeed = textureScrollSpeed;
        this.textureLengthScale = textureLengthScale;

        //instantiate prefabs
        beamStart = ObjectPool.GetInstance(startPrefab, startAnchor.position, startPrefab.transform.rotation, startAnchor).transform; //create and anchor beam start to caster or whatever

        beamEnd = ObjectPool.GetInstance(endPrefab, new Vector3(0, 0, 0), endPrefab.transform.rotation).transform;
        line = ObjectPool.GetInstance(lineRenderPrefab, new Vector3(0, 0, 0), lineRenderPrefab.transform.rotation).GetComponent<LineRenderer>();

        

        if (isRotateType)
        {
            beamEnd.SetParent(beamStart); //we will rotate entire thing, clamp to whatever it intersects with
        } else
            beamEnd.position = startPos; //assign to position set by spellHit

        //make the end caps face each other
        beamStart.LookAt(beamEnd.position);
        beamEnd.LookAt(beamStart.position);

        //update line renderer
        line.positionCount = 2;

        hitTimer = spellHit.interval; //begin countdown
        durationTimer = spellHit.duration; //begin countdown
        spellHit.caster.onDeath += OnParentDeath; //destroy when caster dies
    }

    public const float collisionTargetOffset = 1.5f; //how far to overshoot track target to ensure collision

    float hitTimer;
    float durationTimer;
    RaycastHit hit;
    Collider[] hits = new Collider[0];
    Entity targetTemp;
    private void Update()
    {
        durationTimer -= Time.deltaTime;
        if (durationTimer <= 0)
        {
            CleanDestroy(spellHit.caster);
        }
        if (spellHit == null || spellHit.caster == null || spellHit.caster.destroyed)
            CleanDestroy();

        if (beamStart == null || beamEnd == null || line == null)
            return;
        if (beamStart.parent != null)
            beamStart.localPosition = Vector3.zero; //fix weird offset issue

        //do tasks depending on the type of beam
        if (isTrackingType)
        {
            //make the end caps face each other
            beamStart.transform.LookAt(beamEnd.transform.position);
            beamEnd.transform.LookAt(beamStart.transform.position);

            if (trackSpeed <= 0) //if don't have track speed
                beamEnd.position = trackTarget.position; //follow target exactly
            else
            {
                //move to target and slightly overshoot to ensure collision
                beamEnd.position = Vector3.MoveTowards(beamEnd.position, trackTarget.position + ((trackTarget.position - spellHit.caster.transform.position).normalized * collisionTargetOffset), spellHit.moveSpeed * Time.deltaTime);
            }
        } else
        if (isRotateType)
        {
            beamStart.Rotate(rotateMovement); //turn the beam along axis


            //update beam line and end pos
            //do raycast, either clamp to whatever object it hits or make the beam a predetermined length
            if (Physics.Raycast(beamStart.position, beamEnd.position - beamStart.position, out hit, rayDefaultLength, WorldFunctions.worldMask, QueryTriggerInteraction.UseGlobal)) //raycast between start end end points
                beamEnd.position = hit.point - ((beamEnd.position - beamStart.position).normalized * beamEndOffset); //set to point + offset
            else
                beamEnd.position = beamStart.position + ((beamEnd.position - beamStart.position).normalized * rayDefaultLength); //set to max length
        }
        else
        if(isMoveToPosType)
        {
            //make the end caps face each other
            beamStart.transform.LookAt(beamEnd.transform.position);
            beamEnd.transform.LookAt(beamStart.transform.position);

            beamEnd.transform.position = Vector3.MoveTowards(beamEnd.transform.position, moveToPos, spellHit.moveSpeed * Time.deltaTime);
        }
        //update line renderer
        line.SetPosition(0, beamStart.position);
        line.SetPosition(1, beamEnd.position);

        //scale the texture based on distance
        float distance = Vector3.Distance(spellHit.origin, beamEnd.transform.position);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);

        //scroll the beam texture
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);


    } //end Update

    private void FixedUpdate()
    {
        if (!Server.isServer)
        {
            return;
        }
        if (spellHit == null || spellHit.caster == null || spellHit.caster.destroyed)
            CleanDestroy();

        if (beamStart == null || beamEnd == null || line == null)
            return;

        //do hit effects
        hitTimer -= Time.deltaTime;
        //on every beam hit interval try hit...
        if (hitTimer <= 0)
        {
            hitTimer = spellHit.interval;
            //Ext.RunTaskThread(() =>
            //{
            hits = Physics.OverlapCapsule(beamStart.transform.position, beamEnd.transform.position, line.startWidth * raycastWidthMultiplier, WorldFunctions.entityMask); //get all entities beam hits within capsule of line width
            //});

            for (int i = 0; i < hits.Length; i++)
            {
                targetTemp = hits[i].GetComponent<Entity>(); //temp
                if (spellHit != null && targetTemp != null && targetTemp != spellHit.caster && !targetTemp.destroyed && calcValidTarget(targetTemp, spellHit.caster) == castFailCodes.success) //if is Entity / exists
                    spellHit.spell.OnAnimationHit(new spellHit(spellHit, spellHit.caster, targetTemp, spellHit.origin, spellHit.vTarget)); //do damage or whatever

            }
        }

    } //end func FixedUpdate

    void OnParentDeath(Entity parent, spellHit killingBlow = null)
    {
        CleanDestroy(parent);
    }

    void CleanDestroy(Entity parent = null)
    {
        if(parent != null)
            parent.onDeath -= OnParentDeath; //unsub
        if(beamStart != null)
            ObjectPool.Reclaim(beamStart.gameObject);
        if(beamEnd != null)
            ObjectPool.Reclaim(beamEnd.gameObject);
        if (line != null)
            ObjectPool.Reclaim(line.gameObject);
        line = null;
        spellHit = null;
        Destroy(this.gameObject);
    }

} //end class Beam
