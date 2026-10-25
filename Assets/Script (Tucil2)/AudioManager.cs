using System.Collections.Generic;
using UnityEngine;

public enum Sfx { Shoot, Coin, Heal, EnemyDie, Hurt, Jump }

[System.Serializable]
public class SfxEntry
{
    public Sfx id;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.5f;

    [Header("Sound effects")]
    [SerializeField] private List<SfxEntry> sfxList = new List<SfxEntry>();
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private readonly Dictionary<Sfx, SfxEntry> sfxLookup = new Dictionary<Sfx, SfxEntry>();

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = Mathf.Clamp01(value);
            if (musicSource != null) musicSource.volume = musicVolume;
        }
    }

    public float SfxVolume
    {
        get => sfxVolume;
        set => sfxVolume = Mathf.Clamp01(value);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        foreach (SfxEntry entry in sfxList)
            sfxLookup[entry.id] = entry;
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    // Safe static shortcut: does nothing if there is no AudioManager in the scene.
    public static void Play(Sfx id)
    {
        if (Instance != null) Instance.PlaySfx(id);
    }

    public void PlaySfx(Sfx id)
    {
        if (!sfxLookup.TryGetValue(id, out SfxEntry entry) || entry.clip == null)
        {
            Debug.LogWarning($"AudioManager: no clip assigned for {id}");
            return;
        }
        sfxSource.PlayOneShot(entry.clip, entry.volume * sfxVolume);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    private void OnValidate()
    {
        // lets you drag the volume slider while the game runs
        if (musicSource != null) musicSource.volume = musicVolume;
    }
}