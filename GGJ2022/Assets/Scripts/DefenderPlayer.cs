using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderPlayer : Player
{
    // Start is called before the first frame update
    void Start()
    {
        ForwardKeyCode = KeyCode.W;
        BackwardKeyCode = KeyCode.S;
        LeftKeyCode = KeyCode.A;
        RightKeyCode = KeyCode.D;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    protected override void SetWalking(bool isWalking) {
        
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