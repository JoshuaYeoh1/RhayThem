using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    float defScaleX;

    void Awake()
    {
        defScaleX = transform.localScale.x;

        StartCoroutine(aimAnim());
    }

    IEnumerator aimAnim()
    {
        transform.LeanScaleX(0,0);

        transform.LeanScaleX(defScaleX,.2f).setEaseOutSine();

        yield return new WaitForSeconds(.55f);

        transform.LeanScaleX(0,.2f).setEaseOutSine();

        yield return new WaitForSeconds(.2f);

        Destroy(gameObject);
    }
}
