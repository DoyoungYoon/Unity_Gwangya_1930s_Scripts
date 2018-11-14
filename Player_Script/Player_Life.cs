using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Life : MonoBehaviour, IDamageble {
    private float _startingHealth;
    private float _maximumHealth;
    private float _health;
    protected bool dead;
    public bool isCutScene;
    public Player_Stat stat;
    public Player_Char player;

    // Use this for initialization
    void Start () {
        _maximumHealth = stat.maximumHealth;
        _startingHealth = stat.maximumHealth;
        _health = _startingHealth;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeHit(100);
        }
    }

    public void TakeHit(float damage)                   //bullet에 맞았을 시 , 데미지 입고, health = 0시 죽음
    {
        int randAvoid = Random.Range(0, 100);
        if((int)stat.agility >= randAvoid)
        {
            damage = 0;
            print("회피!");
        }
        if (player.isInvincibility && isCutScene)                         //무적 스킬 사용 상태시 데미지를 0으로 설정, 컷씬일때 데미지 무시
        {
            damage = 0;
            print("무적 스킬로 공격 데미지 0 설정");
        }
        
        _health -= damage;
        if (_health <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        player.isControl = false;
        GetComponentInChildren<Player_Animation>().Die();
        Invoke("OpenGameOverCanvas", 6f);
        //GameObject.Destroy(this.gameObject);
    }

    void OpenGameOverCanvas()
    {
        GameObject.Find("Main Camera").transform.Find("GameOverCanvas").gameObject.SetActive(true);
    }

    public float maximumHealth
    {
        get { return _maximumHealth; }
        set { _maximumHealth = value; }
    }

    public float health
    {
        get { return _health; }
        set { _health = value; }
    }

    public float startingHealth
    {
        get { return _startingHealth; }
        set { _startingHealth = value; }
    }

}
