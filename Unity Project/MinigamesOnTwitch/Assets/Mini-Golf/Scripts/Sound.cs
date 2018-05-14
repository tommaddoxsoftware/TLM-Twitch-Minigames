using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name; // sound name

    public AudioClip clip; // soun clip

    public AudioMixerGroup audioMixerGroup; // audio mixer group

    public bool loop; // boolean loop, used for looping sounds

    [Range(0f, 1f)]
    public float volume; // float value for volume in the range of 0 - 1
    [Range(0.1f, 3f)]
    public float pitch; // float value for pitch in the range of 0.1 - 3

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f; // float value for randomVolume in the range of 0 - 0.5
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f; // float value for randomPitch in the range of 0 - 0.5

    [HideInInspector]
    public AudioSource source; // Audio Source which is hidden in the inspector view


}
