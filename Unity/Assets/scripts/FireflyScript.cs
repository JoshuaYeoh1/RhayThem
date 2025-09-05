using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireflyScript : MonoBehaviour
{
    Light2D light2d;

    void Awake()
    {
        light2d = GetComponent<Light2D>();

        light2d.color = new Color(light2d.color.r,light2d.color.g,light2d.color.b,Random.Range(.1f,.2f));

        light2d.pointLightOuterRadius = Random.Range(0.25f,1f);
    }

    public void beat()
    {
        StartCoroutine(shine());
    }

    IEnumerator shine()
    {
        yield return new WaitForSeconds(Random.Range(0,.1f));

        light2d.color = Color.HSVToRGB(Random.Range(0f,1f), Random.Range(.5f,1f), 1);

        light2d.pointLightOuterRadius = Random.Range(0.25f,1f);
        
        light2d.color = new Color(light2d.color.r,light2d.color.g,light2d.color.b,Random.Range(.75f,.9f));

        yield return new WaitForSeconds(Random.Range(.1f,.3f));

        light2d.color = new Color(light2d.color.r,light2d.color.g,light2d.color.b,Random.Range(.1f,.2f));
    }
}
