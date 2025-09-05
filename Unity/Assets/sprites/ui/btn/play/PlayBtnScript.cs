using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBtnScript : MonoBehaviour
{
    AudioManager snd;
    public AudioClip[] snd_proceed;

    void Awake()
    {
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();
    }

    void OnMouseDown()
    {
        SingletonScript.instance.viewTutorial();

        snd.playSFX(snd_proceed,transform,1);
    }
}
