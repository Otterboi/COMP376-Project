using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Vector2 offset; // Offset to control camera position
    public float panSpeed; // Speed at which the camera pans

    private Vector2 velocity = Vector2.zero;

    void Update()
    {
        if (player != null)
        {
          
            Vector2 desiredPosition = (Vector2)player.position + offset;  // Calculate the desired position           
            Vector2 playerDirection = player.GetComponent<Rigidbody2D>().velocity.normalized;  // Get the player's direction
            
            // Adjust the camera's position based on the player's direction
            Vector2 panOffset = playerDirection * panSpeed;
            desiredPosition += panOffset;

            // Move the camera to the desired position
            Vector2 movePosition = Vector2.SmoothDamp((Vector2)transform.position, desiredPosition, ref velocity, 0.3f);
            transform.position = new Vector3(movePosition.x, movePosition.y, transform.position.z);

        }
    }
}
