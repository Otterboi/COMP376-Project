using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Player health inherits from CharacterHealth
public class PlayerHealth : CharacterHealth
{
    // Intangibility Variables
    public float intangibilityDuration; // Duration of intangibility frames
    private bool isIntangible = false; // Tracks if the player is currently intangible
    private float intangibilityTimer = 0.0f; // Timer for intangibility duration

    // Blinking Variables
    private SpriteRenderer spriteRenderer; // Reference to the player's SpriteRenderer
    public float blinkDuration; // Duration of each blink
    public int blinkCount; // Number of blinks

    // Player Respawn Variables
    public float offStageDeath; // Y position for falling off death (-5.1 is the stage bounds)
    public Vector2 respawnPoint; // Can set to starting point or manually
    public float respawnDelay;

    private Rigidbody2D rb; // Reference to the player's Rigidbody2D
	public AudioSource hit_sound;
    

    // Initialize player-specific values
    protected override void Start()
    {
        base.Start(); // Call the base class Start method
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        // Ensure respawnPoint is set
        if (respawnPoint == Vector2.zero)
        {
            respawnPoint = transform.position; // Set the initial respawn point to the player's starting position
        }
    }

    private void Update()
    {
        // Handle intangibility timer
        if (isIntangible)
        {
            intangibilityTimer -= Time.deltaTime; // Reduce intangibility frames
            if (intangibilityTimer <= 0)
            {
                // Reset intangibility state
                isIntangible = false;
                intangibilityTimer = 0;
            }
        }

        // Check if the player has fallen off the stage
        if (transform.position.y < offStageDeath)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Check if the collided object is an enemy
        {
            Debug.Log("Collided with enemy");
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>(); // Get the EnemyHealth component
            Knockback playerKnockback = collision.gameObject.GetComponent<Knockback>(); // Get the Knockback component
            if (enemyHealth != null)
            {
                if (!isIntangible)
                {

                    //playerKnockback.ApplyKnockback(transform); //Apply knockback
                    Debug.Log("Step 1");
                    if (currentHealth > 0)
                    {
                        hit_sound.Play();
                    }
                    TakeDamage(enemyHealth.collisionDamage); // Inflict damage to the player
                    Debug.Log("Collision damage: " + enemyHealth.collisionDamage);

                    // Set intangibility state
                    isIntangible = true; // Set intangibility state
                    intangibilityTimer = intangibilityDuration;

                    // Blinking Effect with Knockback
                    StartCoroutine(BlinkEffectWithKnockback(playerKnockback, collision.transform));
                }

                // Inflict damage to the enemy
                //enemyHealth.TakeDamage(collisionDamage);
                //Debug.Log("Inflicted damage to enemy: " + collisionDamage);
            }
        }
    }

    // Detect if player is still in collision with the enemy during intangibility
    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>(); // Get the EnemyHealth component
        Knockback playerKnockback = collision.gameObject.GetComponent<Knockback>(); // Get the Knockback component
        if (collision.gameObject.CompareTag("Enemy") && !isIntangible)
        {
            //playerKnockback.ApplyKnockback(transform); //Apply knockback
            if(currentHealth > 0)
            {
                hit_sound.Play();
            }
            
            TakeDamage(enemyHealth.collisionDamage); // Inflict damage to the player
            Debug.Log("Collision damage: " + enemyHealth.collisionDamage);

            // Set intangibility state
            isIntangible = true; // Set intangibility state
            intangibilityTimer = intangibilityDuration;

            // Blinking Effect with Knockback
            StartCoroutine(BlinkEffectWithKnockback(playerKnockback, collision.transform));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Check if the collided object is an enemy
        {
            Debug.Log("Collided with enemy");
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>(); // Get the EnemyHealth component
            Knockback playerKnockback = collision.gameObject.GetComponent<Knockback>(); // Get the Knockback component
            if (enemyHealth != null)
            {
                if (!isIntangible)
                {
                    //playerKnockback.ApplyKnockback(transform); //Apply knockback
                    if (currentHealth > 0)
                    {
                        hit_sound.Play();
                    }
                    TakeDamage(enemyHealth.collisionDamage); // Inflict damage to the player
                    Debug.Log("Collision damage: " + enemyHealth.collisionDamage);

                    // Set intangibility state
                    isIntangible = true; // Set intangibility state
                    intangibilityTimer = intangibilityDuration;

                    // Blinking Effect with Knockback
                    StartCoroutine(BlinkEffectWithKnockback(playerKnockback, collision.transform));
                }
            }
        }
    }

    // Detect if player is still in collision with the trigger during intangibility
    private void OnTriggerStay2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>(); // Get the EnemyHealth component
        Knockback playerKnockback = collision.gameObject.GetComponent<Knockback>(); // Get the Knockback component
        if (collision.gameObject.CompareTag("Enemy") && !isIntangible)
        {
            //playerKnockback.ApplyKnockback(transform); //Apply knockback
            if (currentHealth > 0)
            {
                hit_sound.Play();
            }
            TakeDamage(enemyHealth.collisionDamage); // Inflict damage to the player
            Debug.Log("Collision damage: " + enemyHealth.collisionDamage);

            // Set intangibility state
            isIntangible = true; // Set intangibility state
            intangibilityTimer = intangibilityDuration;

            // Blinking Effect with Knockback
            StartCoroutine(BlinkEffectWithKnockback(playerKnockback, collision.transform));
        }
    }

    // Blinking effect + knockback coroutine
    public IEnumerator BlinkEffectWithKnockback(Knockback knockbackScript, Transform enemy)
    {
        GetComponent<Animator>().SetBool("isHurt", true); // Start hurt animation
        yield return new WaitForSeconds(0.1f); // Ensure hurt animation doesnt happen after knockback

        // Apply knockback using the Knockback script
        knockbackScript.ApplyKnockback(transform);

        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkDuration);
        }
        GetComponent<Animator>().SetBool("isHurt", false); // Stop hurt animation 
    }

    // Method to handle player death
    protected override void Die()
    {
        Cursor.visible = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        base.Die();
        currentHealth = 0; // Set health to zero 
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // Update the health bar to zero
        }
        AudioManager.Instance.StopMusic();
        Debug.Log("Character died!"); // Log the death event

        //StartCoroutine(Respawn()); // Start the respawn coroutine
    }

    // Coroutine to handle player respawn
    private IEnumerator Respawn()
    {
        Debug.Log("Respawn coroutine started");
        yield return new WaitForSeconds(respawnDelay); // Wait for the respawn delay
        transform.position = respawnPoint; // Reset the player's position to the respawn point
        currentHealth = maxHealth; // Reset the player's health
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth); // Reset the health bar
        }
        Debug.Log("Player respawned!");
    }

    private void playerHurt()
    {
        GetComponent<Animator>().SetBool("isHurt", true);
    }
}