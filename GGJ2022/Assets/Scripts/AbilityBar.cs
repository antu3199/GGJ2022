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

    void Update()
    {
        // Listen for ability taps using the keyboard shortcuts
        if (Input.GetKey(Ability1Key)) {
            OnAbility1Tapped();
        }

        if (Input.GetKey(Ability2Key)) {
            OnAbility2Tapped();
        }

        if (Input.GetKey(Ability3Key)) {
            OnAbility3Tapped();
        }

        if (Input.GetKey(UltimateAbilityKey)) {
            OnUltimateAbilityTapped();
        }
    }

    public void OnAbility1Tapped() {

    }

    public void OnAbility2Tapped() {

    }

    public void OnAbility3Tapped() {

    }

    public void OnUltimateAbilityTapped() {

    }
}
