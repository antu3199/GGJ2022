using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerSword : MonoBehaviour
{
    [SerializeField] AttackerPlayer Attacker;

    private void OnTriggerEnter(Collider other)
    {
       // Debug.LogError("Attacker sword collision detected with " + other.gameObject.name);
        if (Attacker == null) {
            Debug.LogError("The attacker is null in the sword script :((");
        }

        // We also only want to check for collisions when the attacker is doing an attack animation
        if (Attacker.IsDoingBasicAttack()) {
            // Check if the sword collided with an enemy
            if (other.gameObject.tag == "Enemy") {
                Debug.Log("Attacker sword and enemy collision detected");
                EnemyAI enemy = (EnemyAI)other.gameObject.GetComponentInChildren<EnemyAI>();
                enemy.GetAttacked(Attacker.BasicAttackDamage);
            }
        }
    }
}
