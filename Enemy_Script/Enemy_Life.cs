using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Life : MonoBehaviour,IDamageble {

    public float startingHealth;
    public float health;
    public bool dead;
    private float voiceCoolTime;
    private float time;

    public AudioSource soundPlayer;
    public AudioClip[] damagedVoice;
    public AudioClip[] dyingVoice;

	void Start () {
        health = startingHealth;
        time = Time.time;
        voiceCoolTime = 3f;
    }

    public void TakeHit(float damage)                   //bullet에 맞았을 시 , 데미지 입고, health = 0시 죽음
    {
        if(GetComponent<Enemy_AI>().aiState == Enemy_AI.AI_State.Reconnaissance)
        {
            if(Enemy_AI.isCombat == false)
            {
                Enemy_AI.isCombat = true;
            }

            GameObject Player = GameObject.FindGameObjectWithTag("Player_");
            GetComponent<Enemy_AI>().SetTarget(Player);
        }


        float lastTime = Time.time;
        health -= damage;
        if(health<=0 && !dead)
        {
            Die();
        }
        if(lastTime - time > voiceCoolTime)
        {
            int randVoice = Random.Range(0, damagedVoice.Length);
            soundPlayer.PlayOneShot(damagedVoice[randVoice]);
            time = Time.time;
        }
   }

    public void TakeHit(float damage, bool isAssassination)                   //bullet에 맞았을 시 , 데미지 입고, health = 0시 죽음
    {
        float lastTime = Time.time;
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
        if (lastTime - time > voiceCoolTime)
        {
            int randVoice = Random.Range(0, damagedVoice.Length);
            soundPlayer.PlayOneShot(damagedVoice[randVoice]);
            time = Time.time;
        }
    }

    protected void Die()
    {
        Debug.Log("주금");
        dead = true;
        GetComponent<Enemy_AI>().aiActive = false;
        GetComponent<Enemy_AI>().Dead();
        GetComponentInChildren<Soldier_Animation>().Dead();
        Destroy(GetComponent<Collider>(), 3f);
        Debug.Log("꿱");
        GameObject.Destroy(this.gameObject , 10f);
    }
}
