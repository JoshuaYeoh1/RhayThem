using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    public GameObject objectToDestroy;

    public void destroy()
    {
        Destroy(objectToDestroy.gameObject);
    }
}
