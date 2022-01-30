using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlitzcrankHook : Ability
{
    [SerializeField] float Speed;

    void Start()
    {
        
    }

    public override void DoAbility()
    {
        Debug.Log("Blitzcrank hook casted!");
        if (CasterPlayer == null) return;

        // Check that the caster player is the Defender
        if (CasterPlayer.tag == "Defender") {
            CasterPlayer.DoUltimateAbility();
        }

        GameManager gameManager = (GameManager)GameManager.Instance;

        if (gameManager == null) {
            Debug.LogError("Game Manager null :((");
            return;
        }

        // Get the furthest enemy from the caster player
        EnemyAI furthestEnemy = gameManager.GetEnemyPool().GetFurthestEnemyFromPlayer(CasterPlayer);

        if (furthestEnemy != null) {
            // Make the player face in the direction of the furthest enemy, so it's throwing the shield to the enemy
            int damping = 2;
            Vector3 lookPos = furthestEnemy.gameObject.transform.position - CasterPlayer.gameObject.transform.position;
            lookPos.y = 0;

            var rotation = Quaternion.LookRotation(lookPos);
            CasterPlayer.gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping); 

            StartCoroutine(PullEnemyCoroutine(furthestEnemy));
        } else {
            // What to do if blitz hook is used when no enemies are there?
            Debug.LogError("Blitz hook casted but no enemies");
        }
    }

    private IEnumerator PullEnemyCoroutine(EnemyAI enemy) {
        if (enemy == null) yield break;

        yield return new WaitForSeconds(0.5f); // for the enemy to turn to the slime

        while (true) {
            // https://handyopinion.com/move-gameobject-to-another-with-speed-variation-in-unity/
            float step = Speed * Time.deltaTime;
            float distance = Vector3.Distance(enemy.transform.position, CasterPlayer.transform.position);

            if (distance < 0.8) // at the end it will slow down to half of its speed
            {
                step = step / 2;
            }

            enemy.gameObject.transform.position = Vector3.MoveTowards(enemy.gameObject.transform.position, CasterPlayer.transform.position, step);

            // Check if the enemy reached approximately to the caster player
            if (distance < 2f) {
                break;
            }

            yield return null;
        }
    }
}
