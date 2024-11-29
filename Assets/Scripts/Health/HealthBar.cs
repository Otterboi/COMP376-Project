using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Component that represents the healthbar

    // Set the maximum health value and initialize the slider
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health; // Set the maximum value of the slider
        slider.value = health; // Set the current value of the slider to the maximum
    }

    // Update the health value on the slider
    public void SetHealth(int health)
    {
        slider.value = health; // Update the slider's value to the current health
    }
}