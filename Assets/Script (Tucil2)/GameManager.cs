using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private Vector2 checkpoint;
    private int checkpointHealth = 100;
    private bool hasCheckpoint;
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsWon { get; private set; }
    public event Action GameWon;

    public event Action<int> ScoreChanged;
    public event Action GameEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Time.timeScale = 1f;  
        
        SaveSystem.Delete();
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb != null && kb.lKey.wasPressedThisFrame && !IsGameOver && !IsWon) LoadGame();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        ScoreChanged?.Invoke(Score);
    }

    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Time.timeScale = 0f;
        GameEnded?.Invoke();
    }

    public void SaveGame()
    {
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player == null) return;
        checkpointHealth = player.CurrentHealth;

        SaveData data = new SaveData
        {
            score = Score,
            playerHealth = player.CurrentHealth,
            posX = hasCheckpoint ? checkpoint.x : player.transform.position.x,
            posY = hasCheckpoint ? checkpoint.y : player.transform.position.y,
            musicVolume = AudioManager.Instance != null ? AudioManager.Instance.MusicVolume : 0.5f,
            sfxVolume = AudioManager.Instance != null ? AudioManager.Instance.SfxVolume : 1f
        };
        SaveSystem.Save(data);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.Load();
        if (data == null)
        {
            Debug.Log("No save file found.");
            return;
        }
        checkpointHealth = data.playerHealth;

        Score = data.score;
        ScoreChanged?.Invoke(Score);

        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player != null)
        {
            player.SetCurrent(data.playerHealth);
            player.transform.position = new Vector3(data.posX, data.posY, 0f);
            if (player.TryGetComponent(out Rigidbody2D rb)) rb.linearVelocity = Vector2.zero;
        }

        SetCheckpoint(new Vector2(data.posX, data.posY));   // new line

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.MusicVolume = data.musicVolume;
            AudioManager.Instance.SfxVolume = data.sfxVolume;
        }
        Debug.Log("Game loaded.");
    }

    public void WinGame()
    {
        if (IsGameOver || IsWon) return;
        IsWon = true;
        Time.timeScale = 0f;
        GameWon?.Invoke();
    }
    public void SetCheckpoint(Vector2 position)
    {
        checkpoint = position;
        hasCheckpoint = true;
    }

    public void OnPlayerDied()
    {
        if (hasCheckpoint) Respawn();
        else GameOver();
    }

    private void Respawn()
    {
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player == null) return;

        player.SetCurrent(checkpointHealth);
        player.transform.position = checkpoint;
        if (player.TryGetComponent(out Rigidbody2D rb)) rb.linearVelocity = Vector2.zero;

        Debug.Log("Respawned at checkpoint");
    }
}