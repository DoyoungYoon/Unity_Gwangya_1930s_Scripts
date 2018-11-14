using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarExplosion : MonoBehaviour {
    private NavMeshAgent nav;
    public Transform target;
    public GameObject[] flag;
    public GameObject smokeEffect;
    public int kindOfFlag;
    GameObject newSmokeEffect;
    public bool isCarHitGrenade;
    
    Vector3 modifiedPosition;
    void Start()
    {
        nav = GetComponentInParent<NavMeshAgent>();
        modifiedPosition = transform.position - nav.transform.position;
    }
    void LateUpdate()
    {
        transform.position = nav.transform.position + modifiedPosition; 
        transform.rotation = nav.transform.rotation;

        if (newSmokeEffect != null)
        {
            newSmokeEffect.transform.position = transform.position;
        }
        if (isCarHitGrenade)
        {
            if(target != null)
            {
                Quaternion q = Quaternion.identity;
                Vector3 lookVector3 = (target.position - transform.position).normalized;
                q.SetLookRotation(lookVector3);
                transform.rotation = q;
                transform.rotation = Quaternion.Slerp(transform.rotation, q, 10f * Time.deltaTime);

            }
        }


    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Grenade")
        {
			GameObject.Find("GameManager").GetComponent<Game_Manager>().isExplodeCar = true;
            isCarHitGrenade = true;
            GetComponentInParent<OrderCarExplosion>().explosionAudio.Play();
            this.GetComponent<Rigidbody>().isKinematic = false;
            collider.GetComponent<Grenade>().Explosion();
            newSmokeEffect = Instantiate(smokeEffect, transform.position, transform.rotation);
            nav.enabled = true;
            Invoke("_CarExplosion", 10f);
            if (target != null)
            {
                nav.SetDestination(target.position);
            }
            else
            {
                _CarExplosion();
            }

            //this.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (collider.gameObject.tag == "Flag")
        {
            GetComponentInParent<OrderCarExplosion>().explosionAudio.Pause();
            _CarExplosion();
            collider.GetComponentInParent<FlagFire>().FlagFireEffect();
        }
        else if(collider.gameObject.tag == "enemy"|| collider.gameObject.tag == "Player_" || collider.gameObject.tag == "Crowd")
        {
            this.GetComponent<Rigidbody>().isKinematic = true;

        }
    }
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "area1")
        {
            target = flag[0].transform;
			GameObject.Find ("GameManager").GetComponent<Game_Manager> ().carInFlag = 1;
            kindOfFlag = 1;
        }
        else if (collider.gameObject.tag == "area2")
        {
            target = flag[1].transform;
			GameObject.Find ("GameManager").GetComponent<Game_Manager> ().carInFlag = 2;
            kindOfFlag = 2;
        }
        else if (collider.gameObject.tag == "area3")
        {
            target = null;
			GameObject.Find ("GameManager").GetComponent<Game_Manager> ().carInFlag = 0;
            kindOfFlag = 0;
        }
    }

    void _CarExplosion()
    {
        GetComponentInParent<OrderCarExplosion>().CarExplosion();
        Destroy(gameObject);
    }
}
