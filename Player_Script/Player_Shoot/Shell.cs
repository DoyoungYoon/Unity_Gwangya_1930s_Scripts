using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {
    Rigidbody rb;
    Default_Gun gun;
    public float randomShellOut;
    // Use this for initialization
    void Start () {
        float shellOutX = Random.Range(randomShellOut, randomShellOut+10f);
        float shellOutZ = Random.Range(-3f, 3f);

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.rotation * new Vector3(shellOutX, 0, shellOutZ));
        Destroy(rb, 1f);
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
