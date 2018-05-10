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
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (state.loop)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AnimTrigger();
            }
            //Visual Debug
            //anim.SetBool("Loop state", true);
        }
        //else
           // anim.SetBool("Loop state", false);
            
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
