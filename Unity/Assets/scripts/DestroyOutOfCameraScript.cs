using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfCameraScript : MonoBehaviour
{
    GameObject oobPositive, oobNegative;
    int killTimer = 3;
    bool isOOB;
    [HideInInspector]
    public bool killed;

    void Awake()
    {
        oobPositive = GameObject.FindGameObjectWithTag("oobpositive");

        oobNegative = GameObject.FindGameObjectWithTag("oobnegative");
    }

    void Update()
    {
        if(transform.position.x>oobPositive.transform.position.x || transform.position.y>oobPositive.transform.position.y || transform.position.x<oobNegative.transform.position.x || transform.position.y<oobNegative.transform.position.y)
        {
            isOOB = true;
        }
        else
        {
            isOOB = false;
        }

        if(isOOB)
        {
            StartCoroutine(ToBeDestroyed());
        }
        else
        {
            StopCoroutine(ToBeDestroyed());
        }
    }

    IEnumerator ToBeDestroyed()
    {
        yield return new WaitForSeconds(killTimer);

        if(isOOB)
        {
            //Debug.Log("Removed OOB: "+gameObject.name);

            killed=true;

            yield return new WaitForSeconds(.1f);

            Destroy(gameObject);
        }   
    }
}
