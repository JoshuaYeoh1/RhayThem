using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBtnScript : MonoBehaviour
{
    void OnMouseDown()
    {
        SingletonScript.instance.spawnPlayer();
    }
}
