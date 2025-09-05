using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class SkinRandomizerScript : MonoBehaviour
{
    public GameObject[] skins;

    public int skin;

    void Awake()
    {
        skin = Random.Range(0,skins.Length);

        for(int i=0; i<skins.Length; i++)
        {
            if(i!=skin)
            {
                skins[i].SetActive(false);
                //Destroy(skins[i]);
            }
            else
            {
                skins[i].SetActive(true);
            }
        }

        //StartCoroutine(deleteUnused());
    }

    IEnumerator deleteUnused()
    {
        yield return new WaitForSeconds(.5f);

        for(int i=0; i<skins.Length; i++)
        {
            if(i!=skin)
            {
                Destroy(skins[i]);
            }
        }
    }
}
