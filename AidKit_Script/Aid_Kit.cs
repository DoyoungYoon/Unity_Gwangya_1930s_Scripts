using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aid_Kit : MonoBehaviour {
    public float heal;
    public GameObject aidKitEffect;
    public AudioClip effectSound;
    // Use this for initialization
    void Start () {
        heal = 40;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(new Vector3(0.0f, 30.0f * Time.fixedDeltaTime, 0.0f));
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player_")
        {
            Player_Life player = collider.gameObject.GetComponent<Player_Life>();
            if (player.health != player.maximumHealth) 
            {
                if (player != null)
                {
                    player.health += heal;
                    if (player.health >= player.maximumHealth)
                    {
                        player.health = player.maximumHealth;
                    }
                }
                player.GetComponent<Player_Char>().PlaySound(effectSound);
                Instantiate(aidKitEffect, player.gameObject.transform);
                GameObject.Destroy(this.gameObject);
            }
        }
    }


}
