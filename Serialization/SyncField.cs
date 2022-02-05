using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// interface contains the Compile function, meaning it is part of the dirty mask creation scheme 
/// </summary>
public interface ISync
{
    void Compile();
} //end interface ISync


public abstract class SyncField : ISync {

    public bool isStatic; //whether only needs to be synced once

    //keep track of dirty mask stuff here since we scan all the SyncComponents and initialize them, and their fields (these SyncFields) are initialized before their constructor
    public static Dictionary<string, int> dirtyMaskSchemaByteNums = new Dictionary<string, int>(); //stores the dirty mask schema, specifically what byte fields are synced to
    public static Dictionary<string, int> dirtyMaskSchemaBitwiseNums = new Dictionary<string, int>(); //stores the bitwise operation for each field type, like write 00000001 or 00000010
    public static int dirtyMaskLength; //how many bytes long the dirty mask is
    /// <summary>
    /// math convenience array that allows you to easier perform a bit check on all bits in a byte 
    /// </summary>
    public static readonly int[] dirtymaskbits = { 1, 2, 4, 8, 16, 32, 64, 128 };
    public static SyncField[] syncFields = new SyncField[0]; //all syncfields that exist
    public static int totalNumFieldsExist = SyncObject.baseNumFields; //cache of how many exist after base num fields on SyncObject. Init to however much we have as base

    public readonly int dirtyMaskBitwiseNum; //num we write to the dirty mask in a bitwise operator when we're dirty to signal we're dirty
    public readonly int dirtyMaskByteNum; //index of the byte in the dirty mask we write to

    public string label;
    public SyncObject parent; //need to keep track of this for custom Syncing extensions that use this field in .Write

    static Dictionary<char, string> accountFieldChars = new Dictionary<char, string>() { }; //maintains int ID to string syncfield name for auto-synching account data into entity
    public char accountFieldCode = default;
    public bool isAccountField => accountFieldCode != default;
    protected bool justSentAccountUpdate; //flag to true to indicate we just built to the AccountServer, thus are still dirty to clients... to remove the false negative

    /// <summary>
    /// write itself into the static variables so we can account for this object in the dirty mask schema
    /// </summary>
    public virtual void Compile()
    {
        if (dirtyMaskSchemaByteNums.ContainsKey(label))
            return; //already added this SyncField type

        Array.Resize(ref syncFields, syncFields.Length + 1); //resize array. It NEEDS to be an array since this array is HEAVILY accessed and initialized only on startup
        syncFields[syncFields.Length - 1] = this;

        totalNumFieldsExist = syncFields.Length + SyncObject.baseNumFields;  //also include SyncObject.baseNumFields when finding which byte to write to

        dirtyMaskSchemaByteNums.Add(
        label,
        UnityEngine.Mathf.FloorToInt(totalNumFieldsExist / 8)
        ); //find the index of the byte we write to

        dirtyMaskSchemaBitwiseNums.Add(label,
        dirtymaskbits[totalNumFieldsExist % 8]); //what number we write into the dirty mask

        dirtyMaskLength = UnityEngine.Mathf.CeilToInt(totalNumFieldsExist / 8f); //recalculate length of dirty mask as we're adding more SyncComponent types
        SyncObject.dirtyMaskEnd = dirtyMaskLength + SyncObject.dirtyMaskStartPos; //cache

    }

    protected SyncField(string label, bool isStatic, SyncObject parent = null, char accountFieldCode = default)
    {
        if (parent != null)
        {
            this.parent = parent;
            parent.AddField(label, this); //allow it to be accessed by string name
        }

        this.label = label;
        this.isStatic = isStatic;

        dirtyMaskSchemaByteNums.TryGetValue(label, out dirtyMaskByteNum);
        dirtyMaskSchemaBitwiseNums.TryGetValue(label, out dirtyMaskBitwiseNum);

        this.accountFieldCode = accountFieldCode;
        if (accountFieldCode != default && !accountFieldChars.ContainsKey(accountFieldCode))
            accountFieldChars.Add(accountFieldCode, label); //add reference
    }

    /// <summary>
    /// get length of field in bytes
    /// </summary>
    public abstract int GetCurrentTotalFieldLength();

    /// <summary>
    /// get worst case scenario field length for allocating array size at the beginnning
    /// </summary>
    /// <returns></returns>
    public abstract int GetWorstTotalFieldLength();

    /// <summary>
    /// overrideing this function to return false will allow the readPos to be changed in manual control instead of within .TryWrite
    /// </summary>
    /// <returns></returns>
    public virtual bool MoveReadPosAfterWrite() {
        return true;
    }
    
    /// <summary>
    /// will check NeedsBuild, and if it does, will call BuildTo
    /// </summary>
    public void TryBuildTo(ref int lastWritePos, ref byte[] target)
    {
        if (NeedsBuild() || justSentAccountUpdate)
        { //if haven't built to anything or just built to AccountServer packet... which would mean still techincally dirty
            justSentAccountUpdate = false; //reset flag
            //Logger.Log(label + " needs build");
            BuildTo(ref lastWritePos, ref target);
            Build_AddDirty(ref target); //write to dirty mask
            //Logger.Log("new write pos " + lastWritePos);
        }

    }

    void Build_AddDirty(ref byte[] target)
    {
        target[dirtyMaskByteNum + SyncObject.dirtyMaskStartPos] = (byte)(target[dirtyMaskByteNum + SyncObject.dirtyMaskStartPos] | dirtyMaskBitwiseNum); //write bit to dirty mask using addition
    }

    /// <summary>
    /// implementation of whether this field needs to be synchronized
    /// </summary>
    /// <returns></returns>
    public abstract bool NeedsBuild();

    /// <summary>
    /// serialize data to a byte stream. Check manually if the data needs to be serialized first
    /// </summary>
    public abstract void BuildTo(ref int lastWritePos, ref byte[] target);

    /// <summary>
    /// create a new array that will fit data and populate it with BuildTo... used for AccountClient serialization since we need to make a copy of this instance's data, so we can send it in batch with other data
    /// </summary>
    public byte[] BuildToEmpty()
    {
        byte[] returns = new byte[GetWorstTotalFieldLength()];
        int lastWritePos = 0;
        BuildTo(ref lastWritePos, ref returns);
        return returns;
    } //end func BuildToEmpty


    public void TryWrite(ref byte[] data, ref int dirtyMaskPos, ref int readPos, ref SyncObject target, Client context)
    {
        //Logger.Log("check " + label + " byte num " + dirtyMaskByteNum + " bit & " + dirtyMaskBitwiseNum + " flag is: " + data[dirtyMaskPos + dirtyMaskByteNum]);
        if((data[dirtyMaskPos + dirtyMaskByteNum] & dirtyMaskBitwiseNum) == dirtyMaskBitwiseNum)
        { //if there is data written for us, as specified by the dirty mask
            //Logger.Log("check passed");
            if(target != null)
                Write(ref data, ref readPos, ref target, context); //set the data
            if (MoveReadPosAfterWrite())
            {
               // Logger.Log("adding " + GetWorstTotalFieldLength() + " started at " + data[readPos] + " now we're at " + data[readPos + GetWorstTotalFieldLength()]);
                readPos += GetWorstTotalFieldLength();
            }
        } else
        {
            //Logger.Log("check failed");
        }
    }
    public abstract void Write(ref byte[] data, ref int readPos, ref SyncObject target, Client context);

    public virtual void CalcDefault()
    {
        //SyncField<T> will recalculate based on mods to base value from items/talents that were called using SyncField<T>.AddMod
    }

