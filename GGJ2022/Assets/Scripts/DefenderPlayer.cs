using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderPlayer : Player
{
    public float Attack1Multiplier = 1;

    
    void Start()
    {
        ForwardKeyCode = KeyCode.W;
        BackwardKeyCode = KeyCode.S;
        LeftKeyCode = KeyCode.A;
        RightKeyCode = KeyCode.D;
    }

    new void Update()
    {
        base.Update();
    }

    public override void DoAbility1() {
        if (AnimationController != null) {
            AnimationController.ResetTrigger("DoBlock");
            AnimationController.SetTrigger("DoBlock");
        }
    }

    public override void DoAbility2() {
        if (AnimationController != null) {
            AnimationController.ResetTrigger("DoRunAndBlock");
            AnimationController.SetTrigger("DoRunAndBlock");
        }
    }

    public override void DoAbility3() {
        if (AnimationController != null) {
            AnimationController.ResetTrigger("DoShieldBash");
            AnimationController.SetTrigger("DoShieldBash");
        }
    }

    public override void DoUltimateAbility() {
        if (AnimationController != null) {
            AnimationController.ResetTrigger("DoBlitzcrankHook");
            AnimationController.SetTrigger("DoBlitzcrankHook");
        }
    }
}
