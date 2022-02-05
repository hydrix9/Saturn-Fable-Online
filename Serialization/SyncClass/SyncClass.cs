using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region oldMethod
/*
public interface IAutoSync
{

}


/// <summary>
/// this class tries to automatically sync certain classes using generic arrays for properties, and scanning these to determine serialization protocol like length of fields and dirty masks
/// </summary>
public class SyncClass : MonoBehaviour {
    public static List<SyncClass> syncClasses = new List<SyncClass> { null }; //make first entry null to reduce ambiguity of null int index
    public static bool syncClassesListed;
    public SyncClass()
    {
        syncClasses.Add(this); //add to singleton list to allow for id-to-SyncClass
    }


    
    //public string[] stringPropNames = new string[0];
    //public string[] stringPropValues = new string[0];
   // public Action<string>[] stringPropOnChanged = new Action<string>[0];
    

    public string[] int8PropNames = new string[0];
    public int[] int8PropValues = new int[0];
    public Action<int>[] int8PropOnChanged = new Action<int>[0];


    public string[] int16PropNames = new string[0];
    public int[] int16PropValues = new int[0];
    public Action<int>[] int16PropOnChanged = new Action<int>[0];


    bool dirty = false;
    byte[] data;
    int lastWritePos;
    CoroutineHandle updateRoutine;
    public float updateInterval;

    // Use this for initialization
    void Awake () {
        if (!syncClassesListed) {
            Ext.GetAllTypesInherited(typeof(SyncClass)).ForEach(entry => Activator.CreateInstance(entry)); //run default 0-parameter constructor on each inherited class, adding it to the singleton list and assigning ID. See constructor
            syncClassesListed = true;
        }

        Init();
        Server.instance.onStartServer += (l) => ServerInit();
    }

    public virtual void Init()
    {
        //dirtyMask = new byte[(stringPropValues.Length + int8PropValues.Length + int16PropValues.Length) / 8]; //initialize to have 1 bit per value
    }
	
    /// <summary>
    /// initialize variables for sending to clients
    /// </summary>
    public virtual void ServerInit()
    {
        data = new byte[int8PropValues.Length + (int16PropValues.Length * 2) + 2]; //allocate to something like worst case scenario plus netcode + id
        data[0] = netcodes.syncClass; //netcode
        data[1] = (byte)syncClasses.IndexOf(this); //id

        updateRoutine = Timing.RunCoroutine(UpdateRoutine()); //start updating

    }


    public void WriteInt8(ref int index, int newValue)
    {
        int8PropValues[index] = newValue; //assign value
        int8PropOnChanged[index](newValue); //run command

        if(Server.isServer)
        {
            dirty = true;
            data[lastWritePos] = (byte)index; //write id
            data[lastWritePos + 1] = (byte)newValue;
            lastWritePos += 2;
        }
    }

    public void WriteInt16(ref int index, int newValue)
    {
        int16PropValues[index] = newValue; //assign value
        int16PropOnChanged[index](newValue); //run command

        if (Server.isServer)
        {
            dirty = true;
            data[lastWritePos] = (byte)(index + int8PropValues.Length); //write id
            data[lastWritePos + 1] = (byte)newValue;
            data[lastWritePos + 2] = (byte)(newValue >> 8);

            lastWritePos += 3;
        }
    }

    byte[] sends;
    public IEnumerator<float> UpdateRoutine()
    {
        while (true)
        {
            if (!Server.isServer)
                yield break; //stop running, not server anymore

            if (!dirty)
            { //if nothing changed, continue waiting
                yield return updateInterval;
                continue;
            }

            //copy data to a temp array
            Array.Resize<byte>(ref sends, lastWritePos); //resize
            Array.Copy(data, sends, lastWritePos); //copy
            Server.instance.SendNow(ref sends);

            lastWritePos = 2; //start after netcode and id
            dirty = false;
        }
    }

}
*/
#endregion


/// <summary>
/// contains static methods for writing data types such as int8, implement it to define a write function
/// </summary>
public abstract class SyncClass
{
    /* make SyncClass generic?...
    /// <summary>
    /// implement and define how and which variables to access from T using the static functions on SyncClass
    /// </summary>
    public abstract void Write(T variable, ref int lastWritePos, ref byte[] target);
    */

    public static void WriteInt8(ref int variable, ref int lastWritePos, ref byte[] target)
    {
        target[lastWritePos] = (byte)variable;
        lastWritePos += 1;
    }
    public static void WriteInt16(ref int variable, ref int lastWritePos, ref byte[] target)
    {
        target[lastWritePos] = (byte)variable;
        target[lastWritePos + 1] = (byte)(variable >> 8);
        lastWritePos += 2;
    }
    public static void WriteFloat16_To1000th(ref float variable, ref int lastWritePos, ref byte[] target)
    {
        target[lastWritePos] = (byte)(variable * 1000);
        target[lastWritePos + 1] = (byte)((int)(variable * 1000) >> 8);
        lastWritePos += 2;
    }


} //end class SyncClass<T>
