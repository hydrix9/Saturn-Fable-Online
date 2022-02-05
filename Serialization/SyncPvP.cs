using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// contains pvp reward multipliers
/// </summary>
public class SyncPvP : SyncComponent
{
    public struct RecentKill
    {
        public RecentKill(int killer, int killed) : this()
        {
            this.killer = killer;
            this.killed = killed;
            utcDate = DateTime.UtcNow;
            init = true;
        } //ent ctor
        public bool init;
        public int killer;
        public int killed;
        public DateTime utcDate; //from DateTime.UtcNow on creation
    } //end struct RecentKill

    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts

    public const float maxPvpMultiplier = 2; //for serializing in editor
    public const float maxArenaMultiplier = 2;

    //codepoint constants
    public static readonly string
        pvpMultiplier = "pvpMultiplier",
        arenaMultiplier = "arenaMultiplier";

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncFields = new SyncField[]
        {
            new SyncFloat16Field_1024(pvpMultiplier, false, maxPvpMultiplier, parent),
            new SyncFloat16Field_1024(arenaMultiplier, false, maxArenaMultiplier, parent)
        };
    } //end func SetDefaults

} //end func SyncPvP