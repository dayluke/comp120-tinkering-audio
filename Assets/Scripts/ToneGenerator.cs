using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToneGenerator : MonoBehaviour
{
    public AudioSource audioSource;
    public Tone[] tones = new Tone[0];

    private int timeIndex = 0;
    
    private void Update()
    {
        tones[0].deltaTime = Time.deltaTime;
    }

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

    public void OnSaveClick()
    {
        Debug.Log(audioSource);
        SaveWavUtil.Save("D:\\workspace\\comp120-tinkering-audio", audioSource.clip);
    }
}