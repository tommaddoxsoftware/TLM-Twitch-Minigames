using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

    private int numPlayers = 0;
    private float fadeSpeed = 1.0f;
    public float delay = 4f;

    private Text uiText;
    [SerializeField]
    private GameObject usrNamePrefab;
    [SerializeField]
    private GameObject usrScorePrefab;
    [SerializeField]
    private Text usrEventText;
   

    private bool animFin = false;
    private Camera mainCam;    
    private Animation cameraIntro;
    [SerializeField]
    private GameObject gameUi;

    private void Start()
    {
        gameUi.SetActive(false);
        mainCam = Camera.main;        
    }

    private void Update()
    {
        Animator anim = mainCam.GetComponent<Animator>();
                
        if (!animFin)
        {
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Camera Idle"))
            {
                animFin = true;
                anim.enabled = false;
                gameUi.SetActive(true);
            }
        }
        
    }
    public void UISetPlayerName(GameObject playerObj, string usrName)
    {
        //Get the text object attached to ball UI
        uiText = playerObj.GetComponentInChildren<Text>();
        uiText.text = usrName;

        //Set their name, and set colour to player's assigned colour
        uiText.color = playerObj.GetComponent<Ball>().playerColour;

        //Send player joined event
        SendEventMessage(usrEventText, "Welcome, " + usrName);
    }

    private void SendEventMessage(Text eventText, string newMsg)
    {
        eventText.text = newMsg;
        FadeIn(1.0f, eventText);
    }

    public Color ColorFromUsername(string username)
    {
        Random.seed = username.Length + (int)username[0] + (int)username[username.Length - 1];
        return new Color(Random.Range(0.15f, 0.55f), Random.Range(0.10f, 0.55f), Random.Range(0.15f, 0.55f));
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
        usrName.transform.position = usrName.transform.position - new Vector3(0, 50 * numPlayers, 0);
        usrScore.transform.position = usrScore.transform.position - new Vector3(0, 50 * numPlayers, 0);
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
    }

    public void FadeIn(float fadeSpeed, Text textToUse)
    {       
        StartCoroutine(FadeInText(fadeSpeed,textToUse));
    }
    public void FadeOut(float fadeSpeed, Text textToUse)
    {
        StartCoroutine(FadeOutText(fadeSpeed, textToUse));
    }

    private IEnumerator FadeInText(float fadeSpeed, Text text)
    {
        //Set alpha to 0
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        //While alpha is less than 1, fade it in.
        while(text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime * fadeSpeed));
            yield return null;
        }

        yield return new WaitForSeconds(4.0f);
        FadeOut(fadeSpeed, usrEventText);
    }
    private IEnumerator FadeOutText(float fadeSpeed, Text text)
    {
        //Set alpha to 1
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

        //While alpha is less than 1, fade it in.
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * fadeSpeed));
            yield return null;
        }
    }


}
