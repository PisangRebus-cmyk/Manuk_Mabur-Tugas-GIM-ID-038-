using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthFill;
    [SerializeField] private float fillSpeed = 5f;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winScoreText;

    private Health playerHealth;
    private float targetFill = 1f;
    private bool paused;

    private void Start()
    {
        winPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        playerHealth = FindAnyObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Changed += OnHealthChanged;
            OnHealthChanged(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }

        GameManager gm = GameManager.Instance;
        gm.ScoreChanged += OnScoreChanged;
        gm.GameEnded += ShowGameOver;
        OnScoreChanged(gm.Score);

        
        gm.GameWon += ShowWin;
    }

    private void OnDestroy()
    {
        if (playerHealth != null) playerHealth.Changed -= OnHealthChanged;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ScoreChanged -= OnScoreChanged;
            GameManager.Instance.GameEnded -= ShowGameOver;
            GameManager.Instance.GameWon -= ShowWin;
        }
    }

    private void Update()
    {
        // smooth bar: slides toward the target, and turns green -> red as HP drops
        healthFill.fillAmount = Mathf.MoveTowards(healthFill.fillAmount, targetFill,
        fillSpeed * Time.unscaledDeltaTime);
        healthFill.color = Color.Lerp(Color.red, Color.green, healthFill.fillAmount);

        var kb = Keyboard.current;
        if (kb != null && kb.escapeKey.wasPressedThisFrame && !GameManager.Instance.IsGameOver && !GameManager.Instance.IsWon)
        TogglePause();
    }

    private void OnHealthChanged(int current, int max)
    {
        targetFill = (float)current / max;
        healthText.text = $"{current} / {max}";
    }

    private void OnScoreChanged(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    private void ShowGameOver()
    {
        finalScoreText.text = $"Final score: {GameManager.Instance.Score}";
        gameOverPanel.SetActive(true);
    }

    public void TogglePause()
    {
        paused = !paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
    }

    // Button functions (hooked up in the Inspector)
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSavedGame()
    {
        if (paused) TogglePause();
        GameManager.Instance.LoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();   // does nothing inside the Editor, only in a built game
    }

    private void ShowWin()
    {
        winScoreText.text = $"Final score: {GameManager.Instance.Score}";
        winPanel.SetActive(true);
    }
}