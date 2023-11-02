using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{

    public static MusicManager Instance { get; private set; }

    private const string PLAYER_PREFS_MUSIC_EFFECTS_VOLUME = "MusicVolume";

    private AudioSource audioSource;
    private float volume = .3f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_EFFECTS_VOLUME, .3f);
        audioSource.volume = volume;
        
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    public void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }

    }
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f) { volume = 0f; }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;

    }
}