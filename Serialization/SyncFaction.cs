using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class SyncFaction : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts

    [SerializeField]
    private string startFaction = defaultFaction; //for serializing in editor
    public static string pirates = "Pirate"; //prevent typo

    //codepoint for name of variables stored on SyncObject
    public static readonly string faction = "faction";

    /// <summary>
    /// used anywhere you need to get all the entities in a faction, to, for example, apply a StatMod to all members
    /// </summary>
    static ConcurrentDictionary<int, List<Entity>> factionRosters = new ConcurrentDictionary<int, List<Entity>>();

    public static readonly string docileFaction = "_docile";
    public static readonly string defaultFaction = "default";

    Entity parent;

    #region debug

    public bool buttonSetFaction; //click in editor to set to setFactionDebug
    public string setFactionDebug;
#if UNITY_EDITOR
    private void OnGUI()
    {
        if(buttonSetFaction)
        {
            buttonSetFaction = false;
            parent.Set<string>(SyncFaction.faction, setFactionDebug);
        }

    } //end func ONGUI
#endif
    #endregion


    public override void ServerInit(SyncObject parent)
    {
        base.ServerInit(parent);

        string faction = parent.Get<string>(SyncFaction.faction);
        if (faction != "" && faction != null)
        {
            GameMode.instance.TryAddFaction(faction, false); //make sure we have a reference...
        }
    }

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {

        if (parent != null && parent.e != null)
        {
            //if (parent.e == null)
            //    throw new System.Exception(); //not an entity...
            this.parent = parent.e;
            this.parent.onClean += CleanRefs; //remove from faction roster and stuff
        }

        syncFields = new SyncField[] {
            new SyncStringFixedField(faction, false, startFaction, 32, parent, OnUpdate) //if it is an entity and exists, use OnUpdate which will add it to a faction roster dict
        };

        string initialValue = (syncFields[0] as SyncStringFixedField).value;
        OnUpdate(ref initialValue); //init

    } //end SetDefaults

    int oldFactionID;
    void OnUpdate(ref string newFaction)
    {
        if(Server.isServer)
        {
            if(!GameMode.FactionExists(newFaction))
            { //if just set to a faction that doesn't exist....
                GameMode.instance.TryAddFaction(newFaction, false, null); //add this faction
            }
        }

        if (parent != null && parent.e != null)
        {
            parent.factionID = GameMode.GetFactionID(newFaction); //update ref
            if (parent.factionID <= 0)
                Logger.LogError("unable to find faction id for " + newFaction + " for entity " + parent.e.name);

            if (Server.isServer)
            { //also change team mods
                TryRemoveFromRoster(ref oldFactionID);
                TryAddToRoster(ref parent.factionID);

                //really no good place to put this unless you want to make a new class just to subscribe to this OnUpdate on every entity
                TeamSpellModifier.instance.UpdateTeamMods(parent, oldFactionID);

                oldFactionID = parent.factionID;
            }
        }
    }

    /// <summary>
    /// copy one entity's faction to another
    /// </summary>
    public static void CopyTo(Entity from, Entity to)
    {
        to.Set<string>(faction, from.Get<string>(faction));
    }

    void TryRemoveFromRoster(ref int factionID)
    {
        if(factionRosters.ContainsKey(factionID))
        {
            factionRosters[factionID].Remove(parent);
        }
    }
    void TryAddToRoster(ref int factionID)
    {
        if (!factionRosters.ContainsKey(factionID))
            factionRosters.TryAdd(factionID, new List<Entity>()); //init
        factionRosters[factionID].Add(parent);
    }
    
    /// <summary>
    /// returns true if found, out parameter "returns" contains the found entry
    /// </summary>
    public static bool TryGetFactionMembers(int factionID, out List<Entity> returns)
    {
        return TryGetFactionMembers(ref factionID, out returns);
    }
    /// <summary>
    /// returns true if found, out parameter "returns" contains the found entry
    /// </summary>
    public static bool TryGetFactionMembers(ref int factionID, out List<Entity> returns)
    {
        if(factionRosters.TryGetValue(factionID, out returns))
        {
            returns.RemoveAll(entry => entry == null);
            return true;
        } else
        {
            return false;
        }
    } //end func TryGetFactionMembers


    void CleanRefs(SyncObject owner)
    {
        TryRemoveFromRoster(ref oldFactionID);
    }

} //end class SyncFaction