    /// <summary>
    /// subscribe to updates on all SyncFields of type T of name label. Generally bad design to use this unless you are doing something clientside like GUI
    /// </summary>
    public static void Listen<T>(string label, Action<T, SyncObject> onUpdate)
    {
        if (!SyncField<T>.onUpdateGlobalDict.ContainsKey(label)) //if haven't added empty delegate... do it before setting value
            SyncField<T>.onUpdateGlobalDict.Add(label, new SyncField<T>.OnUpdateGlobal(onUpdate)); //create new empty delegate in dictionary
        else
            SyncField<T>.onUpdateGlobalDict[label] += new SyncField<T>.OnUpdateGlobal(onUpdate); //add to delegate call (after casting)

        //Logger.LogWarning(SyncField<int>.onUpdateGlobalDict);
    }
    public static void UnListen<T>(string label, Action<T, SyncObject> onUpdate)
    {
        SyncField<T>.OnUpdateGlobal casted = new SyncField<T>.OnUpdateGlobal(onUpdate);
        SyncField<T>.onUpdateGlobalDict[label] -= casted;
    }

    public abstract void ResetToDefaultValue();

    /// <summary>
    /// clean refs
    /// </summary>
    public virtual void Destroy()
    {
        parent = null;
    }
}


#pragma warning disable CS0661 // 'SyncField<T>' defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS0660 // 'SyncField<T>' defines operator == or operator != but does not override Object.Equals(object o)
public abstract class SyncField<T> : SyncField, IEquatable<T>
#pragma warning restore CS0660 // 'SyncField<T>' defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // 'SyncField<T>' defines operator == or operator != but does not override Object.GetHashCode()
{
    public struct Mod_FromSpell
    {
        public Func<T, SyncObject, T> calc;
        public spellHit spellHit;
        public DateTime expiresOn;

        public Mod_FromSpell(spellHit spellHit, Func<T, SyncObject, T> calc, DateTime expiresOn)
        {
            this.calc = calc;
            this.spellHit = spellHit;
            this.expiresOn = expiresOn;
        }
    }

    public struct Mod_FromItem
    {
        public Func<T, SyncObject, int, T> calc;
        public Item item;
        public int amount;

        public Mod_FromItem(Item item, Func<T, SyncObject, int, T> calc, int amount)
        {
            this.calc = calc;
            this.item = item;
            this.amount = amount;
        }
    }


    protected T _value;
    public virtual T value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            if (!Server.isServer || NeedsBuild())
            { //if something changed
                onUpdate?.Invoke(ref _value); //also trigger update delegate
                if (onUpdateGlobalDict.ContainsKey(label))
                    onUpdateGlobalDict[label].Invoke(value, parent); //also trigger global update
            }
        }
    }
    public T oldValue;

    private T defaultValue; //set on startup so we can restore to default

    public delegate void OnUpdate(ref T value);
    public event OnUpdate onUpdate;

    public delegate void OnUpdateGlobal(T newValue, SyncObject parent);
    public static Dictionary<string, OnUpdateGlobal> onUpdateGlobalDict = new Dictionary<string, OnUpdateGlobal>(); //lets you subscribe to events of type T with string label

    /*
    public delegate T OnCalcDefault(T currentValue, SyncObject parent); //delegate which allows you to change the default value calculation until unsubscribed, which is more secure than performing "inverse" calculations to reverse effects
    public event OnCalcDefault onCalcDefault;

    Dictionary<Spell, OnCalcDefault> onCalcDefaultMap = new Dictionary<Spell, OnCalcDefault>(); //spells that modify the base value
    */
    public List<Mod_FromSpell> spellModifiers = new List<Mod_FromSpell>();
    public List<Mod_FromItem> itemModifiers = new List<Mod_FromItem>();

    List<Func<T, SyncObject, T>> permanentMods = new List<Func<T, SyncObject, T>>();

    public SyncField(string label, bool isStatic, T defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, parent, accountFieldCode)
    {
        oldValue = defaultValue; //suppress updates momentarily
        value = defaultValue;
        //this.value = defaultValue;
        this.defaultValue = defaultValue;

        if (onUpdate != null)
            this.onUpdate += onUpdate;

        if (parent != null)
        {
            this.onUpdate?.Invoke(ref _value); //initial update
            if(onUpdateGlobalDict.ContainsKey(label))
                onUpdateGlobalDict[label]?.Invoke(_value, parent); //initial update
        }

        if(accountFieldCode != default && parent != null && parent.e.isPlayer)
        { //if this field should be synced to account
            this.onUpdate += AddDirtyAccountField_OnUpdate; //make onUpdate send this data to AccountServer
        }
        
    } //end constructor
    public bool Equals(T other)
    {
        return EqualityComparer<T>.Default.Equals(value, other);
    }
    public static bool operator ==(SyncField<T> field, T other)
    {
        return EqualityComparer<T>.Default.Equals(field.value, other);
    }
    public static bool operator !=(SyncField<T> field, T other)
    {
        return !EqualityComparer<T>.Default.Equals(field.value, other);
    }
    public static implicit operator T(SyncField<T> field)
    {
        return field.value;
    }

    /// <summary>
    /// add a mod with a spellHit as a reference so we can find and remove it later or automatically if it expired
    /// </summary>
    public void AddMod(spellHit spellHit, Func<T, SyncObject, T> calcEffect, DateTime expiresOn, ref bool ensureUnique)
    {
        //remove all old instances if it must be unique to spell
        if (ensureUnique)
            RemoveMod(spellHit.spell);

        spellModifiers.Add(new Mod_FromSpell(spellHit, calcEffect, expiresOn)); //add to list of calculations
        CalcDefault(); //fairly recalculate value
    }

    /// <summary>
    /// add a mod with an Item as a reference so we can find and remove it later
    /// </summary>
    public void AddMod(Item item, Func<T, SyncObject, int, T> calcEffect, int amount)
    {
        RemoveMod(item, amount); //currently always remove old...

        itemModifiers.Add(new Mod_FromItem(item, calcEffect, amount)); //add to list of calculations

        CalcDefault(); //fairly recalculate value
    }

    /// <summary>
    /// add a modifier that changes this base value with no hope of ever removing it.
    /// should only be used for direct and explicit scripting, like calculating health from level.
    /// caution- possible to accidentally add the same value multiple times
    /// </summary>
    public void AddMod_Permanent(Func<T, SyncObject, T> calcEffect)
    {
        permanentMods.Add(calcEffect); //add to list of calculations

        CalcDefault(); //fairly recalculate value
    }


    public void RemoveMod(Item item, int amount)
    {
        for (int i = itemModifiers.Count - 1; i >= 0; i--) //go backwards because of iterating over list
            if (itemModifiers[i].item == item && itemModifiers[i].amount == amount)
            {
                itemModifiers.RemoveAt(i);
                break; //break after first find...
            }

        CalcDefault(); //recalculate value from scratch...
    } //end func RemoveMod

    //private- probably shouldn't call this from outside, so you dont remove spells that don't belong to the player when their own one expires
    private void RemoveMod(Spell spell)
    {
        for (int i = spellModifiers.Count - 1; i >= 0; i--) //go backwards because of iterating over list
            if (spellModifiers[i].spellHit.spell == spell)
            {
                spellModifiers.RemoveAt(i);
                break; //break after first find...
            }

        CalcDefault(); //recalculate value from scratch...
    } //end func RemoveMod

    /// <summary>
    /// remove all outdated effects and recalcualte base value
    /// </summary>
    public override void CalcDefault()
    {
        if (parent == null || parent.gameObject == null)
            return; //destroyed

        T newValue = defaultValue; //calculate from start
        for (int i = itemModifiers.Count - 1; i >= 0; i--)
        { //go backwards because of iterating over list if items that mod this stat
            newValue = itemModifiers[i].calc(newValue, parent, itemModifiers[i].amount); //calc effects of this mod
        }

        for (int i = spellModifiers.Count - 1; i >= 0; i--)
        { //go backwards because of iterating over list
            if (spellModifiers[i].expiresOn <= DateTime.UtcNow || !parent.e.statusEffects.Contains(spellModifiers[i].spellHit))
            {
                //Debug.LogWarning(spellModifiers[i].spellHit.spell + " expired from " + parent);
                spellModifiers.RemoveAt(i); //expire this mod
            }
            else
            {
                if (spellModifiers[i].spellHit.stacks <= 1) //if doesn't has multiple stacks
                    newValue = spellModifiers[i].calc(newValue, parent); //calc effects of this mod once
                else //has multiple stacks, so loop through and apply each one
                    for (int x = 0; x < spellModifiers[i].spellHit.stacks; x++)
                        newValue = spellModifiers[i].calc(newValue, parent);
            }
        }
        //Logger.Log("new value set to " + newValue + " from " + value + " after " + spellModifiers.Count + " spellModifiers and " + itemModifiers.Count + " itemModifiers");
        value = newValue; //set and update
    } //end func CalcDefault

    /// <summary>
    /// how to add a value to this, if at all. Most likely, this.value += value. If not, throw NotImplicatedException
    /// </summary>
    public abstract void Addition(T value);
    /// <summary>
    /// how to subtract a value to this, if it all. Most likely, this.value -= value If not, throw NotImplicatedException
    /// </summary>
    /// <param name="value"></param>
    public abstract void Subtraction(T value);
    /// <summary>
    /// how to mult a value to this, if at all. Most likely, this.value *= value. If not, throw NotImplicatedException
    /// </summary>
    public abstract void Multiply(T value);
    /// <summary>
    /// how to divide a value to this, if it all. Most likely, this.value /= value If not, throw NotImplicatedException
    /// </summary>
    /// <param name="value"></param>
    public abstract void Divide(T value);



    /// <summary>
    /// how to add a value to this, if at all. Most likely, this.value += value. If not, throw NotImplicatedException
    /// </summary>
    public abstract void Addition(ref T value);
    /// <summary>
    /// how to subtract a value to this, if it all. Most likely, this.value -= value If not, throw NotImplicatedException
    /// </summary>
    /// <param name="value"></param>
    public abstract void Subtraction(ref T value);
    /// <summary>
    /// how to mult a value to this, if at all. Most likely, this.value *= value. If not, throw NotImplicatedException
    /// </summary>
    public abstract void Multiply(ref T value);
    /// <summary>
    /// how to divide a value to this, if it all. Most likely, this.value /= value If not, throw NotImplicatedException
    /// </summary>
    /// <param name="value"></param>
    public abstract void Divide(ref T value);

    /// <summary>
    /// how to add a value to this (but clamped to two values), if at all. Most likely, this.value += value. If not, throw NotImplicatedException
    /// </summary>
    public abstract void Addition_Clamped(ref T value, ref T min, ref T max);
    /// <summary>
    /// how to subtract a value to this (but clamped to two values), if it all. Most likely, this.value -= value If not, throw NotImplicatedException
    /// </summary>
    /// <param name="value"></param>
    public abstract void Subtraction_Clamped(ref T value, ref T min, ref T max);
    /// <summary>
    /// how to mult a value to this (but clamped to two values), if at all. Most likely, this.value *= value. If not, throw NotImplicatedException
    /// </summary>
    public abstract void Multiply_Clamped(ref T value, ref T min, ref T max);
    /// <summary>
    /// how to divide a value to this (but clamped to two values), if it all. Most likely, this.value /= value If not, throw NotImplicatedException
    /// </summary>
    /// <param name="value"></param>
    public abstract void Divide_Clamped(ref T value, ref T min, ref T max);



    public override void Destroy()
    {
        base.Destroy();
        onUpdate = null;
        spellModifiers.Clear();
        itemModifiers.Clear();
    }

    //used if you want to call OnUpdate internaly without "can only appear left side of +=" delegate error when for example, overriding the default getter/setter on this.value
    protected void TriggerOnUpdate(ref T value)
    {
        onUpdate?.Invoke(ref value);
    }

    void AddDirtyAccountField_OnUpdate(ref T value)
    {
        if (Server.isServer)
        {
            AccountClient.instance.TryAddDirtyPlayerData(parent.e.p, this);
            //oldValue = defaultValue; //try and flag that this field is still dirty, even after "building" to the Account packet update ///actually changed to using this bool flag below, which should be more reliable and explicit
            justSentAccountUpdate = true; //flag that should still be dirty to clients...
        }
    } //end 

    /// <summary>
    /// call CalcDefault along with defaultValue set on startup, which should fairly recalculate using mods and sync to clients
    /// </summary>
    public override void ResetToDefaultValue()
    {
        CalcDefault();
    }

} //end class SyncField

