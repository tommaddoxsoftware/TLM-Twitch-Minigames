using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
    public GameObject aim;
    public GameObject power;

    public int maxPower = 50;
    public int minPower = 1;

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
	void Start () {
        m_rigid = this.GetComponent<Rigidbody>();

        GameObject minigameManager = GameObject.Find("MinigameManager");

        m_lvlController = minigameManager.GetComponent<LevelController>();

        oobTimeout = minigameManager.GetComponent<BallControl>().outOfBoundsTimeout;

        ScalePower();
 
        //Generate a colour for the player
        playerColour = GameObject.Find("UiManager").GetComponent<UiController>().ColorFromUsername(usrName);

        //Once we have a colour, add them to the scoreboard UI, and set the ball UI (Their username)
        GameObject uiManager = GameObject.Find("UiManager");
        uiManager.GetComponent<UiController>().AddToScoreboard(usrName, this.gameObject);
        uiManager.GetComponent<UiController>().UISetPlayerName(this.gameObject, usrName); 

    }

    private void Awake()
    {
        m_start = transform.position;
    }

    // Update is called once per frame
    void Update () {
        this.transform.GetChild(0).rotation = Quaternion.Euler(m_lockPos, m_angle, m_lockPos); //Locks the Aim and power from rotating

        if(m_rigid.velocity.magnitude < 0.3f)
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
        if(coll.gameObject.tag == "GolfCourse")
        {
            m_outOfBounds = true;
            //Debug.Log("Player left bounds!");
            StartCoroutine("OutOfBoundsTimer");
        }   
    }
    private void OnCollisionEnter(Collision coll)
    {
        //Ball is touching a playable area of the course - deemed in bounds.
        if(coll.gameObject.tag == "GolfCourse")
        {
            bool foundLevel = false;
            GameObject parent = coll.transform.parent.gameObject;

            //Goes through all parents until the course is found or no more parents exist
            while (parent != null && !foundLevel)
            {
                //Compares the parent to the current level
                if (parent == m_lvlController.CurrentCourse)
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
            //Debug.Log("In bounds");
            m_outOfBounds = false;
        }
    }

    IEnumerator OutOfBoundsTimer()
    {
        //Wait for 3 seconds - then perform an OutOfBounds check. This value should probably be set via an editor value
        yield return new WaitForSeconds(oobTimeout);
        if(m_outOfBounds)
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
        m_rigid.isKinematic = true;
        m_rigid.isKinematic = false;
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
        //Calculates a modified length so that the power indicator isn't massive
        float altLenght = m_power / 6;

        //Scale the angle and power indicator appropriately
        power.transform.localScale = new Vector3(0.2f, 0.06f, 2 + altLenght);
        power.transform.localPosition = new Vector3(0, 0, 0.9f + (altLenght / 2));
    }

    public void Command(string[] cmd)
    {
        //Propells the ball in the direction
        if (cmd[0].ToLower() == "!hit")
        {
            
            if (m_rigid.velocity == Vector3.zero)
            {
                //Store the transform (so we can access the position) before we fire the ball
                m_lastPosition = gameObject.transform.position;

                this.transform.GetChild(0).gameObject.SetActive(false);
                Vector3 dir = this.transform.position - aim.transform.position;//Gets the vector for the direction to an point infront of the ball

                m_rigid.velocity = -dir * m_power; //Applys the velocity to the ball

                Invoke("SetMovement", 3);

                //Update scores
                strokeCount++;
                GameObject.Find("UiManager").GetComponent<UiController>().UpdateScore(scoreBoardStrokeUi.GetComponent<Text>(), strokeCount.ToString());

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
                if (angVal >= 0 || angVal <= 360) 
                {
                    //Stores the angle
                    m_angle = angVal; 
                }
            }
            catch { }
        }

        if (cmd[0].ToLower() == "!adjust" || cmd[0].ToLower() == "!ad")
        {
            try
            {
                int adjVal = int.Parse(cmd[1]) + m_angle;

                //Limits max and min angle
                if (adjVal > 360)
                {
                    adjVal -= 360;
                }
                else if (adjVal < 0)
                {
                    adjVal += 360;
                }

                m_angle = adjVal;
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
                if (pwVal >= minPower && pwVal <= maxPower) 
                {
                    m_power = pwVal;                    
                }
                if(pwVal > maxPower)
                {
                    //Do something here, possibly send admin message to twitch chat
                    m_power = maxPower; 
                }
                if (pwVal < minPower)
                {
                    m_power = minPower;
                }

                ScalePower();
            }
            catch { }
        }

        if (cmd[0].ToLower() == "!reset")
        {
            this.transform.position = m_start;
        }
    }
}
