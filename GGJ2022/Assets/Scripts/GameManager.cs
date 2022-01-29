using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Global<GameManager>
{
    [SerializeField] EnemyPool EnemyPool;
    [SerializeField] AttackerPlayer AttackerPlayer;
    [SerializeField] DefenderPlayer DefenderPlayer;

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
