using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPauser: MonoBehaviour
{    
    void Awake() {    
        PauseController pauser = (PauseController)PauseController.Instance;
        Debug.Log(pauser);
        pauser.Attach(this.gameObject);
    }
}
