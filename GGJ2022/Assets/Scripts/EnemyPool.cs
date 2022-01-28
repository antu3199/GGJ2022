using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The EnemyPool prefab which has this script is gonna have all the Enemies gameobjects as children
public class EnemyPool : MonoBehaviour
{
    public List<GameObject> GetEnemiesInRoom() {
        List<GameObject> enemies = new List<GameObject>();

        foreach (Transform child in transform) {
            if (child.gameObject.tag == "Enemy") {
                enemies.Add(child.gameObject);
            }
        }

        return enemies;
    }

    public EnemyAI GetFurthestEnemyFromPlayer(Player player) {
        if (player == null) {
            Debug.LogError("EnemyPool.GetFurthestEnemyFromPlayer received a null player parameter");
            return null;
        }

        List<GameObject> allEnemies = GetEnemiesInRoom();
        float highestDistance = 0;
        EnemyAI furthestEnemy = null;

        foreach(GameObject enemy in allEnemies) {
            float distanceFromPlayer = Vector3.Distance(enemy.transform.position, player.gameObject.transform.position);
            Debug.Log("EnemyPool.GetFurthestEnemyFromPlayer: Distance from player and enemy " + distanceFromPlayer);

            if (distanceFromPlayer > highestDistance) {
                highestDistance = distanceFromPlayer;
                furthestEnemy = enemy.GetComponent<EnemyAI>();
            }
        }

        return furthestEnemy;
    }
}
