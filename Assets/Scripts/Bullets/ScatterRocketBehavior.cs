using UnityEngine;

public class ScatterRocketBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector2 direction;

    void Start()
    {
        direction = transform.right;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
}