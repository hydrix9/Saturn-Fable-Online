using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Miner : AI_CombatBehaviour
{
    public static readonly SpellBehaviour mineMinerals = new SpellBehaviour<S_MineMinerals>(MineMinerals, CanMineMinerals, 11);

    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        behaviours = new SpellBehaviour[]
        {
            mineMinerals,
        };
    }

    protected override void Start_Init()
    {

    }

    static void MineMinerals(AI self, SpellBehaviour behaviour, Entity target)
    {
        //Logger.Log("AI casting beam attack ... " +
        behaviour.spell.TryStartCast(target, self.e, default, default);
        //);
    }
    static bool CanMineMinerals(AI self)
    {
        //return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position);
        return true;
    }


} //end class AI_Shotgunner
