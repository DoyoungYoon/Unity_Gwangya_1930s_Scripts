using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour {

    public enum CoverType
    {
        Half, Full
    }

    public CoverType type;
    public bool isCovering;

	// Use this for initialization
	void Start () {
        isCovering = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
