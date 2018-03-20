﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
    public Text velTxt;
    public Text magTxt;
    public Text magLossTxt;

    public GameObject aim;
    public GameObject power;

    public int maxPower = 50;
    public int minPower = 1;

    private int m_lockPos = 0;
    private int m_angle;

    private float m_power = 1;

    //Used to control respawning
    private bool m_outOfBounds = false;
    private Vector3 m_lastPosition;

    private Rigidbody m_rigid;

    
    private int oobTimeout;

    private float magLosStore = 0;

	//Use this for initialization
	void Start () {
        m_rigid = this.GetComponent<Rigidbody>();
        oobTimeout = GameObject.Find("MinigameManager").GetComponent<BallControl>().outOfBoundsTimeout;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.GetChild(0).rotation = Quaternion.Euler(m_lockPos, m_angle, m_lockPos); //Locks the Aim and power from rotating
        /*
        Debug.Log("Speed: " + m_rigid.velocity.magnitude);
       Debug.Log("Velocity: " + m_rigid.velocity);
       */
       if(m_rigid.velocity.magnitude < 0.3f)
        {
            StopBall();
        }

       /*
        velTxt.text = "Vel: " + this.GetComponent<Rigidbody>().velocity;
        magTxt.text = "Mag: " + this.GetComponent<Rigidbody>().velocity.magnitude;

        float magLoss = magLosStore - this.GetComponent<Rigidbody>().velocity.magnitude;

        magLossTxt.text = "MLoss: " + magLoss;

        magLosStore = this.GetComponent<Rigidbody>().velocity.magnitude;
        */
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
            m_outOfBounds = false;
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

    public void StopBall()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        m_rigid.isKinematic = true;
        m_rigid.isKinematic = false;
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
        //Scale shit appropriately
        power.transform.localScale = new Vector3(0.2f, 0.06f, m_power);
        power.transform.localPosition = new Vector3(0, 0, 0.9f - (1 - m_power) / 2);
    }

    public void Command(string[] cmd)
    {
        //Propells the ball in the direction
        if (cmd[0].ToLower() == "hit")
        {
            
            if (m_rigid.velocity == Vector3.zero)
            {
                //Store the transform (so we can access the position) before we fire the ball
                m_lastPosition = gameObject.transform.position;

                this.transform.GetChild(0).gameObject.SetActive(false);
                Vector3 dir = this.transform.position - aim.transform.position;//Gets the vector for the direction to an point infront of the ball

                m_rigid.velocity = -dir * m_power; //Applys the velocity to the ball
            }
        }

        if (cmd[0].ToLower() == "stop")
        {
            StopBall();
        }


        //Sets the angle of the ball
        if (cmd[0].ToLower() == "angle")
        {
            try //Trys to convert the command to a string
            {
                int angVal = int.Parse(cmd[1]);

                if (angVal >= 0 || angVal <= 360) //Checks if the angle is valid
                {
                    m_angle = angVal; //Stores the angle
                }
            }
            catch { }
        }

        if (cmd[0].ToLower() == "adjust")
        {
            try
            {
                int adjVal = int.Parse(cmd[1]) + m_angle;

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
        if (cmd[0].ToLower() == "power")
        {
            try
            {
                float pwVal = float.Parse(cmd[1]);

                if (pwVal >= minPower && pwVal <= maxPower) //Checks if the angle is valid
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
    }
}
