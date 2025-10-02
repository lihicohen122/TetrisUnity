using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")] [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TMP_Text highScoreText;


    private int score;


    //highscore board sutff some times its shit sometimes is not
    const string HighScoreKey = "HighScore";
    public int Score { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    public void AddScore(int linesCleared)
    {
        int points = 0;

        if (linesCleared == 1)
        {
            points = 100;
        }
        else if (linesCleared == 2)
        {
            points = 300;
        }
        else if (linesCleared == 3)
        {
            points = 500;
        }
        else if (linesCleared >= 4)
        {
            points = 800;
        }
        //bonus for clearing more than two lines- in addition to points
        if (linesCleared >= 2)
        {
            points += 200;
        }

        score += points;
        Score = score;

        int hs = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (score > hs)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText) scoreText.text = score.ToString();
        if (highScoreText) highScoreText.text = PlayerPrefs.GetInt(HighScoreKey, 0).ToString();
    }

}