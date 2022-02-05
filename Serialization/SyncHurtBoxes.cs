using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// define where HurtBoxes go when they are created on spell cast
/// </summary>
public class SyncHurtBoxes : MonoBehaviour
{
    private Dictionary<string, HurtBoxPoint> hurtBoxPoints = new Dictionary<string, HurtBoxPoint>();

    private void Awake()
    {
        //populate dictionary from HurtBoxPoints in children of this Entity using the label as a key
        hurtBoxPoints = GetComponentsInChildren<HurtBoxPoint>().ToDictionary(entry => entry.label);
    } //end func Awake

    public Transform GetHurtBoxPoint(string label)
    {
        if (hurtBoxPoints.ContainsKey(label))
            return hurtBoxPoints[label].transform;
        else
            return transform; //start point label not found, just use ourselves as a base, not sure what else to do
    } //end func GetHurtBoxPoint

} //end SyncHurtBoxes