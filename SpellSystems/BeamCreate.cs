using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamCreate : MonoBehaviour
{
    public struct prefabIndex
    {
        public const int line = 0, beamEnd = 1;
    }


    public static void Single(spellHit spellHit, GameObject startPrefab, GameObject endPrefab, GameObject linePrefab, Transform startAnchor, Vector3 startPos, ref float speed, ref float scale, ref Vector3 rotationSpeed, Func<Entity, Entity, int> calcValidTarget)
    {
        Beam beam = new GameObject().AddComponent<Beam>().Set(startPrefab, endPrefab, linePrefab, startAnchor, startPos, spellHit, ref speed, ref scale, ref rotationSpeed, calcValidTarget);
    }
    public static void Single(spellHit spellHit, GameObject startPrefab, GameObject endPrefab, GameObject linePrefab, Transform startAnchor, Vector3 startPos, ref float speed, ref float scale, Transform trackTarget, ref float trackSpeed, Func<Entity, Entity, int> calcValidTarget)
    {
        Beam beam = new GameObject().AddComponent<Beam>().Set(startPrefab, endPrefab, linePrefab, startAnchor, startPos, spellHit, ref speed, ref scale, trackTarget, ref trackSpeed, calcValidTarget);
    }

    public static void FourWay_vMoveToPos(spellHit spellHit, GameObject startPrefab, GameObject endPrefab, GameObject linePrefab, Transform startAnchor, Vector3 startPos, ref float startRange, ref float speed, ref float scale, Func<Entity, Entity, int> calcValidTarget)
    {
        Vector3 beamEndPos;
        Beam beam;

        //forward
        beamEndPos = (spellHit.caster.transform.forward * startRange) + startPos;
        beam = new GameObject().AddComponent<Beam>().Set_vMoveToPos(startPrefab, endPrefab, linePrefab, startAnchor, spellHit.caster.transform.position, spellHit, ref speed, ref scale, ref beamEndPos, calcValidTarget);

        //right
        beamEndPos = (spellHit.caster.transform.right * startRange) + startPos;
        beam = new GameObject().AddComponent<Beam>().Set_vMoveToPos(startPrefab, endPrefab, linePrefab, startAnchor, spellHit.caster.transform.position, spellHit, ref speed, ref scale, ref beamEndPos, calcValidTarget);

        //back
        beamEndPos = (-spellHit.caster.transform.forward * startRange) + startPos;
        beam = new GameObject().AddComponent<Beam>().Set_vMoveToPos(startPrefab, endPrefab, linePrefab, startAnchor, spellHit.caster.transform.position, spellHit, ref speed, ref scale, ref beamEndPos, calcValidTarget);

        //left
        beamEndPos = (-spellHit.caster.transform.right * startRange) + startPos;
        beam = new GameObject().AddComponent<Beam>().Set_vMoveToPos(startPrefab, endPrefab, linePrefab, startAnchor, spellHit.caster.transform.position, spellHit, ref speed, ref scale, ref beamEndPos, calcValidTarget);

    }

} //end class Beams
