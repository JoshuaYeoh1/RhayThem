using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BounceCameraEdgeScript : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject oobPositive, oobNegative;
    Vector2 camEdgeP, camEdgeN;
    public bool infBounces;
    public int bounceCount;

    void Awake()
    {
        oobPositive = GameObject.FindGameObjectWithTag("oobpositive");
        oobNegative = GameObject.FindGameObjectWithTag("oobnegative");
        rb = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        camEdgeP = new Vector2(oobPositive.transform.position.x-3, oobPositive.transform.position.y-3);
        camEdgeN = new Vector2(oobNegative.transform.position.x+3, oobNegative.transform.position.y+3);

        if(bounceCount>0 || infBounces)
        {
            if(transform.position.x>camEdgeP.x && rb.velocity.x>0)
            {
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                bounceCount--;
            }
            else if(transform.position.x<camEdgeN.x && rb.velocity.x<0)
            {
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                bounceCount--;
            }

            if(transform.position.y>camEdgeP.y && rb.velocity.y>0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                bounceCount--;
            }
            else if(transform.position.y<camEdgeN.y && rb.velocity.y<0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                bounceCount--;
            }
        }
    }
}
