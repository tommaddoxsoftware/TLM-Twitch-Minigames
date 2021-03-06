﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MinigolfController))]
public class LevelController : MonoBehaviour
{
    //Public
    public GameObject[] levels;

    //Private
    private GameObject[] m_starts;
    private GameObject[] m_ends;

    private MinigolfController m_ballControl;

    private bool[] m_playerState;

    private int m_level;

    [Tooltip("Customisable name for start point")]
    [SerializeField]
    private string m_startName = "Start";

    [Tooltip ("Customisable name for end point")]
    [SerializeField]
    private string m_endName = "End";


    // Use this for initialization
    void Start()
    {
        //Gets the ball controller
        m_ballControl = this.GetComponent<MinigolfController>(); 

        m_level = 0; //Starts the level at level 1

        m_starts = new GameObject[levels.Length];
        m_ends = new GameObject[levels.Length];

        //Finds the start and finish for all levels 
        for (int i = 0; i < levels.Length; i++)
        {

            //Finds the start and ends
            m_starts[i] = levels[i].transform.Find("START").gameObject;
            m_ends[i] = levels[i].transform.Find("END").gameObject;
            
            //Disables objectives
            m_starts[i].SetActive(false);
            m_ends[i].SetActive(false);

        }

        //Activates starting level
        m_starts[0].SetActive(true);
        m_ends[0].SetActive(true);
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
            //Checks if that player has finnished
            if (!m_playerState[playingPlayers[i]]) 
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
        //Save the score to player's score array, and reset
        Ball player = ball.GetComponent<Ball>();
        player.overallScore += player.strokeCount;
        player.playerScore[m_level] = player.strokeCount;
        player.strokeCount = 0;
        Debug.Log("Player scored! It took them " + player.overallScore + " turns");
        UiController uiControl = GameObject.Find("UiManager").GetComponent<UiController>();
        uiControl.PopulateScoreboard();

        FinishBall(ballIndex);
    }

    public void FinishBall(int ballIndex)
    {

        // Play sound PlayerCompleteCourse1 when a player collides with the hole
        FindObjectOfType<AudioManager>().Play("PlayerCompletedCourse");

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
                    //Deactivates the level that was just done
                    m_starts[m_level].SetActive(false);
                    m_ends[m_level].SetActive(false);

                   

                    //Increases the level
                    m_level++;

                    UiController uiControl = GameObject.Find("UiManager").GetComponent<UiController>();
                    uiControl.UpdateCourseNum(m_level+1);

                    //Activates the next level
                    m_starts[m_level].SetActive(true);
                    m_ends[m_level].SetActive(true);
                }
                else
                {
                    //##CODE FOR END GAME##
                    //Temp code repeats level
                    m_starts[m_level].SetActive(false);
                    m_ends[m_level].SetActive(false);

                    m_level = 0;

                    m_starts[m_level].SetActive(true);
                    m_ends[m_level].SetActive(true);
                }

                //Move the balls to the new start point
                m_ballControl.MoveBalls();


                // Run PlayCourseCompleted after 0.5 seconds
                Invoke("PlayCourseCompleted", 0.5f);

                ResetBallState();
            }
        }
    }

    // Play sound for all players completed the current course
    private void PlayCourseCompleted()
    {
        FindObjectOfType<AudioManager>().Play("CourseCompleted"); // find AudioManager in scene and play sound
    }

    //Resets the players states to false
    private void ResetBallState()
    {
        for (int i = 0; i < m_playerState.Length; i++)
        {
            m_playerState[i] = false;
        }
    }

    public void SetPlayerState(int player, bool state)
    {
        m_playerState[player] = state;
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

    public GameObject CurrentCourseObject
    {
        get { return levels[m_level]; }
    }

    public int CurrentCourse
    {
        get { return m_level; }
    }
}
