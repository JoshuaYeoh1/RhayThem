using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeBtnScript : MonoBehaviour
{
    public GameObject pauseMenu;

    void OnMouseDown()
    {
        //Time.timeScale = 1;

        pauseMenu.SetActive(false);
    }
}
