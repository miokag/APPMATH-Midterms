using System.Collections;
using UnityEngine;

public class ScatterTowerBehavior : MonoBehaviour
{
    [Header("Rocket")]
    [SerializeField] private GameObject rocket;
    [SerializeField] private float rocketCooldown = 5f;
    [SerializeField] private int rocketCount = 4;
    [SerializeField] private float rocketLifetime = 3f;
    
    private void Start()
    {
        StartCoroutine(FireRocketsRoutine());
    }

    private IEnumerator FireRocketsRoutine()
    {
        while (true)
        {
            InstantiateRocket();
            yield return new WaitForSeconds(rocketCooldown);
        }
    }

    private void InstantiateRocket()
    {
        float angleIncrement = 360f / rocketCount; // Divide 360 degrees by the rocket count
        for (int i = 0; i < rocketCount; i++)
        {
            float angle = i * angleIncrement;

            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector2 spawnPosition = (Vector2)transform.position;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject newRocket = Instantiate(rocket, spawnPosition, rotation);

            Destroy(newRocket, rocketLifetime);
        }
    }

    public void IncreaseRocketCount(int amount)
    {
        rocketCount = Mathf.Min(rocketCount + amount, 8);
    }

    public int RocketCount
    {
        get { return rocketCount; }
    }
}