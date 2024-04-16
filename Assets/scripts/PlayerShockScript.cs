using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShockScript : MonoBehaviour
{
    public Animator anim;
    [HideInInspector]
    public BoxCollider2D coll;
    
    void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;
    }

    public void offShock()
    {
        anim.SetTrigger("off");

        coll.enabled = false;
    }
}
