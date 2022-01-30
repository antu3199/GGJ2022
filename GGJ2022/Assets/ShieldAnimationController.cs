using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimationController : MonoBehaviour
{
    public DefenderPlayer DefenderPlayer;

    public ShieldCollsionEffect shieldPrefab;

    bool IsMovingToPlayer = false;

    public void SetCanMove(int Enabled)
    {
         DefenderPlayer.CanMove = Enabled == 0 ? false : true;
    }

    public void SetIsUsingAbility(int Enabled)
    {
        DefenderPlayer.IsUsingAbility = Enabled == 0 ? false : true;
    }

    public void ActivateShield()
    {
        ShieldCollsionEffect shield = GameObject.Instantiate<ShieldCollsionEffect>(shieldPrefab, DefenderPlayer.transform.position, Quaternion.identity);
        shield.ActivateShield(DefenderPlayer);
    }

    public void MoveToPlayer()
    {
        
    }


    // Not best practice, but for time
    void Update()
    {

    }


}
