using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightborne : EnemyHealth
{
    public Transform player;  // Reference to the player
    public Vector2 detectionBoxSize; // Box collider radius
    public float chargeSpeed;  // Speed that enemy charges with
    public float attackRange;  // Attack range
    public float attackCooldown;  // Wait time between attacks
    private bool isCharging = false; // Flag to check if the enemy is currently charging
    private float lastAttackTime = 0f; // Time of the last attack
    public float attackTime; // Manually set attack animation time
    public float hurtboxShift; // Enlargen Nightborne hurtbox

    private Animator animator; // Reference to the Animator component
    private BoxCollider2D hurtbox; // Reference to the enemy's collider
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    public Knockback knockback; // Reference to the Knockback script


    public GameObject animationResetterPrefab; // Prefab for the AnimationResetter
    public float deathDelay; // Time for death animation (GetCurrentAnimatorStateInfo(0).length wasn't working out so manually setting time)
    public AudioSource hitSFX;
    public AudioSource playerHitSFX;

    private bool facingRight = true; // Flag to check the direction the enemy is facing

    protected override void Start()
    {
        base.Start();  // Sets enemy health
        animator = GetComponent<Animator>();
        hurtbox = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        knockback = GetComponent<Knockback>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hitSFX.Play();
        }

    }

    /*    void Update()
        {
            // Calculate the distance to the player
            if(player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                // Check if the player is within the detection radius
                if (distanceToPlayer <= detectionRadius)
                {
                    isCharging = true;
                }
                else
                {
                    isCharging = false;
                }
                animator.SetBool("isCharging", isCharging);  // Set if enemy isCharging

                // Move enemy towards the player
                if (isCharging)
                {
                    MoveToPlayer(distanceToPlayer);
                }
            }

        }*/

    void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            return;
        }
        // Move enemy towards the player
        if (isCharging && player != null)
        {
            MoveToPlayer(Vector2.Distance(transform.position, player.position));
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            return;
        }
        // Calculate the distance to the player
        if (player != null)
        {
            Vector2 directionToPlayer = player.position - transform.position;
            bool playerInDetectionBox = Mathf.Abs(directionToPlayer.x) <= detectionBoxSize.x / 2 &&
                                        Mathf.Abs(directionToPlayer.y) <= detectionBoxSize.y / 2;

            // Check if the player is within the detection box
            if (playerInDetectionBox)
            {
                isCharging = true;
            }
            else
            {
                isCharging = false;
            }
            animator.SetBool("isCharging", isCharging);  // Set if enemy isCharging

            // Move enemy towards the player
/*            if (isCharging)
            {
                MoveToPlayer(directionToPlayer.magnitude);
            }*/
        }
    }


    void MoveToPlayer(float distanceToPlayer)
    {
        // If the enemy is not within attack range, move towards the player
        if (distanceToPlayer > attackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * chargeSpeed * Time.deltaTime);
            Debug.Log("Moving towards player with speed: " + chargeSpeed);

            // Flip the enemy to face the player
            if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
            {
                Flip();
            }
        }
        else
        {
            animator.SetBool("isCharging", false); // Stop running

            // If within attack range and cooldown period has passed, attack the player
            if (Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    void AttackPlayer()
    {
        animator.SetBool("isAttacking", true); // Set enemy to isAttacking 
        Debug.Log("isAttacking: " + animator.GetBool("isAttacking"));

        // Start the coroutine to apply damage after the attack animation
        StartCoroutine(ApplyDamageAfterAnimation());  
    }

    void OnDrawGizmosSelected()
    {
        // Draw a red wire box in the editor to visualize the detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }

    IEnumerator ApplyDamageAfterAnimation()
    {
        // Wait for the attack animation to complete
        yield return new WaitForSeconds(attackTime); // Attack animation time

        // Give the player more time to dodge
        //yield return new WaitForSeconds(0.25f);

        // Check if the player is still within attack range
        if(player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                // Apply damage to the player
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    if (playerHealth.getCurrentHealth() > 0)
                    {
                        playerHitSFX.Play();
                    }
                    playerHealth.TakeDamage(attackDamage);
                    Debug.Log("Damage inflicted to player: " + attackDamage);

                    //knockback.ApplyKnockback(player); // Apply knockback to the player
                    StartCoroutine(playerHealth.BlinkEffectWithKnockback(knockback, transform)); // Call the coroutine to apply knockback and blink effect
                }
            }
            else
            {
                Debug.Log("Player dodged the attack!");
            }

            // Reset the isAttacking parameter after the attack
            animator.SetBool("isAttacking", false);
        }

    }


    // Method to handle enemy death
    protected override void Die()
    {
        hurtbox.enabled = false;  // Disable the enemy hurtbox collider               
        rb.isKinematic = true;

        // Destroy the enemy health slider
/*        if (enemyBar != null && enemyBar.healthSlider != null)
        {
            Debug.Log("Destroying health slider.");
            Destroy(enemyBar.healthSlider.gameObject);
        }
        else
        {
            Debug.LogError("Health slider not found!");
        }*/

        animator.SetBool("isDead", true);  // Set death animation to true
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // Instantiate the AnimationResetter and use it to reset the animation
        GameObject resetter = Instantiate(animationResetterPrefab);
        AnimationResetter animationResetter = resetter.GetComponent<AnimationResetter>();
        animationResetter.ResetHurtAnimation(animator, animator.GetCurrentAnimatorStateInfo(0).length + 0.1f);

        Destroy(gameObject, deathDelay);  // Destroy the enemy GameObject after the death animation
        Debug.Log("Nightborne has died!"); // Log the death event
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
