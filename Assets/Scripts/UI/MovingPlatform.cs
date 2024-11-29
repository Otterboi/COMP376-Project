using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Points platform travels between 
    public Transform pointA;
    public Transform pointB;
    public Transform platform;

    public float speed = 1f; // Movement speed
    int direction = 1; // Platform direction
    private Transform playerTransform; // Reference to the player's transform
    private Rigidbody2D playerRigidbody; // Reference to the player's Rigidbody2D
    private Vector2 previousPosition; // Store the platform's previous position

    void Start()
    {
        platform.position = pointA.position; // Starting position
        previousPosition = platform.position; // Initialize previous position
        Debug.Log("Starting at PointA: " + pointA.position);
    }

    void Update()
    {
        // Move platform to the target position
        Vector2 target = platformLocation();

        // Interpolate between two vectors
        platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);

        // Calculate the movement of the platform
        Vector2 movement = (Vector2)platform.position - previousPosition;

        // Move the player along with the platform if they are not actively moving
        if (playerTransform != null && playerRigidbody != null)
        {
            if (Mathf.Abs(playerRigidbody.velocity.x) < 0.1f) // Check if the player is not moving horizontally
            {
                playerTransform.position += (Vector3)movement;
            }
        }

        // Update the previous position
        previousPosition = platform.position;

        // Change direction
        float distance = (target - (Vector2)platform.position).magnitude;;
        if (distance <= 0.1f)
        {
            direction *= -1;
        }
    }

    // Method to return target location
    Vector2 platformLocation()
    {
        if (direction == 1)
        {
            return pointB.position; // Move towards pointB
        }
        else
        {
            return pointA.position; // Move towards pointA
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.tag == "Player")
        {
            playerTransform = collision.transform; // Store the player's transform
            playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>(); // Store the player's Rigidbody2D
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the exiting object is the player
        if (collision.gameObject.tag == "Player")
        {
            playerTransform = null; // Clear the player's transform reference
            playerRigidbody = null; // Clear the player's Rigidbody2D reference
        }
    }
}