using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Player CasterPlayer; // The player casting the ability

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCasterPlayer(Player player) {
        CasterPlayer = player;
    }

    public abstract void DoAbility();
}
