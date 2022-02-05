using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

//SyncObject is basically a container for id, type, health, maxHealth, convenience variables, and serialization with these variables. Classes like Entity inherit and extend the serialization functionality by adding raw on top of it and expecting the data to be there.
//This differs from MovementSync which contains serialization functions for movement, and classes that extend from this define CSP or possibly different movement and input behaviours. This gives us a wide range of object type/ movement type possibilities
[RequireComponent(typeof(NetworkMovement3D))]
public class SyncObject : MonoBehaviour
{


    public bool isStatic = false;
    public bool canMove = true;

    /// <summary>
    /// whether this should persist and be synchronized to clients regardless of where they are
    /// </summary>
    public bool godObject = false;

    public int id;


    public int type; //this is [very conveniently] generated at runtime from the list you made in inspector on WorldFunctions

    /// <summary>
    /// false on server if controlled by server, false on client if controlled by client
    /// </summary>
    public bool remoteControlled = true;

    public RenderInstance renderInstance;
    private Entity _e;
    public Entity e => destroyed ? null : _e; //force .e to be null if this is in the process of being destroyed
    public Player p; //reference to player if it is one
    public Summoned summoned; //set on Summoned.Awake()
    public bool isSummoned => summoned != null; //set on Summoned.Awake()

    public bool isPlayer => p != null;

    public MovementSync ms;
    public NetworkMovement nm;

    //checks if we compiled for this update and sends cached version, or else builds it
    protected byte[] latestBuild;


    protected int lastWritePos = 1;

    protected int lastUpdate;


    public delegate void OnUpdate();
    public event OnUpdate onUpdate;

    public delegate void OnClean(SyncObject self);
    public event OnClean onClean; //called at death, removes references

    //[HideInInspector]
    public SyncField[] syncFields = new SyncField[0]; //classes that contain fields that need to be serialized

    private Dictionary<string, SyncField> syncFieldsTyped = new Dictionary<string, SyncField>(); //store and retrieve values by name with value being a generic SyncField type

    public int fieldsTotalLengthUpdates; //SyncFields total length + baseFieldsLength
    public int syncFieldsTotalLength;
    public const int baseFieldLength = 16; //array length from id, position, and base data. 16 after id(2) pos(12) rotation (2)
    public const int baseNumFields = 2; //how many fields are taking up a bit in the dirty mask. right now 2 after pos and rotation
    public const int dirtyMaskStartPos = 2; //starts after 0-1 of id
    public static int dirtyMaskEnd; //where data starts, cached to save some calculations in tight loops


    protected virtual void Start()
    {
        if(id < 1)
        { //ID should never be zero, and is set before Start() (which is called on first frame) from this.Init() during NetworkIO.AddEntity which is implemented on Client and Server
            Logger.LogError(name + " was not instantiated properly from AddEntity");
        }
    }

    public void AddField(string key, SyncField value)
    {
        //Logger.Log("adding field " + key + " to " + name);
        syncFieldsTyped.Add(key, value);
    }

    //get and set generic synced types. Basically just handles the boxing and unboxing with a neat API
    /// <summary>
    /// read a SyncValue key. Highly recommended to use a "public static readonly string" instead of a string literal, to maintain a codepoint
    /// </summary>
    public T Get<T>(string key)
    {
        return syncFieldsTyped[key] as SyncField<T>;
    }
    public void Set<T>(string key, T value) {

        if (syncFieldsTyped.ContainsKey(key))
            (syncFieldsTyped[key] as SyncField<T>).value = value;
        else
            throw new NotImplementedException(name + " does not contain property " + key + " of type " + typeof(T));
    }

    public T GetOrDefault<T>(string key)
    {

        if (syncFieldsTyped.ContainsKey(key))
        {
            return syncFieldsTyped[key] as SyncField<T>;
        }
        else
            return default; //return default null version of T
    }

    /// <summary>
    /// return a syncfield object to subscribe to updates or otherwise from a SyncComponent on this entity
    /// </summary>
    /// <typeparam name="T">type of field it is (int, string, float, int[], etc)</typeparam>
    /// <param name="key">name of the field in the SyncField class</param>
    /// <returns></returns>
    public SyncField<T> GetSyncField<T>(string key)
    {
        return syncFieldsTyped[key] as SyncField<T>;
    }

