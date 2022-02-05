using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if using //scrapped in favor of using Entities with an AI instead of overloading a spellHit to be able to interact with an Entity object

/// <summary>
/// summoned entity that calls spell.IntervalAction every interval 
/// </summary>
public class Totem : Summoned
{

    public override void Set(spellHit spellHit, bool destroyOnCasterDeath = true)
    {
        base.Set(spellHit, destroyOnCasterDeath);
        intervalTimer = spellHit.interval; //start countdown
        durationTimer = spellHit.duration; //start countdown

        if (destroyOnCasterDeath)
            spellHit.caster.onDeath += DestroyClean;
    }

    float intervalTimer;
    float durationTimer;
    // Update is called once per frame
    void Update()
    {
        intervalTimer -= Time.deltaTime;
        durationTimer -= Time.deltaTime;

        if(intervalTimer <= 0)
        { //on spell interval
            intervalTimer = spellHit.interval; //reset
            Activate();
        }

        if(durationTimer <= 0)
        { //if done
            DestroyClean(); //kill self
        }

    }

    public void Activate()
    {
        spellHit.spell.IntervalEffect(spellHit);
    }

    protected override void Clean()
    {
        base.Clean();

    }
} //end class Totem
#endif