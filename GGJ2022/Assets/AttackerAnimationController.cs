using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerAnimationController : MonoBehaviour
{
    public AttackerPlayer AttackerPlayer;
    public AttackerSword AttackerSword;
    public void ActivateSwordWithDamage(float Damage)
    {
        AttackerSword.ActivateSwordWithDamage(Damage);
    }

    public void DeactivateSword()
    {
        AttackerSword.DeactivateSword();
    }

    public void SetCanMove(int Enabled)
    {
        AttackerPlayer.CanMove = Enabled == 0 ? false : true;
    }

    public void SetIsUsingAbility(int Enabled)
    {
        AttackerPlayer.IsUsingAbility = Enabled == 0 ? false : true;
    }
}
