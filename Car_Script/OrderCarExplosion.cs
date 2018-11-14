using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCarExplosion : MonoBehaviour {

    public GameObject carObject;
    public GameObject crashedCar;
    public AudioSource explosionAudio;
    public AudioClip explosionClip;

    public void CarExplosion()
    {
        Destroy(carObject.gameObject);
        Instantiate(crashedCar, carObject.transform.position, carObject.transform.rotation);
        //crashedCar.GetComponent<Rigidbody>().AddForce(new Vector3(0f,0.1f * Time.deltaTime, .5f * Time.deltaTime));
        explosionAudio.PlayOneShot(explosionClip);
        Destroy(this.gameObject, 7f);
    }
}
