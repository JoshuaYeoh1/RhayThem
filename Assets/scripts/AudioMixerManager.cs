using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    // AudioManager audioManager;

    // void Awake()
    // {
    //     audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    // }
    
    public void setMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume",Mathf.Log10(level)*20f);
    }
    
    public void setSFXVolume(float level)
    {
        audioMixer.SetFloat("sfxVolume",Mathf.Log10(level)*20f);
    }
    
    public void setMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume",Mathf.Log10(level)*20f);
    }

    // void OnDisable()
    // {
    //     PlayerPrefs.SetFloat(audioManager.MASTER_KEY, masterVolume.value);

    //     PlayerPrefs.SetFloat(audioManager.MUSIC_KEY, musicVolume.value);

    //     PlayerPrefs.SetFloat(audioManager.SFX_KEY, sfxVolume.value);
    // }
}
