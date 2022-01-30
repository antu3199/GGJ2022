using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderShield : MonoBehaviour
{
    [SerializeField] DefenderPlayer Defender;
    public bool IsShieldActive{get; set;}
    public float NextShieldDamage{get; set;}

    private void OnTriggerEnter(Collider other)
    {
        if (Defender == null) {
            Debug.LogError("The defender is null in the shield script :((");
        }

        // Use events to determine whether or not to do damage
        if (IsShieldActive) {
            // Check if the shield collided with an enemy
            if (other.gameObject.tag == "Enemy") {
                EnemyAI enemy = (EnemyAI)other.gameObject.GetComponentInChildren<EnemyAI>();
                enemy.GetAttacked(NextShieldDamage * Defender.Attack1Multiplier);
            }
        }
    }

    public void ActivateShieldWithDamage(float Damage)
    {
        IsShieldActive = true;
        NextShieldDamage = Damage;
    }

    public void DeactivateShieldWithDamage()
    {
        IsShieldActive = false;
    }
}
