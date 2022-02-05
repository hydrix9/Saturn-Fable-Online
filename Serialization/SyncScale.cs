using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncScale : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts

    public static readonly string scale = "scale";

    //true will scale the owner in addition to the target, but will do this automaticall anyway if the target is a child of the owner
    //this is done to ensure that colliders present on the Entity scale in addition to the target
    public bool scaleOwner = true; 
    public Transform target;
    public SyncObject _parent;
    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = new SyncField[]
        {
            new SyncVector3Field(scale, false, Vector3.one, parent, OnUpdateScale)
        };

        if (target == null && parent != null && parent.e != null && parent.e.body != null)
            target = parent.e.body;
        if (parent != null)
        {

            parent.Set<Vector3>(SyncScale.scale, parent.transform.lossyScale); //set the initial scale to current value

            this._parent = parent;
        }
    } //end SetDefaults

    void OnUpdateScale(ref Vector3 newValue)
    {
        if (target != null)
        {
            //if our body is a child of the actual SyncObject, scale the parent instead
            //otherwise, just scale the body as normal
            if (_parent != null && (scaleOwner || target.parent == _parent.transform))
            {
                SetLossyScale(newValue, _parent.transform);
                //if isn't child of this parent, also scale the body
                if (target.parent == null || target.parent != _parent.transform)
                    SetLossyScale(newValue, target);
            }
            else
                SetLossyScale(newValue, target);

        } //end if target != null


    } //end OnUpdateScale

    //setting lossy scale isn't normally supported in Unity, likely due to the necessity of a parent scale calculation, but without this, we can't sync deterministically between client and server
    void SetLossyScale(Vector3 newValue, Transform target)
    {
        if (target == null)
            return;


        float gameScale = GameScaleConfig.instance.gameScale; //cache, global scale multiplier

        if (target.parent != null)
        {
            Vector3 parentLossyScale = target.parent.lossyScale; //cache
            target.localScale = new Vector3(newValue.x / parentLossyScale.x * gameScale, newValue.y / parentLossyScale.y * gameScale, newValue.z / parentLossyScale.z * gameScale);
        }
        else
            target.localScale = newValue * gameScale; //no transform parent, so don't need to do any weird calculations

    } //end func SetLossyScale

} //end class SyncScale