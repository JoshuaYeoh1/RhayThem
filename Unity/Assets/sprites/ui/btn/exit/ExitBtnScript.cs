using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBtnScript : MonoBehaviour
{
    public Animator transition;

    AudioManager snd;
    public AudioClip[] snd_gonk, snd_tween;

    void Awake()
    {
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();
    }

    void OnMouseDown()
    {
        StartCoroutine(quit());

        snd.playSFX(snd_gonk,transform,1);
    }

    IEnumerator quit()
    {
        Time.timeScale = 0;
        
        transition.SetTrigger("out");

        snd.playSFX(snd_tween,transform,1);

        yield return new WaitForSecondsRealtime(1);

        Application.Quit();

        Debug.Log("Game Closed");
    }
}
