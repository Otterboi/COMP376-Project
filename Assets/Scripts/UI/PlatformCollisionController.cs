using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollisionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Find all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Ignore collisions with all enemies
        Collider2D platformCollider = GetComponent<Collider2D>();

        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();

            if (platformCollider != null && enemyCollider != null)
            {
                Physics2D.IgnoreCollision(platformCollider, enemyCollider);
            }
        }
    }
}
