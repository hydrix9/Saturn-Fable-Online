using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// serves as a handle for the point a HurtBox should be instantiated on. Define .label and attach to a transform to reference this starting point in code from HurtBoxCreate and SyncHurtBoxes
/// </summary>
public class HurtBoxPoint : MonoBehaviour
{
    public string label;

} //end class HurtBoxPoint