using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimationController : MonoBehaviour
{
    public DefenderPlayer DefenderPlayer;
    public DefenderShield DefenderShield;

    public ShieldCollsionEffect shieldPrefab;

    bool IsMovingToPlayer = false;

    public void SetCanMove(int Enabled)
    {
         DefenderPlayer.CanMove = Enabled == 0 ? false : true;
    }

    public void SetIsUsingAbility(int Enabled)
    {
        DefenderPlayer.IsUsingAbility = Enabled == 0 ? false : true;
    }

    public void ActivateShield()
    {
        ShieldCollsionEffect shield = GameObject.Instantiate<ShieldCollsionEffect>(shieldPrefab, DefenderPlayer.transform.position, Quaternion.identity);
        shield.ActivateShield(DefenderPlayer);
    }

     public void ActivateShieldWithDamage(float Damage)
    {
        DefenderShield.ActivateShieldWithDamage(Damage);
    }

    public void DeactivateShieldWithDamage()
    {
        DefenderShield.DeactivateShieldWithDamage();
    }

    public void MoveToPlayer()
    {
        IsMovingToPlayer = true;
    }


    // Not best practice, but for time
    void Update()
    {
        if (IsMovingToPlayer)
        {
            float thresh = 1f;
            Vector3 attackerPosition = GameManager.Instance.GetAttackerPlayer().transform.position;
            Vector3 defenderPosition = DefenderPlayer.transform.position;

            float dist = Vector3.Distance(attackerPosition, defenderPosition);
            if (dist > thresh)
            {
                Vector3 newPos = Vector3.Lerp(attackerPosition, defenderPosition, 0.05f * Time.deltaTime);
                DefenderPlayer.PlayerVelocity = (attackerPosition - defenderPosition) * 5;
            }
            else
            {
                IsMovingToPlayer = false;
                DefenderPlayer.PlayerVelocity = Vector3.zero;
            }

        }
    }


}
