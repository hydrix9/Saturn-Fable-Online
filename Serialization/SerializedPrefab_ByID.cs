using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class SerializedPrefab_ByID : SyncComponent
{
    public static Dictionary<Type, GameObject[]> schema = new Dictionary<Type, GameObject[]>(); //top level schema for all SerializedPrefab_ByID
    public static Dictionary<Type, string[]> names_schema = new Dictionary<Type, string[]>();

    [HideInInspector]
    public SyncObject parent;

    [HideInInspector]
    public GameObject[] objects = new GameObject[0]; //cache
    [HideInInspector]
    public string[] object_names = new string[0];

    public GameObject current;
    public string defaultType;
    public string initialConfig;
    [HideInInspector]
    public bool isLocalOnly;

    public string targetTypeName; //key used for SyncField

    /*
     * OKAY. This is how Addressables loading works: Create a group folder for organizational purposes
     * in the addressables window. Choose a name, here referred to as "addressablesNameLiteral", 
     * and rename each addressable GameObject in the addressables window in the group folder, so that everything you want loaded is a duplicate name,
     * the one you chose, "addressablesNameLiteral"
     * To differentiate between these, you use labels, or in our case, look at the name of the actual GameObject.
     * This class is meant to handle the schema automatically by assigning a dictionary of name and ID for serialization, and also handle the Addressables loading automatically
     * 
     * the Group folder doesn't allow you to do anything...its just organizational. Completely stupid, as the only way to collect assets by a group
     * is therefore to name them as duplicate entries then differentiate them by an additional label... or look at them individually manually
     * you literally create a group folder then group them through reference by setting each entry to a duplicate name.... there is no documentation of this...
     */
    [HideInInspector]
    public string addressablesNameLiteral; //set in Constructor method

    SyncInt8Field typeField;

    /// <summary>
    /// bootleg class constructor because this is actually forced to be implemented and MonoBehaviour hates actual class constructors...
    /// </summary>
    /// <param name="addressablesNameLiteral">see this class file for explaination on addressables</param>
    public abstract void Constructor(out string addressablesNameLiteral, out bool isLocalOnly);

    void TryInit(Action callback = null)
    { //try init if not done so aleady on our class type
        targetTypeName = GetType().Name + "_type";
        Constructor(out addressablesNameLiteral, out isLocalOnly); //get default values from implementing classes

        if (schema.ContainsKey(this.GetType()))
            return;


        Addressables.LoadAssetsAsync<GameObject>(addressablesNameLiteral, null).Completed += (results)=> {
            Init_ID(results.Result);
            callback?.Invoke();
        }; //add each to a list...
             //try to init with the list we gathered

    } //end func TryInit

    /// <summary>
    /// implement a switch and what to do with it to the target class
    /// </summary>
    /// <param name="configType"></param>
    public abstract void SetConfig(string configType);


    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        TryInit(SetInitialValue); //try put ourself in the schema, try to set initial value after this async function finishes

        if (!schema.ContainsKey(this.GetType()))
        { //if still haven't loaded our Addressables....

        }
        else
        {
            objects = schema[this.GetType()]; //cache
            object_names = names_schema[this.GetType()]; //cache
        }

        this.parent = parent;

        typeField = new SyncInt8Field(targetTypeName, false, 0, parent, OnUpdate);
        
        syncFields = new SyncField[] {
            typeField
        };

        SetInitialValue();


    } //end func SetDefaults

    //popualte sync
    void SetInitialValue()
    {
        if (parent != null && typeField != null)
        {

            typeField.value = GetTypeID(defaultType); //set starting value
            if (TypeExists(defaultType))
                TrySet(GetType_FromName(defaultType));
        }
    }

    public void Init_ID(IList<GameObject> possible)
    {
        if(possible == null)
        {
            Debug.LogWarning("got no results for type of " + this.GetType() + " with addressables label/path " + addressablesNameLiteral);
            return;
        }
        if (schema.ContainsKey(this.GetType()))
        {
            return; //if already in schema, don't do anything
        }
        schema.Add(this.GetType(), possible.ToArray());
        names_schema.Add(this.GetType(), possible.Select(entry => entry.name).ToArray());

        Logger.LogWarning(this.GetType() + " initialized to serialize " + schema[this.GetType()].Length + " different " + " types");
    }


    public void OnUpdate(ref int newID)
    {
        if(TypeExists(newID)) //if id is valid
            TrySet(GetType_FromID(newID), false);
    }

    /// <summary>
    /// how and where to place component
    /// </summary>
    protected abstract GameObject Set(GameObject type);

    GameObject lastSetPrefab;
    //notify- whether this will sync to clients (would only be false if calling from OnUpdate)
    public void TrySet(GameObject type, bool notify = true)
    {

        if (Server.isServer && notify) //if is server and calling this directly, not from OnUpdate
            typeField.value = GetTypeID(type.name); //serialize to clients

        if (current != null)
        {
            if (lastSetPrefab != null && type == lastSetPrefab)
                return; //same as current
            lastSetPrefab = type;

            Destroy(current);
        }
        if (CanSet())
        {
            current = Set(type);
        }
    }



    /// <summary>
    /// whether allowed to set this component, because of being a local side component only or otherwise
    /// </summary>
    /// <returns></returns>
    bool CanSet()
    {
        return !isLocalOnly || (parent.e != null && parent.e.p != null && parent.e.p.isLocal); //make it so we can only set if it either isn't local only or this is a local entity
    }

    public GameObject GetType_FromID(int id)
    {
        return objects[id];
    }
    /// <summary>
    /// goes by name of class. Case sensitive
    /// </summary>
    public GameObject GetType_FromName(string name)
    {
        if (Array.IndexOf(object_names, name) < 0)
        { //if not found
            Logger.LogError("unable to find " + this.GetType() + " type " + name);
        }
        return objects[Array.IndexOf(object_names, name)];
    }
    public int GetTypeID(string name)
    {
        return Array.IndexOf(object_names, name);
    }

    public bool TypeExists(int id)
    {
        return id <= objects.Length && id >= 0 && objects.Length > 0;
    }
    /// <summary>
    /// goes by name of class. Case sensitive
    /// </summary>
    public bool TypeExists(string name)
    {
        return Array.IndexOf(object_names, name) > 0; //0 is purposefully null
    }

    public GameObject GetDefaultType()
    {
        return GetType_FromName(defaultType);
    }
}
