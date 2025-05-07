using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    [Header("Boss Setup")]
    public GameObject[] bossPrefabs;
    public Transform spawnPoint;
    public int bossMaxHealth = 100;

    private GameObject currentBoss;
    private int currentHealth;

    void Start()
    {
        SpawnRandomBoss();
    }

    void SpawnRandomBoss()
    {
        int index = Random.Range(0, bossPrefabs.Length);
        
        currentHealth = bossMaxHealth;
    }

    public void DamageBoss(int amount)
    {
        if (currentBoss == null) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            DefeatBoss();
        }
    }

    void DefeatBoss()
    {
        Destroy(currentBoss);
        LoadVictoryScene();
    }

    void LoadVictoryScene()
    {
        SceneManager.LoadScene("Victory");
    }

    
}
