using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimationController : MonoBehaviour
{
    public DefenderPlayer DefenderPlayer;

    public void SetCanMove(int Enabled)
    {
         DefenderPlayer.CanMove = Enabled == 0 ? false : true;
    }

    public void SetIsUsingAbility(int Enabled)
    {
        DefenderPlayer.IsUsingAbility = Enabled == 0 ? false : true;
    }


}
