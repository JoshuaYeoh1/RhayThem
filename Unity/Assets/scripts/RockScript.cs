using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RockScript : MonoBehaviour
{
    SpriteRenderer sr;
    Rigidbody2D rb;
    public GameObject parent, breakDecal, rockChildren, gems;
    GameObject spawner;
    float health, healthmax, scale;
    Vector2 defScale;
    bool isShocked, randScale=true, cleanUp;
    public bool gem;

    AudioManager snd;
    public AudioClip[] snd_break;
    public AudioClip[] snd_breakgem;
    public AudioClip[] snd_hit;
    public AudioClip[] snd_hitgem;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spawner = GameObject.FindGameObjectWithTag("spawner");
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();
    }

    void Start()
    {
        if(randScale)
        {
            scale = Random.Range(.1f,3);
        }

        parent.transform.localScale = new Vector2(scale,scale);

        healthmax = scale*100;
        health = healthmax;

        defScale = transform.localScale;
    }

    void Update()
    {
        if(!SingletonScript.instance.isPlayerAlive && !cleanUp)
        {
            cleanUp = true;

            StartCoroutine(cleanUpEnum());
        }
    }

    public void hit(float dmg, float kb)
    {
        health -= dmg;

        //StartCoroutine(hurtColor());

        StartCoroutine(hurtAnim());

        AddForceAtAngle(Random.Range(0f,360f), kb);

        snd.playSFX(snd_hit,transform,1);

        if(gem)
        {
            snd.playSFX(snd_hitgem,transform,1);
        }

        if(health <= 0)
        {
            die();
        }
    }

    void die()
    {
        GameObject mybreak = Instantiate(breakDecal, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(0f,360f))));

        mybreak.transform.localScale = new Vector2(scale, scale);

        if(scale >= 1)
        {
            multiply();

            if(gem)
            {
                dropGems();

                snd.playSFX(snd_breakgem,transform,1);
            }
            else
            {
                snd.playSFX(snd_break,transform,1);
            }
        }

        Destroy(parent);
    }

    void multiply()
    {
        for (int i = 0; i < Random.Range(3,6); i++)
        {
            GameObject myrock = Instantiate(rockChildren, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(0f,360f))));

            myrock.transform.parent = spawner.transform;

            Rigidbody2D[] rb = myrock.GetComponentsInChildren<Rigidbody2D>();
            RockScript[] rs = myrock.GetComponentsInChildren<RockScript>();

            for(int j=0; j<rb.Length; j++)
            {
                rb[j].AddForce(myrock.transform.up*Random.Range(50f,300f));

                rs[j].randScale = false;

                rs[j].scale = (scale)/(Random.Range(1.5f,2.5f));
            }
        }
    }

    void dropGems()
    {
        for (int i = 0; i < Random.Range(1,5); i++)
        {
            GameObject mygem = Instantiate(gems, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(0f,360f))));

            mygem.GetComponent<Rigidbody2D>().AddForce(mygem.transform.up*Random.Range(250f,750f));

            mygem.transform.parent = spawner.transform;
        }
    }

    public void AddForceAtAngle(float angle, float force)
    {
        Quaternion rotation = Quaternion.Euler(0,0,angle);

        Vector3 direction = rotation*Vector3.up;

        rb.velocity = new Vector2(0,0);

        rb.AddForce(direction*force);
    }

    IEnumerator hurtColor()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(.1f);

        sr.color = Color.white;
    }

    IEnumerator hurtAnim()
    {
        float squashvalue = .1f;

        transform.LeanScale(new Vector2(defScale.x-squashvalue,defScale.y-squashvalue),.05f).setEaseInSine();

        yield return new WaitForSeconds(.05f);

        transform.LeanScale(new Vector2(defScale.x+squashvalue,defScale.y+squashvalue),.1f).setEaseOutSine();

        yield return new WaitForSeconds(.1f);

        transform.LeanScale(defScale,.15f).setEaseInOutSine();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="playershock" || other.tag=="enemyshock")
        {
            isShocked=true;

            StartCoroutine(gettingShocked());
        }
        else if(other.tag=="playerlaser" || other.tag=="enemylaser")
        {
            hit(100+Random.Range(-10f,10f),500);
        }
        else if(other.tag=="explosion")
        {
            hit(100+Random.Range(-10f,10f),500);
        }
        else if(other.tag=="minewave")
        {
            hit(50+Random.Range(-10f,10f),500);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag=="playershock" || other.tag=="enemyshock")
        {
            isShocked=false;
        }
    }

    IEnumerator gettingShocked()
    {
        while(isShocked)
        {
            hit(Random.Range(7f,12f),250);

            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator cleanUpEnum()
    {
        yield return new WaitForSeconds(Random.Range(.5f,2f));

        float time=Random.Range(.5f,2f);

        transform.LeanScale(Vector2.zero,time).setEaseInOutSine();

        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
