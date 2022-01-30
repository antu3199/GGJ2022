using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimationController : MonoBehaviour
{
    public DefenderPlayer DefenderPlayer;
    public DefenderShield DefenderShield;

    public ShieldCollsionEffect shieldPrefab;

    bool IsMovingToPlayer = false;

    public float MovementSpeed = 5;
    public int ShieldHealth = 400;

    float maxTime = 2f;
    float currentTime = 0;

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
        currentTime = 0;
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
                DefenderPlayer.PlayerVelocity = (attackerPosition - defenderPosition) * MovementSpeed;
                currentTime += Time.deltaTime;

                if (currentTime >= maxTime)
                {
                    currentTime = 0;
                    IsMovingToPlayer = false;
                    DefenderPlayer.PlayerVelocity = Vector3.zero;
                }
            }
            else
            {
                IsMovingToPlayer = false;
                DefenderPlayer.PlayerVelocity = Vector3.zero;
                GameManager.Instance.GetAttackerPlayer().AddShield(ShieldHealth);
            }
        }
    }


}
