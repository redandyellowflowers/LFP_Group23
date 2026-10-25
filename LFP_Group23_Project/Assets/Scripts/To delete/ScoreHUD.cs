using TMPro;
using UnityEngine;

public class ScoreHUD : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    void Awake() { tmp = GetComponent<TextMeshProUGUI>(); }

    void Update()
    {
        if (ScoreManager.Instance != null && tmp != null)
            tmp.text = "Score: " + ScoreManager.Instance.GetScore();
    }
}