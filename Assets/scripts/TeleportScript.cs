using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public GameObject player;

    public void spawnPlayer()
    {
        GameObject myplayer = Instantiate(player, transform.position, Quaternion.identity);

        myplayer.GetComponent<PlayerScript>().spawn();

        SingletonScript.instance.playerScript = myplayer.GetComponent<PlayerScript>();

        SingletonScript.instance.player = myplayer;
    }
}
