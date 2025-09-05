using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTriggerScript : MonoBehaviour
{
    public WiggleScript cameraShake;
    public SpawnerScript spawnerScript;
    public LasershowScript lasershowScript;
    public RhayScript rhayScript;
    SpriteRenderer sr;
    public bool secondary;
    bool cooldown;
    public bool sustaining, beating;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        beating = cooldown;
    }

    void LateUpdate()
    {
        if(secondary && SingletonScript.instance.isPlayerAlive && SingletonScript.instance.playerScript.weapon=="shock")
        {
            if(sustaining)
            {
                if(Input.GetKey(KeyCode.Mouse0))
                {
                    SingletonScript.instance.playerScript.shootShock();
                }
                else if(Input.GetKeyUp(KeyCode.Mouse0))
                {
                    SingletonScript.instance.playerScript.offShock();
                }
            }
            else
            {
                SingletonScript.instance.playerScript.offShock();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        sr.color = Color.red;

        if(!secondary)
        {
            cameraShake.shake();

            StartCoroutine(beat());
        }
        else
        {
            sustaining = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        sr.color = Color.green;

        if(secondary)
        {
            sustaining = false;
        }
    }

    IEnumerator beat()
    {
        if(!cooldown)
        {
            cooldown = true;

            if(SingletonScript.instance.isPlayerAlive)
            {
                if(Input.GetKey(KeyCode.Mouse0))
                {
                    SingletonScript.instance.playerScript.shoot();
                }
                else if(Input.GetKey(KeyCode.Mouse1))
                {
                    SingletonScript.instance.playerScript.dash();
                }
            }
            
            spawnerScript.beat();
            lasershowScript.beat();
            rhayScript.beat();

            GameObject[] firefly = GameObject.FindGameObjectsWithTag("firefly");
            
            for(int i=0; i<firefly.Length; i++)
            {
                firefly[i].GetComponent<FireflyScript>().beat();
            }
            
            yield return new WaitForSeconds(.1f);

            cooldown = false;
        }
    }
}
