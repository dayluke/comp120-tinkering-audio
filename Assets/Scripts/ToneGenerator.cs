using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// AUTHOR: Luke Day,
/// LICENSE: MIT License
/// </summary>
public class ToneGenerator : MonoBehaviour
{
    [Range(1,100)]
    public int sampleDuration = 100;
    public AudioSource audioSource;
    public Tone[] tones = new Tone[0];
    
    /// <summary>
    /// Changes the 'deltaTime' attribute in the Tone class, as this has to be
    /// called from the main thread - and cannot be called from OnAudioFilterRead.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(CreateToneAudioClip());
        }
    }
    
    /// <summary>
    /// Handles the outputting of audio.
    /// Sets the 'samples' array equal to the tone that has been set in the inspector.
    /// The 'otherTones' array is used for the AddSine function in the Tone class.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="channels"></param>
    private AudioClip CreateToneAudioClip()
    {
        Tone[] otherTones = tones.Where(w => w != tones[0]).ToArray();
        int sampleLength = tones[0].sampleRate * sampleDuration;

        float maxValue = 1f / 4f;

        AudioClip audioClip = AudioClip.Create("Tone", sampleLength, 1, tones[0].sampleRate, false);

        float[] samples = new float[sampleLength];

        // Loops for the sample length, going through every tone and adding it to the
        // output 'samples' array.
        for (int i = 0; i < sampleLength; ++i)
        {
            float s = 0;
            foreach (Tone tone in tones)
            {
                s += tone.PlaySound(i, otherTones);
            }
            float v = s * maxValue;
            samples[i] = v;
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }

    /// <summary>
    /// Randomises the variable values that are assigned in the Tone class.
    /// This method heavily utilises the Random.Range function, which returns
    /// a int/float (depending on the paramters you pass it) between the two 
    /// parameters.
    /// The Enum.Getvalues method returns the number of enum values in the 
    /// specified enum data type.   
    /// </summary>
    public void OnRandomToneClick()
    {
        tones[0].type = (SoundType)Random.Range(0, System.Enum.GetValues(typeof(SoundType)).Length);
        tones[0].frequency = Random.Range(50, 500);
        tones[0].amplitude = Random.Range(0.2f, 1);
        tones[0].timeBetweenBeats = Random.Range(0.1f, 1);
    }

    /// <summary>
    /// Saves the Wav file to the path specified by the user.
    /// </summary>
    public void OnSaveClick()
    {
        string path = EditorUtility.SaveFilePanel("Where do you want the wav file to go?", "", "", "wav");
        SaveWavUtil.Save(path, CreateToneAudioClip());
    }
}