public class SyncInt8Field : SyncField<int>
{

    public SyncInt8Field(string label, bool isStatic, int defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 1;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return 1;
    }

    public override bool NeedsBuild()
    {
        return oldValue != value;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldValue = value; //we're keeping track of when a value changes so we can sync it
        target[lastWritePos] = (byte)value; //write value
        lastWritePos += 1; //move write pos
    }
    public override void Addition(int value)
    {
        this.value += value;
    }
    public override void Subtraction(int value)
    {
        this.value -= value;
    }
    public override void Multiply(int value)
    {
        this.value *= value;
    }
    public override void Divide(int value)
    {
        this.value /= value;
    }
    public override void Addition(ref int value)
    {
        this.value += value;
    }
    public override void Subtraction(ref int value)
    {
        this.value -= value;
    }
    public override void Multiply(ref int value)
    {
        this.value *= value;
    }
    public override void Divide(ref int value)
    {
        this.value /= value;
    }
    public override void Addition_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value + value, min), max);
    }
    public override void Subtraction_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value - value, min), max);
    }
    public override void Multiply_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value * value, min), max);
    }
    public override void Divide_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value / value, min), max);
    }


    public override void Write(ref byte[] data, ref int startPos,ref SyncObject target, Client context)
    {
        this.value = data[startPos];
    }

} //end class SyncInt8Field

public class SyncInt16Field : SyncField<int>
{

    public SyncInt16Field(string label, bool isStatic, int defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 2;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return 2;
    }

    public override bool NeedsBuild()
    {
        return oldValue != value;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldValue = value; //we're keeping track of when a value changes so we can sync it
        target[lastWritePos] = (byte)value; //write value
        target[lastWritePos + 1] = (byte)(value >> 8); //write value second half
        lastWritePos += 2; //move write pos
    }
    public override void Addition(int value)
    {
        this.value += value;
    }
    public override void Subtraction(int value)
    {
        this.value -= value;
    }
    public override void Multiply(int value)
    {
        this.value *= value;
    }
    public override void Divide(int value)
    {
        this.value /= value;
    }
    public override void Addition(ref int value)
    {
        this.value += value;
    }
    public override void Subtraction(ref int value)
    {
        this.value -= value;
    }
    public override void Multiply(ref int value)
    {
        this.value *= value;
    }
    public override void Divide(ref int value)
    {
        this.value /= value;
    }
    public override void Addition_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value + value, min), max);
    }
    public override void Subtraction_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value - value, min), max);
    }
    public override void Multiply_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value * value, min), max);
    }
    public override void Divide_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value / value, min), max);
    }


    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        this.value = data[startPos] | (data[startPos + 1] << 8);
    }

    /*
    const float precision = 1024; //note that this will make overflow errors on input floats > (65535 / precision)
    public void ToFloat_NonAlloc(out float returns)
    {
        returns = value / precision;
    }
    public void FromFloat(ref float value)
    {
        this.value = (int)(value * precision);
    }
    */

} //end class SyncInt16Field


public class SyncInt32Field : SyncField<int>
{

    public SyncInt32Field(string label, bool isStatic, int defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 4;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return 4;
    }

