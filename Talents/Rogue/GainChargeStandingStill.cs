using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainChargeStandingStill : MonoBehaviour
{
    NetworkMovement3D nm;
    SyncFloat16Field_8192 syncChargeField;
    Entity target;

    float modifier;
    float minStillnessTime;
    public void Set(float modifier, float interval, float minStillnessTime)
    {
        this.minStillnessTime = minStillnessTime;
        this.modifier = modifier;
        this.interval = interval;
    }

    private void Start()
    {
        nm = GetComponent<NetworkMovement3D>();
        target = GetComponent<Entity>();
        syncChargeField = (SyncFloat16Field_8192)target.GetSyncField<float>(SyncCharge.charge);
    }

    float currentStillnessTime = 0;
    float interval;
    float timer = 0;
    private void Update()
    {
        currentStillnessTime += Time.deltaTime; //count up time standing stil

        if (nm.forward || nm.backward || nm.left || nm.right) //if they moved using any input
            currentStillnessTime = 0; //reset stillness time

        if(timer > 0)
            timer -= Time.deltaTime;
        if(timer <= 0 && currentStillnessTime > minStillnessTime)
        {
            timer = interval + timer; //reset, but add overflow duration to be fair
            AddModifier();
        }
        
    }

    //actually add the target charge
    void AddModifier()
    {
        syncChargeField.value += modifier;
    }
} //end class GainChargeStandingStill