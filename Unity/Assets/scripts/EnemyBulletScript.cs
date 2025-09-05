using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    SkinRandomizerScript skinner;
    Rigidbody2D rb;
    public GameObject impact;
    GameObject spawner;
    float dmg=50;

    AudioManager snd;
    public AudioClip[] ric;

    void Awake()
    {
        skinner = GetComponent<SkinRandomizerScript>();
        rb = GetComponent<Rigidbody2D>();
        spawner = GameObject.FindGameObjectWithTag("spawner");
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag=="player")
        {
            other.gameObject.GetComponent<PlayerScript>().hit(dmg+Random.Range(-10f,10f),750);
        }
        else if(other.gameObject.tag=="rock")
        {
            other.gameObject.GetComponent<RockScript>().hit(dmg+Random.Range(-10f,10f),750);
        }

        Vector2 midpoint = new Vector2((transform.position.x+other.transform.position.x)/2, (transform.position.y+other.transform.position.y)/2);

        GameObject myimpact = Instantiate(impact, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(1,360f))));

        myimpact.transform.parent = spawner.transform;

        snd.playSFX(ric,transform,1);

        Destroy(gameObject);
    }

    public void AddForceAtAngle(float angle, float force)
    {
        Quaternion rotation = Quaternion.Euler(0,0,angle);

        Vector3 direction = rotation*Vector3.up;

        rb.AddForce(direction*force);
    }
}