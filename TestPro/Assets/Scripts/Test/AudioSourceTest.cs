using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceTest : MonoBehaviour {

    public AudioSource audio;
    public AudioClip clip;

    public int position = 0;
    public int samplerate;      //音频长度
    public int frequency;       //采样频率


    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        clip = audio.clip;
        frequency = clip.frequency;
    }

    private float[] samples;
    // Use this for initialization
    void Start () {
        int length = clip.channels * clip.samples;
        samples = new float[length];
        int scale = 3;
        samplerate = length * scale;
        //float[] newSamples = new float[samplerate * channels];
        float[] newSamples = new float[length / scale];
        clip.GetData(samples, 0);
        float temp = 0;
        for (int i = 0; i < length; i++)
        {
            temp += samples[i];
            if (i % scale == 0)
            {
                newSamples[i / scale] = temp;
                temp = 0;
            }
            //for (int j = 0; j < scale; j++)
            //{
            //    newSamples[i * scale + j] = temp;
            //}
        }
        //float[] samples2 = new float[length];
        //for (int i = 0; i < length; i++)
        //{
        //    samples2[i] = newSamples[i / scale];
        //}
        AudioClip newClip = AudioClip.Create("newClip", newSamples.Length, clip.channels, frequency, false);
        newClip.SetData(newSamples, 0);
        audio.clip = newClip;
        audio.Play();
        //float pitch = audio.pitch;
        //audio.pitch = 2;
        Debug.Log(clip.length);
        Debug.Log(newClip.length);
    }

    void OnAudioRead(float[] data)
    {
        print(data.Length);
        int count = 0;
        while (count < data.Length)
        {
            //data[count] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate));
            data[count] = samples[count / 2] * 2;
            position++;
            count++;
        }
    }


    // Update is called once per frame
    void Update () {
        //Debug.Log(audio.time);
	}
}
