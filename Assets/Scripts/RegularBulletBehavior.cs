using UnityEngine;

public class RegularBulletBehavior : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private Transform targetTransform;
    private float killDistance; // This will be set from the turret's upgrade

    public void Initialize(Vector2 direction, float speed, Transform target, float distance)
    {
        moveDirection = direction;
        moveSpeed = speed;
        targetTransform = target;
        killDistance = distance; // Set the kill distance from turret
    }

    void Update()
    {
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        // Check if the bullet hits the target
        if (targetTransform != null && IsTargetHit())
        {
            Debug.Log("Enemy hit!");
            Destroy(gameObject); 
            Destroy(targetTransform.gameObject);
            // if (GameManager.Instance != null)  
            // {
            //     GameManager.Instance.AddGold(5); 
            // }
        }

        if (transform.position.magnitude > 7f)
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
}
