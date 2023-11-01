using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    [SerializeField] private AudioClipsRefSO audioClipRefsSO;
    private AudioClip[] threeTwoOne;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    {
        
        //Player.Instance.OnPickedSomething += Player_OnPickedSomething;
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        //Player player = Player.Instance;
        //PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }




    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        //rather than using this method (if you want to access more options of the AudioSource component such as pan)
        // you can create a prefab of each audio file and instantiate/destroy them
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * volume);
    }
    public void PlayCountdownSound(int countdownNumber)
    {
        PlaySound(audioClipRefsSO.threeTwoOne[countdownNumber], Vector3.zero);
    }
//    public void PlayWarningSound(Vector3 position)
//    {
//        PlaySound(audioClipRefsSO.warning, position);
//    }

    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f) { volume = 0f; }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();

    }

    public float GetVolume()
    {
        return volume;
    }
}