    public void Addition<T>(string key, T value)
    {
        (syncFieldsTyped[key] as SyncField<T>).Addition(value);
    }
    public void Subtraction<T>(string key, T value)
    {
        (syncFieldsTyped[key] as SyncField<T>).Subtraction(value);
    }
    public void Multiply<T>(string key, T value)
    {
        (syncFieldsTyped[key] as SyncField<T>).Multiply(value);
    }
    public void Addition<T>(string key, ref T value)
    {
        (syncFieldsTyped[key] as SyncField<T>).Addition(ref value);
    }
    public void Subtraction<T>(string key, ref T value)
    {
        (syncFieldsTyped[key] as SyncField<T>).Subtraction(ref value);
    }
    public void Multiply<T>(string key, ref T value)
    {
        (syncFieldsTyped[key] as SyncField<T>).Multiply(ref value);
    }
    public void Divide<T>(string key, ref T value)
    {
        if (syncFieldsTyped.ContainsKey(key))
            (syncFieldsTyped[key] as SyncField<T>).Divide(ref value);
    }
    public void Addition_Clamped<T>(string key, ref T value, T min, T max)
    {
        (syncFieldsTyped[key] as SyncField<T>).Addition_Clamped(ref value, ref min, ref max);
    }
    public void Subtraction_Clamped<T>(string key, ref T value, T min, T max)
    {
        (syncFieldsTyped[key] as SyncField<T>).Subtraction_Clamped(ref value, ref min, ref max);
    }
    public void Multiply_Clamped<T>(string key, ref T value, T min, T max)
    {
        (syncFieldsTyped[key] as SyncField<T>).Multiply_Clamped(ref value, ref min, ref max);
    }
    public void Divide_Clamped<T>(string key, ref T value, T min, T max)
    {
        if (syncFieldsTyped.ContainsKey(key))
            (syncFieldsTyped[key] as SyncField<T>).Divide_Clamped(ref value, ref min, ref max);
    }

    public void TryAddition<T>(string key, T value)
    {
        if (syncFieldsTyped.ContainsKey(key))
            (syncFieldsTyped[key] as SyncField<T>).Addition(value);
    }
    public void TrySubtraction<T>(string key, T value)
    {
        if (syncFieldsTyped.ContainsKey(key))
            (syncFieldsTyped[key] as SyncField<T>).Subtraction(value);

    }
    public void TryMultiply<T>(string key, T value)
    {
        if (syncFieldsTyped.ContainsKey(key))
            (syncFieldsTyped[key] as SyncField<T>).Multiply(value);
    }
    public void TryDivide<T>(string key, T value)
    {
        if (syncFieldsTyped.ContainsKey(key))
            (syncFieldsTyped[key] as SyncField<T>).Divide(value);
    }
    public bool ContainsSyncField(string key)
    {
        return syncFieldsTyped.ContainsKey(key);
    }

    public virtual void Init_Refs_p1(NetworkIO context)
    {
        _e = this as Entity; //convenience variable... easier than casting
        ms = GetComponent<MovementSync>();
        nm = GetComponent<NetworkMovement>();
        if (nm == null)
            Logger.LogError("NetworkMovement not found on " + name + " during init");
    }

    public virtual void ServerInit_Refs_p2(NetworkIO context)
    {

    }

    public virtual void Init_Calculations_p5(NetworkIO context)
    {

    }


    public virtual void ServerInit_Synch_p4(NetworkIO context)
    {

        //buildInitSize = metaDataPos.totalLengthSyncObject; //total length of all fields

        renderInstance = Server.instance.AddRenderInstance(); //add a reference on the server that won't dissappear on destroy- it remains on the Server.instance class
        renderInstance.obj = this;

        if ((this as Entity) != null && (this as Entity).p != null) //if this class inherits from player...
            renderInstance.isPlayer = true;

        //register into tree, causing us to get data on entities surrounding us (and them, us), but do so after two frames to allow final position calculations to happen
        //otherwise could potentially register into block 0,0,0 or something
        Ext.CallDelayed((int)(Time.deltaTime * 2 * 1000), () =>
        {
            if(this != null && gameObject != null && renderInstance != null) //if still exists after one frame..
                renderInstance.ReRegister(); //add into tree first time. This will register us into a "renderedEntities" list in our local block, which is a cache used to gather who to send data to.
        });

        //ServerInitSyncFields();


        new byte[] { (byte)id, (byte)(id >> 8) }.CopyTo(latestBuild, 0); //initialize completely static parts of array (id)

    }

    /*
    /// <summary>
    /// called on server init. Adds all SyncComponents and decides on a dirty mask length
    /// </summary>
    void ServerInitSyncFields()
    {

    }
    */

    /// <summary>
    /// initializes synchronized fields into dictionary so we can Get() and Set()
    /// </summary>
    public void Init_SyncFields_p3(NetworkIO context)
    {
        Logger.Log("initalizing sync fields on " + name);
        SyncComponent[] syncComponents = GetComponents<SyncComponent>();
        for (int i = 0; i < syncComponents.Length; i++)
        {
            syncComponents[i].Init(this, context);

            syncComponents[i].ServerInit(this); //initializes all its fields
            syncFieldsTotalLength += SyncComponent.totalFieldsLengths[syncComponents[i].GetType()]; //cache so we can allocate array
        }

        fieldsTotalLengthUpdates = syncFieldsTotalLength + baseFieldLength; //also include base data like id, position, and rotation. Need this on both client and server so we can understand length of fields in metadata
        latestBuild = new byte[fieldsTotalLengthUpdates + SyncField.dirtyMaskLength];
    }

