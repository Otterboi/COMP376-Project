using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private bool isDamageOverTime = false;
    [SerializeField] private float damageInterval = 1f;

    [Header("Hazard Behavior")]
    [SerializeField] private bool isMoving = false;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector2 moveDirection = Vector2.right;
    [SerializeField] private float moveDistance = 5f;

    private Vector2 startPosition;
    private float timeSinceLastDamage;
    private bool playerInHazard;

    void Start()
    {
        startPosition = transform.position;
        timeSinceLastDamage = 0f;
    }

    void Update()
    {
        if (isMoving)
        {
            // Calculate movement
            float newPosition = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = startPosition + (moveDirection * newPosition);
        }

        // Handle continuous damage
        if (isDamageOverTime && playerInHazard)
        {
            timeSinceLastDamage += Time.deltaTime;
            if (timeSinceLastDamage >= damageInterval)
            {
                DamagePlayer();
                timeSinceLastDamage = 0f;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInHazard = true;
            if (!isDamageOverTime)
            {
                DamagePlayer(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInHazard = false;
            timeSinceLastDamage = 0f;
        }
    }

    private void DamagePlayer(GameObject player = null)
    {
        // If no player reference is passed, try to find the player in the hazard trigger
        if (player == null && playerInHazard)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"Player took {damageAmount} damage from hazard");
            }
        }
    }

    // visualization of hazard area
    private void OnDrawGizmos()
    {
        if (isMoving)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>()?.bounds.size ?? Vector3.one);

            // Draw movement path
            Gizmos.color = Color.yellow;
            Vector3 leftPoint = startPosition - (Vector2)(moveDirection * moveDistance);
            Vector3 rightPoint = startPosition + (Vector2)(moveDirection * moveDistance);
            Gizmos.DrawLine(leftPoint, rightPoint);
        }
    }
}