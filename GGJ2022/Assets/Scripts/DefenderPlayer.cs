using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderPlayer : Player
{
    // Start is called before the first frame update
    void Start()
    {
        forwardKeyCode = KeyCode.W;
        backwardKeyCode = KeyCode.S;
        leftKeyCode = KeyCode.A;
        rightKeyCode = KeyCode.D;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
