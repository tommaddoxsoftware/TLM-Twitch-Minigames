using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMenuManager : MonoBehaviour {
    [SerializeField]
    private InputField usernameInput;
    [SerializeField]
    private InputField oauthInput;
    [SerializeField]
    private Slider maxPlayerSlider;
    [SerializeField]
    private Text maxPlayerLbl;

    private void Start()
    {
        //Set listener for input fields. Delegate the function to SetUsername and SetOAuth
        usernameInput.onEndEdit.AddListener(delegate { SetUsername(usernameInput); });
        oauthInput.onEndEdit.AddListener(delegate { SetOAuth(oauthInput); });
        maxPlayerSlider.onValueChanged.AddListener(delegate { SetMaxPlayers(maxPlayerSlider); });

        //Set Max Players
        if (PlayerPrefs.HasKey("MaxPlayers"))
            maxPlayerLbl.text = PlayerPrefs.GetInt("MaxPlayers").ToString();
        else
        {
            maxPlayerLbl.text = "1";
            PlayerPrefs.SetInt("MaxPlayers", 1);
        }

        if (PlayerPrefs.HasKey("TwitchUsr"))
            usernameInput.text = PlayerPrefs.GetString("TwitchUsr");
        if (PlayerPrefs.HasKey("TwitchAuth"))
            oauthInput.text = PlayerPrefs.GetString("TwitchAuth");

    }
    
    //Button Press - gets OAuth URL for user
    public void GetTwitchAuth()
    {
        Application.OpenURL("https://twitchapps.com/tmi/");
    }

    //Set username input field based on playerprefs
    private void SetUsername(InputField usr)
    {
        PlayerPrefs.SetString("TwitchUsr", usr.text);
    }

    //Set OAuth input field based on playerprefs  
    private void SetOAuth(InputField auth)
    {
        PlayerPrefs.SetString("TwitchAuth", auth.text);
    }

    //Set Max Player slider value based on playerprefs
    private void SetMaxPlayers(Slider maxPlayers)
    {
        PlayerPrefs.SetInt("MaxPlayers", (int) maxPlayers.value);
        maxPlayerLbl.text = maxPlayers.value.ToString();

    }
    public void ExitApplication()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit ();
        #endif
    }
}
