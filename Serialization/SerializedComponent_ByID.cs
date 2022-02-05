using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[UnityEngine.Scripting.Preserve] //don't strip ctor
public abstract class SerializedComponent_ByID : SyncComponent
{
    public static Dictionary<Type, List<Type>> schema = new Dictionary<Type, List<Type>>();
    public static Dictionary<Type, List<string>> names_schema = new Dictionary<Type, List<string>>();

    [HideInInspector]
    public SyncObject parent;

    [HideInInspector]
    public List<Type> types = new List<Type>() { default }; //cache
    [HideInInspector]
    public List<string> type_names = new List<string>() { default };

    public Component current;
    public string defaultType;
    public string initialConfig;
    public readonly bool isLocalOnly;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isLocalOnly">whether to allow creation of this component on entities not under local control, such as player controllers</param>
    [UnityEngine.Scripting.Preserve] //don't strip ctor
    public SerializedComponent_ByID(Type type, bool isLocalOnly = false)
    {
        this.isLocalOnly = isLocalOnly;
        Init_ID(type); //insert into the serialization schema
    }


    /// <summary>
    /// implement a switch and what to do with it to the target class
    /// </summary>
    /// <param name="configType"></param>
    [UnityEngine.Scripting.Preserve] //don't strip ctor
    public abstract void SetConfig(string configType);

    [UnityEngine.Scripting.Preserve] //don't strip ctor
    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        this.parent = parent;

        types = schema[this.GetType()]; //cache
        type_names = names_schema[this.GetType()]; //cache

        syncFields = new SyncField[] {
            new SyncInt8Field(this.GetType().Name + "_type", false, 0, parent, OnUpdate)
        };

        if (parent != null)
        {
            (syncFields[0] as SyncField<int>).value = GetTypeID(defaultType); //set starting value
        }
    }

    [UnityEngine.Scripting.Preserve] //don't strip ctor
    public void Init_ID(Type type)
    {
        if (schema.ContainsKey(this.GetType()))
            return; //if already in schema, don't do anything
        schema.Add(this.GetType(), new List<Type>());
        names_schema.Add(this.GetType(), new List<string>());

        var inheriting = AppDomain.CurrentDomain.GetAssemblies() //linq expression to get all spells
        .SelectMany(s => s.GetTypes())
        .Where((p) => {
            if (type.IsAssignableFrom(p) && !p.IsInterface && p.Name != this.GetType().Name)
            { //if inherit ISpell interface and not ISpell and not Spell, this is a spell we created
               // Logger.Log(p.Name + " inherits from " + type.Name);
                return true;
            }
            else
                return false;
        });
        schema[this.GetType()] = inheriting.Select(entry => entry).ToList();
        names_schema[this.GetType()] = inheriting.Select(entry => entry.Name).ToList();


        Logger.LogWarning(this.GetType() + " initialized to serialize " + schema[this.GetType()].Count + " different " + " types");
    }


    public void OnUpdate(ref int newID)
    {
        if(TypeExists(newID))
            TrySet(GetType_FromID(newID));
    }

    /// <summary>
    /// how and where to place component
    /// </summary>
    protected abstract Component Set(Type type);

    public void TrySet(Type type)
    {
        if (current != null)
        {
            if (type == current.GetType())
                return; //same as current

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

    public Type GetType_FromID(int id)
    {
        return types[id];
    }
    /// <summary>
    /// goes by name of class. Case sensitive
    /// </summary>
    public Type GetType_FromName(string name)
    {
        if(type_names.IndexOf(name) < 0)
        { //if not found
            Logger.LogError("unable to find " + this.GetType() + " type " + name);
        }
        return types[type_names.IndexOf(name)];
    }
    public int GetTypeID(string name)
    {
        return type_names.IndexOf(name);
    }

    public bool TypeExists(int id)
    {
        return id <= types.Count && id >= 0 && types.Count > 0;
    }
    /// <summary>
    /// goes by name of class. Case sensitive
    /// </summary>
    public bool TypeExists(string name)
    {
        return type_names.IndexOf(name) > 0; //0 is purposefully null
    }

    public Type GetDefaultType()
    {
        return GetType_FromName(defaultType);
    }
}
