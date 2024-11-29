// Author: Joseph Pagliuca - 40092947

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    // Public variables for player movement, speed, double jumping and wall sliding/jumping.
    [Header("Movement Properties")]
    [SerializeField] public float speed = 10f;
    [SerializeField] int maxJumps = 2;
    [SerializeField] public float jumpingPower = 16f;
    [SerializeField] public float doubleJumpPower = 30f;
    [SerializeField] public float wallSlidingSpeed = 2;
    [SerializeField] public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // Player movement and sprite changing variabales.
    private float horizontal;
    private bool isFaceingRight = false;
    private bool doubleJump;
    private int jumpCount = 0;

    // Variables for wall sliding and jumping.
    [Header("Wall Jump")]
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    private bool isWallSliding;
    public bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;


    // Varaibels for dashing.
    [Header("Dashing")]
    [SerializeField] public float dashingPower = 10f;
    [SerializeField] public float dashingTime = 0.1f;
    [SerializeField] public float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;
    float ogGravity;


    // Gets components from unity editor.
    [Header("Unity Editor Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer trailRenderer;

    // Variables for crouching
    [Header("Crouching")]
    [SerializeField] public Vector2 crouchColliderSize; // Set in inspector
    [SerializeField] public Vector2 crouchColliderOffset; // Set in inspector
    public bool isCrouching;
    private BoxCollider2D boxCollider;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;



    private Vector2 externalForce; // collision force 
    private float externalForceDuration; // Knockback duration
    private float externalForceTimer;

    private PlayerHealth playerHealth;
    #endregion



    void Start()
    {
        // Store the original collider size and offset
        boxCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = boxCollider.size;
        originalColliderOffset = boxCollider.offset;
        Debug.Log("Original Collider Size: " + originalColliderSize); // Use to determine crouch size
        Debug.Log("Original Collider Offset: " + originalColliderOffset); // Use to determine crouch offset
        trailRenderer.sortingLayerName = "Tiles";
        playerHealth = GetComponent<PlayerHealth>();
        ogGravity = rb.gravityScale;
    }

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    // Movement Functions
    #region MovementFunctions
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Allows the player to wall jump.
            if (isWallSliding)
            {
                // Only lets the player wall jump if they are already sliding on a wall.
                // When they jump off a wall it will launch them in the opposite direction of where the player is looking at.
                // Ex: If they are looking right and jump off the wall, the sprite will turn left and jump in that directing.
                if (wallJumpingCounter > 0f)
                {
                    isWallJumping = true;
                    rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
                    wallJumpingCounter = 0f;

                    if (transform.localScale.x != wallJumpingDirection)
                    {
                        isFaceingRight = !isFaceingRight;
                        Vector3 localScale = transform.localScale;
                        localScale.x *= -1f;
                        transform.localScale = localScale;
                    }
                    Invoke(nameof(StopWallJumping), wallJumpingDuration);
                }
                // TODO: Does walljump count as one of your jumps?
                jumpCount++;
            }
            else
            {
                if (jumpCount < maxJumps - 1)
                {
                    isDashing = false;
                    // _EndDash(); // Is this necessary?
                    rb.velocity = new Vector2(rb.velocity.x, jumpCount > 0 ? doubleJumpPower : jumpingPower);
                    jumpCount++;
                }
            }
        }

    }
    // Allows the player to wall slide.
    private void WallSlide()
    {
        // As the player is touching a wall, they will slowly slide down the wall until they reach the bottom.
        if (IsWalled() && !IsGrounded())// && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            wallJumpingCounter -= Time.deltaTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            isWallSliding = false;
        }
    }


    // Sets the ability to wall jump to false.
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    // Flips the sprite depending on what direction they are looking at.
    private void Flip()
    {
        if (isFaceingRight && horizontal < 0f || !isFaceingRight && horizontal > 0f)
        {
            isFaceingRight = !isFaceingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

    }

    // Coroutine that allows the player to dash whenever they press the dash button.
    // Ignores all gravity when dashing until the end of the dashing time.
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!canDash) return;
            canDash = false;
            isDashing = true;

            rb.gravityScale = 0f;
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
            trailRenderer.emitting = true;
            StartCoroutine(_DashCooldown());
        }
    }
    private IEnumerator _DashCooldown()
    {
        yield return new WaitForSeconds(dashingTime);
        _EndDash();
    }
    private void _EndDash()
    {
        trailRenderer.emitting = false;
        rb.gravityScale = ogGravity;
        isDashing = false;
    }
    // Change the player's collider size
    private void Crouch()
    {
        isCrouching = true;

        // Adjust the player's collider size and offset for crouching
        boxCollider.size = crouchColliderSize;
        boxCollider.offset = crouchColliderOffset;
        // Trigger crouch animation
        GetComponent<Animator>().SetBool("isCrouching", true);
        Debug.Log("Crouch Collider Size: " + boxCollider.size);
        Debug.Log("Crouch Collider Offset: " + boxCollider.offset);
    }

    private void StandUp()
    {
        isCrouching = false;
        // Reset the player's collider size and offset to original
        boxCollider.size = originalColliderSize;
        boxCollider.offset = originalColliderOffset;
        // Trigger stand up animation
        GetComponent<Animator>().SetBool("isCrouching", false);
        Debug.Log("Reset Collider Size: " + boxCollider.size);
        Debug.Log("Reset Collider Offset: " + boxCollider.offset);
    }
    #endregion

    void Update()
    {

        // Reset dashing and double jump if the player is on the ground
        if (playerHealth.getCurrentHealth() <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (IsGrounded())
        {
            canDash = true;
            jumpCount = 0;
            doubleJump = true;
        }


        // Moves left and right by pressing A or D on the keyboard.
        horizontal = Input.GetAxisRaw("Horizontal");
        // Wall sliding functionality.
        WallSlide();
        // Stop sprit flipping from happeneing if the player is wallJumping.
        if (!isWallJumping)
        {
            Flip();
        }
    }




    // Method to apply collision force
    public void ApplyExternalForce(Vector2 force)
    {
        // Set the external force to be applied in the next FixedUpdate
        externalForce = force;
        Debug.Log("External Force Applied: " + force);
    }

    private void FixedUpdate()
    {
        // If player is dashing don't let them do any other movement.
        if (isDashing)
        {
            return;
        }

        // Apply external force if it's set
        if (externalForce != Vector2.zero)
        {
            Debug.Log("Before Applying External Force: " + rb.velocity);
            rb.velocity += externalForce; // Apply the external force
            Debug.Log("Applying External Force: " + externalForce + ", New Velocity: " + rb.velocity);

            // Gradually reduce the external force
            externalForce = Vector2.Lerp(externalForce, Vector2.zero, Time.fixedDeltaTime * 40f);
            Debug.Log("After Reducing External Force: " + externalForce);
        }
        else
        {
            // Lets the player move left and right if they're not wall jumping.
            if (!isWallJumping)
            {
                Vector2 movement = new Vector2(horizontal * speed, rb.velocity.y);
                rb.velocity = movement;
            }
        }
    }

    // Checks if the player is grounded using a ground check object.
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundlayer);
    }

    // Checks if the player is touching a wall using a wall check object.
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    public int getJumpCount() { return jumpCount; }


    // This should probably go in the Teleporter code
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Teleporter"))
        {
            collision.gameObject.GetComponent<Teleporter>().teleporterAnim.SetBool("teleport", true);
        }
    }

}
