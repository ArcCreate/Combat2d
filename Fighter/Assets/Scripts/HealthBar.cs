using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //refrences
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void setMaxHealth(int maxDamage)
    {
        slider.maxValue = maxDamage;
        slider.value = 0;

        fill.color = gradient.Evaluate(1f);
    }

    public void setDamage(int damaged)
    {
        slider.value = damaged;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
