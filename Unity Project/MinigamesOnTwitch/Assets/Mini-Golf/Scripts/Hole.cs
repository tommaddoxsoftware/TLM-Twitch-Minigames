using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {
    private LevelController controller;

    // Use this for initialization
    void Start () {
        //Finds the level controller
        controller = GameObject.Find("MinigameManager").GetComponent<LevelController>();
    }
	
    private void OnTriggerEnter(Collider other)
    {
        //Checks the other is a ball  ##CHANGE TAG##
        if (other.tag == "Player") 
        {
            //Updates the golf controller
            controller.BallHole(other.gameObject); 
        }
    }
}
