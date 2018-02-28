using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(TwitchIRC))]
public class Test : MonoBehaviour
{
    public int maxMessages = 100;

    public Text test;
    public Text test2;

    public GameObject boxey;

    private TwitchIRC IRC;
    private GameJoin m_playerList;
    private StringSplitter m_Splitter;

    private bool notWon = false;

    private LinkedList<GameObject> messages = new LinkedList<GameObject>();

    private string[] textFile;

    private int randomNum;

    // Use this for initialization
    void Start()
    {
        IRC = this.GetComponent<TwitchIRC>();
        //IRC.SendCommand("CAP REQ :twitch.tv/tags"); //register for additional data such as emote-ids, name color etc.
        IRC.messageRecievedEvent.AddListener(OnChatMsgRecieved);

        m_playerList = this.GetComponent<GameJoin>();
        m_Splitter = this.GetComponent<StringSplitter>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnChatMsgRecieved(string msg)
    {
        //parse from buffer.
        int msgIndex = msg.IndexOf("PRIVMSG #");
        string msgString = msg.Substring(msgIndex + IRC.channelName.Length + 11);
        string user = msg.Substring(1, msg.IndexOf('!') - 1);

        //remove old messages for performance reasons.
        if (messages.Count > maxMessages)
        {
            Destroy(messages.First.Value);
            messages.RemoveFirst();
        }

        string[] msgArray = m_Splitter.Splitter(msgString, ' ');

        m_playerList.Commands(msgArray, user); //Joining commands

        if (m_playerList.CheckIfPlayer(user) != -1) //Checks if the player has joined
        {
            //text game
            if (msgString == "start")
            {
                test.enabled = true;
                var sr = File.OpenText("Assets/Games_Scripts/Phrases.txt");
                textFile = sr.ReadToEnd().Split("\n"[0]);
                randomNum = Random.Range(0, textFile.Length);
                test.text = textFile[randomNum].Substring(0, textFile[randomNum].Length - 1);
            }
            if (msgString.ToLower().Contains(test.text.ToLower()) & notWon == false)
            {
                Debug.Log("----------------------------");
                test2.text = "Winner: " + user;
                notWon = true;
            }


            //end text game

            //move cube
            if (msgString == "Move left")
            {
                Vector3 position = boxey.transform.position;
                position.x += -1;
                boxey.transform.position = position;
            }
            else if (msgString == "Move right")
            {
                Vector3 position = boxey.transform.position;
                position.x += 1;
                boxey.transform.position = position;
            }
            else if (msgString == "Move down")
            {
                Vector3 position = boxey.transform.position;
                position.y += -1;
                boxey.transform.position = position;
            }
            else if (msgString == "Move up")
            {
                Vector3 position = boxey.transform.position;
                position.y += 1;
                boxey.transform.position = position;
            }
            //end move cube

            //colour cube
            if (msgString.Contains("!cubecolour"))
            {
                boxey.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }
            //end colour cube

            //test.text = user;
        }
        Debug.Log(user + " :: " + msgString);
        //Debug.Log(msgString);
        //Debug.Log(test.text);

        ////add new message.
        //CreateUIMessage(user, msgString);


    }
}
