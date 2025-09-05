using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShockScript : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider2D coll;
    
    void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;
    }

    public void offShock()
    {
        StartCoroutine(offAnim());
    }

    IEnumerator offAnim()
    {
        coll.enabled = false;

        transform.LeanScaleX(0,.1f).setEaseOutSine();

        yield return new WaitForSeconds(.2f);

        Destroy(gameObject);
    }
}
