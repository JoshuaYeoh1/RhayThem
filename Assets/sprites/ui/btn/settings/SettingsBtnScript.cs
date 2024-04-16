using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsBtnScript : MonoBehaviour
{
    public GameObject pauseMenu;

    AudioManager snd;
    public AudioClip[] snd_pause;

    void Awake()
    {
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();
    }

    void OnMouseDown()
    {
        pauseMenu.SetActive(true);

        snd.playSFX(snd_pause,transform,1);

        //Time.timeScale = 0;
    }
}
