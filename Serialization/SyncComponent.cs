using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// abstract interfacing class that allows you to create arbtrary serializable attributes with multiple SyncValues, and adds these to the global dirty mask schema
/// </summary>
public abstract class SyncComponent : MonoBehaviour, ISync
{
    public static Dictionary<Type, int> totalFieldsLengths = new Dictionary<Type, int>();

    /// <summary>
    /// whether to call ResetFieldsToDefault during Server.onStartGame
    /// </summary>
    public abstract bool resetToDefaultValues_OnGameRestart { get; }

    public virtual void Compile()
    {
        if (totalFieldsLengths.ContainsKey(this.GetType()))
            return; //already added this Component type to schema

        int totalFieldLength = 0;

        SetDefaults(null, null, out syncFields); //instantiate all syncFields

        for (int i = 0; i < syncFields.Length; i++) {
            syncFields[i].Compile();
            totalFieldLength += syncFields[i].GetWorstTotalFieldLength(); //add field length in bytes to total
        }

        syncFields = null; //technically shouldn't need this floating around
        totalFieldsLengths.Add(this.GetType(), totalFieldLength);
    }

    protected SyncField[] syncFields = new SyncField[0]; //fields serialized and deserialized

    /// <summary>
    /// creates instances of all fields so they can be called with SyncObject.Get
    /// </summary>
    public void Init(SyncObject parent, NetworkIO context)
    {
        SetDefaults(parent, context, out syncFields);

        parent.onClean += OnClean; //clean own refs when destroyed
        
        if(resetToDefaultValues_OnGameRestart)
            Server.instance.onSetupGame += ResetFieldsToDefaults;
    } //end function Init

    /// <summary>
    /// called on SyncObject creation before Awake
    /// </summary>
    public virtual void ServerInit(SyncObject parent)
    {

        //setup references on parent SyncObject
        int oldLength = parent.syncFields.Length;
        Array.Resize(ref parent.syncFields, oldLength + syncFields.Length);
        for (int i = 0; i < syncFields.Length; i++) {
            parent.syncFields[oldLength + i] = syncFields[i]; //give reference
        }
    }

    public abstract void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields);


    /// <summary>
    /// remove all references to avoid memory leaks
    /// </summary>
    public virtual void OnClean(SyncObject owner)
    {
        for (int i = 0; i < syncFields.Length; i++)
            syncFields[i].Destroy();

        Server.instance.onSetupGame -= ResetFieldsToDefaults; //gc
        syncFields = null;
    }

    /// <summary>
    /// somewhat dangerous, preferred to just reset the fields individually
    /// </summary>
    protected void ResetFieldsToDefaults()
    {
        if(syncFields != null && syncFields.Length > 0)
            for (int i = 0; i < syncFields.Length; i++)
            {
                if(syncFields[i] != null)
                    syncFields[i].ResetToDefaultValue(); //set to defaultValue which should also trigger updates
            }
    }

} //end class SyncComponent