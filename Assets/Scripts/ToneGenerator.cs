using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToneGenerator : MonoBehaviour
{
    public AudioSource audioSource;
    public Tone[] tones = new Tone[0];

    private int timeIndex = 0;
    
    /// <summary>
    /// Changes the 'deltaTime' attribute in the Tone class, as this has to be
    /// called from the main thread - and cannot be called from OnAudioFilterRead.
    /// </summary>
    private void Update()
    {
        tones[0].deltaTime = Time.deltaTime;
    }

    /// <summary>
    /// Handles the outputting of audio.
    /// Sets the 'data' array equal to the tone that has been set in the inspector.
    /// The 'otherTones' array is used for the AddSine function in the Tone class.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="channels"></param>
    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            Tone[] otherTones = tones.Where(w => w != tones[0]).ToArray(); // Removes the tone that we are adding on to, as this doesn't need to be added to itself.
            data[i] = tones[0].PlaySound(timeIndex, otherTones);
            timeIndex++;
        }

        //wavData = data; #### try and save to wav file? -- see below
    }

    /// <summary>
    /// Saves the Wav file to the path specified.
    /// </summary>
    public void OnSaveClick()
    {
        Debug.Log(audioSource);
        SaveWavUtil.Save("D:\\workspace\\comp120-tinkering-audio", audioSource.clip);
    }
}