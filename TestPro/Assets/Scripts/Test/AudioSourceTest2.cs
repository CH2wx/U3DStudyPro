using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceTest2 : MonoBehaviour {

    public AudioSource audio;
    public AudioMixer mixer;
    public float volume = 0.7f;
    public float pitch = 1.0f;

    private void SetAudioMixerInfo()
    {
        mixer.SetFloat("MasterVolume", volume);
        mixer.SetFloat("MasterPitch", pitch);
        BoxCollider box = new BoxCollider();
    }

    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
        audio.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[0];
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            SetAudioMixerInfo();
        }
	}
}
