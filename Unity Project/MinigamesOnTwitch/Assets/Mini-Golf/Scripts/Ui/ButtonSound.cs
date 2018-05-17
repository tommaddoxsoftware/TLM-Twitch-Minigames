using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour {

    public AudioSource buttonSound; //Audio source for button sound effects
    public AudioSource menuSound; // Audio source for menu music
    public AudioClip buttonHover; // audio clip for button hover sound effect
    public AudioClip buttonPress; // audio clip for button press sound effect 

    public void Start()
    {
        menuSound.loop = true; // set menu music to loop
        buttonSound.loop = false; // set button sounds to not loop
        menuSound.playOnAwake = true; // set menu music to play on awake
        menuSound.Play(); // play menu music
    }

    public void PlayHover()
    {
        buttonSound.PlayOneShot(buttonHover); // play one shot of button hover audio clip
    }

    public void PlayPress()
    {
        buttonSound.PlayOneShot(buttonPress); // play one shot of button press audio clip
    }
}