    public override bool NeedsBuild()
    {
        return oldValue != value;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldValue = value; //we're keeping track of when a value changes so we can sync it
        target[lastWritePos] = (byte)value; //write value
        target[lastWritePos + 1] = (byte)(value >> 8); //write second byte
        target[lastWritePos + 2] = (byte)(value >> 16); //write third byte
        target[lastWritePos + 3] = (byte)(value >> 24); //write fourth byte
        lastWritePos += 4; //move write pos
    }
    public override void Addition(int value)
    {
        this.value += value;
    }
    public override void Subtraction(int value)
    {
        this.value -= value;
    }
    public override void Multiply(int value)
    {
        this.value *= value;
    }
    public override void Divide(int value)
    {
        this.value /= value;
    }
    public override void Addition(ref int value)
    {
        this.value += value;
    }
    public override void Subtraction(ref int value)
    {
        this.value -= value;
    }
    public override void Multiply(ref int value)
    {
        this.value *= value;
    }
    public override void Divide(ref int value)
    {
        this.value /= value;
    }
    public override void Addition_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value + value, min), max);
    }
    public override void Subtraction_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value - value, min), max);
    }
    public override void Multiply_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value * value, min), max);
    }
    public override void Divide_Clamped(ref int value, ref int min, ref int max)
    {
        this.value = Math.Min(Math.Max(this.value / value, min), max);
    }


    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        this.value = data[startPos] | (data[startPos + 1] << 8) | (data[startPos + 2] << 16) | (data[startPos + 3] << 24);
    }

    /*
    const float precision = 1024; //note that this will make overflow errors on input floats > (65535 / precision)
    public void ToFloat_NonAlloc(out float returns)
    {
        returns = value / precision;
    }
    public void FromFloat(ref float value)
    {
        this.value = (int)(value * precision);
    }
    */

} //end class SyncInt32Field


/// <summary>
/// float to x / 1024 precision synchronized to two bytes. Anything over (ushort.MaxValue / 1024) will overflow (roughly 64) which should be plenty for 1.0F based systems. Anything under 1 / 1024 will lose precision (~1/10th of a percent)
/// </summary>
public class SyncFloat16Field_1024 : SyncField<float>
{
    const float precision = 1024;
    
    float _floatValue;
    int _intValue; //internal int used for serialization since floats cant use bitwise operators
    public override float value
    {
        get
        {
            return _floatValue;
        }
        set
        {
            _intValue = (int)(value * precision);
            _floatValue = _intValue / precision; //also divide it again to emulate precision errors, making it deterministic
            if (!Server.isServer || NeedsBuild())
            {
                TriggerOnUpdate(ref _floatValue);
                if (onUpdateGlobalDict.ContainsKey(label))
                    onUpdateGlobalDict[label].Invoke(value, parent); //also trigger global update
            }
        }
    }

    public SyncFloat16Field_1024(string label, bool isStatic, float defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {

    }


    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 2;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return 2;
    }

    public override bool NeedsBuild()
    {
        return oldValue != value;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldValue = value; //we're keeping track of when a value changes so we can sync it
        target[lastWritePos] = (byte)_intValue; //write value
        target[lastWritePos + 1] = (byte)(_intValue >> 8); //write value second half
        lastWritePos += 2; //move write pos
    }
    public override void Addition(float value)
    {
        this.value += value;
    }
    public override void Subtraction(float value)
    {
        this.value -= value;
    }
    public override void Multiply(float value)
    {
        this.value *= value;
    }
    public override void Divide(float value)
    {
        this.value /= value;
    }
    public override void Addition(ref float value)
    {
        this.value += value;
    }
    public override void Subtraction(ref float value)
    {
        this.value -= value;
    }
    public override void Multiply(ref float value)
    {
        this.value *= value;
    }
    public override void Divide(ref float value)
    {
        this.value /= value;
    }
    public override void Addition_Clamped(ref float value, ref float min, ref float max)
    {
        this.value = Math.Min(Math.Max(this.value + value, min), max);
    }
    public override void Subtraction_Clamped(ref float value, ref float min, ref float max)
    {
        this.value = Math.Min(Math.Max(this.value - value, min), max);
    }
    public override void Multiply_Clamped(ref float value, ref float min, ref float max)
    {
        this.value = Math.Min(Math.Max(this.value * value, min), max);
    }
    public override void Divide_Clamped(ref float value, ref float min, ref float max)
    {
        this.value = Math.Min(Math.Max(this.value / value, min), max);
    }


    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        this.value = (data[startPos] | data[startPos + 1] << 8) / precision;
    }

} //end class SyncFloat16Field_1024



/// <summary>
/// float to x / 8192 precision synchronized to two bytes. Anything over (ushort.MaxValue / 8192) will overflow (roughly 8) which should be plenty for 1.0F based systems. Anything under 1 / 8192 will lose precision (~1/100th of a percent)
/// </summary>
public class SyncFloat16Field_8192 : SyncField<float>
{
    const float precision = 8192;

    float _floatValue;
    int _intValue; //internal int used for serialization since floats cant use bitwise operators
    public override float value
    {
        get
        {
            return _floatValue;
        }
        set
        {
            _intValue = (int)(value * precision);
            if (_intValue != 0)
                _intValue += 1; //offset by 1 to ensure it rounds up and not down... otherwise 0.2 => 0.19997... this will get divided by 8192
            _floatValue = _intValue / precision; //also divide it again to emulate precision errors, making it deterministic
            if (!Server.isServer || NeedsBuild())
            {
                TriggerOnUpdate(ref _floatValue);
                if (onUpdateGlobalDict.ContainsKey(label))
                    onUpdateGlobalDict[label].Invoke(value, parent); //also trigger global update
            }
        }
    }

    /// <summary>
    /// float to x / 8192 precision synchronized to two bytes. Anything over (ushort.MaxValue / 8192) will overflow (roughly 8) which should be plenty for 1.0F based systems. Anything under 1 / 8192 will lose precision (~1/100th of a percent)
    /// </summary>
    public SyncFloat16Field_8192(string label, bool isStatic, float defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {

    }


    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 2;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return 2;
    }

    public override bool NeedsBuild()
    {
        return oldValue != value;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldValue = value; //we're keeping track of when a value changes so we can sync it
        target[lastWritePos] = (byte)_intValue; //write value
        target[lastWritePos + 1] = (byte)(_intValue >> 8); //write value second half
        lastWritePos += 2; //move write pos
    }
    public override void Addition(float value)
    {
        this.value += value;
    }
    public override void Subtraction(float value)
    {
        this.value -= value;
    }
    public override void Multiply(float value)
    {
        this.value *= value;
    }
    public override void Divide(float value)
    {
        this.value /= value;
    }
    public override void Addition(ref float value)
    {
        this.value += value;
    }
    public override void Subtraction(ref float value)
    {
        this.value -= value;
    }
    public override void Multiply(ref float value)
    {
        this.value *= value;
    }
    public override void Divide(ref float value)
    {
        this.value /= value;
    }
    public override void Addition_Clamped(ref float value, ref float min, ref float max)
    {
        this.value = Math.Min(Math.Max(this.value + value, min), max);
    }
    public override void Subtraction_Clamped(ref float value, ref float min, ref float max)
    {
        this.value = Math.Min(Math.Max(this.value - value, min), max);
    }
    public override void Multiply_Clamped(ref float value, ref float min, ref float max)
    {
        this.value = Math.Min(Math.Max(this.value * value, min), max);
    }
    public override void Divide_Clamped(ref float value, ref float min, ref float max)
    {
        this.value = Math.Min(Math.Max(this.value / value, min), max);
    }


    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        this.value = (data[startPos] | data[startPos + 1] << 8) / precision;
    }

} //end class SyncFloat16Field_8192



public class SyncBoolField : SyncField<bool>
{

    public SyncBoolField(string label, bool isStatic, bool defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 1;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return 1;
    }

