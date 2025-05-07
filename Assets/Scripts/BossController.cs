using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;
    public float moveSpeed = 1f;
    public float fireInterval = 4f;
    public AudioSource audioSource;
    public AudioClip plasmaSound;
    public int maxHealth = 100;
    private int currentHealth;

    public bool engaged = false;
    public Transform player;

    private float fireTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (engaged && player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            fireTimer += Time.deltaTime;
            if (fireTimer >= fireInterval)
            {
                ShootAtPlayer();
                fireTimer = 0f;
            }
        }
    }

    public void Engage()
    {
        engaged = true;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        fireTimer = fireInterval;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        // Optionally: trigger explosion, play animation, etc.
    }

    void ShootAtPlayer()
    {
        if (player != null && bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.transform.forward = (player.position - firePoint.position).normalized;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = bullet.transform.forward * bulletSpeed;

                audioSource.PlayOneShot(plasmaSound);
        }
    }
}
