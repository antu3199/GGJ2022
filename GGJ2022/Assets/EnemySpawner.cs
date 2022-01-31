using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public float TimeBeforeSpawnMin = 3f;
    public float TimeBeforeSpawnMax = 15f;

    float TimeBeforeSpawn = 5f;

    float curTime = 0;

    public EnemyAI enemyPrefab;

    void Start()
    {
        RandomizeTimeBeforeSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime >= TimeBeforeSpawn)
        {
            EnemyAI newEnemy = GameObject.Instantiate<EnemyAI>(enemyPrefab, transform.position, transform.rotation);
            curTime = 0;
            newEnemy.transform.SetParent(transform.parent);
            RandomizeTimeBeforeSpawn();
        }
    }

    void RandomizeTimeBeforeSpawn()
    {
        TimeBeforeSpawn = Random.Range(TimeBeforeSpawnMin, TimeBeforeSpawnMax);
    }
}
