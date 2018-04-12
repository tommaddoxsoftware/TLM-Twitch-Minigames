using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {
    [SerializeField]
    private InputField usernameInput;
    [SerializeField]
    private InputField oauthInput;

    private void Start()
    {
        //Set listener for input fields. Delegate the function to SetUsername and SetOAuth
        usernameInput.onEndEdit.AddListener(delegate { SetUsername(usernameInput); });
        oauthInput.onEndEdit.AddListener(delegate { SetOAuth(oauthInput); });
    }
    
    public void GetTwitchAuth()
    {
        Application.OpenURL("https://twitchapps.com/tmi/");
    }

    private void SetUsername(InputField usr)
    {
        PlayerPrefs.SetString("TwitchUsr", usr.text);
    }
    
    private void SetOAuth(InputField auth)
    {
        PlayerPrefs.SetString("TwitchAuth", auth.text);
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