    //values to check dirty mask. Protected so as to inherit without "public"
    protected Vector3 oldPos = new Vector3(float.MinValue, float.MinValue, float.MinValue); //this will always be dirty on first frame
    protected Quaternion oldRotation = Quaternion.Euler(float.MinValue, float.MinValue, float.MinValue);
    protected int oldHealth = int.MinValue;
    protected int oldMaxHealth = int.MinValue;

    public virtual void BuildData()
    {
        //Ext.RunTaskThread(() =>
        //{
            lastUpdate = Server.updateNum; //used to check cache time

            for (int i = dirtyMaskStartPos; i < dirtyMaskEnd; i++) //start after id
                latestBuild[i] = 0; //clear the dirty mask
            lastWritePos = dirtyMaskEnd; //set to after 2 bytes of id and dirty mask


            //check freshness of all variables, writing to the buffer all changes. Then, this buffer is simply copied directly to a buffer about to be sent to clients which is resized first.

            if (!transform.rotation.Equals(oldRotation))
            {
                latestBuild[dirtyMaskStartPos] |= 2; //write bit to dirty mask
                ms.BuildRotation();

                latestBuild[lastWritePos] = ms.rotation[0]; //build meta data
                latestBuild[lastWritePos + 1] = ms.rotation[1]; //build meta data

                lastWritePos += 2;
                oldRotation.w = transform.rotation.w;
                oldRotation.x = transform.rotation.x;
                oldRotation.y = transform.rotation.y;
                oldRotation.z = transform.rotation.z;

            } //end if new rot
            if (canMove)
            {
                if (!oldPos.Equals(transform.position))
                {
                    
                    if ((latestBuild[dirtyMaskStartPos] & 2) != 2) //if didn't include rotation, tack it on now regardless of no changes
                    {
                        latestBuild[lastWritePos] = ms.rotation[0]; //build meta data
                        latestBuild[lastWritePos + 1] = ms.rotation[1]; //build meta data
                        latestBuild[dirtyMaskStartPos] |= 2; //write bit to dirty mask

                        lastWritePos += 2;
                    }
                    

                    latestBuild[dirtyMaskStartPos] |= 1; //write bit
                    ms.GetPosCode().CopyTo(latestBuild, lastWritePos);
                    lastWritePos += 12;
                    oldPos.x = transform.position.x; //have to set all three because Vector3 is a reference type
                    oldPos.y = transform.position.y;
                    oldPos.z = transform.position.z;

                    //oldPos.x = ms.position.x;
                    // oldPos.y = ms.position.y;
                    // oldPos.z = ms.position.z;
                } //end if new pos
            } //end if can move

            for (int i = 0; i < syncFields.Length; i++)
            {
                syncFields[i].TryBuildTo(ref lastWritePos, ref latestBuild);
            }
        //});
    } //end func BuildData

    bool buildDataThisFrame;

