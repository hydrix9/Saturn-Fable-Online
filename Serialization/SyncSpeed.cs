using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * save the speed on client side, then replay back, hopefully deterministic
 * have to save speed in Client or NetworkMovement so you can play back CSP?
*/

public class SyncSpeed : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //call base.ResetFieldsToDefaults when game restarts

    //codepoint for name of variables stored on SyncObject
    public static readonly string
        currentSpeed = "speed",
        maxSpeed = "maxSpeed",
        forwardVelocity = "velocity", //velocity, or actually inertia, which is the product of acceleration
        forwardAcceleration = "accel",
        sprintSpeed = "sprintSpeed", //how much to mult maxSpeed by
        stamina = "stamina",
        maxStamina = "maxStamina",
        staminaDrainPerTick = "staminaDrain",
        staminaRegenPerTick = "staminaRegen",
        moveBackwardsModifier = "mvBackwards",
        immobilized = "immobilized",
        stunned = "stunned"
    ;

    #region params
    public float startMaxStamina = 10;
    public float startStaminaDrainPerTick = 0.1f;
    public float startStaminaRegenPerTick = 0.3f;
    public float startSprintSpeed = 1.5f;
    public float startForwardAcceleration = 0.05f;
    public float startMaxSpeed = 1;
    #endregion

    SyncFloat16Field_1024 currentSpeedField;
    SyncFloat16Field_1024 maxSpeedField;
    SyncFloat16Field_1024 forwardVelocityField;
    SyncFloat16Field_1024 forwardAccelerationField;
    SyncFloat16Field_1024 sprintSpeedField;
    SyncFloat16Field_1024 staminaField;
    SyncFloat16Field_1024 maxStaminaField;
    SyncFloat16Field_1024 staminaDrainPerTickField;
    SyncFloat16Field_1024 staminaRegenPerTickField;

    SyncInt16Field staminaField_int;
    SyncInt16Field maxStaminaField_int;
    SyncInt16Field staminaDrainPerTickField_int;
    SyncInt16Field staminaRegenPerTickField_int;

    public bool staminaField_IsInt = false; //whether to use float or int based stamina
    public bool useEnergyInstead = false; //whether to use SyncEnergy.energy or stamina
    SyncInt16Field syncEnergyField;
    SyncInt16Field syncMaxEnergyField;

    NetworkMovement nm;
    PlayerMovement playerMovement; //doesn't necessarily exist
    Entity parent;

    public override void ServerInit(SyncObject parent)
    {
        base.ServerInit(parent);

        /* not sure why this was here to begin with
        string faction = parent.Get<string>(SyncFaction.faction);
        if (faction != "")
        {
            GameMode.instance.TryAddFaction(faction); //make sure we have a reference...
        }
        */
    }

    private void Start()
    {
        if(parent != null)
        {
            syncEnergyField = parent.GetSyncField<int>(SyncEnergy.energy) as SyncInt16Field;
            syncMaxEnergyField = parent.GetSyncField<int>(SyncEnergy.maxEnergy) as SyncInt16Field;
        } else
        {
        }
    } //end func Start


    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        currentSpeedField = new SyncFloat16Field_1024(currentSpeed, false, 0, parent);
        maxSpeedField = new SyncFloat16Field_1024(maxSpeed, false, startMaxSpeed, parent);
        forwardVelocityField = new SyncFloat16Field_1024(forwardVelocity, false, 0, parent);
        forwardAccelerationField = new SyncFloat16Field_1024(forwardAcceleration, false, startForwardAcceleration, parent);
        sprintSpeedField = new SyncFloat16Field_1024(sprintSpeed, false, startSprintSpeed, parent);
        
        //if using energy then it must be an int
        if (staminaField_IsInt || useEnergyInstead)
        {
            staminaField_int = new SyncInt16Field(stamina, false, (int)startMaxStamina, parent);
            maxStaminaField_int = new SyncInt16Field(maxStamina, false, (int)startMaxStamina, parent);
            staminaDrainPerTickField_int = new SyncInt16Field(staminaDrainPerTick, false, (int)startStaminaDrainPerTick, parent);
            staminaRegenPerTickField_int = new SyncInt16Field(staminaRegenPerTick, false, (int)startStaminaRegenPerTick, parent);
        }
        else
        {
            staminaField = new SyncFloat16Field_1024(stamina, false, startMaxStamina, parent);
            maxStaminaField = new SyncFloat16Field_1024(maxStamina, false, startMaxStamina, parent);
            staminaDrainPerTickField = new SyncFloat16Field_1024(staminaDrainPerTick, false, startStaminaDrainPerTick, parent);
            staminaRegenPerTickField = new SyncFloat16Field_1024(staminaRegenPerTick, false, startStaminaRegenPerTick, parent);
        }

        syncFields = new SyncField[] {
            currentSpeedField,
            maxSpeedField,
            forwardVelocityField,
            forwardAccelerationField,
            sprintSpeedField,
            staminaField_IsInt ? (SyncField)staminaField_int : (SyncField)staminaField,
            staminaField_IsInt ? (SyncField)maxStaminaField_int : (SyncField)maxStaminaField,
            staminaField_IsInt ? (SyncField)staminaDrainPerTickField_int : (SyncField)staminaDrainPerTickField,
            staminaField_IsInt ? (SyncField)staminaRegenPerTickField_int : (SyncField)staminaRegenPerTickField,
            new SyncFloat16Field_1024(moveBackwardsModifier, false, 1, parent),
            new SyncBoolField(immobilized, false, false, parent),
            new SyncBoolField(stunned, false, false, parent),

        };

        if(parent != null)
        {
            nm = parent.GetComponent<NetworkMovement>();
            this.parent = parent.e;
        }

    } //end func SetDefaults

    [HideInInspector]
    public int lastSNumUpdate; //prevent double updating stamina between receiving inputs and manually updating

    //wrapper to call all relevant Server updates
    public void CalcUpdates_Server()
    {
        if (lastSNumUpdate == Server.instance.snum)
            return; //already updated, likely due to receiving inputs
        lastSNumUpdate = Server.instance.snum;

        CalcStaminaUpdates_Server();
        ApplyVelocity_Server();
    } //end func CalcUpdates_Server

    //move stamina in the direction of either 0 or maxStamina, based on if isSprinting
    public void CalcStaminaUpdates_Server()
    {

        if (useEnergyInstead)
        { //have to perform this null check due to not being init on first frame, since it is init during Start
            if (syncEnergyField != null)
            {
                if (nm.sprint)
                    syncEnergyField.value = (int)Mathf.MoveTowards(syncEnergyField.value, 0, staminaDrainPerTickField_int.value);
                else
                    syncEnergyField.value = (int)Mathf.MoveTowards(syncEnergyField.value, syncMaxEnergyField.value, staminaRegenPerTickField_int.value);
            }
        }
        else
        {
            if (nm.sprint)
                staminaField.value = Mathf.MoveTowards(staminaField.value, 0, staminaDrainPerTickField.value);
            else
                staminaField.value = Mathf.MoveTowards(staminaField.value, maxStaminaField.value, staminaRegenPerTickField.value);
        }
    } //end func UpdateStamina_Server

    float forwardVelocityTemp;
    float finalSpeedTemp;
    //calc and apply velocity/inertia and final speed
    public void ApplyVelocity_Server()
    {
        CalcVelocity(forwardVelocityField.value, out forwardVelocityTemp, out finalSpeedTemp, (nm.forward || nm.backward || nm.left || nm.right), nm.sprint);
        currentSpeedField.value = finalSpeedTemp;
        forwardVelocityField.value = forwardVelocityTemp;

    } //end func ApplyVelocity_Server

    /*
    public float testForwardAcceleration = 1f;
    public float testSprintSpeed = 1f;
    public float testMaxSpeed = 1;
    */

    //calculate forward velocity/inertia based on sprinting and acceleration
    public void CalcVelocity(float startVelocity, out float forwardVelocity, out float finalSpeed, bool isGainingVelocity, bool isSprinting)
    {

        //move velocity toward acceleration or decelleration
        //move speed as a product of velocity and ?.sprinting

        //TODO- provide a means to overload isMovingFoward check via respective PlayerMovement or AI movement script
        //can even use isDecelerating to move toward the opposite direction twice as fast


        //please rewrite me
        forwardVelocity = Mathf.MoveTowards(startVelocity, (isGainingVelocity ? ((isSprinting && syncEnergyField.value - (staminaField_IsInt ? staminaDrainPerTickField_int.value : staminaDrainPerTickField.value) > 0) ? sprintSpeedField.value * maxSpeedField.value : maxSpeedField.value) : 0), isSprinting ? forwardAccelerationField.value * sprintSpeedField.value : forwardAccelerationField.value);

        if (useEnergyInstead)
        {
            finalSpeed = forwardVelocity;
            //finalSpeed = ((nm.sprint && syncEnergyField.value - (staminaDrainPerTickField_int.value) > 0) ? sprintSpeedField.value : 1); ///this version won't use velocity, just sprint or normal
        }
        else
            finalSpeed = forwardVelocity;

    } //end func GetVelocity

} //end class SyncSpeed
