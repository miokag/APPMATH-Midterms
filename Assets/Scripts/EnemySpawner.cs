using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform doubleCubicSpawn;
    public Transform targetLocation;
    public float quinticSpawnInterval = 6f;
    
    private bool isSpawning = true;

    void Start()
    {
        StartCoroutine(SpawnDoubleCubicEnemies());
    }

    private void Update()
    {
        if (GameManager.Instance.playerHP <= 0) StopSpawning();
    }

    IEnumerator SpawnDoubleCubicEnemies()
    {
        while (isSpawning)
        {
            SpawnEnemy(doubleCubicSpawn, MovementType.DoubleCubic);
            yield return new WaitForSeconds(quinticSpawnInterval);
        }
    }
    
    void SpawnEnemy(Transform spawnPoint, MovementType moveType)
    {
        if (!isSpawning) return;

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