using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetectionScript : MonoBehaviour
{
    public int sampleWindow=2048;

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition-sampleWindow;

        if(startPosition<0)
        {
            return 0;
        }

        float[] waveData = new float[sampleWindow];

        clip.GetData(waveData, startPosition);

        //compute loudness
        float totalLoudness = 0;

        for(int i=0;i<sampleWindow;i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness/sampleWindow;
    }
}
