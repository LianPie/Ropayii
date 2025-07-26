using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int lives = 3;
    public Text livesText;

    public int score = 0;
    public Text scoreText;

    void Start()
    {
        UpdateLivesUI();
        UpdateScoreUI();
    }

    public void LoseLife()
    {
        lives--;

        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            // می‌تونی بازی رو ریست کنی یا صفحه پایان بیاری
            // مثلا:
            // UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        UpdateLivesUI();
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}