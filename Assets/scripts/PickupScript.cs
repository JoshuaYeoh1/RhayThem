using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public GameObject health, laser, shock, shotgun;
    string chosen;
    bool cleanUp;

    void Awake()
    {
        health.SetActive(false);
        laser.SetActive(false);
        shock.SetActive(false);
        shotgun.SetActive(false);
    }
    
    void Update()
    {
        if(!SingletonScript.instance.isPlayerAlive && !cleanUp)
        {
            cleanUp = true;

            StartCoroutine(cleanUpEnum());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="player")
        {
            other.gameObject.GetComponent<PlayerScript>().pickup(chosen);

            Destroy(gameObject);
        }
    }

    public void chooseType(string type)
    {
        if(type=="health")
        {
            health.SetActive(true);
        }
        else if(type=="laser")
        {
            laser.SetActive(true);
        }
        else if(type=="shock")
        {
            shock.SetActive(true);
        }
        else if(type=="shotgun")
        {
            shotgun.SetActive(true);
        }
        chosen=type;
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
