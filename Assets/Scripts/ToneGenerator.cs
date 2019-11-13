using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToneGenerator : MonoBehaviour
{
    public Tone[] tones = new Tone[0];
    public bool AddWhiteNoise;
    public WhiteNoise wn = new WhiteNoise();
    private int timeIndex = 0;

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            //data[i] = CreateSine(amplitude1, timeIndex, frequency1, offset1);
            //data[i] = AddSine(timeIndex, tones[0].amplitude, tones[1].amplitude, tones[0].frequency, tones[1].frequency, tones[0].offset, tones[1].offset);

            if (AddWhiteNoise)
            {
                tones[0].frequency = wn.GenerateWhiteNoise(tones[0].frequency);
            }

            Tone[] otherTones = tones.Where(w => w != tones[0]).ToArray(); // Removes the tone that we are adding on to, as this doesn't need to be added to itself.

            data[i] = tones[0].AddSine(timeIndex, otherTones);
            timeIndex++;
        }
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

    public float CreateSine(int time)
    {
        return amplitude * Mathf.Sin((frequency * time) - offset);
    }

    public float AddSine(int time, Tone[] tones)
    {
        float totalSine = this.CreateSine(time);

        foreach (Tone sineWave in tones)
        {
            totalSine += sineWave.CreateSine(time);
        }

        return totalSine;
    }
}

[System.Serializable]
public class WhiteNoise
{
    [Range(0,0.5f)]
    public float amplitude;

    [Range(0, 1)]
    public float offset;

    public float GenerateWhiteNoise(float frequency)
    {
        System.Random rand = new System.Random();
        float value = (float)(rand.NextDouble() * 2.0 - 1.0 + offset) * amplitude;
        return value * frequency;
    }
}