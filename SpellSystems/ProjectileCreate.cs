using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCreate
{

    public class TwoD {
        public class XZ
        {



            /// <summary>
            /// create projectiles starting radius away with a count, and even spacing 
            /// </summary>
            public static void FromCirclePoints(spellHit spellHit, GameObject prefab, Func<spellHit, Vector3> calcCenter, float startRadius, SpellStaticProperties.HomingType homingType, SpellStaticProperties.RotationType rotationType, float startOffset = 0, float turnSpeed = 3f)
            {
                float radians = 360 / spellHit.summonsCount * 3.14F / 180; //calculate even radians between each point
                float offset = 0f; //temp
                GameObject target; //temp
                for (int i = 0; i < spellHit.summonsCount; i++)
                {
                    offset = radians * i + startOffset;
                    target = ObjectPool.GetInstance(prefab, calcCenter != null ? calcCenter.Invoke(spellHit) : spellHit.caster.transform.position + new Vector3(startRadius * Mathf.Cos(offset), 0, startRadius * Mathf.Sin(offset)), prefab.transform.rotation);
                    SetProjectileParams(i, target, spellHit, calcCenter != null ? calcCenter.Invoke(spellHit) : spellHit.caster.transform.position, homingType, rotationType, true, turnSpeed);
                }
            }


            /// <summary>
            /// create projectiles starting radius away with a count, and even spacing 
            /// </summary>
            public static void FromArc(spellHit spellHit, GameObject prefab, float arcLength, float arcOffset, Vector3 center, float startRadius, SpellStaticProperties.HomingType homingType, SpellStaticProperties.RotationType rotationType, float turnSpeed = 3f, Action<spellHit, GameObject> callback = null)
            {
                float radians = arcLength / spellHit.summonsCount * Mathf.Deg2Rad; //calculate even radians between each point
                float offset = 0f; //temp
                float rotOffset = (360 - Mathf.Abs(spellHit.caster.transform.eulerAngles.y) - arcOffset + 90) * Mathf.Deg2Rad;
                GameObject target; //temp
                for (int i = 0; i < spellHit.summonsCount; i++)
                {
                    offset = (radians * i) + rotOffset;
                    
                    target = ObjectPool.GetInstance(prefab, center + new Vector3(startRadius * Mathf.Cos(offset), 0, startRadius * Mathf.Sin(offset)), prefab.transform.rotation);
                    SetProjectileParams(i, target, spellHit, center, homingType, rotationType, true, turnSpeed);
                    callback?.Invoke(spellHit, target);
                }
            }

            /// <summary>
            /// create projectiles starting radius away with a count, and even spacing 
            /// </summary>
            public static void Single(spellHit spellHit, GameObject prefab, Vector3 startPos, SpellStaticProperties.HomingType homingType, SpellStaticProperties.RotationType rotationType, float turnSpeed = 3f, bool destroyOnhit = true, Action<spellHit, GameObject> callback = null)
            {
                GameObject target; //temp
                target = ObjectPool.GetInstance(prefab, startPos, spellHit.caster.transform.rotation);
                SetProjectileParams(0, target, spellHit, startPos, homingType, rotationType, true, turnSpeed, destroyOnhit);
                callback?.Invoke(spellHit, target);

            }
        } //end class XZ

        public class XY
        {

        } //end class XY

    } //end class TwoD

    public class ThreeD
    {

    } //end class ThreeD

    private static void SetProjectileParams(int index, GameObject target, spellHit spellHit, Vector3 center, SpellStaticProperties.HomingType homingType, SpellStaticProperties.RotationType rotationType, bool lockXZ, float turnSpeed, bool destroyOnhit = true)
    {

        //set refs and write this object to spellhit's references to created OnAnimationHit objects (on caster, so they remember the total length of fields)
        spellHit.caster.syncCasting.syncUnsentCasts.SetObjectSerializationID(index, spellHit,
            Projectile.AddTo(target, spellHit, homingType, lockXZ, turnSpeed, destroyOnhit)
            );
        SetRotationType(spellHit, target, center, rotationType);

    }

    static void SetRotationType(spellHit spellHit, GameObject target, Vector3 center, SpellStaticProperties.RotationType rotationType)
    { //rotate object based on SpellStaticProperties.RotationType
        Vector3 finalRot = target.transform.rotation.eulerAngles; //used to restore what rotations were set on the prefab

        switch (rotationType)
        {
            case SpellStaticProperties.RotationType.facingAwayOrigin:
#if UNITY_EDITOR
                Debug.DrawRay(target.transform.position, center - target.transform.position, Color.yellow, 10f);
                Debug.DrawRay(center, Vector3.up, Color.green, 10f);
                Debug.DrawRay(target.transform.position, Vector3.up, Color.red, 10f);
#endif
                if(target.transform.position != center) //add this line otherwise will print the error "look rotation viewing vector is zero" if they are ==
                    target.transform.forward = target.transform.position - center;
                break;
            case SpellStaticProperties.RotationType.facingTowardOrigin:
                target.transform.forward = center - target.transform.position;
                break;
            case SpellStaticProperties.RotationType.facingSpellTarget:
                if (spellHit.target != null)
                    target.transform.forward = spellHit.target.transform.position - center;
                else
                    Logger.LogWarning("didn't find target on spellHit");
                break;
            case SpellStaticProperties.RotationType.randomY:
                target.transform.forward = new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
                break;
            default:
                break;
        }

        
        if (finalRot.x == 0)
            finalRot.x = target.transform.eulerAngles.x;
        if (finalRot.y == 0)
            finalRot.y = target.transform.eulerAngles.y;
        if (finalRot.z == 0)
            finalRot.z = target.transform.eulerAngles.z;

        target.transform.rotation = Quaternion.Euler(finalRot); //restore what axises were on the prefab
        
    }

} //end class Projectiles
