using System.Collections;
using UnityEngine;

public class ScatterTowerBehavior : MonoBehaviour
{
    [Header("Scatter Tower Settings")]
    [SerializeField] private GameObject bulletPrefab; // Regular bullet prefab
    [SerializeField] private float fireInterval = 1f; // Time between shots
    [SerializeField] private int bulletCount = 4; // Number of bullets per shot
    [SerializeField] private float bulletSpeed = 5f; // Speed of bullets
    [SerializeField] private float bulletLifetime = 3f; // Lifetime of bullets
    [SerializeField] private float bulletKillDistance = 0.8f; // Distance at which bullets destroy enemies

    private void Start()
    {
        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            FireBullets();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireBullets()
    {
        float angleIncrement = 360f / bulletCount; // Divide 360 degrees by the bullet count

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleIncrement; // Calculate the angle for each bullet
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)); // Convert angle to direction

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            RegularBulletBehavior bulletBehavior = bullet.GetComponent<RegularBulletBehavior>();

            if (bulletBehavior != null)
            {
                // Assign a target to the bullet
                Transform target = FindClosestEnemy();
                bulletBehavior.Initialize(direction, bulletSpeed, target, bulletKillDistance);
            }

            Destroy(bullet, bulletLifetime); // Destroy bullet after lifetime
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistanceSquared = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float dx = transform.position.x - enemy.transform.position.x;
            float dy = transform.position.y - enemy.transform.position.y;
            float distanceSquared = dx * dx + dy * dy;

            if (distanceSquared < closestDistanceSquared)
            {
                closestEnemy = enemy.transform;
                closestDistanceSquared = distanceSquared;
            }
        }

        return closestEnemy;
    }

    public void IncreaseBulletCount(int amount)
    {
        bulletCount = Mathf.Min(bulletCount + amount, 8); // Limit bullet count to 8
    }

    public int BulletCount
    {
        get { return bulletCount; }
    }
}