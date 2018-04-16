using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour {

    private int numPlayers = 0;
    public float delay = 4f;

    // private TextMeshProUGUI uiText;
    private Text uiText;
    [SerializeField]
    private GameObject usrNamePrefab;
    [SerializeField]
    private GameObject usrScorePrefab;
    [SerializeField]
    private GameObject usrWelcomePrefab;
    [SerializeField]
    private GameObject usrLeftGamePrefab;

	public void UISetPlayerName(GameObject playerObj, string usrName)
    {
        //Get the text object attached to ball UI
        uiText = playerObj.GetComponentInChildren<Text>();
        uiText.text = usrName;

        /*
        //Set their name, and set colour to player's assigned colour
        string usrName3L;
        usrName3L = usrName.Substring(0, 3);

        // uiText.text = usrName; // change uiText to full player name
        uiText.text = usrName3L; // change uiText to only the first 3 letters of player name
        */

        //Set their name, and set colour to player's assigned colour
        uiText.color = playerObj.GetComponent<Ball>().playerColour;

        //Welcome Player Message
        /*
        // Spawn new game object of player join game message for the GUI
        GameObject usrWelcomeMsg = Instantiate(usrWelcomePrefab, GameObject.Find("PlayerMessage").transform);

        //Get the text component for the message
        TextMeshProUGUI message = usrWelcomeMsg.GetComponent<TextMeshProUGUI>();

        // Set the join message
        message.text = (usrName + " has joined the game!");

        // Set Position of message
        message.transform.position = message.transform.position - new Vector3(0, 30, 0);

        // Destroy the game object after the value of delay
        Destroy(usrWelcomeMsg, delay);
        */
    }

    public Color ColorFromUsername(string username)
    {
       Random.seed = username.Length + (int)username[0] + (int)username[username.Length - 1];
        return new Color(Random.Range(0.25f, 0.55f), Random.Range(0.20f, 0.55f), Random.Range(0.25f, 0.55f));

        //        return new Color(Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
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
        usrName.transform.position = usrName.transform.position - new Vector3(0, 30 * numPlayers, 0);
        usrScore.transform.position = usrScore.transform.position - new Vector3(0, 30 * numPlayers, 0);
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

        // Player Left GUI Message
        GameObject usrLeaveGameMsg = Instantiate(usrLeftGamePrefab, GameObject.Find("PlayerMessage").transform);           // Spawn new game object of player that left the game
        TextMeshProUGUI message = usrLeaveGameMsg.GetComponent<TextMeshProUGUI>();  //Get the text component for the message
        message.text = (name.GetComponent<Text>().text + " has left the game!\n Booho");   // Set the join message
        message.transform.position = message.transform.position - new Vector3(0, 30, 0);    // Set Position of message
        Destroy(usrLeaveGameMsg, delay);    // Destroy the game object after the value of delay

        name.GetComponent<Text>().text = "PLAYER LEFT";
        score.GetComponent<Text>().text = "DNF";


        //This'll be used when I've figured out a way to "resort" the scoreboard
        /*
        Destroy(name);
        Destroy(score);
        */

    }


}
