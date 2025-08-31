using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager Instance;

    public GameObject Ball;
    public GameObject Wall;
    public GameObject DropPoint;
    public GameObject spawner;
    public SpriteRenderer ballSpriteRenderer;
    public SpriteRenderer bgSpriteRenderer;

    public int lives = 3;
    public TMP_Text livesText;

    public int score = 0;
    public int bestScore = 0;
    public TMP_Text scoreText;
    public TMP_Text bestScoreText;

    // Menu system variables
    public GameObject menuPanel;
    public Button startButton;
    public Button skinPackButton;
    public Button soundSettingsButton;
    public TMP_Text menuTitleText;

    // Sound settings menu
    public GameObject soundSettingsMenu;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Button soundBackButton;

    // Skin pack system variables
    public GameObject skinPackMenu;
    public Button[] skinPackButtons;
    public Button backButton;
    public GameObject[] packOwnedIcons; // Checkmarks for owned packs

    // Skin pack data
    [System.Serializable]
    public class bgPack
    {
        public string packName;
        public Sprite stage1; // 0 points
        public Sprite stage2; // 10 points  
        public Sprite stage3; // 20 points
    }

    // Skin pack data
    [System.Serializable]
    public class SkinPack
    {
        public string packName;
        public Sprite stage1Ball; // 0 points
        public Sprite stage2Ball; // 10 points  
        public Sprite stage3Ball; // 20 points
        public bool isPurchased; // Set to true after IAP
        public GameObject PurchaseBtn; // Set to true after IAP
    }

    public SkinPack[] skinPacks = new SkinPack[3]; // 3 packs in inspector
    public bgPack[] bgpack = new bgPack[1]; // 3 packs in inspector

    private int currentPackIndex = -1; // -1 = default pack
    private bool gameStarted = false;

    // Audio settings
    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadAllData();
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
        UpdateBestScoreUI();
    }

    private void LoadAllData()
    {
        // Load best score
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // Load audio settings
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Apply audio settings
        if (Audio.Instance != null)
        {
            Audio.Instance.SetMusicVolume(musicVolume);
            Audio.Instance.SetSFXVolume(sfxVolume);
        }
    }

    private void SaveAllData()
    {
        // Save best score
        PlayerPrefs.SetInt("BestScore", bestScore);

        // Save audio settings
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.Save();
    }

    private void LoadPurchasedPacks()
    {
        // Load purchased status from PlayerPrefs
        skinPacks[0].isPurchased = true; // First pack is always owned

        for (int i = 1; i < skinPacks.Length; i++)
        {
            Debug.Log(skinPacks[i].packName + " " + i);
            skinPacks[i].isPurchased = PlayerPrefs.GetInt($"PackPurchased_{i}", 0) == 1;
        }
    }

    public void UnlockPack(int packIndex)
    {
        if (packIndex >= 0 && packIndex < skinPacks.Length)
        {
            skinPacks[packIndex].isPurchased = true;
            PlayerPrefs.SetInt($"PackPurchased_{packIndex}", 1);
            SaveAllData();
            UpdatePackUI();
        }
    }

    public void clickNoise()
    {
        Audio.Instance.SFXplayer(Audio.Instance.BtnPress);
    }

    private void SetupButtonListeners()
    {
        // Main menu buttons
        startButton?.onClick.RemoveAllListeners();
        startButton?.onClick.AddListener(StartGame);

        skinPackButton?.onClick.RemoveAllListeners();
        skinPackButton?.onClick.AddListener(ShowSkinPackMenu);

        soundSettingsButton?.onClick.RemoveAllListeners();
        soundSettingsButton?.onClick.AddListener(ShowSoundSettingsMenu);

        // Sound settings buttons
        musicVolumeSlider?.onValueChanged.RemoveAllListeners();
        musicVolumeSlider?.onValueChanged.AddListener(SetMusicVolume);

        sfxVolumeSlider?.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider?.onValueChanged.AddListener(SetSFXVolume);

        soundBackButton?.onClick.RemoveAllListeners();
        soundBackButton?.onClick.AddListener(ShowMainMenu);

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

        // Set initial slider values
        if (musicVolumeSlider != null) musicVolumeSlider.value = musicVolume;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = sfxVolume;
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
                if(pack.PurchaseBtn != null) pack.PurchaseBtn.SetActive(!pack.isPurchased);
            }

            // Enable/disable pack buttons based on purchase status
            if (i < skinPackButtons.Length)
            {
                skinPackButtons[i].interactable = pack.isPurchased;
            }
        }
    }

    private void UpdateBestScoreUI()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = $"Best: {bestScore}";
        }
    }

    public void ShowMenu(string title = "Game Over")
    {
        gameStarted = false;

        // Check if we need to update best score
        if (score > bestScore)
        {
            bestScore = score;
            UpdateBestScoreUI();
            SaveAllData();
        }

        Audio.Instance.MusicSwitch(Audio.Instance.MenueMusic);

        menuPanel?.SetActive(true);
        Wall?.SetActive(true);
        spawner?.SetActive(false);
        skinPackMenu?.SetActive(false);
        soundSettingsMenu?.SetActive(false);

        menuTitleText?.SetText(title);
    }

    public void ShowSoundSettingsMenu()
    {
        clickNoise();
        menuPanel?.SetActive(false);
        skinPackMenu?.SetActive(false);
        soundSettingsMenu?.SetActive(true);
    }

    public void ShowSkinPackMenu()
    {
        clickNoise();
        menuPanel?.SetActive(false);
        skinPackMenu?.SetActive(true);
        soundSettingsMenu?.SetActive(false);
        UpdatePackUI();
    }

    public void ShowMainMenu()
    {
        clickNoise();
        skinPackMenu?.SetActive(false);
        soundSettingsMenu?.SetActive(false);
        menuPanel?.SetActive(true);
    }

    public void SelectPack(int packIndex)
    {
        clickNoise();
        if (packIndex < 0 || packIndex >= skinPacks.Length) return;

        var pack = skinPacks[packIndex];

        if (pack.isPurchased)
        {
            currentPackIndex = packIndex;
            ApplyStage1Ball(); // Start with stage 1 ball
            ShowMainMenu();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (Audio.Instance != null)
        {
            Audio.Instance.SetMusicVolume(musicVolume);
        }
        SaveAllData();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        if (Audio.Instance != null)
        {
            Audio.Instance.SetSFXVolume(sfxVolume);
        }
        SaveAllData();
    }

    private void UpdateBallSprite()
    {
        if (currentPackIndex < 0 || ballSpriteRenderer == null) return;

        var pack = skinPacks[currentPackIndex];
        var Backgroundpack = bgpack[0];

        if (score >= 20 && pack.stage3Ball != null)
        {
            ballSpriteRenderer.sprite = pack.stage3Ball;
            bgSpriteRenderer.sprite = Backgroundpack.stage3;
        }
        else if (score >= 10 && pack.stage2Ball != null)
        {
            ballSpriteRenderer.sprite = pack.stage2Ball;
            bgSpriteRenderer.sprite = Backgroundpack.stage2;
        }
        else if (pack.stage1Ball != null)
        {
            ballSpriteRenderer.sprite = pack.stage1Ball;
            bgSpriteRenderer.sprite = Backgroundpack.stage1;
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
        clickNoise();
        if (currentPackIndex < 0)
        {
            Debug.Log("Please select a skin pack first!");
            return;
        }

        Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * 5f;

        Wall?.SetActive(false);
        spawner?.SetActive(true);

        lives = 3;
        score = 0;

        menuPanel?.SetActive(false);
        skinPackMenu?.SetActive(false);
        soundSettingsMenu?.SetActive(false);

        gameStarted = true;

        Audio.Instance.MusicSwitch(Audio.Instance.LevelMusic);

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
        Audio.Instance.SFXplayer(Audio.Instance.ExtraLife);
        lives++;
        UpdateLivesUI();
    }

    public void LoseLife()
    {
        lives--;

        if (lives > 0)
        {
            Debug.Log("lost a life");
            Audio.Instance.SFXplayer(Audio.Instance.LostLife);

            if (Ball != null && DropPoint != null)
            {
                Ball.transform.position = DropPoint.transform.position;
            }
        }
        else
        {
            Debug.Log("gameOver");
            Audio.Instance.SFXplayer(Audio.Instance.GameOver);
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
            if (score > 10 && score < 20)
            {
                Audio.Instance.SFXplayer(Audio.Instance.BigBallBounce);
            }
            else
            {
                Audio.Instance.SFXplayer(Audio.Instance.BallBounce);
            }
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

    // Handle application focus/pause events to save data
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAllData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveAllData();
    }
}