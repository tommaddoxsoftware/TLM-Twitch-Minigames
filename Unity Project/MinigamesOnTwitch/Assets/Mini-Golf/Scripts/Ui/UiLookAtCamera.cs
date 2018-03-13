using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLookAtCamera : MonoBehaviour {

    
    private Camera gameCam;
    private Quaternion origRot;
    private Vector3 origPos;

	// Use this for initialization
	void Start () {
        //Set gameCam to the scene's camera
        gameCam = Camera.main;

        origRot = transform.rotation;
        origPos = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
     //    transform.rotation = origRot;
        transform.rotation = origRot * Quaternion.Euler(0, gameCam.transform.rotation.eulerAngles.y,0);

        Debug.Log("Rotation of Camera: " + gameCam.transform.rotation.eulerAngles);
        
	}
}
