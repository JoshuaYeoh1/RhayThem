using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhayScript : MonoBehaviour
{
    public GameObject them;
    Vector2 defscale;
    Animator anim;
    bool alt=true;

    void Awake()
    {
        defscale = transform.localScale;
        anim = GetComponent<Animator>();
    }

    public void beat()
    {
        StartCoroutine(bounce());

        alt=!alt;

        if(alt)
        {
            anim.SetTrigger("1");
        }
        else
        {
            anim.SetTrigger("2");
        }

        them.transform.LeanRotateZ(Random.Range(-30,30f),Random.Range(.1f,.3f)).setEaseInOutSine();
    }

    IEnumerator bounce()
    {
        transform.LeanScale(new Vector2(defscale.x*1.1f,defscale.y*1.1f),.1f).setEaseInOutSine();

        yield return new WaitForSecondsRealtime(.1f);

        transform.LeanScale(defscale,.2f).setEaseInOutSine();
    }
}
