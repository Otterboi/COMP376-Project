using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class shoot : MonoBehaviour
{

	// Variables
	public Transform spawnPoint;  // The point where the bullet will be spawned
	public Transform crouchSpawnPoint; // The point where the bullet will be spawned when crouching
	public GameObject bulletPrefab;
	//public float bulletSpeed;
	//private Rigidbody2D rb;
	public float delay;  // Delay before the bullet is shot
	public float destroyDelay;  // Delay before the bullet is destroyed
	private Vector2 facingDirection; // Character direction

	private PlayerMovement playerMovement; // Reference to PlayerMovement script
	private bool canShoot = true;

	[SerializeField] private AudioSource shootSound;
	// Instantiates a bullet after a delay
	// Update is called once per frame

	IEnumerator cooldown()
	{
		yield return new WaitForSeconds(delay);
		canShoot = true;
	}
	void Start()
	{
		playerMovement = GetComponent<PlayerMovement>(); // Get the PlayerMovement component
	}

	public void Shoot(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			if (!canShoot) return;
			shootSound.Play();
			GameObject bullet = Instantiate(
				bulletPrefab,
				playerMovement.isCrouching ? crouchSpawnPoint.position : spawnPoint.position, // choose spawnpoint if player is crouching
				transform.rotation
			); // Spawn bullet

			// Determine direction based on the character's local scale
			// If transform.localScale.x is positive the character is facing right, negative otherwise
			facingDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
			bullet.GetComponent<Bullet>().SetDirection(facingDirection); // Set bullet ditrection
			Debug.Log("Bullet instantiated after delay");
			Destroy(bullet, destroyDelay); // Destroy bullet object
			canShoot = false;
			StartCoroutine(cooldown());
		}
	}

}
