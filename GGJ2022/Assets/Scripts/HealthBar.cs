using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider Slider;

    private float _lerpDuration = 1; 

    public void SetTotalHealth(int totalHealth) {
        Slider.maxValue = totalHealth;
        Slider.value = totalHealth;
    }

    public void SetHealth(int newHealth) {
        if (newHealth <= 0) return;
        
        // Lerp the slider to the new health
        StartCoroutine(HealthbarLerp(newHealth));
    }

    IEnumerator HealthbarLerp(float newValue) {
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
