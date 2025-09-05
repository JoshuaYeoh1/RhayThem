using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarScript : MonoBehaviour
{
    public GameObject pivot, bar, barbg;

    public Color barColor=Color.red, barBGColor=Color.black;

    void Start()
    {
        bar.GetComponent<SpriteRenderer>().color = barColor;
        barbg.GetComponent<SpriteRenderer>().color = barBGColor;
    }

    void Update()
    {
        if(pivot.transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
    }
}
