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
    [SerializeField]
    private GameObject startBtn;
    [SerializeField]
    private Text welcomeLbl;

    private void Start()
    {
        //Set listener for input fields. Delegate the function to SetUsername and SetOAuth
        usernameInput.onEndEdit.AddListener(delegate { SetUsername(usernameInput); });
        oauthInput.onEndEdit.AddListener(delegate { SetOAuth(oauthInput); });
        maxPlayerSlider.onValueChanged.AddListener(delegate { SetMaxPlayers(maxPlayerSlider); });

        //Check if the app has been loaded before. Probably a better way to do it, but this works just fine.
        if(!PlayerPrefs.HasKey("FirstTime"))
        {
            PlayerPrefs.SetInt("FirstTime", 1);
        }
        else
        {
            PlayerPrefs.SetInt("FirstTime", 0);
        }

        //If it IS the players first time, let's send the user to the tutorial!
        if (PlayerPrefs.GetInt("FirstTime") == 1) {
            //Tutorial is at index 2
            GameObject.Find("GameManager").GetComponent<SceneManager>().LoadScene(2);
        }
        else
        {            //Not the first time - but make sure that OAuth and Channel name is set!
            if (PlayerPrefs.HasKey("TwitchUsr") && PlayerPrefs.HasKey("TwitchAuth"))
            {
                //Set the inputs to the playerprefs
                usernameInput.text = PlayerPrefs.GetString("TwitchUsr");
                oauthInput.text = PlayerPrefs.GetString("TwitchAuth");
                welcomeLbl.text = "Welcome back, " + PlayerPrefs.GetString("TwitchUsr") + "!\nNot you? Change user info in Settings!";

                EnableStartButton();
            }
            else
            {
                //They've completed the tutorial - but haven't yet set their information!
                welcomeLbl.text = "Welcome to Minigames on Twitch!\nHead to Settings to connect this app to your twitch channel!";
            }

        }

        //Set Max Players
        if (PlayerPrefs.HasKey("MaxPlayers"))
            maxPlayerLbl.text = PlayerPrefs.GetInt("MaxPlayers").ToString();
        else
        {
            maxPlayerLbl.text = "1";
            PlayerPrefs.SetInt("MaxPlayers", 1);
        }
        
    }
    private void EnableStartButton()
    {
        //OAuth and Username set - enable the button!
        startBtn.GetComponent<Button>().interactable = true;
    }

    public void GetTwitchAuth()
    {
        Application.OpenURL("https://twitchapps.com/tmi/");
    }

    //Saves Username from input field to playerprefs
    private void SetUsername(InputField usr)
    {
        PlayerPrefs.SetString("TwitchUsr", usr.text);

        //Change Welcome Label text. Check if OAuth is set.
        welcomeLbl.text = "Welcome back, " + PlayerPrefs.GetString("TwitchUsr") + "!\nNot you? Change user info in Settings!";
        if (!PlayerPrefs.HasKey("TwitchAuth"))
            welcomeLbl.text += "\nPlease make sure you set your OAuth in settings!";
        
        //If both inputs are set - enable start button
        if (PlayerPrefs.HasKey("TwitchAuth"))
            EnableStartButton();
    }
    
    //Saves OAuth from input field to playerprefs
    private void SetOAuth(InputField auth)
    {
        PlayerPrefs.SetString("TwitchAuth", auth.text);

        //If both inputs are set - enable start button
        if (PlayerPrefs.HasKey("TwitchUsr"))
            EnableStartButton();
    }

    //Sets the maxplayers slider value
    private void SetMaxPlayers(Slider maxPlayers)
    {
        PlayerPrefs.SetInt("MaxPlayers", (int) maxPlayers.value);
        maxPlayerLbl.text = maxPlayers.value.ToString();

    }

    //Checks if game is running in unity editor for testing. In a deployed build, this quits the app.
    public void ExitApplication()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit ();
        #endif
    }
}
