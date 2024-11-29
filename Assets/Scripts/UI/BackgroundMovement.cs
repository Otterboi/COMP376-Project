using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{

    public float speed;  // Speed of background movement                
    public float amount; // Amount of movement
    private Vector3 startPos; // Starting position 
    private bool isMoving = true; // Turn off after panel is open

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position; // Store the initial position of the object
    }

    // Update is called once per frame
    void Update()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {                                  
            isMoving = false; // Stop background movement
        }

        // Move the Screen if isMoving is true
        if (isMoving)
        {
            // Calculate the new X position using a sine wave
            float newX = startPos.x + Mathf.Sin(Time.time * speed) * amount;

            // Update the position of the object
            transform.position = new Vector3(newX, startPos.y, startPos.z);
        }
    }
}
