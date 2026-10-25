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

        if (PowerUpManager.Instance == null)
        {
            Debug.LogError("[PowerUpShop] PowerUpManager.Instance is NULL.");
            return;
        }

        var pool = new List<PowerUpData>(PowerUpManager.Instance.allPowerUps);

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
        if (PowerUpManager.Instance == null) return;

        availableScore -= data.cost;
        PowerUpManager.Instance.AddPowerUp(data);

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
            ScoreManager.Instance.AddScore(availableScore);
        }

        RefreshUI();
        RollOffers();
    }

    public void Reroll()
    {
        if (PowerUpManager.Instance == null) return;
        if (!allowReroll || availableScore < rerollCost) return;
    }

    public void Continue()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();

        if (!string.IsNullOrEmpty(nextLevelSceneName))
            SceneManager.LoadScene(nextLevelSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}