    public override bool NeedsBuild()
    {
        return oldValue != value;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldValue = value; //we're keeping track of when a value changes so we can sync it
        target[lastWritePos] = value ? (byte)1 : (byte)0; //write value
        lastWritePos += 1; //move write pos
    }
    public override void Addition(bool value)
    {
        throw new NotImplementedException("addition not supported on bool value");
    }
    public override void Subtraction(bool value)
    {
        throw new NotImplementedException("subtraction not supported on bool value");
    }
    public override void Multiply(bool value)
    {
        throw new NotImplementedException("mult not supported on bool value");
    }
    public override void Divide(bool value)
    {
        throw new NotImplementedException("divide not supported on bool value");
    }
    public override void Addition(ref bool value)
    {
        throw new NotImplementedException("addition not supported on bool value");
    }
    public override void Subtraction(ref bool value)
    {
        throw new NotImplementedException("subtraction not supported on bool value");
    }
    public override void Multiply(ref bool value) {
        throw new NotImplementedException("mult not supported on bool value");
    }
    public override void Divide(ref bool value)
    {
    throw new NotImplementedException("divide not supported on bool value");
    }
    public override void Addition_Clamped(ref bool value, ref bool min, ref bool max)
    {
        throw new NotImplementedException("addition not supported on bool value");
    }
    public override void Subtraction_Clamped(ref bool value, ref bool min, ref bool max)
    {
        throw new NotImplementedException("subtraction not supported on bool value");
    }
    public override void Multiply_Clamped(ref bool value, ref bool min, ref bool max)
    {
        throw new NotImplementedException("mult not supported on bool value");
    }
    public override void Divide_Clamped(ref bool value, ref bool min, ref bool max)
    {
        throw new NotImplementedException("divide not supported on bool value");
    }


    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        this.value = data[startPos] == 1;
    }

} //end class SyncInt8Field


/// <summary>
/// sync multiple int16s at the same time, when data comes in as several variables at once. Will automatically handle and split it into an array
/// </summary>
public class SyncInt16MultiField : SyncField<int[]>
{
    private int totalFieldLength;
    bool needsBuild = false;

    public SyncInt16MultiField(string label, bool isStatic, int[] defaultValues, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValues, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
        totalFieldLength = defaultValues.Length * 2; //2 bytes per field
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return totalFieldLength;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return totalFieldLength;
    }


    /// <summary>
    /// setting this.value won't flag needsBuild unless you set the entire array, so this will flag properly after setting a value in the array
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    public void Set(int value, int index)
    {
        needsBuild = true;
        _value[index] = value;

        TriggerOnUpdate(ref _value);
        if (onUpdateGlobalDict.ContainsKey(label))
            onUpdateGlobalDict[label].Invoke(_value, parent); //also trigger global update
    } //end Set

    public override bool NeedsBuild()
    {
        return needsBuild;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        needsBuild = false; //we're keeping track of when a value changes so we can sync it
        for(int i = 0; i < _value.Length; i ++)
        {
            target[lastWritePos] = (byte)_value[i]; //write value
            target[lastWritePos + 1] = (byte)(_value[i] >> 8); //write value second half
            lastWritePos += 2; //move write pos
        }
    }
    public override void Addition(int[] value)
    {
        throw new NotImplementedException("cannot implement addition on a multi field");
    }
    public override void Subtraction(int[] value)
    {
        throw new NotImplementedException("cannot implement subtraction on a multi field");
    }
    public override void Multiply(int[] value)
    {
        throw new NotImplementedException("cannot implement multiplication on a multi field");
    }
    public override void Divide(int[] value)
    {
        throw new NotImplementedException("cannot implement division on a multi field");
    }

    public override void Addition(ref int[] value)
    {
        throw new NotImplementedException("cannot implement addition on a multi field");
    }
    public override void Subtraction(ref int[] value)
    {
        throw new NotImplementedException("cannot implement subtraction on a multi field");
    }
    public override void Multiply(ref int[] value)
    {
        throw new NotImplementedException("cannot implement multiplication on a multi field");
    }
    public override void Divide(ref int[] value)
    {
        throw new NotImplementedException("cannot implement division on a multi field");
    }
    public override void Addition_Clamped(ref int[] value, ref int[] min, ref int[] max)
    {
        throw new NotImplementedException("clamped addition not supported on multi value");
    }
    public override void Subtraction_Clamped(ref int[] value, ref int[] min, ref int[] max)
    {
        throw new NotImplementedException("clamped subtraction not supported on multi value");
    }
    public override void Multiply_Clamped(ref int[] value, ref int[] min, ref int[] max)
    {
        throw new NotImplementedException("clamped mult not supported on multi value");
    }
    public override void Divide_Clamped(ref int[] value, ref int[] min, ref int[] max)
    {
        throw new NotImplementedException("clamped divide not supported on multi value");
    }


    public override void Write(ref byte[] data, ref int startPos,ref SyncObject target, Client context)
    {
        for (int i = 0; i < _value.Length; i++)
        {
            value[i] = data[startPos + (i * 2)] | (data[startPos + (i * 2) + 1] << 8);
        }
    }

} //end class SyncInt16MultiField



/// <summary>
/// sync multiple int32s at the same time, when data comes in as several variables at once. Will automatically handle and split it into an array
/// </summary>
public class SyncInt32MultiField : SyncField<int[]>
{
    private int totalFieldLength;
    bool needsBuild = false;

    public SyncInt32MultiField(string label, bool isStatic, int[] defaultValues, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValues, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
        totalFieldLength = defaultValues.Length * 4; //4 bytes per field
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return totalFieldLength;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return totalFieldLength;
    }


    /// <summary>
    /// setting this.value won't flag needsBuild unless you set the entire array, so this will flag properly after setting a value in the array
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    public void Set(int value, int index)
    {
        needsBuild = true;
        _value[index] = value;

        TriggerOnUpdate(ref _value);
        if (onUpdateGlobalDict.ContainsKey(label))
            onUpdateGlobalDict[label].Invoke(_value, parent); //also trigger global update
    } //end Set

    public override bool NeedsBuild()
    {
        return needsBuild;
    }

    int tempBuild;
    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        needsBuild = false; //we're keeping track of when a value changes so we can sync it
        for (int i = 0; i < _value.Length; i++)
        {
            tempBuild = _value[i]; //cache
            target[lastWritePos] = (byte)tempBuild; //write value
            target[lastWritePos + 1] = (byte)(tempBuild >> 8); //write value second half
            target[lastWritePos + 2] = (byte)(tempBuild >> 16);
            target[lastWritePos + 3] = (byte)(tempBuild >> 24);
            lastWritePos += 4; //move write pos
        }
    }
    public override void Addition(int[] value)
    {
        throw new NotImplementedException("cannot implement addition on a multi field");
    }
    public override void Subtraction(int[] value)
    {
        throw new NotImplementedException("cannot implement subtraction on a multi field");
    }
    public override void Multiply(int[] value)
    {
        throw new NotImplementedException("cannot implement multiplication on a multi field");
    }
    public override void Divide(int[] value)
    {
        throw new NotImplementedException("cannot implement division on a multi field");
    }

    public override void Addition(ref int[] value)
    {
        throw new NotImplementedException("cannot implement addition on a multi field");
    }
    public override void Subtraction(ref int[] value)
    {
        throw new NotImplementedException("cannot implement subtraction on a multi field");
    }
    public override void Multiply(ref int[] value)
    {
        throw new NotImplementedException("cannot implement multiplication on a multi field");
    }
    public override void Divide(ref int[] value)
    {
        throw new NotImplementedException("cannot implement division on a multi field");
    }
    public override void Addition_Clamped(ref int[] value, ref int[] min, ref int[] max)
    {
        throw new NotImplementedException("clamped addition not supported on multi value");
    }
    public override void Subtraction_Clamped(ref int[] value, ref int[] min, ref int[] max)
    {
        throw new NotImplementedException("clamped subtraction not supported on multi value");
    }
    public override void Multiply_Clamped(ref int[] value, ref int[] min, ref int[] max)
    {
        throw new NotImplementedException("clamped mult not supported on multi value");
    }
    public override void Divide_Clamped(ref int[] value, ref int[] min, ref int[] max)
    {
        throw new NotImplementedException("clamped divide not supported on multi value");
    }

    int tempWrite;
    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        for (int i = 0; i < _value.Length; i++)
        {
            tempWrite = startPos + (i * 4); //get index
            value[i] = data[tempWrite] | (data[tempWrite + 1] << 8) | (data[tempWrite + 2] << 16) | (data[tempWrite + 3] << 24);
        }
    }

} //end class SyncInt32MultiField


