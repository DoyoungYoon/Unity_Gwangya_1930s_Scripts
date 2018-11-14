using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {
    
    public GameObject destroyThings;

    public void Destroy()
    {
        Instantiate(destroyThings, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
