using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Point2 : MonoBehaviour {

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player_")
        {
            GetComponentInParent<Game_Manager>().isGoToPoint2 = true;
            Destroy(gameObject);
        }
    }

}