/// <summary>
/// sync multiple int16s at the same time, when data comes in as several variables at once. Will automatically handle and split it into an array
/// </summary>
public class SyncStringFixedMultiField_UTF8 : SyncField<string[]>
{
    private int totalFieldLength;
    bool needsBuild = false;
    int Length;

    public SyncStringFixedMultiField_UTF8(string label, bool isStatic, string[] defaultValues, int Length, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValues, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
        this.Length = Length;
        totalFieldLength = defaultValues.Length * Length; //2 bytes per field
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return totalFieldLength;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return totalFieldLength;
    }

    public override bool NeedsBuild()
    {
        return needsBuild;
    }


    /// <summary>
    /// setting this.value won't flag needsBuild unless you set the entire array, so this will flag properly after setting a value in the array
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    public void Set(string value, int index)
    {
        needsBuild = true;
        _value[index] = value;

        TriggerOnUpdate(ref _value);
        if (onUpdateGlobalDict.ContainsKey(label))
            onUpdateGlobalDict[label].Invoke(_value, parent); //also trigger global update
    } //end Set

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        needsBuild = false; //we're keeping track of when a value changes so we can sync it

        for (int i = 0; i < totalFieldLength; i++) //need to first clear the buffer in case old data persists in the buffer
            target[lastWritePos + i] = 0;

        for (int i = 0; i < _value.Length; i++)
        {
            NetworkIO.StringSerialize(ref target, ref value[i], ref lastWritePos);
            lastWritePos += Length; //move write pos
        }
    }

    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        int tempPos = startPos; //use a var that we can change
        for (int i = 0; i < _value.Length; i++)
        {
            value[i] = NetworkIO.StringDeserialize(ref data, ref tempPos, Length);
            tempPos += Length;
        }
    }

    public override void Addition(string[] value)
    {
        throw new NotImplementedException("addition on string[] value not implemented");

    }
    public override void Subtraction(string[] value)
    {
        throw new NotImplementedException("subtraction on string[] value not implemented");
    }
    public override void Multiply(string[] value)
    {
        throw new NotImplementedException("addition on string[] value not implemented");
    }
    public override void Divide(string[] value)
    {
        throw new NotImplementedException("division on quaternion value not implemented");
    }
    public override void Addition(ref string[] value)
    {
        throw new NotImplementedException("addition on string[] value not implemented");
    }
    public override void Subtraction(ref string[] value)
    {
        throw new NotImplementedException("subtraction on string[] value not implemented");
    }
    public override void Multiply(ref string[] value)
    {
        throw new NotImplementedException("addition on string[] value not implemented");
    }
    public override void Divide(ref string[] value)
    {
        throw new NotImplementedException("division on quaternion value not implemented");
    }
    public override void Addition_Clamped(ref string[] value, ref string[] min, ref string[] max)
    {
        throw new NotImplementedException("clamped addition not supported on string[] value");
    }
    public override void Subtraction_Clamped(ref string[] value, ref string[] min, ref string[] max)
    {
        throw new NotImplementedException("clamped subtraction not supported on string[] value");
    }
    public override void Multiply_Clamped(ref string[] value, ref string[] min, ref string[] max)
    {
        throw new NotImplementedException("clamped mult not supported on string[] value");
    }
    public override void Divide_Clamped(ref string[] value, ref string[] min, ref string[] max)
    {
        throw new NotImplementedException("clamped divide not supported on string[] value");
    }

} //end class SyncStringFixedMultiField_UTF8


/// <summary>
/// sync string[] at the same time, when data comes in as several variables at once. Will automatically handle and split it into an array
/// </summary>
public class SyncStringFixedMultiField_UTF16 : SyncField<string[]>
{
    private readonly int totalFieldLength;
    bool needsBuild = false;
    readonly int Length;
    readonly int entryBytesLength; //length in bytes after accounting for 2 bytes per char in Unicode

    public SyncStringFixedMultiField_UTF16(string label, bool isStatic, string[] defaultValues, int Length, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValues, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
        this.Length = Length;
        totalFieldLength = defaultValues.Length * Length * 2;
        entryBytesLength = Length * 2;
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return totalFieldLength;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return totalFieldLength;
    }

    public override bool NeedsBuild()
    {
        return needsBuild;
    }


    /// <summary>
    /// setting this.value won't flag needsBuild unless you set the entire array, so this will flag properly after setting a value in the array
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    public void Set(string value, int index)
    {
        needsBuild = true;
        _value[index] = value;

        TriggerOnUpdate(ref _value);
        if (onUpdateGlobalDict.ContainsKey(label))
            onUpdateGlobalDict[label].Invoke(_value, parent); //also trigger global update
    } //end Set

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        needsBuild = false; //we're keeping track of when a value changes so we can sync it

        for (int i = 0; i < totalFieldLength; i++) //need to first clear the buffer in case old data persists in the buffer
            target[lastWritePos + i] = 0;

        for (int i = 0; i < _value.Length; i++)
        {
            NetworkIO.StringSerialize_UTF16(ref target, ref value[i], ref lastWritePos);
            lastWritePos += entryBytesLength; //move write pos
        }
    }

    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        int tempPos = startPos; //use a var that we can change
        for (int i = 0; i < _value.Length; i++)
        {
            value[i] = NetworkIO.StringDeserialize_UTF16(ref data, ref tempPos, Length);
            tempPos += entryBytesLength;
        }
    }

    public override void Addition(string[] value)
    {
        throw new NotImplementedException("addition on string[] value not implemented");

    }
    public override void Subtraction(string[] value)
    {
        throw new NotImplementedException("subtraction on string[] value not implemented");
    }
    public override void Multiply(string[] value)
    {
        throw new NotImplementedException("addition on string[] value not implemented");
    }
    public override void Divide(string[] value)
    {
        throw new NotImplementedException("division on quaternion value not implemented");
    }
    public override void Addition(ref string[] value)
    {
        throw new NotImplementedException("addition on string[] value not implemented");
    }
    public override void Subtraction(ref string[] value)
    {
        throw new NotImplementedException("subtraction on string[] value not implemented");
    }
    public override void Multiply(ref string[] value)
    {
        throw new NotImplementedException("addition on string[] value not implemented");
    }
    public override void Divide(ref string[] value)
    {
        throw new NotImplementedException("division on quaternion value not implemented");
    }
    public override void Addition_Clamped(ref string[] value, ref string[] min, ref string[] max)
    {
        throw new NotImplementedException("clamped addition not supported on string[] value");
    }
    public override void Subtraction_Clamped(ref string[] value, ref string[] min, ref string[] max)
    {
        throw new NotImplementedException("clamped subtraction not supported on string[] value");
    }
    public override void Multiply_Clamped(ref string[] value, ref string[] min, ref string[] max)
    {
        throw new NotImplementedException("clamped mult not supported on string[] value");
    }
    public override void Divide_Clamped(ref string[] value, ref string[] min, ref string[] max)
    {
        throw new NotImplementedException("clamped divide not supported on string[] value");
    }

} //end class SyncStringFixedMultiField_UTF16



public class SyncVector3Field : SyncField<Vector3>
{

    public SyncVector3Field(string label, bool isStatic, Vector3 defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
    }

