using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallControl))]
public class LevelController : MonoBehaviour
{
    //Public
    public GameObject[] levels;

    //Private
    private GameObject[,] m_objectives;

    private BallControl m_ballControl;

    private bool[] m_playerState;

    private int m_level;

    private bool[] playerState;

    // Use this for initialization
    void Start()
    {
        m_ballControl = this.GetComponent<BallControl>(); //Finds the ball controller

        m_level = 0; //Starts the level at level 1

        m_objectives = new GameObject[levels.Length, 2]; //Sets up the array for the start and finish  
                                                            //[level,0] start | [level,1] end

        //Finds the start and finish for all levels 
        for (int i = 0; i < levels.Length; i++)
        {
            m_objectives[i, 0] = levels[i].transform.Find("Start").gameObject;
            m_objectives[i, 1] = levels[i].transform.Find("End").gameObject;
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
        //Checks that all players have gotten to the hole
        for (int i = 0; i < m_playerState.Length; i++)
        {
            if (m_playerState[i] == false)
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
            m_playerState[ballIndex] = true; //Changes the balls state
            m_ballControl.HideBall(ballIndex); //Hides the ball

            //Checks to see that all balls have finished
            if (CheckAllFinished())
            {
                //Stops the game from going over the number of holes
                if (m_level < levels.Length - 1)
                {
                    m_level++; //Increases the level
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
        get { return m_objectives[m_level, 0].transform.position; }
    }
}
