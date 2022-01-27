using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerPlayer : Player
{
    // Start is called before the first frame update
    void Start()
    {
        ForwardKeyCode = KeyCode.UpArrow;
        BackwardKeyCode = KeyCode.DownArrow;
        LeftKeyCode = KeyCode.LeftArrow;
        RightKeyCode = KeyCode.RightArrow;
    }

    // Update is called once per frame
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

    }

    public override void DoAbility2() {

    }

    public override void DoAbility3() {

    }

    public override void DoUltimateAbility() {

    }
}
