using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing enemy health, inherits from CharacterHealth
public class EnemyHealth : CharacterHealth
{

    public float delay; // Delay before destroying the enemy
    public float hurtAnimationDuration; // Set to hurt animation duration

    // Method to handle enemy death
    protected override void Die()
    {
        base.Die();
        Destroy(gameObject, delay); // Destroy the enemy after a delay 
        Debug.Log("Enemy has died!"); // Log the death event
    }
}
