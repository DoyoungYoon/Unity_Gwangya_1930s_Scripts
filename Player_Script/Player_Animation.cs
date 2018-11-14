using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Animation : MonoBehaviour
{

    private Animator animator;
    
    private GameObject IKPoint;
    
    public float leftIKWeight;



    void Start()
    {
        animator = GetComponent<Animator>();
       
    }

    public void Die()
    {
        animator.Play("PlayerDie_Back", 0);
        animator.Play("PlayerDie_Back", 1);
    }

    public bool isAim()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("RifleAim");
    }

    // Update is called once per frame
    void Update() { 

        animator.SetBool("isCrouch", GetComponentInParent<Player_Char>().isCrouch);
        animator.SetBool("isShoot", GetComponentInParent<Player_Char>().isShoot);
        
        

        if (GetComponentInParent<Player_Char>().GetMoveSpeed() == Vector3.zero)
        {
            animator.SetBool("isWalk", false);
            animator.SetFloat("WalkSpeed", 0);
            animator.SetFloat("WalkDirection", 0);
        }
        else
        {
            Quaternion q = Quaternion.Euler(0, -transform.parent.rotation.eulerAngles.y, 0);
            Vector3 dir = GetComponentInParent<Player_Char>().GetMoveSpeed();

            Vector3 result = (q * dir) * 15;
            
            if (GetComponentInParent<Player_Char>().isCrouch == true)
            {
                result.Normalize();
            }

            animator.SetBool("isWalk", true);

            animator.SetFloat("WalkDirection", result.x);
            animator.SetFloat("WalkSpeed", result.z);
        }


    }

    public void OnFireAnimation()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
    }
}
