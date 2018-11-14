using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_GUIManager : MonoBehaviour {

    bool isInit = false;

    private GameObject player;


    public Slider playerHpBar;
    public Text GunCapacityText;
    public Text GunRemainText;
    

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if(isInit == false)
        {
            isInit = true;
            GUI_Init();
        }
        GUI_Update();
    }

    void GUI_Init()
    {
        playerHpBar.maxValue = player.GetComponent<Player_Life>().maximumHealth;
        Debug.Log(player.GetComponent<Player_Life>().maximumHealth);
    }

    void GUI_Update()
    {
        if (player == null)
        {
            playerHpBar.value = Mathf.Lerp(playerHpBar.value, 0, Time.deltaTime * 5f);
        }
        else
        {
            playerHpBar.value = Mathf.Lerp(playerHpBar.value, player.GetComponent<Player_Life>().health, Time.deltaTime * 5f);
            GunRemainText.text = "" + player.GetComponent<Gun_Control>().equippedGun.getRemainGunCapacity();
            GunCapacityText.text = "" + player.GetComponent<Gun_Control>().equippedGun.capacity;
        }
        
    }
}
