using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AudioTinker : MonoBehaviour {

    //Bass frequency = 30 -> 100Hz
    
    [SerializeField][Range(0,20000)]
    private int frequency = 1500;

    [SerializeField]
    private int sampleDuration = 5;

    [SerializeField][Range(1000,44100)]
    private int sampleRate = 44100;

    private AudioSource audioSource;
    private AudioClip outAudioClip;
    
    
    private void Start() {
        audioSource = GetComponent<AudioSource>();
        outAudioClip = CreateToneAudioClip();
        PlayOutAudio();
    }

    public void PlayOutAudio() {
        audioSource.PlayOneShot(outAudioClip);    
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Play Sound"))
        {
            PlayOutAudio();
        }
    }

    public void StopAudio() {
        audioSource.Stop();
    }
        
    private AudioClip CreateToneAudioClip() {
        int sampleLength = sampleRate * sampleDuration;
        float maxValue = 1f / 4f;
        
        var audioClip = AudioClip.Create("tone", sampleLength, 1, sampleRate, false);
        
        float[] samples = new float[sampleLength];
        for (var i = 0; i < sampleLength; i++) {
            float s = Mathf.Sin(2.0f * Mathf.PI * frequency * ((float) i / (float) sampleRate));
            float v = s * maxValue;
            samples[i] = v;
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }

    
#if UNITY_EDITOR
    //[Button("Save Wav file")]
    private void SaveWavFile() {
        string path = EditorUtility.SaveFilePanel("Where do you want the wav file to go?", "", "", "wav");
        var audioClip = CreateToneAudioClip();
        SaveWavUtil.Save(path, audioClip);
    }
#endif
}