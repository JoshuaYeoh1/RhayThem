using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleXScript : MonoBehaviour
{
    public float frequency, magnitude;
    
    float seed;

    void Awake()
    {
        seed = Random.value;

        StartCoroutine(wiggleX());
    }

    IEnumerator wiggleX()
    {   
        while(true)
        {
            float time = Random.Range(frequency/2,frequency);

            transform.LeanMoveLocalX(Random.Range(-magnitude,magnitude),time).setEaseInOutSine();

            yield return new WaitForSeconds(time);
        }
    }
}
