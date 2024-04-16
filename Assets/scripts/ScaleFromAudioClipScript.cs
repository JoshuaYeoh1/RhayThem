using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromAudioClipScript : MonoBehaviour
{
    public AudioSource source;

    public Vector3 minScale, maxScale;

    public AudioLoudnessDetectionScript detector;

    public float loudnessSensibility = 1, threshold = .1f;

    void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip)*loudnessSensibility;

        if(loudness<threshold)
        {
            loudness=0;
        }

        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }
}
