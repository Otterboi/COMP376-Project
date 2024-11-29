using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerAnimation : MonoBehaviour
{
    // Reference to player components
    public Animator animator;
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public FadeInOut fadeInOut;

    public GameObject DeathScreenUI;
    private bool canDoubleJump = false; // Double jump flag
    private bool canJump = false;


    // Update is called once per frame
    void Update()
    {

        // Player is shooting
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("isShooting", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isShooting", false);
        }

        /*   // Player jumps (detects press)
           if (Input.GetKeyDown(KeyCode.Space))
           {
               animator.SetBool("isJumping", true);
               if (Input.GetKeyDown(KeyCode.Space))
               { 

               }
           }
           else
           {
               animator.SetBool("isJumping", false);
           }*/


        // Get and set the animator with grounded state
        bool isGrounded = playerMovement.IsGrounded();
        animator.SetBool("isGrounded", isGrounded);

      /*  if (isGrounded)
        {
            canJump = true;
            canDoubleJump = false;
        }

        // Player jumps (detects press)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded && canJump)
            {
                // First jump
                animator.SetTrigger("Jump1");
                canJump = false;
                canDoubleJump = true;
            }
            else if (!isGrounded && canDoubleJump)
            {
                // Double jump
                animator.SetTrigger("Jump2");
                canDoubleJump = false;
            }
        }*/

        if (playerMovement.getJumpCount() == 0 && isGrounded && !canDoubleJump)
        {
            animator.SetTrigger("Jump1");
            animator.SetBool("doubleJump", false);
            canDoubleJump = true;
        } 
        if (playerMovement.getJumpCount() == 1 && canDoubleJump)
        {
            animator.SetBool("doubleJump", true);
            canDoubleJump = false;
        }

        // TODO: Check velocity instead of inputs.
        // PLayer is running/moving
        bool mvntCheck = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
        // bool mvntCheck = !Mathf.Approximately(playerMovement.GetRigidbody().velocity.x, 0);
        animator.SetBool("isRunning", mvntCheck);




        // Player is on the ground
        //animator.SetBool("isGrounded", playerMovement.IsGrounded());
        animator.SetBool("isWallJumping", playerMovement.isWallJumping);
        animator.SetBool("isDead", playerHealth.getCurrentHealth() <= 0);
    }

    private IEnumerator DeathStuff()
    {
        fadeInOut.FadeIn();
        yield return new WaitForSeconds(1.5f);
        DeathScreenUI.SetActive(true);
        Destroy(gameObject);
    }

    private void AstridDeath()
    {
        StartCoroutine(DeathStuff());
    }
}
