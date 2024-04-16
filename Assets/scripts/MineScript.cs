using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public GameObject explosion, mineWave;
    GameObject spawner;
    public float waveForce=500;
    bool cleanUp;

    AudioManager snd;
    public AudioClip[] snd_wave;

    void Awake()
    {
        spawner = GameObject.FindGameObjectWithTag("spawner");
	    snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();
    }

    void Update()
    {
        if(!SingletonScript.instance.isPlayerAlive && !cleanUp)
        {
            cleanUp = true;

            StartCoroutine(cleanUpEnum());
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer==8 || other.gameObject.layer==9 || other.gameObject.tag=="minewave")
        {
            explode();
        }
        else if((other.gameObject.layer==3 && other.gameObject.GetComponent<PlayerScript>().isDashing) ||
                (other.gameObject.layer==10 && other.gameObject.GetComponent<EnemyScript>().isDashing))
        {
            explode();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==8 || other.gameObject.layer==9 || other.gameObject.tag=="minewave")
        {
            explode();
        }
    }

    void explode()
    {
        if(SingletonScript.instance.isPlayerAlive)
        {
            float angleoffset = Random.Range(0,360f);

            for(int i=0; i<4; i++)
            {
                float angle = angleoffset+90*i;

                GameObject mywave = Instantiate(mineWave, transform.position, Quaternion.Euler(new Vector3(0,0,angle)));

                mywave.GetComponent<MineWaveScript>().AddForceAtAngle(angle,waveForce);

                mywave.transform.parent = spawner.transform;
            }

            StartCoroutine(wavesound());
        }
        
        Instantiate(explosion, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(0f,360f))));
        
        Destroy(gameObject);
    }

    IEnumerator cleanUpEnum()
    {
        yield return new WaitForSeconds(Random.Range(.5f,2f));

        explode();
    }

    IEnumerator wavesound()
    {
        float time=Random.Range(.25f,.75f);

        yield return new WaitForSeconds(time);

        snd.playSFX(snd_wave,transform,1);
    }
}
