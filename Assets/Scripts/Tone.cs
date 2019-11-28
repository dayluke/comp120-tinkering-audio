using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AUTHOR: Luke Day,
/// LICENSE: MIT License
/// </summary>

/// <summary>
/// The Tone Class is used to create an easy-to-use interface,
/// so that the user can readily change the values of the
/// variables in the inspector at runtime.
/// </summary>
[System.Serializable]
public class Tone
{
    public SoundType type;

    [Range(0, 120)]
    public int sampleDuration = 3;

    [Range(1000, 44100)]
    public int sampleRate = 44100;

    [Range(0, 500)]
    public float frequency;

    [Range(0, 2)]
    public float amplitude;
    
    [Range(0,2)]
    public float timeBetweenBeats;

    public Sawtooth sawtoothProperties;

    private bool playAudio = false;

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
            return CreateMetronome(time);
        }
        else if (type == SoundType.RANDOM)
        {
            return CreateRandom(time);
        }
        else if (type == SoundType.WHITENOISE)
        {
            return CreateWhiteNoise();
        }

        return 0;
    }

#region SoundAlgorithms

    /// <summary>Uses the Sine wave formula to generate a simple sine tone.</summary>
    /// <param name="time"></param>
    public float CreateSine(int time)
    {
        return amplitude * Mathf.Sin(2 * Mathf.PI * time * frequency / sampleRate);
    }

    /// <summary>Uses the Sine wave formula to generate a simple sine tone.
    /// Adds the Sine wave formula of the other tones to this tone.</summary>
    /// <param name="time"></param>
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

    /// <summary>Uses the Sawtooth wave formula to generate a slightly grainy tone.</summary>
    /// <param name="time"></param>
    public float CreateSaw(int time)
    {
        float totalSaw = 0;
        for (int i = 0; i < sawtoothProperties.numberOfWaves; i++)
        {
            totalSaw += amplitude * Mathf.Sin((i + 1 / sawtoothProperties.numberOfWaves) * Mathf.PI * time * (frequency / sampleRate));
        }

        return totalSaw * (sawtoothProperties.inverse == true ? -1 : 1);
    }

    /// <summary>Uses the Triangle wave formula to generate a sharp tone.</summary>
    /// <param name="time"></param>
    public float CreateTri(int time)
    {
        return ((2 * amplitude) / Mathf.PI) * Mathf.Asin(Mathf.Sin(((2 * Mathf.PI) / 100000 * (1 / frequency)) * time));
    }

    /// <summary>
    /// Uses the modulus operator to switch the audio on and off for the time specified in timeBetweenBeats.
    /// This function just plays a simple sine wave when the audio is on.
    /// </summary>
    /// <param name="currentTime"></param>
    /// <param name="playAudio"></param>
    public float CreateMetronome(int currentTime)
    {
        if (currentTime % (sampleRate * timeBetweenBeats) == 0)
        {
            playAudio = !playAudio;
        }

        if (playAudio)
        {
            return amplitude * Mathf.Sin(2 * Mathf.PI * currentTime * frequency / sampleRate);
        }

        return 0;
    }

    /// <summary>
    /// Uses the random function to generate a tone every random seconds.
    /// </summary>
    /// <param name="currentTime"></param>
    /// <param name="playAudio"></param>
    /// <param name="bpm"></param>
    public float CreateRandom(int currentTime, int bpm = 120)
    {
        if (currentTime % (sampleRate * timeBetweenBeats) == 0)
        {
            playAudio = (Random.Range(0, bpm / 60) == 1) ? true : false;
        }

        if (playAudio)
        {
            return amplitude * Mathf.Sin(2 * Mathf.PI * Mathf.RoundToInt(currentTime) * frequency / sampleRate);
        }

        return 0;
    }

    /// <summary>Uses the random function to generate a make-shift white noise tone.</summary>
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

[System.Serializable]
public class Sawtooth
{
    public bool inverse;

    [Range(1, 10)]
    public int numberOfWaves;
}