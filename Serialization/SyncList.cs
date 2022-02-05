using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SyncList<T> : SyncField
{
    public List<T> unsent = new List<T>(); //data that still needs to be serialized. Cleared when sent

    public SyncList(string label, SyncObject parent = null, char accountFieldCode = default) : base(label, false, parent, accountFieldCode)
    {

    }

    /// <summary>
    /// define how to serialize object T using static functions from SyncClass or doing it yourself. Used when iterating unsent entries for serialization
    /// </summary>
    public abstract void BuildEntry(T entry, ref int lastWritePos, ref byte[] target);


    //public abstract void WriteEntry();

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        //build all unsent entries of type T
        for (int i = 0; i < unsent.Count; i++)
            BuildEntry(unsent[i], ref lastWritePos, ref target);
    }


    public override void Write(ref byte[] data, ref int readPos, ref SyncObject target, Client context)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// whether we have data to sync
    /// </summary>
    public override bool NeedsBuild()
    {
        return unsent.Count > 0;
    }

    /// <summary>
    /// add  a new object of type T to be serialized
    /// </summary>
    public void Add(T obj)
    {

    }


    public override void Destroy()
    {
        base.Destroy();
        unsent.Clear();
    }

} //end class SyncField
