using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Default_Gun : MonoBehaviour
{

    private Text BulletText;

    public string gunName;
    public Transform bulletPosition;        //총구 위치
    public Transform shellPosition;         //탄피 위치
    public Bullet bullet;
    public Shell shell;
    public float rpm;                            //연사속도
    public float bulletVelocity;                 //총알 속도
    public float range;                         //사격 범위
    public float damage;                           //총 데미지
    public float addDamage;
    public int capacity;                         //탄창 총알 수
    public float reloading;                   //장전 속도
    public int gunCount;                //주무기 탄창
    public bool isMainGunEmpty;
    private bool isSubGunEmpty;
    Material gunMaterial;
    Vector3 recoilVelocity;             //반동 속도
    Vector3 newBulletPosition;
    Quaternion newBulletRotation;
    Muzzle_Flash muzzleFlash;
    [Header("총기 사운드")]
    public AudioSource audioSource;
    public AudioClip[] fireSoundClip;
    public AudioClip[] reloadingSoundClip;
    //Player_Char player;
    Player_Stat stat;
    [Header("반동")]
    Vector2 recoilVelocityMinMax = new Vector2(0.5f, 2f);            //랜덤 반동
    Vector2 recoilRotUpVelocityMinMax = new Vector2(0, 0);             //랜덤 반동 (위로 튐)
    Vector2 recoilRotSideVelocityMinMax = new Vector2(-3f, 3f);         //랜덤 반동 (옆으로 튐)
    float recoilUpAngle;                      //위로 반동의 각
    float recoilSideAngle;                    //옆으로 반동의 각
    float recoilRotVelocity;                //반동 회전속도

    float nextShotTime;
    public bool isReloading;
    void Start()
    {                 //초기 설정

        //damage += stat.power * addDamage;
        BulletText = GameObject.Find("BulletText").GetComponent<Text>();
        recoilRotVelocity = 10f;
        muzzleFlash = GetComponent<Muzzle_Flash>();

    }

    public void init_DefaultGun(float power, float dexteritiy)
    {
        damage += power * addDamage;
        for(int i = 0; i < dexteritiy; i++)
        {
            reloading *= 0.9f;
        }
        print("데미지"+damage);
        print("재장전"+reloading);
    }

    void LateUpdate()           //그냥 update면 lookat이 계속 업데이트 되어 회전하지 않음
    {
        
        // 사격시 반동 부분 
        if (GetComponentInParent<Player_Char>() != null)
        {
            if (GetComponentInParent<Player_Char>().isShoot)                    //총을 쏘고 있을때 흔들림 없애줌
            {
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilVelocity, 0f * Time.deltaTime);          //반동 제어
                recoilUpAngle = Mathf.SmoothDamp(recoilUpAngle, 0, ref recoilRotVelocity, 15f * Time.deltaTime);
                recoilSideAngle = Mathf.SmoothDamp(recoilSideAngle, 0, ref recoilRotVelocity, 15f * Time.deltaTime);
                transform.eulerAngles += Vector3.left * recoilUpAngle;
                transform.eulerAngles += Vector3.up * recoilSideAngle;
                //print(transform.eulerAngles);
            }
        }

        newBulletRotation.eulerAngles = transform.eulerAngles;
        newBulletPosition = bulletPosition.position;

    }
    public void Shoot()             //총 발사시 연사속도, 데미지 설정해서 총알 생성
    {
        if (capacity > gunCount)
        {
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + rpm / 1000;
                Bullet newBullet = Instantiate(bullet, newBulletPosition, newBulletRotation) as Bullet;
                Shell newShell = Instantiate(shell, shellPosition.position, shellPosition.rotation) as Shell;
                PlaySound(audioSource, fireSoundClip);
                muzzleFlash.Activate();
                
                newBullet.SetSpeed(bulletVelocity);
                newBullet.SetDamage(damage);
                gunCount++;

                BulletText.text = getRemainGunCapacity() + " / " + capacity;


                float randRecoilVelocity = Random.Range(recoilVelocityMinMax.x, recoilVelocityMinMax.y);
                float randRecoilRotUpVelocity = Random.Range(recoilRotUpVelocityMinMax.x, recoilRotUpVelocityMinMax.y);
                float randRecoilRotSideVelocity = Random.Range(recoilRotSideVelocityMinMax.x, recoilRotSideVelocityMinMax.y);
                transform.position -= Vector3.forward * randRecoilVelocity;
                recoilUpAngle += randRecoilRotUpVelocity;
                recoilUpAngle = Mathf.Clamp(recoilUpAngle, 0, 5);
                recoilSideAngle += randRecoilRotSideVelocity;
                recoilSideAngle = Mathf.Clamp(recoilSideAngle, -5, 5);                                                                                //-가 압도적으로 많음    
            }
        }
        else
        {
            GetComponentInParent<Player_Char>().isShoot = false;
        }
    }

    public void Shoot_Enemy()             //총 발사시 연사속도, 데미지 설정해서 총알 생성
    {
        Bullet newBullet = Instantiate(bullet, newBulletPosition, newBulletRotation) as Bullet;
        Shell newShell = Instantiate(shell, shellPosition.position, shellPosition.rotation) as Shell;
        PlaySound(audioSource, fireSoundClip);
        muzzleFlash.Activate();

        newBullet.SetSpeed(bulletVelocity);
        newBullet.SetDamage(damage);
        newBullet.SetTarget("Player_");

        BulletText.text = getRemainGunCapacity() + " / " + capacity;
    }



    public void WaitReload()                //재장전 대기시간
    {
        print("장전 시작");
        isReloading = true;
        PlaySound(audioSource, reloadingSoundClip);
        Invoke("Reload", reloading);
    }

    public void Reload()                       //재장전
    {
        gunCount = 0;
        isMainGunEmpty = false;
        isReloading = false;
        
        BulletText.text = getRemainGunCapacity() + " / " + capacity;
        print("장전 완료");
    }

    public int getRemainGunCapacity()
    {
        return capacity - gunCount;
    }
    
    public void PlaySound(AudioSource sound, AudioClip[] Clip)
    {
        int randSound = Random.Range(0, Clip.Length);
        
        sound.PlayOneShot(Clip[randSound]);
        
    }
}

