using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameStrManager : MonoBehaviour {

    public Text nameText;
    public Text HealthText;
    public Image HealthBar;

    public float maximumStr;
    public float currentStr;
    public Player_Life playerLife;

	// Use this for initialization
	void Start () {

        playerLife = GameObject.Find("Player").GetComponent<Player_Life>();
        
        nameText.text = UpDown.CharacterName;
        
	}
	
	// Update is called once per frame
	void Update () {


        maximumStr = playerLife.maximumHealth;
        currentStr = playerLife.health;


        HealthText.text = currentStr + " / " + maximumStr;
        HealthBar.fillAmount = currentStr / maximumStr;

    }
}
