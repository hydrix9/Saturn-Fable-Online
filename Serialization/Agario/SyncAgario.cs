using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SyncAgario : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => true; //reset all SyncFields and therefore to true on restart

    public const string agarioDots = "Dots";

    private SyncChildrenEnabled syncDots;

    public float dotHideDuration = 30f;

    public float checkDotsInterval = 1.5f;
    float checkDotstimer;

    //transforms that contain dots as children
    public Transform[] dotAreas;

    AgarioDot[] dots;
    private void FixedUpdate()
    {
        if (Server.isServer)
        {
            checkDotstimer -= Time.fixedDeltaTime;
            if (checkDotstimer <= 0)
            {
                checkDotstimer = checkDotsInterval;
                if (dots.Length > 0)
                    CheckDots();
            }
        }
    } //end FixedUpdate

    DateTime checkTime;
    //re-enable dots if they have been inactive for long enough
    AgarioDot checkDotsTemp;
    void CheckDots()
    {
        checkTime = DateTime.UtcNow;
        for (int i = 0; i < dots.Length; i++) {
            //if it isn't active and duration has expired, re-enable
            checkDotsTemp = dots[i];
            if (checkDotsTemp != null && !checkDotsTemp.gameObject.activeSelf && checkTime.Subtract(checkDotsTemp.lastSetDirty).TotalSeconds >= dotHideDuration)
            {
                checkDotsTemp.SetActive(true); //re-enable and sync
            }
        }

    } //end CheckDots

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        syncDots = new SyncChildrenEnabled(agarioDots, parent, GetDots, true); //init

        syncFields = new SyncField[]
        {
            syncDots
        };

        if(parent != null)
        {
            dots = syncDots.GetTargets().Select(entry => entry.GetComponent<AgarioDot>()).ToArray(); //should have been set during constructor with GetDots
        }
    } //end class SetDefaults

    public SyncChildrenEnabled GetDotsSyncField()
    { //safer/ more explicit than just exposing private field
        return syncDots;
    }

    //get dots from target to supply to SyncField, used as parameter func during SyncChildrenEnabled constructor
    GameObject[] GetDots(SyncObject parent)
    {
        //get all dots in all dotAreas
        return dotAreas.Select(entry => entry.GetComponentsInChildren<AgarioDot>(true)).SelectMany(i => i).Select(entry => entry.gameObject).ToArray();
    }

} //end class SyncAgario