using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    
    float startPosX, length, startPosY, height;

    public float parallaxEffect;

    public Camera cam;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }
    
    void Update()
    {
        float tempX = (cam.transform.position.x*(1-parallaxEffect));
        float tempY = (cam.transform.position.y*(1-parallaxEffect));

        float distanceX = (cam.transform.position.x*parallaxEffect);
        float distanceY = (cam.transform.position.y*parallaxEffect);

        transform.position = new Vector2(startPosX+distanceX,startPosY+distanceY);

        if(tempX>startPosX+length)
        {
            startPosX+=length;
        }
        else if(tempX<startPosX-length)
        {
            startPosX-=length;
        }
        
        if(tempY>startPosY+height)
        {
            startPosY+=height;
        }
        else if(tempY<startPosY-height)
        {
            startPosY-=height;
        }
    }

}
