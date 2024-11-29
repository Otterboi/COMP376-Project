using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce; // Collision Force

    public void ApplyKnockback(Transform player)
    {
        Vector2 knockbackDirection = (player.position - transform.position).normalized;

        // Minimum threshold for horizontal force
        float horizontalForce = Mathf.Sign(knockbackDirection.x) * knockbackForce;
        if (Mathf.Abs(horizontalForce) < 0.1f)
        {
            horizontalForce = 0.1f * Mathf.Sign(horizontalForce);
        }

        // Set vertical force to 75% of the horizontal force
        float verticalForce = Mathf.Abs(horizontalForce) * 0.75f * Mathf.Sign(knockbackDirection.y);

        Vector2 combinedForce = new Vector2(horizontalForce, verticalForce);

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            Debug.Log("Applying knockback to player with direction: " + combinedForce);
            playerMovement.ApplyExternalForce(combinedForce); // Changes exteral force vaiable in playerMovement
        }
    }
}