using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerPlayer : Player
{
    public BoxCollider SwordCollider;

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

    protected override void SetWalking(bool isWalking) {
        if (AnimationController != null) {
            AnimationController.SetBool("IsWalking", isWalking);
        }
    }

    public override void DoAbility1() {
        if (AnimationController != null) {
            AnimationController.ResetTrigger("DoBasicAttack");
            AnimationController.SetTrigger("DoBasicAttack");
        }
    }

    public override void DoAbility2() {

    }

    public override void DoAbility3() {

    }

    public override void DoUltimateAbility() {

    }
}
