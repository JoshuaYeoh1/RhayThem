using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    WiggleScript cameraShake;

    AudioManager snd;
    public AudioClip[] snd_explode;

    void Awake()
    {
        cameraShake = GameObject.FindGameObjectWithTag("camshaker").GetComponent<WiggleScript>();
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();

        cameraShake.shake();

        snd.playSFX(snd_explode,transform,1);
    }
}
