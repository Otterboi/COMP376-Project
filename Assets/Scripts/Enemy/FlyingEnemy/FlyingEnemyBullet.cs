using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyBullet : MonoBehaviour
{
    // How fast the buller moves
    [SerializeField] private float speed;

    // Gets player and bullet rigidbody
    private Rigidbody2D rb;
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Bullet moves to last location the players was
        Vector2 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }

    // When the bullet hits anything that isnt the boss it will be destroyed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false); // Make bullets invisible
            Destroy(gameObject, 0.3f); // Added a small delay
        }
    }
}
