using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayPoint : MonoBehaviour {

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player_")
        {
            GetComponentInParent<Game_Manager>().isRunAway = true;
        }
    } 

}
