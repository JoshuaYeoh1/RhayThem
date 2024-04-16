using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider masterSlider, musicSlider, sfxSlider;

    public const string MIXER_MASTER="masterVolume", MIXER_MUSIC="musicVolume", MIXER_SFX="sfxVolume"; 

    void Awake()
    {
        masterSlider.onValueChanged.AddListener(setMasterVolume);

        musicSlider.onValueChanged.AddListener(setMusicVolume);

        sfxSlider.onValueChanged.AddListener(setSFXVolume);
    }

    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 1f);

        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);

        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, masterSlider.value);

        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);

        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
    }

    void setMasterVolume(float value)
    {
        mixer.SetFloat(MIXER_MASTER,Mathf.Log10(value)*20);
    }
    
    void setMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC,Mathf.Log10(value)*20);
    }
    
    void setSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX,Mathf.Log10(value)*20);
    }

}
