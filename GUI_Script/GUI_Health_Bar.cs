using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Health_Bar : MonoBehaviour {

    public Slider healthBar;
    public Camera mainCamera;
    Enemy_Life enemyLife;

	// Use this for initialization
	void Start () {
        enemyLife = GetComponentInParent< Enemy_Life > ();
        healthBar.maxValue = enemyLife.startingHealth;
    }
	
	void Update () {

        //if (enemyLife.health < enemyLife.startingHealth)
        //{
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
            healthBar.value = enemyLife.health;
            transform.forward = mainCamera.transform.forward;
        if (enemyLife.health <= 0)
        {
            Destroy(this.gameObject);
        }
            //healthBar.enabled;
        //}

    }
}
