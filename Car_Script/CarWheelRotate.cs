using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheelRotate : MonoBehaviour {

    Vector3 priorPosition;
	void Start () {
		
	}
	
	void FixedUpdate () {
        float distance = (transform.position - priorPosition).magnitude;
        float velocity = distance / Time.fixedDeltaTime;
        priorPosition = transform.position;
        transform.Rotate(new Vector3(100.0f * velocity, 0.0f,  0.0f));
        
    }
}
