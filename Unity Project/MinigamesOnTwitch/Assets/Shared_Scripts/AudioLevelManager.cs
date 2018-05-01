using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioLevelManager : MonoBehaviour {
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

	// Use this for initialization
	void Start () {

        //Set volume levels if there's any playerprefs, otherwise default values will remain
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
            audioMixer.SetFloat("MasterVol", masterSlider.value);
        }

        if (PlayerPrefs.HasKey("SfxVol"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SfxVol");
            audioMixer.SetFloat("SfxVol", sfxSlider.value);
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
            audioMixer.SetFloat("MusicVol", musicSlider.value);
        }
    }


    public void SetMasterVol(float masterVol)
    {
        PlayerPrefs.SetFloat("MasterVol", masterVol);
        audioMixer.SetFloat("MasterVol", masterVol);
    }

    public void SetSfxVol(float sfxVol)
    {
        PlayerPrefs.SetFloat("SfxVol", sfxVol);
        audioMixer.SetFloat("SfxVol", sfxVol);
    }

    public void SetMusicVol(float musicVol)
    {
        PlayerPrefs.SetFloat("MusicVol", musicVol);
        audioMixer.SetFloat("MusicVol", musicVol);
    }






}
