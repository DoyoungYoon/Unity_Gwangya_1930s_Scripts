using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 플레이어 전체적인 스탯 관리
 * 작성자 : 윤도영
 * 날짜 : 18.03.27
 * 각 스탯의 하는 일은 [캐릭터 상세 정보] 공유 문서에 기록
 */
public class Player_Stat : MonoBehaviour {
    
    private float _maximumHealth;           //체력
    private float _power;                          //힘
    private float _agility;                           //민첩
    private float _dexterity;                       //재주
    private float _walkSpeed;                    //걷는 속도


    void Awake() {
        _maximumHealth = 100f+(UpDown.StrPoint-5)*10;//100f;
        _power = (UpDown.DmgPoint-5f);//0f;
        _agility = (UpDown.AgiPoint-5);//0f;
        _dexterity = (UpDown.DexPoint-5);//0f;
        _walkSpeed = 4f + (1 + (0.03f * agility));
    }

    private void Start()
    {
    }
    public float maximumHealth
    {
        get { return _maximumHealth; }
        set { _maximumHealth = value; }
    }

    public float power
    {
        get { return _power; }
        set { _power = value; }
    }

    public float agility
    {
        get { return _agility; }
        set { _agility = value; }
    }

    public float dexterity
    {
        get { return _dexterity; }
        set { _dexterity = value; }
    }

    public float walkSpeed
    {
        get { return _walkSpeed; }
        set { _walkSpeed = value; }
    }

    /* 사용시
    Player_Stat csst = new Player_Stat();

    csst.data = 10;     // set
    int a = csst.data;  // get
    */

}
