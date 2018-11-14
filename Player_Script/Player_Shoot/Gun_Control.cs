using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Gun_Control : MonoBehaviour {

    public Transform weponHold;
    public Transform grenadeHold;
    public Default_Gun startingGun;
    public Default_Gun equippedGun;
    public Default_Gun mainGun;
    public Default_Gun subGun;

    public AudioSource audioSource;
    public AudioClip[] audioClips;
    enum State { MainGun, SubGun }
    State state;
    public Player_Stat stat;

    public Grenade grenade;
    public int grenadeCapacity;
    public bool isGrenadeEmpty;
    private bool isSwapping;
    public int remainMainGunCapacity;
    public int remainSubGunCapacity;
    private bool isActiveMainGun;
    private bool isActiveSubGun;


    private Image[] UIGunImage = new Image[2];
    private Text BulletText;

    

    // Use this for initialization
    void Awake () {                             //시작시 첫 총의 생성
		if (startingGun != null)
        {
            EquipGun(startingGun);
            Destroy(equippedGun.gameObject);

        }
        else
        {
            EquipGun(startingGun);
        }
    }

    void Start()
    {

        UIGunImage[0] = GameObject.Find("MainWeaponMp18").GetComponent<Image>();
        UIGunImage[1] = GameObject.Find("MainWeaponArisaka").GetComponent<Image>();
        BulletText = GameObject.Find("BulletText").GetComponent<Text>();


        state = State.MainGun;
        grenadeCapacity = 5;
        isGrenadeEmpty = false;
    }




    void WeaponImageChange(int i)
    {
        foreach(var element in UIGunImage)
        {
            element.enabled = false;
        }

            UIGunImage[i].enabled = true;
        

    }


    public void EquipGun(Default_Gun defaultGun)        //장비된 총의 생성
    {

        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(defaultGun, weponHold.position, weponHold.rotation) as Default_Gun;
        float power = stat.power;
        float dexterity = stat.dexterity;
        equippedGun.init_DefaultGun(power, dexterity);
        equippedGun.GetComponent<AcquireGun>().enabled = false;
        Destroy(equippedGun.GetComponent<Collider>());

        equippedGun.transform.parent = weponHold;
    }
	
    public void Shoot()     //장비된 총 발사
    {
        if (equippedGun != null)
        {
            if (!isSwapping)
            {
                equippedGun.Shoot();
            }
        }
    }

    public void Reload()            
    {
        if(equippedGun != null)
        {
            if(equippedGun.capacity != equippedGun.getRemainGunCapacity())
            {
                GetComponentInChildren<Animator>().SetTrigger("Reload");
                equippedGun.WaitReload();

            }
        }
    }
    public void ChangeMainGun()
    {
        if (isActiveMainGun)
        {
            if (equippedGun != null)
            {
                if (state != State.MainGun)
                {
                    if (state == State.SubGun)
                    {
                        remainSubGunCapacity = equippedGun.gunCount;
                    }

                    BulletText.text = (mainGun.capacity - remainMainGunCapacity).ToString() + " / " + mainGun.capacity.ToString();
                    audioSource.PlayOneShot(audioClips[0]);
                    state = State.MainGun;
                    isSwapping = true;
                    Invoke("FinishedChangeMainGun", 0.7f);
                    GetComponentInChildren<Animator>().SetInteger("GunType", 2);
                }
            }
            else
            {

                state = State.MainGun;
                BulletText.text = mainGun.getRemainGunCapacity().ToString() + " / " + mainGun.capacity.ToString();
                audioSource.PlayOneShot(audioClips[0]);
                isSwapping = true;
                Invoke("FinishedChangeMainGun", 0.7f);
                GetComponentInChildren<Animator>().SetInteger("GunType", 2);
            }
        }
     
    }
    public void ChangeSubGun()
    {
        if (isActiveSubGun)
        {
            if (equippedGun != null)
            {
                if (state != State.SubGun)
                {
                    if (state == State.MainGun)
                    {
                        remainMainGunCapacity = equippedGun.gunCount;
                    }
                    

                    state = State.SubGun;
                    audioSource.PlayOneShot(audioClips[1]);
                    BulletText.text = (subGun.capacity - remainSubGunCapacity).ToString() + " / " + subGun.capacity.ToString();
                    isSwapping = true;
                    Invoke("FinishedChangeSubGun", 0.7f);
                    GetComponentInChildren<Animator>().SetInteger("GunType", 1);
                }
            }
            else
            {

                BulletText.text = subGun.getRemainGunCapacity().ToString() + " / " + subGun.capacity.ToString();
                audioSource.PlayOneShot(audioClips[1]);
                state = State.SubGun;
                isSwapping = true;
                Invoke("FinishedChangeSubGun", 0.7f);
                GetComponentInChildren<Animator>().SetInteger("GunType", 1);
            }
        }

       
    }

    public void FinishedChangeMainGun()
    {
        EquipGun(mainGun);
        
        switch (mainGun.gunName)
        {
            case "mp18":
                WeaponImageChange(0);
                break;
            case "arisaka":
                WeaponImageChange(1);
                break;
  
            case "":
                WeaponImageChange(3);
                break;

        }
        
        equippedGun.gunCount = remainMainGunCapacity;
        isSwapping = false;
    }
    public void FinishedChangeSubGun()
    {
        EquipGun(subGun);

        switch (subGun.gunName)
        {
            case "mp18":
                WeaponImageChange(0);
                break;
            case "arisaka":
                WeaponImageChange(1);
                break;
   
            case "":
                WeaponImageChange(3);
                break;

        }


        equippedGun.gunCount = remainSubGunCapacity;
        isSwapping = false;
    }


    private void OnTriggerEnter(Collider collider)                  //총알에 맞은 collision의 tag를 확인 후 적일시 데미지, 아닐시 그냥 파괴
    {
        if (collider.gameObject.tag == "Weapon")
        {
            if (collider.gameObject.GetComponent<Default_Gun>().gunName == "mp18")
            {
                isActiveMainGun = true;
                audioSource.PlayOneShot(audioClips[0]);
                Destroy(collider.gameObject);
            }
            else if (collider.gameObject.GetComponent<Default_Gun>().gunName == "arisaka")
            {
                isActiveSubGun = true;
                audioSource.PlayOneShot(audioClips[1]);
                Destroy(collider.gameObject);
            }
        }
    }




    public void ThrowGrenade(Vector3 currentPos, Vector3 targetPos, float initialAngle,float range)
    {
        Grenade newGrenade = Instantiate(grenade, grenadeHold.position, grenadeHold.rotation) as Grenade;       //수류탄 생성
        newGrenade.ThrowGrenade(currentPos, targetPos, initialAngle);
        newGrenade.SetDamage(range);
        grenadeCapacity--;
        if(grenadeCapacity == 0)
        {
            isGrenadeEmpty = true;
        }
    }
}
