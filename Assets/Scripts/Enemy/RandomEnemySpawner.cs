using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : MonoBehaviour
{
   
    public GameObject[] enemyPrefabs; // Array to hold different enemy prefabs   
    public Transform[] spawnPoints; // Array to hold spawn points in the scene
    public int totalEnemiesToSpawn; // Number of enemies to spawn

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies();
    }

    // Method to spawn enemies at random spawn points
    void SpawnEnemies()
    {
        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            // Randomly select a spawn point from the array
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Randomly select an enemy prefab from the array
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            Vector3 spawnPosition = new Vector2(spawnPoint.position.x, spawnPoint.position.y); // Ensure Z is 0

            // Instantiate the enemy prefab at the selected spawn point
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation); ;

            Debug.Log("Enemy spawned at: " + spawnPoint.position);
        }
    }
}
