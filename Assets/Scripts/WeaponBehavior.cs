using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Camera playerCamera;

    public int maxAmmo = 30;
    public float fireRate = 0.1f;
    public TextMeshProUGUI ammoText;

    private int currentAmmo;
    private float nextTimeToFire = 0f;

    public PlayerHUD playerHUD;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R)) // Optional: Reload
        {
            Reload();
            audioSource.PlayOneShot(reloadSound);
        }
    }

     void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        bullet.transform.forward = playerCamera.transform.forward;
        bullet.GetComponent<Rigidbody>().linearVelocity = bullet.transform.forward * 10;

        currentAmmo--;

        if (playerHUD != null)
        {
            playerHUD.UseAmmo(1); // Update HUD
        }

        UpdateAmmoDisplay();

        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20f);
        foreach (Collider col in hitColliders)
        {
            EnemyPatrol patrol = col.GetComponent<EnemyPatrol>();
            if (patrol != null)
            {
                patrol.Investigate(transform.position);
            }
        }
    }

    void Reload()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();
    }

    void UpdateAmmoDisplay()
    {
        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {currentAmmo}";
        }
    }
}
