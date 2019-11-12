using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToneGenerator : MonoBehaviour
{
    public Tone[] tones = new Tone[0];

    private int timeIndex = 0;

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            //data[i] = CreateSine(amplitude1, timeIndex, frequency1, offset1);
            data[i] = AddSine(timeIndex, tones[0].amplitude, tones[1].amplitude, tones[0].frequency, tones[1].frequency, tones[0].offset, tones[1].offset);
            timeIndex++;
        }
    }

    private float CreateSine(float amplitude, int timeIndex, float frequency, float offset)
    {
        return amplitude * Mathf.Sin((frequency * timeIndex) - offset);
    }

    private float AddSine(int x, float a1, float a2, float f1, float f2, int o1, int o2)
    {
        float sin1 = a1 * Mathf.Sin((f1 * x) + o1);
        float sin2 = a2 * Mathf.Sin((f2 * x) + o2);
        return sin1 + sin2;
    }

    // FORMULA FOR ADDING SINE WAVES: f(x) = a1*sin(f1*x + o1) + a2*sin(f2*x + o2)
}

[System.Serializable]
public class Tone
{
    [Range(0,5)]
    public float frequency;

    [Range(0,2)]
    public float amplitude;

    [Range(-180,180)]
    public int offset;

    public Tone(float freq, float amp, int off)
    {
        frequency = freq;
        amplitude = amp;
        offset = off;
    }
}