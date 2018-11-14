using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagFire : MonoBehaviour {
    public GameObject fireEffect;
    public GameObject flag;
	public Material burnedFlag;


    public void FlagFireEffect()
    {
        //print("깃발 불 이펙트 생성");

        Instantiate(fireEffect, flag.transform.position + new Vector3(0f,-0.5f,0f), flag.transform.rotation);
        Destroy(GetComponent<Collider>());
		flag.GetComponentInChildren<MeshRenderer> ().material = burnedFlag;

    }


}