    public override int GetWorstTotalFieldLength()
    { //how long the serialized data is in bytes worst case
        return 12;
    }
    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 12;
    }

    public override bool NeedsBuild()
    {
        return !oldValue.Equals(value);
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldValue.x = value.x;
        oldValue.y = value.y;
        oldValue.z = value.z;
        NetworkIO.Vector3Serialize(value, ref target, lastWritePos);
        lastWritePos += 12; //move forward 12 bytes, as we're using 4 bytes for each axis
    }

    public override void Addition(Vector3 value)
    {
        this.value += value;
    }
    public override void Subtraction(Vector3 value)
    {
        this.value -= value;
    }
    public override void Multiply(Vector3 value)
    {
        throw new NotImplementedException("multiplication on Vector3 value not implemented");
    }
    public override void Divide(Vector3 value)
    {
        throw new NotImplementedException("division on Vector3 value not implemented");
    }
    public override void Addition(ref Vector3 value)
    {
        this.value += value;
    }
    public override void Subtraction(ref Vector3 value)
    {
        this.value -= value;
    }
    public override void Multiply(ref Vector3 value)
    {
        throw new NotImplementedException("multiplication on Vector3 value not implemented");
    }
    public override void Divide(ref Vector3 value)
    {
        throw new NotImplementedException("division on Vector3 value not implemented");
    }
    public override void Addition_Clamped(ref Vector3 value, ref Vector3 min, ref Vector3 max)
    {
        throw new NotImplementedException("clamped addition not supported on Vector3 value");
    }
    public override void Subtraction_Clamped(ref Vector3 value, ref Vector3 min, ref Vector3 max)
    {
        throw new NotImplementedException("clamped subtraction not supported on Vector3 value");
    }
    public override void Multiply_Clamped(ref Vector3 value, ref Vector3 min, ref Vector3 max)
    {
        throw new NotImplementedException("clamped mult not supported on Vector3 value");
    }
    public override void Divide_Clamped(ref Vector3 value, ref Vector3 min, ref Vector3 max)
    {
        throw new NotImplementedException("clamped divide not supported on Vector3 value");
    }


    public override void Write(ref byte[] data, ref int startPos,ref SyncObject target, Client context)
    {
        this.value = NetworkIO.Vector3Deserialize(ref data, startPos);
    }

} //end SyncVector3Field

public class SyncQuaternionField : SyncField<Quaternion>
{

    public SyncQuaternionField(string label, bool isStatic, Quaternion defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 4;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return 4;
    }

    public override bool NeedsBuild()
    {
        return !oldValue.Equals(value);
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldValue.x = value.x;
        oldValue.y = value.y;
        oldValue.z = value.z;
        oldValue.w = value.w;

        target[lastWritePos] = (byte)value.x;
        target[lastWritePos + 1] = (byte)value.y;
        target[lastWritePos + 2] = (byte)value.z;
        target[lastWritePos + 3] = (byte)value.w;
        lastWritePos += 4; //move forward 12 bytes, as we're using 4 bytes for each axis
    }

    public override void Addition(Quaternion value)
    {
        throw new NotImplementedException("addition on quaternion value not implemented");
    }
    public override void Subtraction(Quaternion value)
    {
        throw new NotImplementedException("subtraction on quaternion value not implemented");
    }
    public override void Multiply(Quaternion value)
    {
        this.value *= value;
    }
    public override void Divide(Quaternion value)
    {
        throw new NotImplementedException("division on quaternion value not implemented");
    }
    public override void Addition(ref Quaternion value)
    {
        throw new NotImplementedException("addition on quaternion value not implemented");
    }
    public override void Subtraction(ref Quaternion value)
    {
        throw new NotImplementedException("subtraction on quaternion value not implemented");
    }
    public override void Multiply(ref Quaternion value)
    {
        this.value *= value;
    }
    public override void Divide(ref Quaternion value)
    {
        throw new NotImplementedException("division on quaternion value not implemented");
    }
    public override void Addition_Clamped(ref Quaternion value, ref Quaternion min, ref Quaternion max)
    {
        throw new NotImplementedException("clamped addition not supported on Quaternion value");
    }
    public override void Subtraction_Clamped(ref Quaternion value, ref Quaternion min, ref Quaternion max)
    {
        throw new NotImplementedException("clamped subtraction not supported on Quaternion value");
    }
    public override void Multiply_Clamped(ref Quaternion value, ref Quaternion min, ref Quaternion max)
    {
        throw new NotImplementedException("clamped mult not supported on Quaternion value");
    }
    public override void Divide_Clamped(ref Quaternion value, ref Quaternion min, ref Quaternion max)
    {
        throw new NotImplementedException("clamped divide not supported on Quaternion value");
    }

    public override void Write(ref byte[] data, ref int startPos,ref SyncObject target, Client context)
    {
        throw new NotImplementedException("quaternion syncing not written yet");
        //this.value = NetworkIO.Vector3Deserialize(ref data, startPos);
    }

} //end SyncQuaternionField

/// <summary>
/// Essentially it is a Vector3 and Quaternion, checks if either is different then syncs both if buildOnAny is set
/// </summary>
public class SyncTransformField : SyncFieldGroup
{

    public Transform target;

    SyncQuaternionField quaternionFieldCache; //prevent having to cast and things
    SyncVector3Field vector3FieldCache;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="buildOnAny"> whether to sync everything if anything is dirty or just use first field (the position) as dirty flag</param>
    public SyncTransformField(string label, bool buildOnAny, bool isStatic, Transform target, SyncObject parent, char accountFieldCode = default) : base(label, buildOnAny, parent)
    {
        this.target = target; this.isStatic = isStatic;
    }

    //override how to check for whether we need to build, because otherwise we have to keep updating some internal Vector3/Quaternion variable to check whether anything has changed from base.NeedsBuild
    public override bool NeedsBuild()
    {
        if (buildOnAny) //depending on dirty flag type, return whether we're dirty
            return target.transform.position != vector3FieldCache.value || target.transform.rotation.Equals(quaternionFieldCache.value);
        else
            return target.transform.position != vector3FieldCache.value;
    }

    public override void SetDefaults(out SyncField[] fields)
    {
        //two syncFields for transform, a Vector3 and a rotation
        fields = new SyncField[]
        {
            new SyncVector3Field("", isStatic, Vector3.zero),
            new SyncQuaternionField("", isStatic, Quaternion.identity)
        };

        vector3FieldCache = fields[0] as SyncVector3Field; //cache
        quaternionFieldCache = fields[1] as SyncQuaternionField; //cache
    }

    
    public override void Destroy()
    { //clean refs
        base.Destroy();
        vector3FieldCache = null;
        quaternionFieldCache = null;
    }

    public override void ResetToDefaultValue()
    {
        throw new NotImplementedException();
    }

} //end SyncTransformField


public class SyncStringFixedField : SyncField<string>
{
    public readonly int Length; //only can be set in constructor

    public SyncStringFixedField(string label, bool isStatic, string defaultValue, int Length, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {
        this.Length = Length;
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return Length;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return Length;
    }

    public override bool NeedsBuild()
    {
        return oldValue != value;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldValue = value; //we're keeping track of when a value changes so we can sync it
        for (int i = 0; i < Length; i++)
            target[lastWritePos + i] = 0;
        NetworkIO.StringSerialize(ref target, value, ref lastWritePos);
        lastWritePos += Length; //move write pos
    }

    public override void Addition(string value)
    {
        value += value;
    }
    public override void Subtraction(string value)
    {
        throw new NotImplementedException("subtraction on string value not implemented");
    }
    public override void Multiply(string value)
    {
        throw new NotImplementedException("addition on string value not implemented");
    }
    public override void Divide(string value)
    {
        throw new NotImplementedException("division on quaternion value not implemented");
    }
    public override void Addition(ref string value)
    {
        value += value;
    }
    public override void Subtraction(ref string value)
    {
        throw new NotImplementedException("subtraction on string value not implemented");
    }
    public override void Multiply(ref string value)
    {
        throw new NotImplementedException("addition on string value not implemented");
    }
    public override void Divide(ref string value)
    {
        throw new NotImplementedException("division on quaternion value not implemented");
    }
    public override void Addition_Clamped(ref string value, ref string min, ref string max)
    {
        throw new NotImplementedException("clamped addition not supported on string value");
    }
    public override void Subtraction_Clamped(ref string value, ref string min, ref string max)
    {
        throw new NotImplementedException("clamped subtraction not supported on string value");
    }
    public override void Multiply_Clamped(ref string value, ref string min, ref string max)
    {
        throw new NotImplementedException("clamped mult not supported on string value");
    }
    public override void Divide_Clamped(ref string value, ref string min, ref string max)
    {
        throw new NotImplementedException("clamped divide not supported on string value");
    }

    public override void Write(ref byte[] data, ref int startPos,ref SyncObject target, Client context)
    {
        this.value = NetworkIO.StringDeserialize(ref data, ref startPos, Length);
        /*
        if(!GameMode.FactionExists(this.value))
        {
            Debug.LogError("unable to find faction " + this.value);
        }
        */
    }

} //end class SyncStringFixedField

/// <summary>
/// DateTime to ~10-100ms precision synchronized to 32 bits, using an int32 internally. 
/// calculates the number of milliseconds since the month started in UTC time, then reduces that by 1.3f
/// 1.3f is used because 2764800000 (milliseconds in 32 days) / 2,147,483,647 (int.MaxValue) is 1.2874
/// there is a window of a few seconds at the beginning of the month where clients can receive a packet and erroneously 
/// calculate it from the previous month, causing it to seem ~30 days old... just check if it's >= 28 days ago to solve that
/// the actual calculations should only be off by 1-2ms but DateTimes are not accurate to the MS and can vary from 10-100ms when reversing
/// </summary>
public class SyncDateTimeField_3232 : SyncField<DateTime>
{
    public const double precision = 1.3f;

