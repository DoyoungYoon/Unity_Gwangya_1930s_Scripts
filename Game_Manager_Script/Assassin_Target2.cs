using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_Target2 : MonoBehaviour {

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player_")
        {
            Destroy(GetComponent<Collider>());
            GetComponentInParent<Game_Manager>().isGoToAssassionPoint2 = true;
        }
    }
}
