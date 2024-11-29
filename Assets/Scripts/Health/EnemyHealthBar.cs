using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the Slider UI element
    public Transform healthBarPosition; // Reference to the empty GameObject above the enemy's head
    public EnemyHealth enemyHealth; // Reference to the enemy's health script

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (enemyHealth != null && healthBarPosition != null)
        {
            // Update the health bar's value
            healthSlider.value = enemyHealth.getCurrentHealth() / (float)enemyHealth.maxHealth;

            // Position the health bar above the enemy's head
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(healthBarPosition.position);
            healthSlider.transform.position = screenPosition;
        }
    }

}
