using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {
    public LevelController controller;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") //Checks the other is a ball  ##CHANGE TAG##
        {
            controller.BallHole(other.gameObject); //Updates the golf controller
        }
    }
}
