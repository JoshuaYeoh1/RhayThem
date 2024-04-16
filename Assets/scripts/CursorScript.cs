using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    Vector2 mousePos, defscale;
    Camera cam;
    float speed=-300;

    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        Cursor.visible = false;
        
        defscale = transform.localScale;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePos;

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            speed=300;

            StartCoroutine(anim());
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            speed=-300;

            StartCoroutine(anim());
        }

        transform.Rotate(0,0,speed*Time.deltaTime,Space.Self);
    }

    IEnumerator anim()
    {
        transform.LeanScale(new Vector2(defscale.x*.75f,defscale.y*.75f),.1f).setEaseInOutSine();

        yield return new WaitForSecondsRealtime(.1f);

        transform.LeanScale(defscale,.1f).setEaseInOutSine();
    }
}
