using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerPlayer : Player
{
    // Damage for all the abilities
    public float BasicAttackDamage = 50f;

    void Start()
    {
        ForwardKeyCode = KeyCode.UpArrow;
        BackwardKeyCode = KeyCode.DownArrow;
        LeftKeyCode = KeyCode.LeftArrow;
        RightKeyCode = KeyCode.RightArrow;
    }

    new void Update()
    {
        base.Update();
    }

    public override void DoAbility1() {
        if (AnimationController != null) {
            AnimationController.ResetTrigger("DoBasicAttack");
            AnimationController.SetTrigger("DoBasicAttack");
        }
    }

    public override void DoAbility2() {
        if (AnimationController != null) {
            AnimationController.ResetTrigger("DoSpinToWin");
            AnimationController.SetTrigger("DoSpinToWin");
        }
    }

    public override void DoAbility3() {
        AnimationController.ResetTrigger("DoQuickAttack");
        AnimationController.SetTrigger("DoQuickAttack");
    }

    public override void DoUltimateAbility() {
        AnimationController.ResetTrigger("DoFinalBreath");
        AnimationController.SetTrigger("DoFinalBreath");
    }
}
