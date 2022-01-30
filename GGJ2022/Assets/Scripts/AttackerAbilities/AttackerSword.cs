using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerSword : MonoBehaviour
{
    [SerializeField] AttackerPlayer Attacker;
    public bool IsSwordActive{get; set;}
    public float NextSwordDamage{get; set;}

    private void OnTriggerEnter(Collider other)
    {
       // Debug.LogError("Attacker sword collision detected with " + other.gameObject.name);
        if (Attacker == null) {
            Debug.LogError("The attacker is null in the sword script :((");
        }

        // Use events to determine whether or not to do damage
        if (IsSwordActive) {
            // Check if the sword collided with an enemy
            if (other.gameObject.tag == "Enemy") {
                Debug.Log("Attacker sword and enemy collision detected");
                EnemyAI enemy = (EnemyAI)other.gameObject.GetComponentInChildren<EnemyAI>();
                enemy.GetAttacked(NextSwordDamage * Attacker.Attack1Multiplier);
            }
        }
    }

    public void ActivateSwordWithDamage(float Damage)
    {
        IsSwordActive = true;
        NextSwordDamage = Damage;
    }

    public void DeactivateSword()
    {
        IsSwordActive = false;
    }
}
