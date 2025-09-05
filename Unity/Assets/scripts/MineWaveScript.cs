//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineWaveScript : MonoBehaviour
{
    Rigidbody2D rb;
    public float dmg=50;
    bool hasHurt;
        
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AddForceAtAngle(float angle, float force)
    {
        Quaternion rotation = Quaternion.Euler(0,0,angle);

        Vector3 direction = rotation*Vector3.up;

        rb.velocity = new Vector2(0,0);

        rb.AddForce(direction*force);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!hasHurt)
        {
            hasHurt = true;

            if(other.gameObject.layer==3)
            {
                other.GetComponent<PlayerScript>().hit(dmg+Random.Range(-10f,10f),750);
            }
            else if(other.gameObject.layer==10)
            {
                other.GetComponent<EnemyScript>().hit(dmg+Random.Range(-10f,10f),750);
            }
        }
    }
}
