using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollsionEffect : MonoBehaviour
{
    private DefenderPlayer defenderPlayer;

    HashSet<Player> players = new HashSet<Player>();

    public float Duration = 5f;

    void Start()
    {
        StartCoroutine(WaitAndDestroy());
    }

    public void ActivateShield(DefenderPlayer player)
    {
        defenderPlayer = player;
    }

    void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.DamageRedirector = RedirectDamage;
            player.RedirectTarget = defenderPlayer;
            players.Add(player);
        }
    }

    void RedirectDamage(float damage)
    {
        defenderPlayer.GetAttacked(damage/2, true);
    }

    void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.DamageRedirector = null;
            player.RedirectTarget = null;
            if (players.Contains(player))
            {
                players.Remove(player);
            }
        }
    }

    void OnDestroy()
    {
        foreach (Player player in players)
        {
            player.DamageRedirector = null;
        }
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(Duration);

        Destroy(this.gameObject);
    }

}
