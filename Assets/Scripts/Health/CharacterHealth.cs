using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for character health management
public class CharacterHealth : MonoBehaviour
{
    public int maxHealth; // Maximum health of the character
    protected int currentHealth; // Current health of the character
    public HealthBar healthBar; // Reference to the HealthBar script
    public EnemyHealthBar enemyBar; // Reference to the HealthBar script
    public bool isPlayer; // If not set to true don't initiallize healthBar
    public int collisionDamage; // Damage character inflicts on contact 
    public int attackDamage; // Damage character inflicts

    // Initialize health values and health bar
    protected virtual void Start()
    {
        currentHealth = maxHealth; // Set current health to maximum health
        // Remove condition if we want to add health bars for ewnemies + will need to make new hbar + sliders objects  
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth); // Initialize the health bar with the maximum health
        }
    }

    // Method to reduce the character's health
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the damage amount
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // Update the health bar
        }

        if (currentHealth <= 0)
        {
            Die(); // Call the Die method if health is zero or less
        }
    }

    // Method to handle character death
    protected virtual void Die()
    {
       // Inherited
    }

    public int getCurrentHealth() { return currentHealth; }
}
