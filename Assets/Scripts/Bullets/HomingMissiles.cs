using UnityEngine;

public class HomingMissiles : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private Transform targetTransform;
    private float killDistance; // This will be set from the turret's upgrade
    private Vector2 spawnPosition;

    public void Initialize(Vector2 direction, float speed, Transform target, float distance)
    {
        moveDirection = direction;
        moveSpeed = speed;
        targetTransform = target;
        killDistance = distance; // Set the kill distance from turret
    }

    void Update()
    {
        if (targetTransform != null)
        {
            moveDirection = (targetTransform.position - transform.position).normalized; // Homing behavior
        }
        
        // Move the bullet in the direction of the target for Homing Missiles
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        // Check if the bullet hits the target
        if (targetTransform != null && IsTargetHit())
        {
            Debug.Log("Enemy hit!");
            Destroy(gameObject); 
            Destroy(targetTransform.gameObject);
            {
                GameObject goldDrop = Instantiate(GameManager.Instance.goldPrefab, transform.position, Quaternion.identity);
                Transform goldUI = GameManager.Instance.goldText.transform;
                goldDrop.GetComponent<GoldDrop>().Initialize(goldUI, 10); 
            }
        }

        if (IsOutOfRange())
        {
            Destroy(gameObject);
        }
    }

    private bool IsTargetHit()
    {
        if (targetTransform == null) return false;

        // Calculate squared distance between the bullet and the target
        float dx = transform.position.x - targetTransform.position.x;
        float dy = transform.position.y - targetTransform.position.y;
        float distanceSquared = dx * dx + dy * dy;

        // Check if the squared distance is within the threshold squared distance
        return distanceSquared <= killDistance * killDistance;
    }

    private bool IsOutOfRange()
    {
        // Calculate squared distance from the spawn position
        float dx = transform.position.x - spawnPosition.x;
        float dy = transform.position.y - spawnPosition.y;
        float distanceSquared = dx * dx + dy * dy;
        
        return distanceSquared > 7f * 7f;
    }
}
