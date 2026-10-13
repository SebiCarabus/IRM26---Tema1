using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;

    private int totalScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        UpdateText();
    }

    public void AddScore(int points)
    {
        totalScore += points;
        UpdateText();
    }

    public void ResetScore()
    {
        totalScore = 0;
        UpdateText();
    }

    private void UpdateText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore;
        }
    }
}