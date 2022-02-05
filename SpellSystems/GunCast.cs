using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class with functions for casting spells on all raycasted targets
/// </summary>
public class GunCast
{
    private const float tracerDuration = 0.05f; //how long to show tracer rounds
    private const float raycastWidthMultiplier = 1.5f;

    public struct prefabIndex
    {
        public const int bulletDecal = 0;
    }

    /// <summary>
    /// calculate and create strictly along XZ plane
    /// </summary>
    public class XZ
    {

        /// <summary>
        /// create an arc of guncasts like from a shotgun
        /// </summary>
        public static void ShotgunPoints(spellHit spellHit, float arcLength, float arcOffset, GameObject lineRenderPrefab, Transform startPos, GameObject bulletDecalPrefab = null)
        {
            float radians = arcLength / spellHit.summonsCount * Mathf.Deg2Rad; //calculate even radians between each point
            float offset = 0f; //temp
            Vector3 target = Vector3.zero; //temp
            bool createBulletDecal = spellHit.target == null && bulletDecalPrefab != null; //create bullet decal if we assumingly hit something on worldmask/ terrain mask
            float tracerWidth = lineRenderPrefab.GetComponent<LineRenderer>().startWidth;

#if UNITY_EDITOR
            Debug.DrawRay(startPos.position, startPos.forward, Color.red, 3f);
#endif
            float rotOffset = Mathf.Abs(Vector3.Angle(Vector3.forward, startPos.forward));
            /*
            if (Vector3.Dot(Vector3.forward, startPos.forward) < 0) //if target pos is "above" us on this plane
                rotOffset = 360 - rotOffset; //convert from "180 endian" to "360 endian," and anything above 180 is mirrored

            //Logger.Log("right: " + Vector3.Dot(Vector3.forward, startPos.right));
            Logger.Log("rotOffset " + rotOffset + " forward " + Vector3.Dot(Vector3.forward, startPos.forward) + " right:" + Vector3.Dot(Vector3.forward, startPos.right));

            rotOffset = (rotOffset + arcOffset) * Mathf.Deg2Rad;
            */
            rotOffset = (360 - Mathf.Abs(startPos.eulerAngles.y) - arcOffset + 90) * Mathf.Deg2Rad;
            Entity hit;

            Collider[] hitColliders;
            for (int i = 0; i < spellHit.summonsCount; i++)
            {
                offset = (radians * i) + rotOffset;
                target.x = spellHit.range * Mathf.Cos(offset) + startPos.position.x;
                target.z = spellHit.range * Mathf.Sin(offset) + startPos.position.z;
                target.y = startPos.position.y; //make sure it is level
                CreateVFX(startPos.position, target, lineRenderPrefab, createBulletDecal ? bulletDecalPrefab : null, createBulletDecal);

               // Debug.DrawRay(startPos.position, target - startPos.position, Color.red, 3f);
                hitColliders = Physics.OverlapCapsule(startPos.position, target, tracerWidth * raycastWidthMultiplier, WorldFunctions.entityMask);
                if(hitColliders.Length > 0) { //if hit any entities with this beam
                    for(int x = 0; x < hitColliders.Length; x++)
                    { //loop through all hits and apply effect
                        hit = hitColliders[x].GetComponent<Entity>();
                        if (spellHit.caster != null && hit != null && (hit != spellHit.caster) && spellHit.spell.CalcValidTarget(hit, spellHit.caster) == castFailCodes.success)
                            spellHit.spell.OnAnimationHit(new spellHit(spellHit, spellHit.caster, hit, spellHit.origin, spellHit.vTarget)); //cast new OnAnimationHit
                    }
                }
            }

        } //end function ShotgunPoints


    } //end class XZ


    static void CreateVFX(Vector3 startPos, Vector3 endPos, GameObject lineRenderPrefab, GameObject bulletDecalPrefab, bool createBulletDecal)
    {
        //create bullet line effect
        LineRenderer line = GameObject.Instantiate(lineRenderPrefab, startPos, Quaternion.identity).GetComponent<LineRenderer>();

        //update line renderer
        line.positionCount = 2;
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);

        if(createBulletDecal && bulletDecalPrefab != null)
        { //if specified function argument
            GameObject bulletDecal = GameObject.Instantiate(bulletDecalPrefab, endPos, bulletDecalPrefab.transform.rotation);
            bulletDecal.transform.LookAt(startPos); //make it face right way
        }

        Ext.DestroyDelayed(line.gameObject, tracerDuration);
    } //end function CreateVFX

} //end class GunCast
