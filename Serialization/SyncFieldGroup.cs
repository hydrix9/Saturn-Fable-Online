using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a group of multiple SyncFields that get synchronized at the same time, allowing us to only check if one value is dirty in the dirty mask
/// </summary>
public abstract class SyncFieldGroup : SyncField
{

    public readonly SyncField[] fields = new SyncField[0];
    public bool buildOnAny;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="buildOnAny">whether to build when any value is dirty rather than 0</param>
    public SyncFieldGroup(string label, bool buildOnAny, SyncObject parent, char accountFieldCode = default) : base(label, false, parent, accountFieldCode)
    {
        this.buildOnAny = buildOnAny;
        SetDefaults(out fields);
    }

    /// <summary>
    /// simply assign SyncField[] fields since we can't do it constructor with private fields
    /// </summary>
    /// <param name="fields"></param>
    public abstract void SetDefaults(out SyncField[] fields);

    /// <summary>
    /// will only Sync if fields[0].NeedsBuild()
    /// </summary>
    /// <returns></returns>
    public override bool NeedsBuild()
    {
        if(buildOnAny)
        {
            //return true if any needs a build, otherwise return false
            for (int i = 0; i < fields.Length; i++)
                if (fields[i].NeedsBuild())
                    return true;

            return false;
        } else
            return fields[0].NeedsBuild(); //use just the first entry as a dirty flag
    }

    int overrideLastWritePos; //use this to manually control where we write data, otherwise they will all write on top of each other. Can't move lastWritePos because will automatically do it from method calling BuildTo!

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        //overrideLastWritePos = lastWritePos; //use this so we don't have to manually move lastWritePos

        for(int i = 0; i < fields.Length; i++)
        {
            //merely build all fields as one
            fields[i].BuildTo(ref lastWritePos, ref target);
        }
    } //end func BuildTo

    /// <summary>
    /// tallies length of all contained fields
    /// </summary>
    public override int GetCurrentTotalFieldLength()
    {
        int total = 0;
        for (int i = 0; i < fields.Length; i++)
        {
            total += fields[i].GetCurrentTotalFieldLength();
        }
        return total;
    }
    public override int GetWorstTotalFieldLength()
    {
        int total = 0;
        for (int i = 0; i < fields.Length; i++)
        {
            total += fields[i].GetWorstTotalFieldLength();
        }
        return total;
    }

    public override bool MoveReadPosAfterWrite()
    {
        return false; //move the readPos after a .Write manually instead from containing .TryWrite
    }

    public override void Write(ref byte[] data, ref int readPos, ref SyncObject target, Client context)
    {
        for(int i = 0; i < fields.Length; i++)
        {
            fields[i].Write(ref data, ref readPos, ref target, context);
            readPos += fields[i].GetWorstTotalFieldLength(); //move read pos so next value can be written
        }
    }
}
