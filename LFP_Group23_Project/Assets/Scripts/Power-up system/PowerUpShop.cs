using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PowerUpShop : MonoBehaviour
{
    [Header("What scene to load after shopping")]
    public string nextLevelSceneName;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public Transform offerContainer;     
    public GameObject offerCardPrefab;     

    [Header("Behaviour")]
    public int offersToShow = 3;
    public bool allowReroll = true;
    public int rerollCost = 10;

    private List<PowerUpData> currentOffers = new List<PowerUpData>();
    private int availableScore;

    void Start()
    {
        availableScore = ScoreManager.Instance != null ? ScoreManager.Instance.GetScore() : 0;
        RefreshUI();
        RollOffers();
    }

    void RefreshUI()
    {
        if (scoreText != null)
            scoreText.text = "Points available: " + availableScore;
    }

    void RollOffers()
    {
        foreach (Transform child in offerContainer)
            Destroy(child.gameObject);

        currentOffers.Clear();

        // NEW: null guard so we don't crash if PowerUpManager is missing.
        if (PowerUpManager.Instance == null)
        {
            Debug.LogError("[PowerUpShop] PowerUpManager.Instance is NULL. " +
                           "Make sure there's exactly one PowerUpManager and it lives in the Main Menu scene.");
            return;
        }

        var pool = new List<PowerUpData>(PowerUpManager.Instance.allPowerUps);

        // Filter out ones the player already owns (optional — comment out to allow stacking)
        // pool.RemoveAll(p => PowerUpManager.Instance.HasPowerUp(p));

        for (int i = 0; i < offersToShow && pool.Count > 0; i++)
        {
            int idx = Random.Range(0, pool.Count);
            currentOffers.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        foreach (var offer in currentOffers)
            CreateCard(offer);
    }
    void CreateCard(PowerUpData data)
    {
        GameObject cardGO = Instantiate(offerCardPrefab, offerContainer);
        var card = cardGO.GetComponent<PowerUpCard>();
        if (card == null) { Debug.LogError("offerCardPrefab needs PowerUpCard component"); return; }

        card.Setup(data, CanAfford(data), () => TryBuy(data));
    }

    bool CanAfford(PowerUpData data) => availableScore >= data.cost;
    void TryBuy(PowerUpData data)
    {
        Debug.Log($"[PowerUpShop] TryBuy called for '{data.powerUpName}'. availableScore={availableScore}, cost={data.cost}"); // existing

        if (availableScore < data.cost) { Debug.Log("[PowerUpShop] Can't afford."); return; }   // NEW log

        if (PowerUpManager.Instance == null)                                                    // NEW guard
        {
            Debug.LogError("[PowerUpShop] PowerUpManager.Instance is NULL!");
            return;
        }

        availableScore -= data.cost;
        Debug.Log($"[PowerUpShop] Deducted. availableScore now {availableScore}");               // NEW

        PowerUpManager.Instance.AddPowerUp(data);
        Debug.Log("[PowerUpShop] Called AddPowerUp.");                                           // NEW

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
            ScoreManager.Instance.AddScore(availableScore);
            Debug.Log("[PowerUpShop] ScoreManager updated.");                                    // NEW
        }
        else Debug.LogError("[PowerUpShop] ScoreManager.Instance is NULL!");                     // NEW

        RefreshUI();
        Debug.Log("[PowerUpShop] Refreshed UI.");                                                // NEW

        RollOffers();
        Debug.Log("[PowerUpShop] Rolled offers.");                                               // NEW
    }
    public void Reroll()
    {
        if (PowerUpManager.Instance == null)
        {
            Debug.LogError("[PowerUpShop] Cannot reroll: PowerUpManager.Instance is NULL.");
            return;
        }

        if (!allowReroll || availableScore < rerollCost) return;
    }

    /*   public void Continue()
       {
           // Reset score for the next level (Hades-style — you spend it, then it's gone)
           if (ScoreManager.Instance != null)
               ScoreManager.Instance.ResetScore();

           if (!string.IsNullOrEmpty(nextLevelSceneName))
               SceneManager.LoadScene(nextLevelSceneName);
           else
               SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       }*/

    public void Continue()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();

        // Use the level name EnemyTracker passed us, if any.
        if (!string.IsNullOrEmpty(EnemyTracker.NextLevelAfterShop))
        {
            string target = EnemyTracker.NextLevelAfterShop;
            EnemyTracker.NextLevelAfterShop = ""; // clear so next level starts fresh
            SceneManager.LoadScene(target);
        }
        else if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
            SceneManager.LoadScene(nextLevelSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}