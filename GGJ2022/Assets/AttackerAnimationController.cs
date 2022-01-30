using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerAnimationController : MonoBehaviour
{
    public AttackerSword AttackerSword;
    public void ActivateSwordWithDamage(float Damage)
    {
        AttackerSword.ActivateSwordWithDamage(Damage);
    }

    public void DeactivateSword()
    {
        AttackerSword.DeactivateSword();
    }
}
