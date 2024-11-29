using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Bullet components
    public float bulletSpeed;
    public int damage;
    private Rigidbody2D rb; // Rigidbody2D component of the bullet
    private Vector2 direction; // Direction the bullet will travel
    public float knockbackForce; // Bullet knockback force
    public GameObject animationResetterPrefab; // Prefab for the AnimationResetter

    // Determines velocity of bullet
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }

    // Method to set the direction of the bullet
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
        Debug.Log("Bullet direction set: " + direction);
    }

    // Detect collision with enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);

        // Check if the collided object has an EnemyHealth component
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Apply damage to the enemy
            Debug.Log("Bullet damage: " + damage);

            // Trigger the hurt animation if it exists
            if(collision.gameObject.name != "Boss Bullet(Clone)" && collision.gameObject.name != "FlyingEnemyBullet(Clone)"
                && collision.gameObject.name != "FlyingEnemyBullet Variant Lv3(Clone)")
            {
                Animator enemyAnimator = enemy.GetComponent<Animator>();
                if (DoesParameterExist(enemyAnimator, "isHurt"))
                {
                    Debug.Log("Parameter isHurt exists.");
                    enemyAnimator.SetBool("isHurt", true);
                    // Don't play hurt animation if enemy is dead
                    /*   if(enemy.getCurrentHealth() > 0)
                       {
                           enemyAnimator.SetBool("isHurt", true);
                           Invoke("ResetHurtAnimation", enemy.hurtAnimationDuration); // Call ResetHurtAnimation after delay
                       }*/

                    // Instantiate the AnimationResetter and use it to reset the animation
                    GameObject resetter = Instantiate(animationResetterPrefab);
                    AnimationResetter animationResetter = resetter.GetComponent<AnimationResetter>();
                    animationResetter.ResetHurtAnimation(enemyAnimator, enemy.hurtAnimationDuration);


                }
                // Apply knockback force
                Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
                if (enemyRB != null)
                {
                    Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    enemyRB.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }

        Destroy(gameObject); // Destroy the bullet on collision
    }

    // Check if animator parameter exists
    bool DoesParameterExist(Animator animator, string parameterName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName)
            {
                return true;
            }
        }
        return false;
    }
}