using UnityEngine;

public class SeekerTowerBehavior : TowerBehavior
{
    protected override void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (target.position - transform.position).normalized;

        HomingMissiles bulletBehavior = bullet.GetComponent<HomingMissiles>();
        if (bulletBehavior != null)
        {
            bulletBehavior.Initialize(direction, bulletSpeed, target, bulletKillDistance);
        }
    }
}