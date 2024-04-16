using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShockAnimScript : MonoBehaviour
{
    Animator anim;

    public EnemyShockScript enemyShockScript;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void animidle()
    {
        anim.SetTrigger("idle");

        enemyShockScript.coll.enabled = true;
    }
}
