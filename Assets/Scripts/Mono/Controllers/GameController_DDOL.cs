using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using citdev;

public class GameController_DDOL : MonoBehaviour
{
    public int round = 1;
    public int totalKills = 0;
    public int coins = 0;
    public int PreviousRoundMoves = 0;
    ChangeScene _sceneChanger;
    public PlayerCharacter CurrentCharacter = new PC_Rogue();

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Reset();
    }

    private void Start()
    {
        _sceneChanger = GetComponent<ChangeScene>();
        SetSoundLevel(2);
        SetMusicToggle(false);
    }

    public void ChangeScene(string sceneName)
    {
        _sceneChanger.SwapToScene(sceneName);
    }

    public void Reset()
    {
        PreviousRoundMoves = 0;
    }

    public void CoinBalanceChange(int delta)
    {
        coins += delta;
    }
    public void OnMonsterKilled()
    {
        totalKills += 1;
    }













    public int currentVolume = 1;
    public bool musicOn = false;
    [SerializeField] public AudioSource audioSource_SFX;
    [SerializeField] public AudioSource audioSource_Music;

    public void SetMusicToggle(bool toggle)
    {
        musicOn = toggle;
        SetSoundLevel(currentVolume);
    }

    public void PauseSound()
    {
        audioSource_Music.Pause();
        audioSource_SFX.Pause();
    }

    public void ResumeSound()
    {
        audioSource_Music.UnPause();
        audioSource_SFX.UnPause();
    }

    public void PlaySound(string clipName)
    {
        AudioClip audioClip = GetAudioClipByName(clipName);
        if (audioClip != null)
        {
            audioSource_SFX.PlayOneShot(audioClip);
        }
        else
        {
            Debug.Log("null audio clip: " + clipName);
        }
    }
    public void StopSound()
    {
        audioSource_SFX.Stop();
    }
    public void PlayMusic(string songName)
    {
        AudioClip audioClip = GetSongClipByName(songName);
        if (audioClip != null)
        {
            audioSource_Music.Stop();
            audioSource_Music.clip = audioClip;
            audioSource_Music.Play();
        }
        else
        {
            Debug.Log("null audio clip: " + songName);
        }
    }

    AudioClip GetAudioClipByName(string clipName)
    {
        return (AudioClip)Resources.Load("Sounds/" + clipName);
    }

    AudioClip GetSongClipByName(string clipName)
    {
        return (AudioClip)Resources.Load("Songs/" + clipName);
    }

    public void SetSoundLevel(int volumeLevel)
    {
        currentVolume = volumeLevel;
        if (volumeLevel == 0)
        {
            audioSource_Music.volume = 0f;
            audioSource_SFX.volume = 0f;
        }
        if (volumeLevel == 1)
        {
            audioSource_Music.volume = musicOn ? 0.05f : 0f;
            audioSource_SFX.volume = 0.12f;
        }
        if (volumeLevel == 2)
        {
            audioSource_Music.volume = musicOn ? 0.12f : 0f;
            audioSource_SFX.volume = 0.18f;
        }
        if (volumeLevel == 3)
        {
            audioSource_Music.volume = musicOn ? 0.2f : 0f;
            audioSource_SFX.volume = 0.24f;
        }
    }
}
