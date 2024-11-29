using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{

	// How boss sees player
	[Header("Visualize Player")]
	[SerializeField] private Vector2 lineOfSight;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private GameObject boss;

	// How fast the boss will shoot and what bullet to use
	[Header("Shooting Player")]
	[SerializeField] private GameObject bullet;
	[SerializeField] private Transform bulletSpawn;
	[SerializeField] private float rateOfFire;

	// What the boss needs to shoot mega laser
	[Header("Mega Laser")]
	[SerializeField] private float timeUntilFire;
	[SerializeField] private GameObject laser;
	[SerializeField] private Transform laserSpawn;
	[SerializeField] private AudioSource chargeUpSFX;

	// Helper variables
	private float shootingCooldown;
	private float laserTimer;
	private bool seePlayer;
	private BossHealth bossHP;
	private Animator bossAnimatior;
	private bool isLaserShooting;
	public AudioSource sound;
	

	private void Start()
	{
		// Get boss HP and animator
		bossHP = boss.GetComponent<BossHealth>();
		bossAnimatior = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		// Only do animations when boss is alive
		if (bossHP.Health > 0)
		{
			// Detection box to see if player is in line of sight
			seePlayer = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);

			// If the boss sees the player change to its detected state
			if (seePlayer || isLaserShooting)
			{
				if(seePlayer){
					bossAnimatior.SetBool("seePlayer", true);
				}
				
				ShootAtPlayer();
			}
			else
			{
				// If the boss doesnt see the player reset all cooldown and make it default animation
				sound.Stop();
				shootingCooldown = 0;
				laserTimer = 0;
				bossAnimatior.SetBool("seePlayer", false);
			}
			bossAnimatior.SetBool("isHit", false);
		}
		else
		{
			// When the boss dies it will explode
			bossAnimatior.SetBool("isDead", true);
		}

	}

	// Will shoot at player every specified interval and shoots mega laser
	private void ShootAtPlayer()
	{
		// Timers for both normal shooting and mega laser
		shootingCooldown += Time.deltaTime;
		laserTimer += Time.deltaTime;

		// Shoots at player if the amount of waiting time is over
		if (shootingCooldown > rateOfFire && !isLaserShooting)
		{
			shootingCooldown = 0;
			bossAnimatior.SetBool("isShooting", true);
			Instantiate(bullet, bulletSpawn.position, bullet.transform.rotation);
		}
		else
		{
			bossAnimatior.SetBool("isShooting", false);
		}

		// Shoots mega laser at the player if the amount of waiting time is over while only on the platform
		if (laserTimer > timeUntilFire)
		{
			// sound.Stop();
			if (!sound.isPlaying)
			{
				sound.Play();
			}

			if (!isLaserShooting)
			{
                bossAnimatior.SetBool("isMegaLaser", true);
                isLaserShooting = true;
			}
           
		}
		else
		{
			bossAnimatior.SetBool("isMegaLaser", false);
			isLaserShooting = false;
		}

	}

	// Instanciates the mega laser 
	private void MegaLaser()
	{
        laserTimer = 0;
		chargeUpSFX.Stop();
        bossAnimatior.SetBool("isMegaLaser", false);
		isLaserShooting = false;
        Instantiate(laser, laserSpawn.position, laser.transform.rotation);
	}

	private void PlayMegaLaserSound()
	{
		if(isLaserShooting){
        	chargeUpSFX.Play();
		}
    }

	// Kills the boss
	private void Death()
	{
		Destroy(gameObject);
	}

	// Draws box for editor only to show where the boss can see the player
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, lineOfSight);
	}

	// When the boss gets hit with the players bullet it will take damange and blink
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Bullet"))
		{
			bossAnimatior.SetBool("isHit", true);
			bossHP.DecreaseHP(10);
		}
	}
}
