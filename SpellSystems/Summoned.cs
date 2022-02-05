using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

/// <summary>
/// base class for summoned entities with behaviours like a totem, mine, or sentry. This common base allows us to register on the caster and assign properties genericically through spells using their talents, like for example setting max HP of the summoned
/// </summary>
public class Summoned : MonoBehaviour
{
    public bool isTurret => spellHit != null && spellHit.spellTypes.Contains(spellType.sentry_turret);
    public bool isMinion => spellHit != null && spellHit.spellTypes.Contains(spellType.minion);


    public spellHit spellHit;
    public SyncObject self;

    private void Awake()
    {
        if (self == null)
            self = GetComponent<SyncObject>();

        if (self != null)
        {
            self.summoned = this; //set ref

            if (self.e != null)
            { //if is entity
                self.e.onDeath += OnDeath; //on our own 
            }
        }
    } //end func Awake

    // Start is called before the first frame update
    void Start()
    {
        if(spellHit.caster == null || spellHit.caster.destroyed)
        { //if somehow caster doesn't exist or was destroyed during .Awake or something
            Debug.LogWarning("caster didn't exist or was destroyed during Summoned.Awake");
            //exit
            DestroyClean(self);
            return;
        }

        spellHit.caster.onDeath += OnMasterDeath;
        spellHit.caster.onClean += OnMasterClean;

        if (self.e != null)
        { //if is entity
            SyncFaction.CopyTo(spellHit.caster, self.e); //copy caster faction to us

            //subscribe to changes to owner's faction
            spellHit.caster.GetSyncField<string>(SyncFaction.faction).onUpdate += OnMasterFaction;

            self.Set<int>(SyncHealth.health, self.Get<int>(SyncHealth.maxHealth)); //give max health after talent mods
        }
        self.onClean += Clean;
        spellHit.caster.GetComponent<SyncSummons>().Add(this); //do this here because it will run on both client and server... this will also call OnAddSummoned

        if(spellHit.duration > 0)
            Timing.CallDelayed(spellHit.duration, TryExpire);
    } //end func Start

    void OnMasterFaction(ref string newValue)
    {
        if (self == null)
            return;

        self.Set<string>(SyncFaction.faction, newValue); //copy caster faction to us
    } //end func OnMasterFaction

    public virtual void Set(spellHit spellHit, bool destroyOnCasterDeath)
    {
        this.spellHit = spellHit;

        if (self == null)
            self = GetComponent<SyncObject>();
        if(self != null && self.e != null)
            this.spellHit.target = self.e;

        if (destroyOnCasterDeath)
            spellHit.caster.onDeath += OnMasterDeath;
    }

    void TryExpire()
    {
        //may want to check duration again or something?...
        if (self != null)
        {
            DestroyClean(self);
        }
    }

    //called when owner dies
    void OnMasterDeath(Entity master, spellHit killingBlow = null)
    {

        //look at cast spell types to see if we should also die on master death
        if(spellHit != null)
            for (int i = 0; i < spellHit.spellTypes.Count; i++)
            {
                if (spellHit.spellTypes[i] == spellType.minion)
                {
                    self.Death(); //kill self
                    break;
                }
            }
    } //end func OnMasterDeath

    //called when owner gets destroyed and removed from scene
    void OnMasterClean(SyncObject cleaned)
    {
        if (self != null)
            self.Destroy(); //clean self destruct
        /* too dangerous to do this manually, especially if we ever intend to use object pools for Entities
        else
            GameObject.Destroy(gameObject); //not init yet or something, so just have to destroy self
        */
    } //end func OnMasterDeath


    //called on our own death
    void OnDeath(Entity self, spellHit killingBlow = null)
    {
        //Debug.LogWarning(self + " died");
        DestroyClean(self); //GameMode won't destroy us automatically... probably only destroys mobs spawned from MonsterSpawner or something... and players may get sent back to spawn, not destroyed
    }

    protected void DestroyClean(SyncObject self)
    {
        Clean(self);
        if(self != null)
            self.Destroy();
    }


    //remove refs
    protected virtual void Clean(SyncObject owner)
    {
        if (spellHit != null && spellHit.caster != null)
        {
            if(self.e != null)
            {
                spellHit.caster.GetSyncField<string>(SyncFaction.faction).onUpdate -= OnMasterFaction; //unsubscribe
            }

            spellHit.caster.onDeath -= OnMasterDeath; //unsubscribe
        }
            if (self.e != null)
            self.e.onDeath -= OnDeath; //unsub
        if(self != null)
            self.onClean -= Clean; //unsub
        spellHit = null;
    }
}
