using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHUD : MonoBehaviour
{
    [Header("UI Components")]
    public Slider healthBar;
    public Image healthFill; // Reference to Fill Area Image
    public Slider visibilityBar;
    //public TMP_Text ammoText;

    [Header("Player Stats")]
    public int maxHealth = 100;
    public int maxAmmo = 30;
    public float maxVisibility = 100;

    private int currentHealth;
    private int currentAmmo;
    private float currentVisibility;

    public Transform playerTransform;         // Assign in Inspector
    public LayerMask coverLayer;             // Define which layers are "cover" like buildings
    public float coverCheckDistance = 3f;    // How far above to check
    public PlayerMovement playerMovement;    // Reference to check crouch state


    private bool initialized = false;

    public float CurrentVisibility { get; internal set; }

    void Start()
{
    if (initialized) return;
    initialized = true;

    currentHealth = maxHealth;
    currentAmmo = maxAmmo;
    currentVisibility = 0f;

    UpdateHUD();
}

    void UpdateHUD()
{
    float healthPercent = (float)currentHealth / maxHealth;
    healthBar.value = currentHealth;
    visibilityBar.value = currentVisibility;  // ← No normalization here

     //ammoText.text = "Ammo: " + currentAmmo.ToString();

    Color healthColor = Color.green;
    if (currentHealth <= 20)
        healthColor = Color.red;
    else if (currentHealth <= 50)
        healthColor = Color.yellow;

    Image fill = healthBar.fillRect.GetComponent<Image>();
    if (fill != null)
        fill.color = healthColor;
}


void Update()
{
    UpdateVisibilityBasedOnState();

    // TEMP: Press 'H' to simulate taking 10 damage
    if (Input.GetKeyDown(KeyCode.H))
    {
        TakeDamage(10);
        Debug.Log("Player took 10 damage. Current Health: " + currentHealth);
    }
    if (Input.GetKeyDown(KeyCode.G))
    {
        Debug.Log("Miss you Grandpa... RIP Joseph Gilbert Fernandez");
    }
}

void UpdateVisibilityBasedOnState()
{
    Vector3 rayStart = playerTransform.position + Vector3.up * 0.5f;
    bool isUnderCover = Physics.Raycast(rayStart, Vector3.up, out RaycastHit hit, coverCheckDistance, coverLayer);

    Debug.DrawRay(rayStart, Vector3.up * coverCheckDistance, isUnderCover ? Color.green : Color.red);

    //Debug.Log("Under cover: " + isUnderCover);

    bool isCrouching = playerMovement != null && playerMovement.IsCrouching();

    if (isUnderCover)
    {
        currentVisibility = 0;
    }
    else if (isCrouching)
    {
        currentVisibility = 50;
    }
    else
    {
        currentVisibility = 100;
    }

    UpdateHUD();
}



    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHUD();

        if (currentHealth <= 0)
    {
        LoadGameOverScene();
    }
    }

    void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }


       public void UseAmmo(int amount)
    {
        //Debug.Log("Using ammo: -" + amount);
        currentAmmo -= amount;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        UpdateHUD();
    }


    public void ReloadAmmo()
{
    //Debug.Log("Reloading ammo");
    currentAmmo = maxAmmo;
    UpdateHUD();
}


    public void UpdateVisibility(float amount)
    {
        currentVisibility = Mathf.Clamp(amount, 0f, maxVisibility);
        UpdateHUD();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHUD();
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

}
