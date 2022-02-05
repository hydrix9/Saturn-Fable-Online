using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// more direct than SyncField<T>, allows you to merely setup the handlers and have more control over the length of field
/// </summary>
public abstract class SyncQueue : SyncField
{

    public SyncQueue(string label, int maxLength, SyncObject parent) : base(label, false, parent)
    {

    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        throw new System.NotImplementedException();
    }

    public override int GetCurrentTotalFieldLength()
    {
        throw new System.NotImplementedException();
    }
    public override int GetWorstTotalFieldLength()
    {
        throw new NotImplementedException();
    }

    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        throw new System.NotImplementedException();
    }

    public void Add()
    {

    }

}
