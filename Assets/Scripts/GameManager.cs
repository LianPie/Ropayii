using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager Instance;

    public GameObject Ball;
    public GameObject DropPoint;
    public SpriteRenderer ballSpriteRenderer;

    public int lives = 3;
    public TMP_Text livesText;

    public int score = 0;
    public TMP_Text scoreText;

    // Menu system variables
    public GameObject menuPanel;
    public Button startButton;
    public Button skinPackButton;
    public TMP_Text menuTitleText;

    // Skin pack system variables
    public GameObject skinPackMenu;
    public Button[] skinPackButtons;
    public Button backButton;
    public GameObject[] packOwnedIcons; // Checkmarks for owned packs

    // Skin pack data
    [System.Serializable]
    public class SkinPack
    {
        public string packName;
        public Sprite stage1Ball; // 0 points
        public Sprite stage2Ball; // 10 points  
        public Sprite stage3Ball; // 20 points
        public bool isPurchased; // Set to true after IAP
    }

    public SkinPack[] skinPacks = new SkinPack[3]; // 3 packs in inspector

    private int currentPackIndex = -1; // -1 = default pack
    private bool gameStarted = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (ballSpriteRenderer == null && Ball != null)
        {
            ballSpriteRenderer = Ball.GetComponent<SpriteRenderer>();
        }

        LoadPurchasedPacks();
        ShowMenu("Start Game");
        SetupButtonListeners();
        UpdatePackUI();
    }

    private void LoadPurchasedPacks()
    {
        // Load purchased status from IAP system or PlayerPrefs
        for (int i = 0; i < skinPacks.Length; i++)
        {
            // For testing, you can set defaults here
            // In real game, this would come from IAP system
            skinPacks[i].isPurchased = PlayerPrefs.GetInt($"PackPurchased_{i}", 0) == 1;
        }
    }

    public void UnlockPack(int packIndex)
    {
        if (packIndex >= 0 && packIndex < skinPacks.Length)
        {
            skinPacks[packIndex].isPurchased = true;
            PlayerPrefs.SetInt($"PackPurchased_{packIndex}", 1);
            PlayerPrefs.Save();
            UpdatePackUI();
        }
    }

    private void SetupButtonListeners()
    {
        // Main menu buttons
        startButton?.onClick.RemoveAllListeners();
        startButton?.onClick.AddListener(StartGame);

        skinPackButton?.onClick.RemoveAllListeners();
        skinPackButton?.onClick.AddListener(ShowSkinPackMenu);

        // Navigation buttons
        backButton?.onClick.RemoveAllListeners();
        backButton?.onClick.AddListener(ShowMainMenu);

        // Skin pack selection buttons
        for (int i = 0; i < skinPackButtons.Length; i++)
        {
            int packIndex = i;
            skinPackButtons[i].onClick.RemoveAllListeners();
            skinPackButtons[i].onClick.AddListener(() => SelectPack(packIndex));
        }
    }

    private void UpdatePackUI()
    {
        for (int i = 0; i < skinPacks.Length; i++)
        {
            var pack = skinPacks[i];

            // Update owned icons
            if (i < packOwnedIcons.Length && packOwnedIcons[i] != null)
            {
                packOwnedIcons[i].SetActive(pack.isPurchased);
            }

            // Enable/disable pack buttons based on purchase status
            if (i < skinPackButtons.Length)
            {
                skinPackButtons[i].interactable = pack.isPurchased;
            }
        }
    }

    public void ShowMenu(string title = "Game Over")
    {
        gameStarted = false;
        Time.timeScale = 0f;

        menuPanel?.SetActive(true);
        skinPackMenu?.SetActive(false);

        menuTitleText?.SetText(title);
    }

    public void ShowSkinPackMenu()
    {
        menuPanel?.SetActive(false);
        skinPackMenu?.SetActive(true);
        UpdatePackUI();
    }

    public void ShowMainMenu()
    {
        skinPackMenu?.SetActive(false);
        menuPanel?.SetActive(true);
    }

    public void SelectPack(int packIndex)
    {
        if (packIndex < 0 || packIndex >= skinPacks.Length) return;

        var pack = skinPacks[packIndex];

        if (pack.isPurchased)
        {
            currentPackIndex = packIndex;
            ApplyStage1Ball(); // Start with stage 1 ball
            ShowMainMenu();
        }
    }

    private void UpdateBallSprite()
    {
        if (currentPackIndex < 0 || ballSpriteRenderer == null) return;

        var pack = skinPacks[currentPackIndex];

        if (score >= 20 && pack.stage3Ball != null)
        {
            ballSpriteRenderer.sprite = pack.stage3Ball;
        }
        else if (score >= 10 && pack.stage2Ball != null)
        {
            ballSpriteRenderer.sprite = pack.stage2Ball;
        }
        else if (pack.stage1Ball != null)
        {
            ballSpriteRenderer.sprite = pack.stage1Ball;
        }
    }

    private void ApplyStage1Ball()
    {
        if (currentPackIndex < 0 || ballSpriteRenderer == null) return;

        var pack = skinPacks[currentPackIndex];
        if (pack.stage1Ball != null)
        {
            ballSpriteRenderer.sprite = pack.stage1Ball;
        }
    }

    public void StartGame()
    {
        if (currentPackIndex < 0)
        {
            Debug.Log("Please select a skin pack first!");
            return;
        }

        Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * 10f;

        lives = 3;
        score = 0;

        menuPanel?.SetActive(false);
        skinPackMenu?.SetActive(false);

        Time.timeScale = 1f;
        gameStarted = true;

        if (Ball != null && DropPoint != null)
        {
            Ball.transform.position = DropPoint.transform.position;
        }

        ApplyStage1Ball(); // Reset to stage 1 ball
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
        lives--;

        if (lives > 0)
        {
            Debug.Log("lost a life");
            if (Ball != null && DropPoint != null)
            {
                Ball.transform.position = DropPoint.transform.position;
            }
        }
        else
        {
            Debug.Log("gameOver");
            ShowMenu("Game Over");
        }
        UpdateLivesUI();
    }

    void UpdateLivesUI()
    {
        livesText?.SetText(lives.ToString());
    }

    public void AddScore(int amount = 1)
    {
        if (score != 999 && gameStarted)
        {
            score += amount;
            UpdateBallSprite(); // Check if ball should evolve
            UpdateScoreUI();
        }
    }

    void UpdateScoreUI()
    {
        scoreText?.SetText(score.ToString());
    }

    // Call this from your IAP system when a pack is purchased
    public void OnPackPurchased(int packIndex)
    {
        UnlockPack(packIndex);
        Debug.Log($"Pack {packIndex} purchased!");
    }

    // For testing without IAP
    public void TestUnlockPack(int packIndex)
    {
        UnlockPack(packIndex);
    }
}