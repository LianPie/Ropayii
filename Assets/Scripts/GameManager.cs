using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager Instance;

    public GameObject Ball;
    public GameObject DropPoint;

    public int lives = 3;
    public TMP_Text livesText;

    public int score = 0;
    private int nextLifeThreshold = 100; // First life at 100 points

    public TMP_Text scoreText;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateLivesUI();
        UpdateScoreUI();
    }

    public void GainLife()
    {
        lives++;

        UpdateLivesUI();
    }
    public void LoseLife()
    {
        // می‌تونی بازی رو ریست کنی یا صفحه پایان بیاری
        // مثلا:
        lives--;

        if (lives > 0)
        {
            Debug.Log("lost a life");
            Ball.transform.position = DropPoint.transform.position;
        }
        else
        {
            Debug.Log("gameOver");
            //UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        }

        UpdateLivesUI();
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = lives.ToString();
        }
    }

    public void AddScore(int amount = 1)
    {
        if (score != 999)
        {
            score += amount;

            // Check if we've reached or passed the life threshold
            while (score >= nextLifeThreshold)
            {
                GainLife();
                nextLifeThreshold += 100; // Set next threshold 100 points ahead
            }
        }
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}