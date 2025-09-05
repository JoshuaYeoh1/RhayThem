using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public GameObject hitbox;

    float defScaleX;

    void Awake()
    {
        defScaleX = transform.localScale.x;

        StartCoroutine(LaserAnim());
    }

    IEnumerator LaserAnim()
    {
        transform.LeanScaleX(0,0);

        transform.LeanScaleX(defScaleX,.1f).setEaseOutSine();

        yield return new WaitForSeconds(.05f);

        hitbox.SetActive(false);

        yield return new WaitForSeconds(.05f);

        transform.LeanScaleX(0,.1f).setEaseOutSine();

        yield return new WaitForSeconds(.1f);

        Destroy(gameObject);
    }
}
