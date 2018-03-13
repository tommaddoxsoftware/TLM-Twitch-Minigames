using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {
 
    private Text uiText;

	public void UISetPlayerName(GameObject playerObj, string usrName)
    {
        uiText = playerObj.GetComponent<Text>();
        uiText.text = usrName;
    }

   
}
