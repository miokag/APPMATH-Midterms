using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerHP = 5;
    public int gold = 0;
    
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI waveText;

    public GameObject gameOverUI;
    public GameObject shopUI;
    public GameObject goldPrefab;

    private bool isPaused = false;
    private bool isAnimating = false; // Prevents toggling during animation
    public EnemySpawner enemySpawner;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (hpText == null || goldText == null)
        {
            Debug.LogError("Text fields not assigned in the Inspector.");
        }

        //shopUI.transform.localScale = Vector3.zero; // Start with UI hidden
        //shopUI.SetActive(false);
    }

    void Start()
    {
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        UpdateUI();
    }

    public void TakeDamage()
    {
        playerHP--;
        UpdateUI();
        if (playerHP <= 0)
        {
            GameOver();
        }
    }

    public void AddGold(int amount)
    {
        StartCoroutine(LerpGoldUI(gold, gold + amount));
        gold += amount;
    }
    
    private IEnumerator LerpGoldUI(int startValue, int endValue)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int newGold = (int)Mathf.Lerp(startValue, endValue, elapsed / duration);
            goldText.text = "Gold: " + newGold;
            yield return null;
        }

        goldText.text = "Gold: " + endValue;
    }

    public void UpdateUI()
    {
        hpText.text = "HP: " + playerHP;
        goldText.text = "Gold: " + gold;
    }
    
    public void UpdateWaveText(int waveIndex)
    {
        waveText.text = $"Wave: {waveIndex + 1}";
    }

    void GameOver()
    {
        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        hpText.text = "HP: 0";
        gameOverUI.SetActive(true);
    }
}