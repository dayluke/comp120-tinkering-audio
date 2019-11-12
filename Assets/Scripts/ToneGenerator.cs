using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToneGenerator : MonoBehaviour
{

    [SerializeField]
    [Range(0, 5)]
    private float frequency1 = 0.01f, frequency2 = 0.01f;

    [SerializeField]
    [Range(0, 2)]
    private float amplitude1 = 0.5f, amplitude2 = 0.5f;

    [SerializeField]
    [Range(-180, 180)]
    private int offset1 = 0, offset2 = 0;

    private int timeIndex = 0;

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(amplitude1, timeIndex, frequency1, offset1);

            timeIndex++;
        }
    }

    private float CreateSine(float amplitude, int timeIndex, float frequency, float offset)
    {
        return amplitude * Mathf.Sin((frequency * timeIndex) - offset);
    }
}
