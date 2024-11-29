using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaLaser : MonoBehaviour
{
    // Mega laser shoot time
    [SerializeField] private float laserTime;

    void Start()
    {
        // Shoots the laser for a specific amount of time then deletes it
        StartCoroutine(LaserTime());
    }


    private IEnumerator LaserTime()
    {
        yield return new WaitForSeconds(laserTime);
        Destroy(gameObject);
    }
}
