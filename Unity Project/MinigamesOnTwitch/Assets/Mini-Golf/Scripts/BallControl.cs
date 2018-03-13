using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameJoin))]
public class BallControl : MonoBehaviour
{
    public int maxMessages = 100;

    public Ball ball;

    public GameObject ballPrefab;

    public GameObject start;

    private TwitchIRC m_IRC;
    private LevelController m_levelControl;

    private LinkedList<GameObject> m_messages = new LinkedList<GameObject>();

    private StringSplitter m_Splitter;
    private GameJoin m_playerList;

    private GameObject[] m_playerBalls;
    
    // Use this for initialization
    void Start()
    {
        m_IRC = this.GetComponent<TwitchIRC>();
        m_IRC.messageRecievedEvent.AddListener(OnChatMsgRecieved);

        m_Splitter = this.GetComponent<StringSplitter>();
        m_playerList = this.GetComponent<GameJoin>();

        m_playerBalls = new GameObject[m_playerList.MaxPlayers];

        m_levelControl = this.GetComponent<LevelController>();
        m_levelControl.Init(m_playerList.playerCount);

        
        for (int i = 0; i < m_playerList.MaxPlayers; i++)
        {
            //Creates and set ups all balls
            //eateBall(i);
            m_playerBalls[i] = Instantiate(ballPrefab);
            m_playerBalls[i].SetActive(false);

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

        if (player != -1 && m_levelControl.PlayerState(player) == false) //Checks if the player has joined and that their ball is in play
        {
            //ball.Command(msgArray); //Runs ball commands

            m_playerBalls[player].GetComponent<Ball>().Command(msgArray); //Runs ball commands for the player
        }
    }


    private void CreateBall(int index)
    {
        m_playerBalls[index].transform.position = m_levelControl.StartPos;
        m_playerBalls[index].SetActive(true);
    }

    private void RemoveBall(int index)
    {
        m_playerBalls[index].SetActive(false);
    }

    public int FindBallIndex(GameObject ball)
    {
        //Gets the location of the ball in the array
        for (int i = 0; i < m_playerBalls.Length; i++)
        {
            if (ball == m_playerBalls[i])
            {
                return i; //Rerturns the index of the ball
            }
        }

        return -1; //Returns -1 if gameobject is not a ball
    }

    public void HideBall(int ballIndex)
    {
        m_playerBalls[ballIndex].SetActive(false); //Hides the bal
    }

    public void PlayerJoined(int index)
    {
        CreateBall(index);
        m_playerBalls[index].GetComponent<Ball>().StopBall();
    }

    public void PlayerLeft(int index)
    {
        RemoveBall(index);
        m_playerBalls[index].GetComponent<Ball>().StopBall();
    }

    public void MoveBalls()
    {
        for (int i = 0; i < m_playerBalls.Length; i++)
        {
            m_playerBalls[i].SetActive(true); //Shows the ball 
            m_playerBalls[i].transform.position = m_levelControl.StartPos; //Move the ball to the new start location
            m_playerBalls[i].GetComponent<Ball>().StopBall();
        }
    }
}
