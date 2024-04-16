using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{   
    public AudioSource soundFXObject;

    [Header("-------------  Audio Source -------------")]
    public AudioSource musicSource;
    public AudioSource ambSource;
    
    [Header("-------------  Audio Clip -------------")]
    public AudioClip[] music;
    public AudioClip[] ambient;

    public const string MASTER_KEY = "masterVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    public AudioMixer mixer;

    private void Awake()
    {
        musicSource.clip = music[(Random.Range(0,music.Length))];

        musicSource.Play();
        
        ambSource.clip = ambient[(Random.Range(0,ambient.Length))];

        ambSource.Play();

        LoadVolume();
    }

    public void playSFX(AudioClip[] clip, Transform spawnTransform, float volume)
    {   
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        int rand = Random.Range(0,clip.Length);

        audioSource.clip = clip[rand];

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    void LoadVolume()
    {   
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);

        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);

        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(masterVolume)*20);

        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume)*20);

        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume)*20);
    }
}
