using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpScript : MonoBehaviour
{   
    bool cleanUp;

    void Update()
    {
        if(!SingletonScript.instance.isPlayerAlive && !cleanUp)
        {
            cleanUp = true;

            StartCoroutine(cleanUpEnum());
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
