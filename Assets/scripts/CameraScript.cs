using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera cam;
    Vector2 mousePos;
    public float divide=5;
    float distX,distY;

    void LateUpdate()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(SingletonScript.instance.isPlayerAlive)
        {
            distX = mousePos.x-SingletonScript.instance.player.transform.position.x;
            distY = mousePos.y-SingletonScript.instance.player.transform.position.y;

            transform.position = new Vector2(SingletonScript.instance.player.transform.position.x+(distX/divide),SingletonScript.instance.player.transform.position.y+(distY/divide));
        }
        else
        {
            distX = mousePos.x-SingletonScript.instance.lastPlayerPos.x;
            distY = mousePos.y-SingletonScript.instance.lastPlayerPos.y;

            transform.position = new Vector2(SingletonScript.instance.lastPlayerPos.x+(distX/divide),SingletonScript.instance.lastPlayerPos.y+(distY/divide));
        }
    }
}
