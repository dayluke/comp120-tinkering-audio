using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Tone Class is used to create an easy-to-use interface,
/// so that the user can readily change the values of the
/// variables in the inspector at runtime.
/// </summary>
[System.Serializable]
public class Tone
{
    private const int sampleRate = 44100;

    public float deltaTime;

    public SoundType type;

    [Range(0, 500)]
    public float frequency;

    [Range(0, 2)]
    public float amplitude;
    
    [Range(0,2)]
    public float timeBetweenBeats;

    [Header("For sawtooth")]
    public bool inverse;
    [Range(1, 10)]
    public int numberOfWaves;

    /// <summary>
    /// Constructor method for the Tone class.
    /// Sets the variables passed through the params
    /// to the class variables.
    /// </summary>
    /// <param name="freq"></param>
    /// <param name="amp"></param>
    public Tone(float freq, float amp)
    {
        frequency = freq;
        amplitude = amp;
    }

    /// <summary>
    /// PlaySound determines which 'SoundType' enum the user
    /// chose in the inspector. The respective method then gets
    /// called and the value of the method is returned to the 
    /// main script to be added to the data.
    /// If no enum value was found then a value of 0 is returned.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="tones"></param>
    /// <returns></returns>
    public float PlaySound(int time, Tone[] tones = null)
    {
        if (type == SoundType.SINE)
        {
            return CreateSine(time);
        }
        else if (type == SoundType.ADDSINE)
        {
            return AddSine(time, tones: tones);
        }
        else if (type == SoundType.SAWTOOTH)
        {
            return CreateSaw(time);
        }
        else if (type == SoundType.TRIANGLE)
        {
            return CreateTri(time);
        }
        else if (type == SoundType.METRONOME)
        {
            return CreateMetronome(time, playAudio: true);
        }
        else if (type == SoundType.RANDOM)
        {
            return CreateRandom(time, playAudio: true);
        }
        else if (type == SoundType.WHITENOISE)
        {
            return CreateWhiteNoise();
        }

        return 0;
    }

#region SoundAlgorithms

    public float CreateSine(int time)
    {
        return amplitude * Mathf.Sin(2 * Mathf.PI * time * frequency / sampleRate);
    }

    public float AddSine(int time, Tone[] tones)
    {
        float totalSine = this.CreateSine(time);

        if (tones.Length > 0)
        {
            foreach (Tone sineWave in tones)
            {
                totalSine += sineWave.CreateSine(time);
            }
        }

        return totalSine;
    }

    public float CreateSaw(int time)
    {
        float totalSaw = 0;
        for (int i = 0; i < numberOfWaves; i++)
        {
            totalSaw += amplitude * Mathf.Sin((i + 1 / numberOfWaves) * Mathf.PI * time * (frequency / sampleRate));
        }

        return totalSaw * (inverse == true ? -1 : 1);
    }

    public float CreateTri(int time)
    {
        return ((2 * amplitude) / Mathf.PI) * Mathf.Asin(Mathf.Sin(((2 * Mathf.PI) / 100000 * (1 / frequency)) * time));
    }

    public float CreateMetronome(float currentTime, bool playAudio)
    {
        currentTime -= deltaTime;

        if (currentTime < 0)
        {
            playAudio = !playAudio;
            currentTime = timeBetweenBeats;
        }

        if (playAudio)
        {
            return CreateSine(Mathf.RoundToInt(currentTime));
        }

        return 0;
    }

    public float CreateRandom(float currentTime, bool playAudio, int bpm = 120)
    {
        currentTime -= deltaTime;
        if (currentTime < 0)
        {
            currentTime = timeBetweenBeats;
            playAudio = (Random.Range(0, bpm / 60) == 1) ? true : false;
        }

        if (playAudio)
        {
            return CreateSine(Mathf.RoundToInt(currentTime));
        }

        return 0;
    }

    public float CreateWhiteNoise()
    {
        System.Random rand = new System.Random();
        float value = (float)(rand.NextDouble() * 2.0 - 1.0) * amplitude;
        return value * frequency;
    }

#endregion
}

public enum SoundType
{
    SINE,
    ADDSINE,
    SAWTOOTH,
    TRIANGLE,
    METRONOME,
    RANDOM,
    WHITENOISE
}