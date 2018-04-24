using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

    private int numPlayers = 0;
    private float fadeSpeed = 1.0f;
    public float delay = 4f;

    //Player name text above ball
    private Text uiText;

    //Scoreboard text prefabs
    [SerializeField]
    private GameObject usrNamePrefab;
    [SerializeField]
    private GameObject usrScorePrefab;
    [SerializeField]

    //Event Popup text
    private Text usrEventText;
   
    //Stores Main Camera and its animation controller.
    private bool animFin = false;
    private Camera mainCam;
    private Animator anim;

    //Entire Game UI panel
    [SerializeField]
    private GameObject gameUi;

    //Stores the total number of courses
    private int numCourses;

    //Level Timer
    System.TimeSpan timer;

    //Get UI components for Game Vars
    [SerializeField]
    private Text courseNumUi;
    [SerializeField]
    private Text themeNameUi;
    [SerializeField]
    private Text timerUi;

    private void Start()
    {
        //Disable UI and grab the camera (for flyby anim)
        gameUi.SetActive(false);
        mainCam = Camera.main;
        anim = mainCam.GetComponent<Animator>();

        //Get total number of courses
        numCourses = GameObject.Find("MinigameManager").GetComponent<LevelController>().levels.Length;

    }

    private void Update()
    {

        //Check whether the intro is complete
        if (!animFin)
        {
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Camera Idle"))
            {
                //Disable the animation, cancel the condition for this statement
                animFin = true;
                anim.enabled = false;

                //Enable game UI and set text values, sent event to text popup
                mainCam.rect = new Rect(0.1875f, 0, 0.625f, 0);
                gameUi.SetActive(true);
                SetCourseNum(1);
                SetThemeName("Script Test");
                SetTimer(300);
                SendEventMessage(usrEventText, "Type !join to participate");
            }
        }
        
    }
    private void SetCourseNum(int newCourseNum)
    {       
        //Set the text
        courseNumUi.text = newCourseNum.ToString() + "/" + numCourses;

    }
    private void SetThemeName(string name)
    {
        //Set theme name text
        themeNameUi.text = name;
    }
    private void SetTimer(int seconds)
    {
        timer = System.TimeSpan.FromSeconds(seconds);
        timerUi.text = string.Format("{0}:{1:D2}", (int)timer.TotalMinutes, timer.Seconds);
    }
    public void StartTimer(int seconds)
    {

    }
    private IEnumerator Countdown(int seconds)
    {
        //If the next tick isn't 0, count down
        if (--seconds > 0)
        {
            seconds--;
            yield return new WaitForSeconds(1);
            SetTimer(seconds);
        }
        else
            yield break;
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
        //Set the event text to a new message, start the fade in function
        eventText.text = newMsg;
        FadeIn(1.0f, eventText);
    }

    public Color ColorFromUsername(string username)
    {
        //Generate a colour from the username length
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
        //Change the user's score on the scoreboard
        scoreText.text = newScore;
    }

    public void RemoveFromScoreboard(GameObject player)
    {
        
        //int position = player.GetComponent<Ball>().playerId;
        //Get player's UI components on scoreboard
        GameObject name = player.GetComponent<Ball>().scoreboardNameUi;
        GameObject score = player.GetComponent<Ball>().scoreBoardStrokeUi;

        //Set the text correctly.
        name.GetComponent<Text>().text = "PLAYER LEFT";
        score.GetComponent<Text>().text = "DNF";
    }

    public void FadeIn(float fadeSpeed, Text textToUse)
    {

        StopAllCoroutines();
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
