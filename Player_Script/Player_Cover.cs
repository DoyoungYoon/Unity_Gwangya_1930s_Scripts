using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Cover : MonoBehaviour {

    public GameObject cover;
    public bool isPriorCovering;

	void Start () {
        //StartCoroutine(CheckCovering());
        isPriorCovering = false;

    }
	
	void Update () {
       Collider[] tempObject = Physics.OverlapSphere(transform.position, 1f);
       for (int i = 0; i < tempObject.Length; i++)
       {
           if (tempObject[i].CompareTag("HalfCover"))
           {
               if (GetComponent<Player_Char>().isShoot)
               {
                   GetComponent<Player_Char>().isCrouch = false;
               }
               else
               {
                   GetComponent<Player_Char>().isCrouch = true;
               }
               cover = tempObject[i].gameObject; 
           }
       }
    }
    /*
    IEnumerator CheckCovering()
    {
        print(GetComponent<Player_Char>().isCrouch);
        if (GetComponent<Player_Char>().isCrouch)
        {
            Collider[] tempObject = Physics.OverlapSphere(transform.position, 3f);
            for (int i = 0; i < tempObject.Length; i++)
            {
                if (tempObject[i].CompareTag("HalfCover"))
                {
                   print("엄폐중");
                   if (GetComponent<Player_Char>().isShoot)
                   {
                       GetComponent<Player_Char>().isCrouch = false;
                   }
                   cover = tempObject[i].gameObject;
                }
            }
            //yield return new WaitForEndOfFrame();
            yield return null;
        }
    }
    */
}
