using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip clip;
    public int scale = 1;

    private float[] originalSamples;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        clip = audio.clip;
        originalSamples = new float[clip.samples * clip.channels];
        clip.GetData(originalSamples, 0);
        print(originalSamples.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
        ExchangeFrequency();
    }

    private void ExchangeFrequency()
    {
        //AudioClip newClip = AudioClip.Create("newClip", originalSamples.Length, 1, originalSamples.Length, false);
        float[] samples = new float[originalSamples.Length];
        for (int i = 0; i < originalSamples.Length; i++)
        {
            float v = Mathf.Asin(originalSamples[i]);
            samples[i] = originalSamples[i] + Mathf.Sin(scale * v) / scale;
        }
        audio.clip.SetData(samples, 0);
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExchangeFrequency();
        }
    }
}
