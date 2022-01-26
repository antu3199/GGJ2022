using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerPlayer : Player
{
    // Start is called before the first frame update
    void Start()
    {
        forwardKeyCode = KeyCode.UpArrow;
        backwardKeyCode = KeyCode.DownArrow;
        leftKeyCode = KeyCode.LeftArrow;
        rightKeyCode = KeyCode.RightArrow;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
