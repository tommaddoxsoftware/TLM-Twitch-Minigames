﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {


    public Sound[] sounds; //Array of sounds from the class Sound

	// Use this for initialization
	void Awake () {

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>(); //Add an Audio Source to the Audio Manager
            s.source.clip = s.clip; // Take the clip

            s.source.outputAudioMixerGroup = s.audioMixerGroup; // Take the Audio Mixer Group
            s.source.volume = s.volume; // Take the volume
            s.source.pitch = s.pitch; // Take the pitch
            s.source.loop = s.loop; // Take the boolean of loop
        }
    }

    public void Start()
    {
        Play("BackgroundSound"); // Play background sound on start
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) // if there is no sound of specified name, return a Warning
        {
            Debug.LogWarning("Sound: " + name + "cannot be found!");
            return;
        }

        s.source.volume = s.volume * (1 + UnityEngine.Random.Range(-s.randomVolume / 2f, s.randomVolume / 2f)); // Randomise the original value of volume for the sound
        s.source.pitch = s.pitch * (1 + UnityEngine.Random.Range(-s.randomPitch / 2f, s.randomPitch / 2f)); // Randomise the original value of pitch for the sound
        s.source.Play(); // Play the Sound
    }
}
