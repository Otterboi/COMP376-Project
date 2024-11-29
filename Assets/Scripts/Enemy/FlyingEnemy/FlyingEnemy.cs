using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlyingEnemy : EnemyHealth
{
	enum State
	{
		Wait,
		Waiting,
		Patrol,
		PreShoot,
		Shoot,
	}
	public List<GameObject> waypoints = new List<GameObject>();
	private int currentTargetWaypoint = 0;
	[SerializeField] GameObject m_bullet_prefab;
	[SerializeField] Transform bulletSpawn;
	[SerializeField] float m_speed = 1.0f;
	[SerializeField] float m_delay = 1.0f;
	[SerializeField] float m_preshoot_delay = 1.0f;
	[SerializeField] State m_state = State.Wait;

    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask playerLayer;
	[SerializeField] private Transform playerCheck;
	[SerializeField] private AudioSource hitSFX;

    private bool detectPlayer;
    Vector3 direction;
	private float cooldown = 1;
	
	public Animator animator;
	public Rigidbody2D rb;

    IEnumerator DelaySetState(State p, float delay)
	{
        animator.SetBool("shoot", false);
        m_state = State.Waiting;
		yield return new WaitForSeconds(delay);
		m_state = p;
	}

    // Update is called once per frame
    void Update()
	{
		if(currentHealth <= 0)
		{
			return;
		}
        detectPlayer = Physics2D.OverlapCircle(playerCheck.position, detectionRadius, playerLayer);

		cooldown -= Time.deltaTime;

		if (detectPlayer && cooldown <= 0)
		{
			cooldown = 1;
			m_state = State.Shoot;
			animator.SetBool("shoot", true);

        }

        switch (m_state)
		{
			case State.Patrol:
				{
					Vector3 target_pos = waypoints[currentTargetWaypoint].transform.localPosition;
					if (transform.localPosition == target_pos)
					{
						transform.localPosition = target_pos;
						currentTargetWaypoint = (currentTargetWaypoint + 1) % waypoints.Count;
						m_state = State.Wait;

						// Flip Model
						Vector3 tempScale = transform.localScale;
						tempScale.x *= -1;
						transform.localScale = tempScale;
                    }
					else
					{
						transform.Translate((target_pos - transform.localPosition).normalized * Time.deltaTime * m_speed, Space.Self);
                    }
					break;
				}
			case State.Wait:
				{
					StartCoroutine(DelaySetState(State.Patrol, m_delay));
					break;
				}
			case State.PreShoot:
				{
					StartCoroutine(DelaySetState(State.Shoot, m_preshoot_delay));

					break;
				}
			case State.Shoot:
				{
					/* 
					We have two options here:
					Target the player directly, or only shoot at 45 degrees.
					I'm going to target the player directly.
					 */
					GameObject go = Instantiate(m_bullet_prefab, bulletSpawn.position, Quaternion.identity);
					m_state = State.Wait;
					break;
				}
			default:
				break;
		}
    }

    protected override void Die()
    {
        animator.SetBool("death", true);
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Death()
    {
        Destroy(gameObject);
    }

	private void UnHit()
	{
        animator.SetBool("hit", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Bullet"))
		{
			animator.SetBool("hit", true);
            hitSFX.Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
