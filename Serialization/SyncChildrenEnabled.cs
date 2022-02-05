using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SyncChildrenEnabled : SyncField
{
    //let it be known that this entire class worked flawlessly on the first try without any errors


    const bool forcePowerOfEight = true;

    //try lookup keys on this to set the children by name
    Dictionary<string, int> nameToIndex = new Dictionary<string, int>();

    GameObject[] targets;
    int lengthBytes = 0; //how many bytes to sync
    byte[] build;
    readonly bool onResetSetActive; //whether to show all targets when this component is reset
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="getTargets"></param>
    /// <param name="onResetSetActive">whether to enable all targets when reset</param>
    public SyncChildrenEnabled(string label, SyncObject parent, Func<SyncObject, GameObject[]> getTargets, bool _onResetSetActive, Dictionary<string, int> _nameToIndex = default) : base(label, false, parent)
    {
        onResetSetActive = _onResetSetActive;

        if(parent != null)
        {
            targets = getTargets(parent);

            if (_nameToIndex == default)
                nameToIndex = new Dictionary<string, int>();
            else
                this.nameToIndex = _nameToIndex;
            
            if(forcePowerOfEight)
            {
                int numDeleted = targets.Length % 8;
                for (int i = 1; i <= numDeleted; i++)
                    GameObject.Destroy(targets[targets.Length - i]);
                Array.Resize(ref targets, targets.Length - numDeleted);
            }

            if (targets.Length == 0)
            {
                Logger.LogError("didn't find any children to sync");
                //if you do implement syncing with no targets... need to go back and ensure no index errors occur
            }

            lengthBytes = Mathf.CeilToInt(targets.Length / 8f); //init to number bytes needed

            if (lengthBytes > GetWorstTotalFieldLength())
                Logger.LogError(lengthBytes + " > " + GetWorstTotalFieldLength()); //need to increase GetWorstTotalFieldLength or otherwise will cause an error

            //build = new byte[lengthBytes].Select(entry => byte.MaxValue).ToArray(); //init with all 1s ///don't do this anymore, wasn't proper for things that initialize inactive... instead we write a 1 to build[] during init of each child during InitChild
            build = new byte[lengthBytes];
        }

    } //end constructor

    /// <summary>
    /// manual control over NeedsBuild
    /// </summary>
    /// <param name="index">self index in this.targets</param>
    /// <param name="byteNum">what byte to write to</param>
    /// <param name="bitWritten">what bit to write (should be either </param>
    /// <param name="state">what state to set the child to</param>
    public void SetChildActive(ref int index, ref int byteNum, ref byte bitWritten, bool state)
    {
        needsBuild = true;
        targets[index].SetActive(state);

        //will flip off if it is on, or on if it is off
        //001 ^ 001 = 000
        //000 ^ 001 = 001
        //if this is too glitchy, you can change it to do (state) ? | bitWritten : ^ bitWritten ( |= if it is flipping on, ^= if it is flipping off)
        build[byteNum] ^= bitWritten; //logical XOR operator

    } //end SetChildActive

    public void TrySetFrom(string name, bool value)
    {
        int index;
        if (nameToIndex.TryGetValue(name, out index))
        {
            int byteNum = Mathf.FloorToInt(index / 8f);
            targets[index].SetActive(value);
            build[byteNum] ^= (byte)Math.Pow(2, ((index + 1) % 8 == 0 ? 8 : (index + 1) % 8) - 1); //write respective bit to byteNum based on PoT from index
        }
    } //end func TrySetFrom

    /// <summary>
    /// inform child of what they will write using SetChildActive
    /// </summary>
    public bool InitChild(GameObject target, out int index, out int byteNum, out byte byteWritten)
    {
        if(Array.IndexOf(targets, target) < 0)
        { //this object doesn't have a reference to us, the getTargets function during constructor must not have found this object properly
          //throw new KeyNotFoundException();
            //also possible object is trying to destroy iteself because doesn't fit in the power of 8, so return false instead of throwing an error
            index = 0;
            byteNum = 0;
            byteWritten = 0;
            return false;
        } //sanity

        index = Array.IndexOf(targets, target);
        byteNum = Mathf.FloorToInt(index / 8f); //calc byte they will write to. Use float to allow decimals, then round down to get byte num

        int bitNum; //temp

        if ((index + 1) % 8 == 0) //if it is the last bit (multiple of 8), set to 8 instead of 0 (since 8 % 8 == 0)...
            bitNum = 8; //just set it to 8
        else
            bitNum = (index + 1) % 8; //% = remainder or "modulo" operator, resulting in 1-7

        byteWritten = (byte)Math.Pow(2, bitNum - 1); //remainder "bitNum" is <= 8.... therefore any power of two will be <= 128 (1, 2, 4, 8, 16, 32, 64, 128), giving us a power of two which is always 1 bit in binary. Should subtract one to start on 1 not 2

        if(target.activeSelf) //if this child is initialized active
            build[byteNum] |= byteWritten; //logical OR operator, flip the associated bit in the mask (build[] is initialized to all 0s).. this will set associated bit to 1


        //Logger.Log("index " + index + " gives byteNum " + byteNum + " which writes bit " + byteWritten);
        return true;
    } //end InitChild

    public GameObject[] GetTargets()
    { //more explicit than exposing private field
        return targets;
    }

    public override void BuildTo(ref int lastWritePos, ref byte[] target)
    {
        Array.Copy(build, 0, target, lastWritePos, lengthBytes); //write data to buffer
        lastWritePos += lengthBytes;
        needsBuild = false;
    } //end BuildTo
    
    public override int GetCurrentTotalFieldLength()
    {
        return lengthBytes;
    }

    public override int GetWorstTotalFieldLength()
    {
        return 500; //set arbitrarily 4000/8=500 ... need to implement using GetCurrentTotalFieldLength or read from inspector to do otherwise
    }

    bool needsBuild;
    public override bool NeedsBuild()
    {
        return needsBuild;
    }

    public override void ResetToDefaultValue()
    {
        if(onResetSetActive)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].SetActive(true);
                build = new byte[lengthBytes].Select(entry => byte.MaxValue).ToArray(); //init with all 1s
            }
        }
    } //end func ResetToDefaultValue

    public override void Write(ref byte[] data, ref int readPos, ref SyncObject target, Client context)
    {
        //ERROR- will return an index error if the length of targets isn't a multiple of 8. you need to add some separate structure to handle the remainder if you want to do it this way...
        byte readByte;
        int index = 0; //index in array of targets
        for(int i = 0; i < lengthBytes; i++)
        { //loop through each byte of self, and set all
            index = i * 8; //byte corresponds to 8 entries each
            readByte = data[readPos + i]; //cache

            targets[index].SetActive((readByte & 1) == 1);
            targets[index + 1].SetActive((readByte & 2) == 2);
            targets[index + 2].SetActive((readByte & 4) == 4);
            targets[index + 3].SetActive((readByte & 8) == 8);
            targets[index + 4].SetActive((readByte & 16) == 16);
            targets[index + 5].SetActive((readByte & 32) == 32);
            targets[index + 6].SetActive((readByte & 64) == 64);
            targets[index + 7].SetActive((readByte & 128) == 128);
        }
        //readPos += lengthBytes;

    } //end Write

} //end class SyncAgarioDots