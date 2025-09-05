using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TextPopupScript : MonoBehaviour
{
    public TextMeshPro TextpopupTMP;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        float angle = Random.Range(-25f,25f);

        AddForceAtAngle(angle, 500);

        transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));

        StartCoroutine(anim());
    }

    public void textPopup(float val)
    {
        TextpopupTMP.text = val.ToString();
    }

    public void textPopup(int val)
    {
        TextpopupTMP.text = val.ToString();
    }

    public void textPopup(string val)
    {
        TextpopupTMP.text = val;
    }

    public void AddForceAtAngle(float angle, float force)
    {
        Quaternion rotation = Quaternion.Euler(0,0,angle);

        Vector3 direction = rotation*Vector3.up;

        rb.velocity = new Vector2(0,0);

        rb.AddForce(direction*force);
    }

    IEnumerator anim()
    {
        transform.LeanScale(new Vector2(0,0),0);

        transform.LeanScale(new Vector2(1,1),.2f).setEaseInOutSine();

        yield return new WaitForSeconds(.5f);

        transform.LeanScale(new Vector2(0,0),.5f).setEaseInOutSine();

        yield return new WaitForSeconds(.5f);

        Destroy(gameObject);
    }
}
