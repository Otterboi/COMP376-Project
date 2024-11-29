using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingIfOnPlatform : MonoBehaviour
{
    public float speed = 1f; // Platform speed
    public float offset = 0.625f; // Offset to ensure the player is fully on top of the platform
    public float moveDistance = 5f; // How much the platform moves to reach max height
    private bool playerOnTop = false;
    private Vector2 originalPosition; // Platform position
    private float maxHeight; // Maximum height the platform can reach

    void Start()
    {
        originalPosition = transform.position; // Store the original position of the platform
        maxHeight = originalPosition.y + moveDistance; // Calculate the maximum height
    }

    void Update()
    {
        // Move the platform straight up if the player is on top
        if (playerOnTop && transform.position.y < maxHeight)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else if (!playerOnTop && transform.position.y > originalPosition.y)
        {
            // Move the platform back to its original position
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.tag == "Player")
        {
            // For each collision, check if higher than platform plus offset
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.point.y > transform.position.y + offset)
                {
                    playerOnTop = true;
/*                    collision.transform.SetParent(transform); // Make the player a child of the platform*/
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the exiting object is the player
        if (collision.gameObject.tag == "Player")
        {
            playerOnTop = false; // Set the flag to false when the player leaves the platform
/*            collision.transform.SetParent(null); // Remove the player as a child of the platform*/
        }
    }
}