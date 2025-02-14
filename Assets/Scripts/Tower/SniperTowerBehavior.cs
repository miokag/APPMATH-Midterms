using UnityEngine;

public class SniperTowerBehavior : TowerBehavior
{
    protected override void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (target.position - transform.position).normalized;

        RegularBulletBehavior bulletBehavior = bullet.GetComponent<RegularBulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.Initialize(direction, bulletSpeed, target, bulletKillDistance);
        }
    }
}