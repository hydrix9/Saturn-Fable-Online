using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// gives you manual control of how to handle the data while still registering on the dirty mask schema
/// </summary>
public class SyncBytes : SyncField
{

    public SyncBytes(string label, SyncObject parent, char accountFieldCode = default) : base(label, false, parent, accountFieldCode)
    {

    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {

    }

    public override int GetCurrentTotalFieldLength()
    {
        throw new System.NotImplementedException();
    }
    public override int GetWorstTotalFieldLength()
    {
        throw new System.NotImplementedException();
    }

    public override bool NeedsBuild()
    {
        throw new NotImplementedException();
    }

    public override void Write(ref byte[] data, ref int startPos, ref SyncObject target, Client context)
    {
        throw new System.NotImplementedException();
    }

    public override void ResetToDefaultValue()
    {
        throw new NotImplementedException();
    }
} //end class SyncBytes
