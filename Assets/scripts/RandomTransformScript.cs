using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTransformScript : MonoBehaviour
{
    public float minScale=0.9f,maxScale=1.1f,maxRotate=360f;

    public bool randomScale=true,randomFlip=true,randomRotate;

    void Awake()
    {
        if(randomScale)
        {
            float randScale = Random.Range(minScale,maxScale);
        
            transform.localScale = new Vector2(transform.localScale.x*randScale,transform.localScale.y*randScale);
        }

        if(randomFlip && Random.Range(1,3)==1)
        {
            transform.localScale = new Vector2(transform.localScale.x*-1,transform.localScale.y);
        }

        if(randomRotate)
        {
            transform.eulerAngles = new Vector3(0,0,Random.Range(0f,maxRotate));
        }
    }
    
    void destroy()
    {
        Destroy(gameObject);
    }
}
