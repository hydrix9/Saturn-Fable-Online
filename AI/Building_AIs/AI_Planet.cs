using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Planet : AI_CombatBehaviour
{
    public static readonly SpellBehaviour generateYields = new SpellBehaviour<S_GeneratePlanetYields>(GeneratePlanetYields, CanGeneratePlanetYields, 11);

    public override void SetDefaults(out SpellBehaviour[] behaviours)
    {
        behaviours = new SpellBehaviour[]
        {
            generateYields
        };
    }

    protected override void Start_Init()
    {

    }

    static void GeneratePlanetYields(AI self, SpellBehaviour behaviour, Entity target)
    {
        //Logger.Log("AI casting beam attack ... " +
        behaviour.spell.TryStartCast(target, self.e, default, default);
        //);
    }
    static bool CanGeneratePlanetYields(AI self)
    {
        //return self.currentTarget != null && Vector3.Distance(self.transform.position, self.currentTarget.transform.position);
        return true;
    }


} //end class AI_Planet
