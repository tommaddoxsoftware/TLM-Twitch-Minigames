﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

    private int numPlayers = 0;

    private Text uiText;
    [SerializeField]
    private GameObject usrNamePrefab;
    [SerializeField]
    private GameObject usrScorePrefab;

	public void UISetPlayerName(GameObject playerObj, string usrName)
    {
        //Get the text object attached to ball UI
        uiText = playerObj.GetComponentInChildren<Text>();

        //Set their name, and set colour to player's assigned colour
        uiText.text = usrName;
        uiText.color = playerObj.GetComponent<Ball>().playerColour;
    }

    public Color ColorFromUsername(string username)
    {
       Random.seed = username.Length + (int)username[0] + (int)username[username.Length - 1];
        return new Color(Random.Range(0.25f, 0.55f), Random.Range(0.20f, 0.55f), Random.Range(0.25f, 0.55f));
    }

    public void AddToScoreboard(string username, GameObject player)
    {
        
        //Add a new piece of UI for the player, one for name, one for score
        GameObject usrName = Instantiate(usrNamePrefab, GameObject.Find("Scoreboard").transform);
        GameObject usrScore = Instantiate(usrScorePrefab, GameObject.Find("Scoreboard").transform);

        //Assign the UI GameObjects to ball (Will be used if removing, updating)
        player.GetComponent<Ball>().scoreboardNameUi = usrName;
        player.GetComponent<Ball>().scoreBoardStrokeUi = usrScore;


        //Get the text component for each
        Text name = usrName.GetComponent<Text>();
        Text score = usrScore.GetComponent<Text>();

        //Set color based on the player's assigned colour
        name.color = score.color = player.GetComponent<Ball>().playerColour;

        //Set the appropriate text.
        name.text = username;
        score.text = "0";       
        
        //Position the text based on how many players
        usrName.transform.position = usrName.transform.position - new Vector3(0, 20 * numPlayers, 0);
        usrScore.transform.position = usrScore.transform.position - new Vector3(0, 20 * numPlayers, 0);
        player.GetComponent<Ball>().playerId = numPlayers;
        numPlayers++;
    }

    public void UpdateScore(Text scoreText, string newScore) {
        scoreText.text = newScore;
    }

    public void RemoveFromScoreboard(GameObject player)
    {
        int position = player.GetComponent<Ball>().playerId;
        GameObject name = player.GetComponent<Ball>().scoreboardNameUi;
        GameObject score = player.GetComponent<Ball>().scoreBoardStrokeUi;
        name.GetComponent<Text>().text = "PLAYER LEFT";
        score.GetComponent<Text>().text = "DNF";
        
        //This'll be used when I've figured out a way to "resort" the scoreboard
        /*
        Destroy(name);
        Destroy(score);
        */

    }


}
