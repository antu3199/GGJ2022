using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBar : MonoBehaviour
{
    // Ability keyboard shorcut keys
    [SerializeField] KeyCode Ability1Key;
    [SerializeField] KeyCode Ability2Key;
    [SerializeField] KeyCode Ability3Key;
    [SerializeField] KeyCode UltimateAbilityKey;

    // Different abilities (script attached on Ability prefabs)
    [SerializeField] Ability Ability1;
    [SerializeField] Ability Ability2;
    [SerializeField] Ability Ability3;
    [SerializeField] Ability UltimateAbility;

    [SerializeField] Player Player; // The player who the ability bar belongs to

    void Start()
    {
        if (Player != null) {
            Ability1?.SetCasterPlayer(Player);
            Ability2?.SetCasterPlayer(Player);
            Ability3?.SetCasterPlayer(Player);
            UltimateAbility?.SetCasterPlayer(Player);
        } else {
            Debug.Log("Ability bar doesn't have an assigned caster player");
        }
        
    }

    void Update()
    {
        // Listen for ability taps using the keyboard shortcuts
        if (Input.GetKey(Ability1Key)) {
            Ability1.DoAbility();
        }

        if (Input.GetKey(Ability2Key)) {
            Ability2.DoAbility();
        }

        if (Input.GetKey(Ability3Key)) {
            Ability3.DoAbility();
        }

        if (Input.GetKey(UltimateAbilityKey)) {
            UltimateAbility.DoAbility();
        }
    }

    public void OnAbility1Tapped() {
        Ability1.DoAbility();
    }

    public void OnAbility2Tapped() {
        Ability2.DoAbility();
    }

    public void OnAbility3Tapped() {
        Ability3.DoAbility();
    }

    public void OnUltimateAbilityTapped() {
        UltimateAbility.DoAbility();
    }
}