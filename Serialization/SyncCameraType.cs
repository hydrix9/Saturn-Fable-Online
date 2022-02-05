using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SyncCameraType : SerializedPrefab_ByID
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts

    public Transform buildPosOffset; //where to build things...
    public new Camera camera;

    public override void Constructor(out string addressablesNameLiteral, out bool isLocalOnly)
    {
        addressablesNameLiteral = "CameraController"; isLocalOnly = true;
    }

    protected override GameObject Set(GameObject type)
    {
        Array.ForEach(GameObject.FindObjectsOfType<AudioListener>(), entry => entry.gameObject.SetActive(false)); //"there can only be one" -Unity Technologies
        GameObject newObj = GameObject.Instantiate(type, parent.e.body);
        buildPosOffset = GameObject.Instantiate(new GameObject(), newObj.transform).transform; //create buildPosOffset on the created camera
        camera = newObj.GetComponentInChildren<Camera>();
        return newObj;
    } //end Set

    public override void SetConfig(string configType)
    {
        switch (configType)
        {
            default:
                break;
        }
    }
}
