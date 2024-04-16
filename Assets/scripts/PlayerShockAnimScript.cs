using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShockAnimScript : MonoBehaviour
{
    Animator anim;

    public PlayerShockScript playerShockScript;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void animidle()
    {
        anim.SetTrigger("idle");

        playerShockScript.coll.enabled = true;
    }
}
