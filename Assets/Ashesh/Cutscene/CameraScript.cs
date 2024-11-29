using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Camera script manually set to move in directed position

public class CameraScript : MonoBehaviour
{
    // Public variables to adjust the speed and duration of movements
    public float zoomSpeed = 1f;
    public float moveSpeed = 1.25f;
    public float zoomAmount = 4.5f;
    public float moveUpDuration = 2f;
    public float moveRightDuration = 1f;
    public float pauseDurationUp = 9f;
    public float pauseDuration = 4f; // Duration to pause at each stop
    public float pauseRightDuration = 3.5f; // Duration to pause after moving right
    public float additionalMoveRightDuration = 3f; // Duration for additional right movement
    public Vector3 zoomLocation; // Target position for the zoom

    private Camera cam; // Reference to the Camera component

    // Flags and timers to control camera movement
    private bool isZooming = false;
    private bool isMovingUp = false;
    private bool isMovingRight = false;
    private bool isMovingRightAgain = false;
    private float zoomTime = 0f;
    private float moveUpTime = 0f;
    private float moveRightTime = 0f;
    private float additionalMoveRightTime = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Camera actions
        if (isZooming)
        {
            ZoomIn();
        }
        else if (isMovingUp)
        {
            MoveUp();
        }
        else if (isMovingRight)
        {
            MoveRight();
        }
        else if (isMovingRightAgain)
        {
            MoveRightAgain();
        }
    }

    // Method to start the camera sequence
    public void StartCameraSequence()
    {
        isZooming = true;
    }

    // Method to zoom in the camera towards a target position
    void ZoomIn()
    {
        // Move the camera towards the target position over time
        if (zoomTime < zoomAmount)
        {
            transform.position = Vector3.MoveTowards(transform.position, zoomLocation, zoomSpeed * Time.deltaTime);
            cam.orthographicSize -= zoomSpeed * Time.deltaTime;
            zoomTime += zoomSpeed * Time.deltaTime;
        }
        else
        {
            // Stop zooming and pause before moving upwards
            isZooming = false;
            StartCoroutine(PauseBeforeMovingUp());
        }
    }

    // Pause before moving upwards
    IEnumerator PauseBeforeMovingUp()
    {
        yield return new WaitForSeconds(pauseDurationUp);
        isMovingUp = true;
    }

    // Method to move the camera upwards
    void MoveUp()
    {
        // Moving upwards over time
        if (moveUpTime < moveUpDuration)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            moveUpTime += Time.deltaTime;
        }
        else
        {
            // Stop moving upwards and pause before moving rightwards
            isMovingUp = false;
            StartCoroutine(PauseBeforeMovingRight());
        }
    }

    // Pause before moving rightwards
    IEnumerator PauseBeforeMovingRight()
    {
        yield return new WaitForSeconds(pauseDuration);
        isMovingRight = true;
    }

    // Method to move the camera rightwards
    void MoveRight()
    {
        // Move camera right over time
        if (moveRightTime < moveRightDuration)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            moveRightTime += Time.deltaTime;
        }
        else
        {
            // Stop moving rightwards and pause
            isMovingRight = false;
            StartCoroutine(PauseAfterMovingRight());
        }
    }

    // Coroutine to pause after moving rightwards
    IEnumerator PauseAfterMovingRight()
    {
        yield return new WaitForSeconds(pauseRightDuration);
        isMovingRightAgain = true;
    }

    // Method to move the camera rightwards again
    void MoveRightAgain()
    {
        // Move camera right over time
        if (additionalMoveRightTime < additionalMoveRightDuration)
        {
            transform.Translate(Vector3.right * (moveSpeed * 2.25f) * Time.deltaTime); // Running speed
            additionalMoveRightTime += Time.deltaTime;
        }
        else
        {
            // Stop moving rightwards again
            isMovingRightAgain = false;
        }
    }
}