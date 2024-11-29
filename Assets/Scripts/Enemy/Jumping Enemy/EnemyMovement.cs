using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : EnemyHealth
{
    [Header("Patrolling")]
    [SerializeField] private float speed = 1;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float checkRadious;

    [Header("Jump Attacking")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform player;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize;

    [Header("Jump Attacking")]
    [SerializeField] private Vector2 lineOfSight;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioSource hitSFX;
    
    private float moveDirection = 1;
    private bool facingRight = true;
    private bool isWall;
    private bool isEdge;
    private bool isGrounded;
    private bool seePlayer;
    private Animator enemyAnim;


    private Rigidbody2D enemyRigidBody;

    // Start is called before the first frame update
    protected override void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            return;
        }
        isEdge = Physics2D.Raycast(edgeCheck.position, -transform.up, 1f, groundLayer);
        isWall = Physics2D.OverlapCircle(wallCheck.position, checkRadious, wallLayer);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        seePlayer = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);
        AnimationController();

        if (!seePlayer && isGrounded)
        {
            Petrolling();
        }
    }

    void Petrolling()
    {
        if (!isEdge || isWall)
        {
            if(facingRight)
            {
                Flip();
            }
            else
            {
                Flip();
            }
        }

        enemyRigidBody.velocity = new Vector2(speed * moveDirection, enemyRigidBody.velocity.y);
    }

    void JumpAttack()
    {
        enemyRigidBody.mass = 1.0f;
        float distanceFromPlayer = player.position.x - transform.position.x;

        if (isGrounded)
        {
            enemyRigidBody.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);
        }
        enemyRigidBody.mass = 1000.0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            enemyAnim.SetBool("isHit", true);
            hitSFX.Play();
        }
    }

    void FacePlayer()
    {
        float playerPosition = player.position.x - transform.position.x;

        if (playerPosition < 0 && facingRight)
        {
            Flip();
        }
        else if (playerPosition > 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        moveDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    void AnimationController()
    {
        enemyAnim.SetBool("seePlayer", seePlayer);
        enemyAnim.SetBool("isGrounded", isGrounded);
    }

    protected override void Die()
    {
        enemyRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        enemyAnim.SetBool("isDead", true);
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void UnHit()
    {
        enemyAnim.SetBool("isHit", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, lineOfSight);
    }
}
