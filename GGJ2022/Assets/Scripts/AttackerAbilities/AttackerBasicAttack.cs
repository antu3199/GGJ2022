using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerBasicAttack : Ability
{
    void Start()
    {
        
    }

    public override void DoAbility()
    {
        if (CasterPlayer == null) return;

        // Check that the caster player is the Attacker
        if (CasterPlayer.tag == "Attacker") {
            CasterPlayer.DoAbility1();
        }
    }
}
