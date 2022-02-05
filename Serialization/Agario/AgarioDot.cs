using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AgarioDot : MonoBehaviour
{

    public int expValue = 1;

    public string soundEffect;
    public string soundEffectChannel; //where to play it 
    public static PitchAnimate.Params soundVariance = new PitchAnimate.Params(1, 1, 1, 1, 1, 0.06f, 0.06f, 0, false, 0.1f);
    private int selfIndex;
    private int selfByteNum;
    private byte selfByteWritten;
    private SyncChildrenEnabled syncDots;

    //private static byte nullBit = 0; //what gets written to indicate inactive

    public DateTime lastSetDirty = DateTime.UtcNow;

    public bool interactPlayersOnly = true; //whether only players can collect this dot or all entities
    public int delayCollisionAfterBackOnMS = -1; //whether to give a slight "fairness" delay to possibly harmful agario dots after turning them back on
    
    private void Awake()
    {
        //only trigger collisions if on server
        //if (Server.isServer)
            SetCollisionLayer();
        //else
        //{
            //client would want to hide these to prevent them from hiding, but it is fairly determinstic and we can "fake" hiding it client side for now...
            //gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        //}
    }

    private void Start()
    {
        syncDots = GetComponentInParent<SyncAgario>().GetDotsSyncField(); //init
        //try init
        if (!syncDots.InitChild(gameObject, out selfIndex, out selfByteNum, out selfByteWritten))
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        TrySetFrom(other.GetComponent<Entity>());
    }

    //set the dot when triggered
    void TrySetFrom(Entity entity)
    {
        if (entity == null)
            return;

        if (Server.isServer)
        {
            if (interactPlayersOnly && !entity.isPlayer) //if specified to only work with players and not a player
                return;

            //entity.TryAddition<int>(SyncLevel.experience, expValue);
            ExpConfig.instance.TryAddExp(entity, expValue);

            if (soundEffect != "" && entity.isPlayer && entity.p.isLocal) //only play sounds for local players...
                WorldFunctions.PlaySoundEffectOneShot(entity, ref soundEffect, soundVariance, soundEffectChannel);
            lastSetDirty = DateTime.UtcNow;
        }

        SetActive(false);
    } //end TrySetFrom

    public void SetActive(bool state)
    {
        if (syncDots != null)
        {
            if(delayCollisionAfterBackOnMS > 0 && state)
            { //if configured to do so and turning back on...
                gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); //disable collisions
                Ext.CallDelayed(delayCollisionAfterBackOnMS, SetCollisionLayer);
            }

            syncDots.SetChildActive(ref selfIndex, ref selfByteNum, ref selfByteWritten, state);
        }
    } //end SetActive

    void SetCollisionLayer()
    {
        if(this != null && gameObject != null)
            gameObject.layer = LayerMask.NameToLayer(WorldFunctions.entityInteractionLayerName);
    }

} //end class AgarioDot