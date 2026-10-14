using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCard : MonoBehaviour
{
    public Image iconImage;
    public Image backgroundImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI costText;
    public Button buyButton;

    private PowerUpData data;

    public void Setup(PowerUpData d, bool canAfford, System.Action onBuy)
    {
        data = d;
        iconImage.sprite = d.icon;
        nameText.text = d.powerUpName;
        descText.text = d.description;
        costText.text = d.cost + " pts";

        if (backgroundImage != null)
            backgroundImage.color = d.RarityColor;

        buyButton.interactable = canAfford;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => onBuy?.Invoke());
    }
}