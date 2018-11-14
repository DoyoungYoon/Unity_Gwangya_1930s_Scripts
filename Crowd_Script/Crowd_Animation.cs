using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd_Animation : MonoBehaviour {
    private Crowd_AI crowd;
    private Animator animator;

	void Start () {
        crowd = GetComponent<Crowd_AI>();
        animator = GetComponent<Animator>();
    }

    void Update () {


        if (!crowd.isRunAway)
        {
            if (crowd.nav.velocity.magnitude >= 1f)
            {
                animator.SetBool("isWalk", true);
                animator.SetBool("isStand", false);
            }
            else
            {
                animator.SetBool("isWalk", false);
                animator.SetBool("isStand", true);
            }
        }
        else
        {
            if (crowd.nav.velocity.magnitude >= 1f)
            {
                animator.SetBool("isRun", true);
                animator.SetBool("isStand", false);
            }
            else
            {
                animator.SetBool("isRun", false);
                animator.SetBool("isStand", true);
            }
        }
    }
}
