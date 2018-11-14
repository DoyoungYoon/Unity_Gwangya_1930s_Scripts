using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayCar : MonoBehaviour {

	private void OnTriggerStay(Collider collider)
	{
		if (collider.gameObject.tag == "Car")
		{
            //print("타겟 차가 지나감");
			GetComponentInParent<Game_Manager> ().isGameOver = true;
		}
	}
}
