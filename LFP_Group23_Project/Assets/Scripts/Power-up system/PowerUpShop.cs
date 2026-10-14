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
    public Transform offerContainer;       // parent for the 3 cards
    public GameObject offerCardPrefab;     // a prefab with icon, name, desc, cost, buy button

    [Header("Behaviour")]
    public int offersToShow = 3;
    public bool allowReroll = true;
    public int rerollCost = 50;

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
        // Clean old
        foreach (Transform child in offerContainer)
            Destroy(child.gameObject);

        currentOffers.Clear();
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
        if (availableScore < data.cost) return;

        availableScore -= data.cost;
        PowerUpManager.Instance.AddPowerUp(data);

        // Update ScoreManager so the HUD score reflects the spend
        if (ScoreManager.Instance != null)
        {
            // Reset then re-add the remaining score
            ScoreManager.Instance.ResetScore();
            ScoreManager.Instance.AddScore(availableScore);
        }

        RefreshUI();
        RollOffers(); // re-roll remaining offers after a purchase (optional)
    }

    public void Reroll()
    {
        if (!allowReroll || availableScore < rerollCost) return;
        availableScore -= rerollCost;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
            ScoreManager.Instance.AddScore(availableScore);
        }

        RefreshUI();
        RollOffers();
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