using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimationController : MonoBehaviour
{
    public EnemyAI Enemy;

    public void SetIsAttacking(int Enabled)
    {
        Enemy.IsAttacking = Enabled == 0 ? false : true;
    }
}
