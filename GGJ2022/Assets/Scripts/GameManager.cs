using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Global<GameManager>
{
    [SerializeField] EnemyPool EnemyPool;
    [SerializeField] AttackerPlayer AttackerPlayer;
    [SerializeField] DefenderPlayer DefenderPlayer;

    [SerializeField] Camera attackerCamera;
    public Camera AttackerCamera { get { return attackerCamera; }}
    [SerializeField] Camera defenderCamera;
    public Camera DefenderCamera { get { return defenderCamera; }}

    void Start()
    {
        // Assign them to default in case we forget
        if (attackerCamera == null)
        {
            attackerCamera = Camera.main;
        }

        if (defenderCamera == null)
        {
            defenderCamera = Camera.main;
        }
    }

    public EnemyPool GetEnemyPool() {
        return EnemyPool;
    }

    public AttackerPlayer GetAttackerPlayer() {
        return AttackerPlayer;
    }

    public DefenderPlayer GetDefenderPlayer() {
        return DefenderPlayer;
    }


}
