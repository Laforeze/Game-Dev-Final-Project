using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BossBullet : MonoBehaviour
{
    public float speed = 10f;         // Slow speed
    public float lifetime = 8f;       // Long enough to be visible
    public int damage = 50;           // High damage

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Launch forward
        rb.linearVelocity = transform.forward * speed;

        // Auto-destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check for health and apply damage
        Health target = other.GetComponent<Health>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // Destroy the bullet on contact with anything
        Destroy(gameObject);
    }
}
