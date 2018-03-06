using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour {
    public int maxMessages = 100;

    public Ball ball;

    public GameObject ballPrefab;

    private TwitchIRC m_IRC;
    private LinkedList<GameObject> m_messages = new LinkedList<GameObject>();
    private int randomNum;

    private StringSplitter m_Splitter;
    private GameJoin m_playerList;

    private GameObject[] m_playerBalls;


    // Use this for initialization
    void Start () {
        m_IRC = this.GetComponent<TwitchIRC>();
        m_IRC.messageRecievedEvent.AddListener(OnChatMsgRecieved);

        m_Splitter = this.GetComponent<StringSplitter>();
        m_playerList = this.GetComponent<GameJoin>();

        m_playerBalls = new GameObject[m_playerList.MaxPlayers];

        //Creates all balls
        for (int i = 0; i < m_playerBalls.Length; i++)
        {
            m_playerBalls[i] = Instantiate(ballPrefab);

            //Toggeles colling with each ball
            for (int j = 0; j < m_playerBalls.Length; j++)
            {
                try
                {
                    Physics.IgnoreCollision(m_playerBalls[i].GetComponent<Collider>(), m_playerBalls[j].GetComponent<Collider>()); //Prevents balls from colleding when instantiating
                }
                catch { }
            }
        }
    }


    void OnChatMsgRecieved(string msg)
    {
        //parse from buffer.
        int msgIndex = msg.IndexOf("PRIVMSG #");
        string msgString = msg.Substring(msgIndex + m_IRC.channelName.Length + 11);
        string user = msg.Substring(1, msg.IndexOf('!') - 1);

        Debug.Log(user + " :: " + msgString);

        string[] msgArray = m_Splitter.Splitter(msgString, ' ');

        //remove old messages for performance reasons.
        if (m_messages.Count > maxMessages)
        {
            Destroy(m_messages.First.Value);
            m_messages.RemoveFirst();
        }

        m_playerList.Commands(msgArray, user); //Joining commands

        int player = m_playerList.CheckIfPlayer(user); //Gets the index of a player if they are in the game

        if (player != -1) //Checks if the player has joined
        {
            //ball.Command(msgArray); //Runs ball commands

            m_playerBalls[player].GetComponent<Ball>().Command(msgArray); //Runs ball commands for the player
        }
    }
}
