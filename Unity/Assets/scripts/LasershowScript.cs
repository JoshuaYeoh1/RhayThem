using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LasershowScript : MonoBehaviour
{
    public GameObject[] beams;
    public SpriteRenderer[] beamsSprites;
    public Light2D[] beamsLights;

    void Awake()
    {
        for(int i=0;i<beams.Length;i++)
        {
            beamsSprites[i].color = Color.HSVToRGB(Random.Range(0f,1f), Random.Range(.5f,1f), 1);

            beamsSprites[i].color = new Color(beamsSprites[i].color.r,beamsSprites[i].color.g,beamsSprites[i].color.b,Random.Range(.1f,.2f));

            beamsLights[i].color = beamsSprites[i].color;
        }
    }

    public void beat()
    {
        for(int i=0;i<beams.Length;i++)
        {
            beams[i].transform.LeanRotateZ(Random.Range(-65f,65f),Random.Range(.1f,.3f)).setEaseInOutSine();
        }

        StartCoroutine(shine());
    }

    IEnumerator shine()
    {
        for(int i=0;i<beams.Length;i++)
        {
            beamsSprites[i].color = Color.HSVToRGB(Random.Range(0f,1f), Random.Range(.5f,1f), 1);

            beamsSprites[i].color = new Color(beamsSprites[i].color.r,beamsSprites[i].color.g,beamsSprites[i].color.b,Random.Range(.4f,.6f));

            beamsLights[i].color = beamsSprites[i].color;

            yield return new WaitForSeconds(Random.Range(.1f,.2f));

            beamsSprites[i].color = new Color(beamsSprites[i].color.r,beamsSprites[i].color.g,beamsSprites[i].color.b,Random.Range(.1f,.2f));

            beamsLights[i].color = beamsSprites[i].color;
        }
    }
}
