﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallControl))]
public class LevelController : MonoBehaviour
{
    //Public
    public GameObject[] levels;

    //Private
    private GameObject[] m_starts;
    private GameObject[] m_ends;

    private BallControl m_ballControl;

    private bool[] m_playerState;

    private int m_level;

    // Use this for initialization
    void Start()
    {
        //Gets the ball controller
        m_ballControl = this.GetComponent<BallControl>(); 

        m_level = 0; //Starts the level at level 1

        m_starts = new GameObject[levels.Length];
        m_ends = new GameObject[levels.Length];

        //Finds the start and finish for all levels 
        for (int i = 0; i < levels.Length; i++)
        {
            //Finds the start
            m_starts[i] = levels[i].transform.Find("START").gameObject;
            m_ends[i] = levels[i].transform.Find("END").gameObject;
        }
    }

    // Update is called once per frame
    public void Init(int playerCount)
    {
        //Sets all player states to false at the start
        m_playerState = new bool[playerCount];

        ResetBallState();
    }

    public bool CheckAllFinished()
    {
        //Gets the active players indexes
        List<int> playingPlayers = this.GetComponent<GameJoin>().GetActivePlayers(); 

        for (int i = 0; i < playingPlayers.Count; i++)
        {
            if (!m_playerState[playingPlayers[i]]) //Checks if that player has finnished
            {
                return false;
            }
        }

        return true;
    }

    //Handles what happens when a ball is in the hole
    public void BallHole(GameObject ball)
    {
        //Finds the index of the ball in the ball array
        int ballIndex = m_ballControl.FindBallIndex(ball);

        //If the object isnt a ball do nothing
        if (ballIndex != -1)
        {
            //Changes the balls state
            m_playerState[ballIndex] = true; 

            //Hides the ball
            m_ballControl.HideBall(ballIndex); 

            //Checks to see that all balls have finished
            if (CheckAllFinished())
            {
                //Stops the game from going over the number of holes
                if (m_level < levels.Length - 1)
                {
                    //Increases the level
                    m_level++; 
                }
                else
                {
                    //##CODE FOR END GAME##
                }

                m_ballControl.MoveBalls(); //Move the balls to the new start point

                ResetBallState(); 
            }
        }
    }

    //Resets the players states to false
    private void ResetBallState()
    {
        for (int i = 0; i < m_playerState.Length; i++)
        {
            m_playerState[i] = false;
        }
    }

    //Returns the state of the player
    public bool PlayerState(int player)
    {
        return m_playerState[player];
    }

    //Gives the position of the current levels start point
    public Vector3 StartPos
    {
        get { return m_starts[m_level].transform.position; }
    }
}
