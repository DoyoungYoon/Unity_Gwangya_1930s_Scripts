using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquireGun : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.Rotate(new Vector3(0.0f, 30.0f * Time.fixedDeltaTime, 0.0f));
    }

    private void OnTriggerEnter(Collider collider)                  //총알에 맞은 collision의 tag를 확인 후 적일시 데미지, 아닐시 그냥 파괴
    {
        if (collider.gameObject.tag == "Player_")
        {
            //Destroy(gameObject);
        }
    }

}
