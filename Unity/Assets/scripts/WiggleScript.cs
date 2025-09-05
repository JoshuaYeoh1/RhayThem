using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleScript : MonoBehaviour
{

    public GameObject defaultPosition;
    public float frequency, magnitude, duration;
    float posX, posY;
    public bool wiggle;
    float seed;

    void Awake()
    {
        seed = Random.value;
    }

    void LateUpdate()
    {
        if(wiggle)
        {
            posX = Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1;
            posY = Mathf.PerlinNoise(seed+1, Time.time * frequency) * 2 - 1;

            transform.localPosition = new Vector2(posX, posY) * magnitude;
        }
    }

    public void shake()
    {
        StartCoroutine(shaker());
    }

    IEnumerator shaker()
    {
        transform.position = defaultPosition.transform.position;

        wiggle=true;

        yield return new WaitForSeconds(duration);

        wiggle=false;

        transform.position = defaultPosition.transform.position;
    }
}
