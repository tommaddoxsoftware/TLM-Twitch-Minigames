using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    [SerializeField]
    Animator anim;

    private void Start()
    {
        PlayerPrefs.SetInt("FirstTime", 1);
        anim.SetBool("FirstRun", true);
    }

    // Update is called once per frame
    void Update () {
       
            
	}

    public void AnimTrigger()
    {
        if(anim.GetBool("FirstRun") == true)
        {
            anim.SetBool("FirstRun", false);
        }
        anim.SetTrigger("Continue");
    }
}
