using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RandomScript : MonoBehaviour
{
    public bool randomTranslateX=false, randomTranslateY=false, randomScaleX=true, randomScaleY=true, randomFlipX=false, randomFlipY=false, randomHSV=false;

    public float minTranslate=-5, maxTranslate=5, minScale=.9f, maxScale=1.1f, minRotate=-15, maxRotate=15, minH=0, maxH=1, minS=0, maxS=1, minV=1, maxV=1;

    void Awake()
    {
        translate();
        scale();
        rotate();
        flip();
        hsv();
    }

    void translate()
    {
        if(randomTranslateX)
        {
            transform.position = new Vector2(transform.position.x+Random.Range(minTranslate,maxTranslate), transform.position.y);
        }

        if(randomTranslateY)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y+Random.Range(minTranslate,maxTranslate));
        }            
    }
    
    void scale()
    {  
        float uniformScale = Random.Range(minScale,maxScale);

        if(randomScaleX)
        {
            transform.localScale = new Vector2(transform.localScale.x*uniformScale, transform.localScale.y);
        }

        if(randomScaleY)
        {
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y*uniformScale);
        }            
    }

    void rotate()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+Random.Range(minRotate,maxRotate));
    }

    void flip()
    {
        if(randomFlipX)
        {
            int chance = Random.Range(1,3);

            if(chance==1)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x+180, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }

        if(randomFlipY)
        {
            int chance = Random.Range(1,3);

            if(chance==1)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y+180, transform.eulerAngles.z);
            }
            
        }
    }

    void hsv()
    {
        if(randomHSV)
        {
            SpriteRenderer mysprite = GetComponent<SpriteRenderer>();

            mysprite.color = Color.HSVToRGB(Random.Range(minH,maxH), Random.Range(minS,maxS), Random.Range(minV,maxV));

            Light2D mylight = GetComponent<Light2D>();

            mylight.color = mysprite.color;
        }
    }
}
