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
            if (AddWhiteNoise)
            {
                tones[0].frequency = wn.GenerateWhiteNoise(tones[0].frequency);
            }

            Tone[] otherTones = tones.Where(w => w != tones[0]).ToArray(); // Removes the tone that we are adding on to, as this doesn't need to be added to itself.

            //data[i] = tones[0].AddSine(timeIndex, otherTones);
            data[i] = tones[0].CreateSaw(timeIndex);
            timeIndex++;
        }
    }
}

[System.Serializable]
public class Tone
{
    private const int sampleRate = 44100;

    [Range(0,500)]
    public float frequency;

    [Range(0,2)]
    public float amplitude;

    [Range(1,5)]
    public float offset;

    [Header("For sawtooth")]
    public bool inverse;
    [Range(1,10)]
    public int numberOfWaves;
    public Tone(float freq, float amp, int off)
    {
        frequency = freq;
        amplitude = amp;
        offset = off;
    }

    public float CreateSine(int time)
    {
        return amplitude * Mathf.Sin(offset * Mathf.PI * time * (frequency / sampleRate));
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

    public float CreateSaw(int time)
    {
        float totalSaw = 0;
        for (int i = 0; i < numberOfWaves; i++)
        {
            totalSaw += amplitude * Mathf.Sin((offset * (i + 1 / numberOfWaves)) * Mathf.PI * time * (frequency / sampleRate));
        }

        return totalSaw * (inverse == true ? -1 : 1);
    }

    public float CreateTri(int time)
    {
        return ((2 * amplitude) / Mathf.PI) * Mathf.Asin(Mathf.Sin(((2 * Mathf.PI) / 100000 * (1 / frequency)) * time ));
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