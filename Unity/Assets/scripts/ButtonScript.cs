using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    Vector2 defscale;
    Animator anim;

    AudioManager snd;
    public AudioClip[] snd_click, snd_hover;

    void Awake()
    {
        defscale = transform.localScale;
        anim = GetComponent<Animator>();
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();
    }

    void OnMouseEnter()
    {
        anim.SetBool("hover",true);
        snd.playSFX(snd_hover,transform,1);
    }

    void OnMouseExit()
    {
        anim.SetBool("hover",false);
    }

    void OnMouseDown()
    {
        StartCoroutine(click());
        snd.playSFX(snd_click,transform,1);
    }

    IEnumerator click()
    {
        transform.LeanScale(new Vector2(defscale.x*.75f,defscale.y*.75f),.1f).setEaseInOutSine();

        yield return new WaitForSecondsRealtime(.1f);

        transform.LeanScale(defscale,.1f).setEaseInOutSine();
    }
}
