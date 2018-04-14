using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

	public void OnStartGame()
    {
        //Load Golf Scene. 
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
