using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;       // Single spawn point for simplicity
    public Transform targetLocation;
    public List<Wave> waves;           // List of waves
    public float timeBetweenWaves = 5f; // Time between waves

    private int currentWaveIndex = 0;
    private bool isSpawning = true;

    void Start()
    {
        if (waves.Count > 0)
        {
            StartCoroutine(SpawnWaves());
        }
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count && isSpawning)
        {
            Wave currentWave = waves[currentWaveIndex];
            GameManager.Instance.UpdateWaveText(currentWaveIndex);
            Debug.Log($"Starting Wave {currentWaveIndex + 1}");

            for (int i = 0; i < currentWave.enemyCount; i++)
            {
                if (!isSpawning) yield break; // Stop spawning if the game is over

                SpawnEnemy(spawnPoint, currentWave.movementType);
                yield return new WaitForSeconds(currentWave.spawnInterval);
            }

            Debug.Log($"Wave {currentWaveIndex + 1} Completed");
            currentWaveIndex++;

            if (currentWaveIndex < waves.Count)
            {
                yield return new WaitForSeconds(timeBetweenWaves); // Wait before starting the next wave
            }
        }

        Debug.Log("All Waves Completed");
    }

    void SpawnEnemy(Transform spawnPoint, MovementType moveType)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

        if (movement != null)
        {
            movement.Initialize(targetLocation, moveType);
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
}