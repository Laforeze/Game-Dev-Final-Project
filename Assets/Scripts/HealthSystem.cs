using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Slider healthBar;  // Assign in Inspector
    private int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBar.value = (float)currentHealth / maxHealth;
    }
}