    DateTime _dateTimeValue; //true value
    int _intValue; //internal int used for serialization since floats cant use bitwise operators
    /// <summary>
    /// WARNING- always use UTC time, not DateTime.UtcNow
    /// </summary>
    public override DateTime value
    {
        get
        {
            return _dateTimeValue;
        }
        set
        {
            if (value == default)
            { //prevent integer overflow error by skipping the part where you convert to int
                _dateTimeValue = default;
                TriggerOnUpdate(ref _dateTimeValue);
                if (onUpdateGlobalDict.ContainsKey(label))
                    onUpdateGlobalDict[label].Invoke(_dateTimeValue, parent); //also trigger global update
                return;
            }
            _intValue = (int)((value - new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)).TotalMilliseconds / precision); //set internal int value to value passed to this setter field
            _dateTimeValue = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMilliseconds(_intValue * precision); //set internal value to a recalculated version of int value to simulate precision errors, making it more deterministic
            TriggerOnUpdate(ref _dateTimeValue);
            if (onUpdateGlobalDict.ContainsKey(label))
                onUpdateGlobalDict[label].Invoke(_dateTimeValue, parent); //also trigger global update
        }
    }

    public SyncDateTimeField_3232(string label, bool isStatic, DateTime defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {

    }

    public void SetToNow()
    {
        value = DateTime.UtcNow;
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 4;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return 4;
    }

    int oldIntValue;

    public override bool NeedsBuild()
    {
        return oldIntValue != _intValue;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldIntValue = _intValue; //we're keeping track of when a value changes so we can sync it
        target[lastWritePos] = (byte)_intValue; //write value
        target[lastWritePos + 1] = (byte)(_intValue >> 8); //write second byte
        target[lastWritePos + 2] = (byte)(_intValue >> 16); //write second byte
        target[lastWritePos + 3] = (byte)(_intValue >> 24); //write second byte

        lastWritePos += 4; //move write pos
    }
    public override void Addition(DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Subtraction(DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Multiply(DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Divide(DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Addition(ref DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Subtraction(ref DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Multiply(ref DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Divide(ref DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Addition_Clamped(ref DateTime value, ref DateTime min, ref DateTime max)
    {
        throw new NotImplementedException();
    }
    public override void Subtraction_Clamped(ref DateTime value, ref DateTime min, ref DateTime max)
    {
        throw new NotImplementedException();
    }
    public override void Multiply_Clamped(ref DateTime value, ref DateTime min, ref DateTime max)
    {
        throw new NotImplementedException();
    }
    public override void Divide_Clamped(ref DateTime value, ref DateTime min, ref DateTime max) {
        throw new NotImplementedException();
    }


    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        //this.value = (data[startPos] | data[startPos + 1] << 8) / precision;

        _intValue = data[startPos] | data[startPos + 1] << 8 | data[startPos + 2] << 16 | data[startPos + 3] << 24; //set internal int value to value passed to this setter field
        _dateTimeValue = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMilliseconds(_intValue * precision); //set internal value to a recalculated version of int value to simulate precision errors, making it more deterministic
        TriggerOnUpdate(ref _dateTimeValue);
        if (onUpdateGlobalDict.ContainsKey(label))
            onUpdateGlobalDict[label].Invoke(_dateTimeValue, parent); //also trigger global update

    }

} //end class SyncDateTimeField_3232

/// <summary>
/// stores internally as 8 bytes as a "long" type
/// </summary>
public class SyncDateTimeField_Lossless : SyncField<DateTime>
{
    long _longValue;
    byte[] bytes = new byte[8];

    /// <summary>
    /// WARNING- always use UTC time, not DateTime.UtcNow
    /// </summary>
    public override DateTime value
    {
        get
        {
            return _value;
        }
        set
        {
            _longValue = value.Ticks;
            bytes = BitConverter.GetBytes(_longValue);
            _value = value;
            TriggerOnUpdate(ref _value);
            if (onUpdateGlobalDict.ContainsKey(label))
                onUpdateGlobalDict[label].Invoke(_value, parent); //also trigger global update
        }
    }

    public SyncDateTimeField_Lossless(string label, bool isStatic, DateTime defaultValue, SyncObject parent = null, OnUpdate onUpdate = null, char accountFieldCode = default) : base(label, isStatic, defaultValue, parent, onUpdate, accountFieldCode) //need to inherit base constructor initalize base class variables
    {

    }

    public void SetToNow()
    {
        value = DateTime.UtcNow;
    }

    public override int GetCurrentTotalFieldLength()
    { //how long the serialized data is in bytes
        return 8;
    }
    public override int GetWorstTotalFieldLength()
    { //worst case scenario for length of field, used on buffer initialization
        return 8;
    }

    long oldLongValue;

    public override bool NeedsBuild()
    {
        return oldLongValue != _longValue;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        oldLongValue = _longValue; //we're keeping track of when a value changes so we can sync it
        target[lastWritePos] = bytes[0];
        target[lastWritePos + 1] = bytes[1];
        target[lastWritePos + 2] = bytes[2];
        target[lastWritePos + 3] = bytes[3];
        target[lastWritePos + 4] = bytes[4];
        target[lastWritePos + 5] = bytes[5];
        target[lastWritePos + 6] = bytes[6];
        target[lastWritePos + 7] = bytes[7];

        lastWritePos += 8; //move write pos
    }
    public override void Addition(DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Subtraction(DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Multiply(DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Divide(DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Addition(ref DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Subtraction(ref DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Multiply(ref DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Divide(ref DateTime value)
    {
        throw new NotImplementedException();
    }
    public override void Addition_Clamped(ref DateTime value, ref DateTime min, ref DateTime max)
    {
        throw new NotImplementedException();
    }
    public override void Subtraction_Clamped(ref DateTime value, ref DateTime min, ref DateTime max)
    {
        throw new NotImplementedException();
    }
    public override void Multiply_Clamped(ref DateTime value, ref DateTime min, ref DateTime max)
    {
        throw new NotImplementedException();
    }
    public override void Divide_Clamped(ref DateTime value, ref DateTime min, ref DateTime max)
    {
        throw new NotImplementedException();
    }


    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        //this.value = (data[startPos] | data[startPos + 1] << 8) / precision;

        //_longValue = data[startPos] | data[startPos + 1] << 8 | data[startPos + 2] << 16 | data[startPos + 3] << 24 | data[startPos + 4] << 32 | data[startPos + 5] << 40 | data[startPos + 6] << 48 | data[startPos + 7] << 56; //set internal int value to value passed to this setter field
        _longValue = BitConverter.ToInt64(data, startPos);

        _value = new DateTime(_longValue); //set internal value to a recalculated version of int value to simulate precision errors, making it more deterministic
        TriggerOnUpdate(ref _value);
        if (onUpdateGlobalDict.ContainsKey(label))
            onUpdateGlobalDict[label].Invoke(_value, parent); //also trigger global update

    }

} //end class SyncDateTimeField_3232

