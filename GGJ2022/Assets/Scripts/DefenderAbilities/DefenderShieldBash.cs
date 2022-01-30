using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderShieldBash : Ability
{
    void Start()
    {
        
    }

    public override void DoAbility()
    {
        if (CasterPlayer == null) return;

        // Check that the caster player is the Defender
        if (CasterPlayer.tag == "Defender") {
            CasterPlayer.DoAbility3();
        }
    }
}