    //can be optimized to set a single boolean value to true every time a value is set?
    /// <summary>
    /// whether the dirty mask is clean
    /// </summary>
    public bool IsClean()
    {
        for(int i = dirtyMaskStartPos; i < dirtyMaskEnd; i++)
        {
            if (latestBuild[i] != 0)
            {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// prepare to write data by resizing the array and building data
    /// </summary>
    public void PrepBuild(ref int bufferSize)
    {

        if (id == 0)
        {
            Logger.LogWarning("trying to build data on uninitialized id... skipping");
            return;
        }

        if (destroyed)
            return; //don't send any data on destroyed entities...

        if (lastUpdate != Server.updateNum) //make sure have data to write and not cached
            BuildData(); //just build, don't write

        if (IsClean())
        {
            buildDataThisFrame = false;
            return;
        }
        else
            buildDataThisFrame = true;

        bufferSize += lastWritePos; //inform of new size after including our build
    }

    //not overwritten ATM but that could change

    /// <summary>
    /// adds our data into a byte array that's about to be sent. This method is called from Server update function to write data into that buffer without allocating new buffers. Server builds block when needed, block calls this function on each object
    /// </summary>
    /// <param name=""></param>
    public virtual void BuildDataTo(byte[] updatebytes, ref int writePos)
    {
        if (buildDataThisFrame)
        {
            int _writePos = writePos; //thread can't use ref param
            //Ext.RunTaskThread(() =>
            //{
                Array.Copy(latestBuild, 0, updatebytes, _writePos, lastWritePos); //fast copy partial array. Buffer.BlockCopy faster?...
            //});
            writePos += lastWritePos; //move write pos for data written
        }
    }

    /// <summary>
    /// builds all stats regardless of dirty flags including metadata like name and guild name
    /// </summary>
    /// <param name=""></param>
    public virtual void BuildFullDataTo(byte[] updatebytes, ref int writePos)
    {

        if (id == 0)
        {
            Logger.LogWarning("trying to build full data on uninitialized id... skipping");
            return;
        }

        int _writePos = writePos; //tasks can't use ref param
        //Ext.RunTaskThread(() =>
        //{
            //Logger.Log("start writing at " + updatebytes[updatebytes.Length - 4] + " " + updatebytes[updatebytes.Length - 3] + " " + updatebytes[updatebytes.Length - 2] + " " + updatebytes[updatebytes.Length - 1]);
            //Logger.Log(name + " writing " + (syncFieldsTotalLength + metaDataPos.totalLengthSyncObject) + " from " + writePos + " to " + (writePos + syncFieldsTotalLength + metaDataPos.totalLengthSyncObject));

            updatebytes[_writePos + metaDataPos.id] = (byte)(id);
            updatebytes[_writePos + metaDataPos.id + 1] = (byte)(id >> 8);
            updatebytes[_writePos + metaDataPos.type] = (byte)(type); //include type id, client will instantiate the object from this
            updatebytes[_writePos + metaDataPos.type + 1] = (byte)(type >> 8);

            ms.BuildRotation();
            updatebytes[_writePos + 4] = ms.rotation[0]; //build meta data
            updatebytes[_writePos + 5] = ms.rotation[1]; //build meta data
            ms.GetPosCode().CopyTo(updatebytes, _writePos + 6);

            _writePos += metaDataPos.totalLengthSyncObject; //move past initial metadata

            int prevWrite = _writePos;
            for (int i = 0; i < syncFields.Length; i++)
            {
                syncFields[i].BuildTo(ref _writePos, ref updatebytes); //forcibly build to without checking if its dirty
                _writePos = prevWrite + syncFields[i].GetWorstTotalFieldLength(); //manually move write pos to after worst case length
                prevWrite = _writePos;
            }
        //});

        writePos = _writePos; //set write pos to calling function

        //Logger.Log("wrote until " + writePos);

        //  Ext.DebugByte(metaData, "adding meta: ");
        //Logger.Log("ending writing at " + updatebytes[updatebytes.Length - 4] + " " + updatebytes[updatebytes.Length - 3] + " " + updatebytes[updatebytes.Length - 2] + " " + updatebytes[updatebytes.Length - 1]);

        //Logger.Log("after length: " + updatebytes.Length);
        //Ext.DebugByte(updatebytes, "after BuildFullData:");
    }


    CoroutineHandle deathRoutine; //what allows 0.5f before death. Prevents double call
    /// <summary>
    /// simply destroys object, allowing time to sync 0 health. Healing and Attacking should be locked, so no danger
    /// </summary>
    public virtual void Death(spellHit killingBlow = null)
    {
        /*
        //a player's death should do something else, override this function, and make sure they can be resurrected instantly without bugs from timing
        if(deathRoutine != default(CoroutineHandle))
            deathRoutine = Timing.CallDelayed(0.5f, Destroy);
        */
    }


    public bool destroyed = false;
    /// <summary>
    /// clean self destruct
    /// </summary>
    public virtual void Destroy()
    {
        destroyed = true;
        CleanRefs();

        if (gameObject != null)
            WorldFunctions.ReclaimEntity(this);
    } //end func Destroy

    public virtual void DestroyDelayed(int ms)
    {
        destroyed = true;
        Ext.CallDelayed(ms, ()=> {
            if(gameObject != null)
                this.Destroy();
            });
    } //end func DestroyDelayed

    /// <summary>
    /// removes things like event listeners
    /// </summary>
    protected virtual void CleanRefs()
    {
        destroyed = true;
        if (renderInstance != null)
        {
            if (Server.isServer)
            {
                Server.instance.SendDestroyedObject(renderInstance); //send all players that this object is destroyed
                Server.instance.RemoveEntityRef(ref id);
            } else
            {
                //client should receive a packet from Server soon to destroy it...
            }
            renderInstance.Clear();
        }
        if (onClean != null)
            onClean(this);
        renderInstance = null;
        onUpdate = null;
        //wait for end of frame to clear SyncFields
        Ext.CallDelayed((int)(Time.deltaTime * 1000), () =>
        {
            if(syncFieldsTyped != null)
                syncFieldsTyped.Clear();
            syncFields = new SyncField[0]; //need to clear without making null just yet
        });

        onClean = null;
    }

    public void TriggerOnUpdate()
    {
        if (onUpdate != null)
            onUpdate();
    }


}
