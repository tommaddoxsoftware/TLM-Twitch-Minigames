using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
    public GameObject aim;
    public GameObject power;

    private MinigolfBotReplys m_bot;

    private int m_maxPower = 100;
    private int m_minPower = 1;

    public int aimTimer = 3;

    public GameObject scoreBoardStrokeUi;
    public GameObject scoreboardNameUi;
    public int playerId;

    private int m_lockPos = 0;
    private int m_angle;

    private float m_power = 1;

    public string usrName;
    public Color playerColour;
    private int strokeCount = 0;

    [SerializeField]
    private LevelController m_lvlController;

    //Used to control respawning
    private bool m_outOfBounds = false;
    private Vector3 m_lastPosition;

    private Rigidbody m_rigid;

    private bool m_inMotion = false;

    private int oobTimeout;

    private float magLosStore = 0;

    private Vector3 m_start;

    //Use this for initialization
    void Start() {
        m_rigid = this.GetComponent<Rigidbody>();

        GameObject minigameManager = GameObject.Find("MinigameManager");

        m_lvlController = minigameManager.GetComponent<LevelController>();
        m_start = m_lvlController.StartPos;

        oobTimeout = minigameManager.GetComponent<MinigolfController>().outOfBoundsTimeout;

        ScalePower();

        //Generate a colour for the player
        playerColour = GameObject.Find("UiManager").GetComponent<UiController>().ColorFromUsername(usrName);

        //Once we have a colour, add them to the scoreboard UI, and set the ball UI (Their username)
        GameObject uiManager = GameObject.Find("UiManager");
        uiManager.GetComponent<UiController>().AddToScoreboard(usrName, this.gameObject);
        uiManager.GetComponent<UiController>().UISetPlayerName(this.gameObject, usrName);

    }

    private void OnEnable()
    {
        try
        {
            m_start = m_lvlController.StartPos;
        }
        catch { }
    }

    // Update is called once per frame
    void Update() {
        this.transform.GetChild(0).rotation = Quaternion.Euler(m_lockPos, m_angle, m_lockPos); //Locks the Aim and power from rotating
        //Locks the Aim and power from rotating
        this.transform.GetChild(0).rotation = Quaternion.Euler(m_lockPos, m_angle, m_lockPos);

        if (m_rigid.velocity.magnitude < 0.3f && m_inMotion)
        {
            StopBall();
        }

        //Calculates the delta magnatude
        float magLoss = magLosStore - this.GetComponent<Rigidbody>().velocity.magnitude;

        //Checks if the ball is in motion and the delta magnitude
        if (m_inMotion && magLoss < 0.02)
        {
            StopBall();
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        //Player is now out of bounds / has left one collider and hit another. 
        //Start coroutine, if still out of bounds, respawn them
        if (coll.gameObject.tag == "GolfCourse")
        {
            m_outOfBounds = true;
            //Debug.Log("Player left bounds!");
            StartCoroutine("OutOfBoundsTimer");
        }
    }
    private void OnCollisionEnter(Collision coll)
    {
        //Ball is touching a playable area of the course - deemed in bounds.
        if (coll.gameObject.tag == "GolfCourse")
        {
            bool foundLevel = false;
            GameObject parent = coll.transform.parent.gameObject;

            //Goes through all parents until the course is found or no more parents exist
            while (parent != null && !foundLevel)
            {
                //Compares the parent to the current level
                if (parent == m_lvlController.CurrentCourseObject)
                {
                    Debug.Log("Correct Course");
                    foundLevel = true;
                }
                else {
                    //Attempt to get the next parent
                    try
                    {
                        parent = parent.transform.parent.gameObject;
                    }
                    catch
                    {
                        parent = null;
                    }
                }
            }

            if (foundLevel)
            {
                m_outOfBounds = false;
            }
            else
            {
                //Respawns the player
                Debug.Log("Wrong Course");
                Respawn(m_lastPosition);
                m_outOfBounds = true;
            }
        }
    }
    private void OnCollisionStay(Collision other)
    {
        //Make sure the ball has remained in bounds.
        if (other.gameObject.tag == "GolfCourse")
        {
            //Reset Out Of Bounds timer
            m_outOfBounds = false;
            StopCoroutine("OutOfBoundsTimer");
        }
    }

    IEnumerator OutOfBoundsTimer()
    {
        //Wait for 3 seconds - then perform an OutOfBounds check. This value should probably be set via an editor value
        yield return new WaitForSeconds(oobTimeout);
        if (m_outOfBounds)
        {
            Respawn(m_lastPosition);
        }

    }

    private void Respawn(Vector3 respawnPoint)
    {
        //Just for understanding what position was used to respawn.
        Debug.Log("Attempting to respawn. Current position: " + gameObject.transform.position + " New Position: " + respawnPoint);

        //Remove any momentum whilst resetting position, then respawn at last known position, and reset the object state
        m_rigid.isKinematic = true;
        gameObject.transform.position = respawnPoint;
        m_rigid.isKinematic = false;
    }

    private void SetMovement()
    {
        m_inMotion = true;
    }

    public void StopBall()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        /*
        m_rigid.isKinematic = true;
        m_rigid.isKinematic = false;
        */
        m_rigid.drag = 10.0f;
        m_inMotion = false;

    }

    //Resets the balls angle and power to 0
    public void ResetBallAdjustments()
    {
        m_angle = 0;
        m_power = 1;
        ScalePower();
    }

    private void ScalePower()
    {
        power.SetActive(true);
        StopCoroutine("HideIndicator");
        //Calculates a modified length so that the power indicator isn't massive
        float altLenght = m_power / 6;

        //Scale the angle and power indicator appropriately
        power.transform.localScale = new Vector3(0.2f, 0.06f, 2 + altLenght);
        power.transform.localPosition = new Vector3(0, 0, 0.9f + (altLenght / 2));

        StartCoroutine("HideIndicator");
    }

    IEnumerator HideIndicator()
    {
        yield return new WaitForSeconds(aimTimer);
        power.SetActive(false);
    }


    public void Command(string[] cmd, string user)
    {
        //m_bot.ProcessCommand(user, cmd);

        //Propells the ball in the direction
        if (cmd[0].ToLower() == "!hit")
        {

            if (!m_inMotion)
            {
                m_rigid.drag = 0.8f;
                //Store the transform (so we can access the position) before we fire the ball
                m_lastPosition = gameObject.transform.position;

                this.transform.GetChild(0).gameObject.SetActive(false);
                Vector3 dir = this.transform.position - aim.transform.position;//Gets the vector for the direction to an point infront of the ball

                m_rigid.velocity = -dir * m_power; //Applys the velocity to the ball

                Invoke("SetMovement", 3);

                //Update scores
                strokeCount++;
                GameObject.Find("UiManager").GetComponent<UiController>().UpdateScore(scoreBoardStrokeUi.GetComponent<Text>(), strokeCount.ToString());

                //Sends message to the bot
                m_bot.Hit(user);

                /*******************/
                /*     To Do:     */
                /*****************/
                //Store each player's score per course
                //Only display score per course
            }
        }

        if (cmd[0].ToLower() == "!stop")
        {
            StopBall();
        }


        //Sets the angle of the ball
        if (cmd[0].ToLower() == "!angle" || cmd[0].ToLower() == "!an")
        {
            try //Trys to convert the command to a string
            {
                int angVal = int.Parse(cmd[1]);

                //Checks if the angle is valid
                if (angVal >= 0 && angVal <= 360)
                {
                    //Stores the angle
                    m_angle = angVal;

                    //Sends message to the bot
                    m_bot.Angle(user, m_angle);                    
                }
                else if (angVal > 360)
                {
                    //Stores the angle
                    m_angle = 0;

                    //Sends message to the bot
                    m_bot.OverMaxAngle(user);
                }
                else if (angVal < 0)
                {
                    //Stores the angle
                    m_angle = 0;

                    //Sends message to the bot
                    m_bot.UnderMinAngle(user);
                }

                ScalePower();
            }
            catch { }
        }

        if (cmd[0].ToLower() == "!adjust" || cmd[0].ToLower() == "!ad")
        {
            try
            {
                int adjVal = int.Parse(cmd[1]);

                int newAng = m_angle + adjVal;

                /*
                //Limits max and min angle
                if (newAng > 360)
                {
                    newAng -= 360;
                }
                else if (newAng < 0)
                {
                    newAng += 360;
                }
                */

                m_angle += adjVal;

                //Sends message to the bot
                m_bot.Adjust(user, adjVal, m_angle);

                ScalePower();
            }

            catch { }
        }

        //Sets the angle of the ball
        if (cmd[0].ToLower() == "!power" || cmd[0].ToLower() == "!pwr")
        {
            try
            {
                float pwVal = float.Parse(cmd[1]);

                //Checks if the angle is valid
                if (pwVal >= m_minPower && pwVal <= m_maxPower)
                {
                    m_power = pwVal;

                    //Sends message to the bot
                    m_bot.Power(user, m_power);
                }
                else if (pwVal > m_maxPower)
                {
                    //Do something here, possibly send admin message to twitch chat
                    m_power = m_maxPower;

                    //Sends message to the bot
                    m_bot.OverMaxPower(user, m_maxPower);
                }
                else if(pwVal < m_minPower)
                {
                    m_power = m_minPower;

                    //Sends message to the bot
                    m_bot.UnderMinPower(user, m_minPower);
                }

                ScalePower();
            }
            catch { }
        }

        if (cmd[0].ToLower() == "!reset")
        {
            ResetBall();

            //Sends message to the bot
            m_bot.Reset(user);
        }

        if (cmd[0].ToLower() == "!commands")
        {
            //Sends message to the bot
            m_bot.GolfCommands();
        }
    }

    public void ResetBall()
    {
        //Resets the ball
        this.transform.position = m_start;

        ResetBallAdjustments();
    }

    public int MaxPower {
        set { m_maxPower = value; }
    }

    public int MinPower
    {
        set { m_minPower = value; }
    }

    public MinigolfBotReplys Bot
    {
        set { m_bot = value; }
    }
}
