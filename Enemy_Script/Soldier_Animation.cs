using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier_Animation : MonoBehaviour {

    private Enemy_AI enemy;
    private Animator animator;

	// Use this for initialization
	void Start () {
        enemy = transform.parent.GetComponent<Enemy_AI>();
        animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        if(enemy != null)
        {
            animator.SetFloat("WalkSpeed", enemy.GetNav().velocity.magnitude / 5f);
            animator.SetBool("isFight", enemy.GetIsShoot());
        }
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }
    public void Dead()
    {
        animator.SetTrigger("Dead");
        animator.SetBool("isFight", false);
    }
}
