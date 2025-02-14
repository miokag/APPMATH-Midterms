using UnityEngine;

public abstract class TowerBehavior : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected float turretRange = 4f;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float bulletSpeed = 4f;
    [SerializeField] protected float fireInterval = 0.7f;
    [SerializeField] protected float bulletKillDistance = 0.8f;

    protected float fireTimer = 0f;

    protected virtual void Update()
    {
        if (target == null || !IsTargetInRange())
        {
            FindTarget();
        }

        if (target != null)
        {
            RotateTowardsTarget();
            fireTimer += Time.deltaTime;
            CheckRange();
        }
    }

    protected void RotateTowardsTarget()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected void CheckRange()
    {
        if (target == null) return;

        float dx = transform.position.x - target.position.x;
        float dy = transform.position.y - target.position.y;
        float distanceSquared = dx * dx + dy * dy;

        if (distanceSquared <= turretRange * turretRange && fireTimer >= fireInterval)
        {
            FireBullet();
            fireTimer = 0f;
        }
    }

    protected abstract void FireBullet();

    protected void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistanceSquared = turretRange * turretRange;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float dx = transform.position.x - enemy.transform.position.x;
            float dy = transform.position.y - enemy.transform.position.y;
            float distanceSquared = dx * dx + dy * dy;

            if (distanceSquared <= closestDistanceSquared)
            {
                closestEnemy = enemy.transform;
                closestDistanceSquared = distanceSquared;
            }
        }

        target = closestEnemy;
    }

    protected bool IsTargetInRange()
    {
        if (target == null) return false;

        float dx = transform.position.x - target.position.x;
        float dy = transform.position.y - target.position.y;
        float distanceSquared = dx * dx + dy * dy;

        return distanceSquared <= turretRange * turretRange;
    }

    public void UpgradeSpeed(float speedIncrease)
    {
        bulletSpeed += speedIncrease;
    }

    public void UpgradeRange(float rangeIncrease)
    {
        turretRange += rangeIncrease;
    }

    public void UpgradeKillDistance(float distanceIncrease)
    {
        bulletKillDistance += distanceIncrease;
    }
}