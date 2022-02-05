using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Researcher : AI_CombatBehaviour
{
    public static readonly SpellBehaviour generateResearch = new SpellBehaviour<S_GenerateResearch>(GenerateResearch, CanGenerateResearch, 11);

    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        behaviours = new SpellBehaviour[]
        {
            generateResearch
        };
    }

    protected override void Start_Init()
    {

    }

    static void GenerateResearch(AI self, SpellBehaviour behaviour, Entity target)
    {
        //Logger.Log("AI casting beam attack ... " +
        behaviour.spell.TryStartCast(target, self.e, default, default);
        //);
    }
    static bool CanGenerateResearch(AI self)
    {
        //return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position);
        return true;
    }


} //end class AI_Researcher
