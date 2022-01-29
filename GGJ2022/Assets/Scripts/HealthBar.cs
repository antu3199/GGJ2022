using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider Slider;

    public int TotalHealth;

    private float _lerpDuration = 1; 

    void Start() {
        Slider.maxValue = 1.0f;
    }

    public void SetTotalHealth(int health) {
        TotalHealth = health;
    }

    public void SetHealth(int newHealth) {
        // Lerp the slider to the new health

        if (TotalHealth == 0) {
            Debug.LogError("Trying to set the health bar but the total health is 0");
            return;
        }
        Debug.Log("New health - " + newHealth);
        StartCoroutine(HealthbarLerp(newHealth / TotalHealth));
    }

    IEnumerator HealthbarLerp(int newValue) {
        var startValue = Slider.value;
        float lerp = 0;
        
        while(lerp < 1)
        {
            lerp += Time.deltaTime / _lerpDuration;
            Slider.value = Mathf.Lerp(startValue, newValue, lerp);

             yield return null;
        }
        
        Slider.value = newValue; 
    }
}
