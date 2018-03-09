using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public GameObject aim;
    public GameObject power;

    private int m_lockPos = 0;
    private int m_angle;

    private float m_power = 1;

    private Rigidbody m_rigid;

	//Use this for initialization
	void Start () {
        m_rigid = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.GetChild(0).rotation = Quaternion.Euler(m_lockPos, m_angle, m_lockPos); //Locks the Aim and power from rotating
/*
        Debug.Log("Speed: " + m_rigid.velocity.magnitude);
       Debug.Log("Velocity: " + m_rigid.velocity);
       */
       if(m_rigid.velocity.magnitude < 0.07f)
        {
            StopBall();
        }
    }

    private void StopBall()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        m_rigid.isKinematic = true;
        m_rigid.isKinematic = false;
    }
    public void Command(string[] cmd)
    {
        //Propells the ball in the direction
        if (cmd[0].ToLower() == "hit")
        {
            if (m_rigid.velocity == Vector3.zero)
            {
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

                if (pwVal >= 1 && pwVal <= 1000000) //Checks if the angle is valid
                {
                    m_power = pwVal;

                    power.transform.localScale = new Vector3(0.2f, 0.06f, pwVal);
                    power.transform.localPosition = new Vector3(0, 0, 0.9f - (1 - pwVal) / 2);
                }
            }
            catch { }
        }
    }
}
