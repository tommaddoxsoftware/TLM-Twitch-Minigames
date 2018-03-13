using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {
 
    private Text uiText;

	public void UISetPlayerName(GameObject playerObj, string usrName)
    {
        uiText = playerObj.GetComponentInChildren<Text>();
        uiText.text = usrName;
        uiText.color = ColorFromUsername(usrName);
    }

    Color ColorFromUsername(string username)
    {
        Random.seed = username.Length + (int)username[0] + (int)username[username.Length - 1];
        return new Color(Random.Range(0.25f, 0.55f), Random.Range(0.20f, 0.55f), Random.Range(0.25f, 0.55f));
    }


}
