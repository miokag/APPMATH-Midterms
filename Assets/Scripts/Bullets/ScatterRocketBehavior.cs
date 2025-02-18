using UnityEngine;

public class ScatterRocketBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector2 direction;
    private Transform targetTransform; // Target to track
    private float killDistance; // Distance at which the rocket destroys the target

    public void Initialize(Vector2 direction, float speed, Transform target, float distance)
    {
        this.direction = direction;
        this.speed = speed;
        targetTransform = target;
        killDistance = distance;
    }

    void Update()
    {
        // Move the rocket
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Check if the rocket hits the target
        if (targetTransform != null && IsTargetHit())
        {
            Debug.Log("Enemy hit by scatter rocket!");
            Destroy(gameObject); // Destroy the rocket
            Destroy(targetTransform.gameObject); // Destroy the target
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddGold(5); // Reward gold
            }
        }

        // Destroy the rocket if it goes out of bounds
        if (transform.position.magnitude > 7f)
        {
            Destroy(gameObject);
        }
    }

    private bool IsTargetHit()
    {
        if (targetTransform == null) return false;

        // Calculate squared distance between the rocket and the target
        float dx = transform.position.x - targetTransform.position.x;
        float dy = transform.position.y - targetTransform.position.y;
        float distanceSquared = dx * dx + dy * dy;

        // Check if the squared distance is within the threshold squared distance
        return distanceSquared <= killDistance * killDistance;
    }
}