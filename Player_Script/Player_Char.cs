using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Char : MonoBehaviour
{

    public bool isSelect; // 캐릭터가 선택되어있는지 확인 True 시 선택된 상태 False 시 선택 해제 상태
    public bool isShoot;    //총을 쏘고 있는 상태
    private bool isShootDelay;
    private float shootDelayTime;
    private float shootDelayTimeByGrenade;

    public bool isAttack; //현재 쓰지않음
    bool isThrowGrenade;
    private float throwRange;
    bool isTarget = false;
    public bool isCrouch = false;
    public bool isSprint = false;
    public bool isWalk = false;
    bool isReloading = false;
    float distance;                                                 //player와 mouse 포인터의 거리 계산
    Gun_Control gunControl;
    public Player_Stat stat;
    public float range;                                             //수류탄 폭파 반경
    public float damage;                                        // 현재 플레이어의 공격 데미지
    //Attack_Range attackRange;
    //public GameObject attackRange;
    public Attack_Range attackRange;
    private Vector3 heightCorrectedPoint;
    private Collider targetCollider;
    private float targetRange;
    public float moveSpeed;                                     //움직이는 속도 값
    public Vector3 deltaMoveSpeed;                                 //현재 움직이는 속도 값
    private float walkSpeed;
    private float sprintSpeed;
    private float crouchSpeed;
    public Vector3 lookPoint;
    public GameObject knife;
    //암살 액션 파라미터
    public float assassinationDamage;                       //암살 공격 데미지
    public float assassinationRange;                          //암살 공격 범위
    //무적 스킬 파라미터
    public bool isInvincibility = false;
    private float setInvincibilityTime;
    private float invincibilityTime;
    private bool isInvincibilityCoolTime = false;
    private float invincibilityCoolTime;
    private float setInvincibilityCoolTime;
    public GameObject invincibilityEffect;
    //빠른 장전 스킬 파라미터
    private float originReloading;
    private bool isFastReloading;
    private float fastReloadingTime;
    private float setFastReloadingTime;
    private float fastReloadingCoolTime;
    private float setFastReloadingCoolTime;
    private bool isFastReloadingCoolTime;
    public GameObject fastReloadEffect;
    //사운드 관리
    private float nextTime;
	public bool isAfterShootTarget;
   
    public AudioSource playerSound;
    public AudioClip[] grassWalkSoundClip;
    public AudioClip fastReloadSoundEffect;
    public AudioClip invincibilitySoundEffect;


    public bool isControl;
   
    private Player_Animation animation;

    Vector3 point;
    /*Vector3 cameraToMouse;
    float pointSinDegree;
    float pointTanDegree;
    */
    private GameObject cameraObject;

    void Start()
    {
        animation = GetComponentInChildren<Player_Animation>();
        gunControl = GetComponent<Gun_Control>();
        //stat = GetComponent<Player_Stat>();
        //attackRange = GetComponentInChildren<Attack_Range>();

        try
        {
            damage = gunControl.equippedGun.damage;
        } catch(NullReferenceException e)
        {
            damage = 0;
        }


        walkSpeed = stat.walkSpeed;
        crouchSpeed = walkSpeed/2f;
        sprintSpeed = walkSpeed*1.5f;
        moveSpeed = walkSpeed;
        isSelect = true;
        assassinationDamage = 100f;
        assassinationRange = 1f;
        isShoot = false;
        isShootDelay = false;
        isThrowGrenade = false;
        attackRange.gameObject.SetActive(false);
        range = 5f;
        throwRange = 10f;
        //무적 스킬 파라미터 초기값
        setInvincibilityTime = 5f;
        isInvincibility = false;
        isInvincibilityCoolTime = false;
        invincibilityTime = setInvincibilityTime;
        setInvincibilityCoolTime = 60f;
        invincibilityCoolTime = setInvincibilityCoolTime;
        shootDelayTime = 1f;
        shootDelayTimeByGrenade = 1f;
        setFastReloadingTime = 10f;
        fastReloadingTime = setFastReloadingTime;
        isFastReloadingCoolTime = false;
        isFastReloading = false;
        setFastReloadingCoolTime = 10f;
        fastReloadingCoolTime = setFastReloadingCoolTime;
        isControl = false;
        

    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (isControl)
        {
            MoveToCharacter(v, h);
        }
        

      //  CheckSelect();
    }
    void Update()
    {
        if (isControl)
        {
            CommendToCharacter();
        }
        else
        {
            MoveToCharacter(0, 0);
        }
    }

    public void SetCamera(GameObject camera)
    {
        cameraObject = camera;
    }

    void CheckSelect() // 선택되어있는 경우 레드로 선택되어 있지 않은 경우 블랙으로 OutLine 생성
    {
        if (isSelect == true)
        {
            GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color(1, 0, 0));
        }
        else
        {
            GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color(0, 0, 0));
        }
    }

    void MoveToCharacter(float v, float h)
    {

        
        Transform moveForward = transform;
        Vector3 forward = moveForward.forward;

        if (cameraObject != null)
        {
            moveForward = cameraObject.transform;
            forward = new Vector3(Mathf.Sin(moveForward.eulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(moveForward.eulerAngles.y * Mathf.Deg2Rad));
            
        }
        deltaMoveSpeed = forward * v * moveSpeed + moveForward.right * h * moveSpeed;
        if (deltaMoveSpeed.magnitude > moveSpeed)
        {
            deltaMoveSpeed = (forward * v + moveForward.right * h).normalized * moveSpeed;
        }
        deltaMoveSpeed = deltaMoveSpeed * Time.deltaTime;
        transform.localPosition += deltaMoveSpeed;
        if(deltaMoveSpeed.magnitude > 0)
        {
            if (Time.time > nextTime)
            {
                nextTime = Time.time + (1/moveSpeed)*Time.deltaTime*100;
                int randWalkSound = UnityEngine.Random.Range(0, grassWalkSoundClip.Length);
                playerSound.PlayOneShot(grassWalkSoundClip[randWalkSound]);
            }
        }
    }

    void CommendToCharacter() // 캐릭터의 전체적인 커맨드 명령을 체크하고 수행하는 함수
    {
        if (true)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                       // 마우스 커서에 따라 총구 방향 조정
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            //Plane groundPlane = new Plane(Vector3.up, new Vector3(0f,1f,0f));
            //Plane groundPlane = new Plane(Vector3.up, 10f);
            float rayDistance;
            if (groundPlane.Raycast(ray, out rayDistance))
            {
                point = ray.GetPoint(rayDistance);
                /*cameraToMouse = ray.origin - point;
                pointSinDegree = Vector3.Distance(ray.origin, point) / cameraToMouse.z;
                pointTanDegree = cameraToMouse.y / Vector2.Distance(new Vector2(ray.origin.x , ray.origin.z ),new Vector2(point.x,point.z)) ;
                Vector2 modifiedPoint = new Vector2(1.3f * pointSinDegree, 1.3f / pointTanDegree);
                point.x += modifiedPoint.x;
                point.z += modifiedPoint.y;*/

                Debug.DrawLine(ray.origin,point,Color.red);
                Vector2 lookPointVec2 = new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z);
                if (lookPointVec2.magnitude > 1)
                {
                    lookPoint = new Vector3(point.x, 0f, point.z);
                }

                //if((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > 1)
                //{
                distance = lookPointVec2.magnitude;
                lookPointVec2 = lookPointVec2.normalized * 10;                                             //가까운 지점에서 총구 방향이 이상해지는 부분을 수정하기 위해서 10을 곱해줌
                lookPoint = new Vector3(lookPointVec2.x, 0f, lookPointVec2.y);
                transform.LookAt(lookPoint+transform.position);

                //}
            }

            if (Input.GetButtonDown("Crouch"))            // C키를 눌러 앉기, 서기
            {       
                if (!isCrouch)                                              // 서있을 때, 앉기
                {
                    moveSpeed = crouchSpeed;
                    isCrouch = true;
                }
                else                                                          // 앉아있을 때, 서기
                {
                    moveSpeed = walkSpeed;
                    isCrouch = false;
                }
            }
            else if (Input.GetButtonDown("Reload"))           // R키를 눌러 재장전
            {
                if (!gunControl.equippedGun.isReloading)
                {
                    gunControl.Reload();
                }
            }
            else if (Input.GetButtonDown("SwapMainGun"))          // 총 변경
            {
                if(gunControl.equippedGun != null)
                {
                    if (!gunControl.equippedGun.isReloading)
                    {
                        gunControl.ChangeMainGun();
                        if (isFastReloading)
                        {
                            originReloading = gunControl.equippedGun.reloading;
                            gunControl.equippedGun.reloading *= 0.8f;
                            print("재장전 속도 : " + gunControl.equippedGun.reloading);
                        }
                    }
                }
                else
                {
                    gunControl.ChangeMainGun();
                    if (isFastReloading)
                    {
                        originReloading = gunControl.equippedGun.reloading;
                        gunControl.equippedGun.reloading *= 0.8f;
                        print("재장전 속도 : " + gunControl.equippedGun.reloading);
                    }
                }
            }
            else if (Input.GetButtonDown("SwapSubGun"))
            {
                if (gunControl.equippedGun != null)
                {
                    if (!gunControl.equippedGun.isReloading)
                    {
                        gunControl.ChangeSubGun();
                        if (isFastReloading)
                        {
                            originReloading = gunControl.equippedGun.reloading;
                            gunControl.equippedGun.reloading *= 0.8f;
                            print("재장전 속도 : " + gunControl.equippedGun.reloading);
                        }
                    }
                }
                else
                {
                    gunControl.ChangeSubGun();
                    if (isFastReloading)
                    {
                        originReloading = gunControl.equippedGun.reloading;
                        gunControl.equippedGun.reloading *= 0.8f;
                        print("재장전 속도 : " + gunControl.equippedGun.reloading);
                    }
                }
            }
            else if (Input.GetButtonDown("Grenade"))                //수류탄 투척 준비
            {
				if (isAfterShootTarget) {
					if (!gunControl.isGrenadeEmpty && !isThrowGrenade) {
						//print(distance);
						attackRange.radius = range;
						Cursor.visible = false;
						attackRange.gameObject.SetActive (true);
						isThrowGrenade = true;
					} else if (isThrowGrenade) {
						Cursor.visible = true;
						attackRange.gameObject.SetActive (false);
						isThrowGrenade = false;
					} else {
						Debug.Log ("수류탄이 없습니다.");
					}
				}
                //print(point);
            }
         
            else if (Input.GetButtonDown("Invincibility"))              //무적 스킬 사용
            {
                if (!isInvincibility && !isInvincibilityCoolTime)
                {
                    isInvincibility = true;
                    playerSound.clip = invincibilitySoundEffect;
                    playerSound.PlayDelayed(3f);
                    GameObject effect = Instantiate(invincibilityEffect, transform) as GameObject;
                    Destroy(effect.gameObject, setInvincibilityTime+1f);
                    print("무적 시작");
                }
            }
            else if (Input.GetButtonDown("FastReload"))
            {
                if (!isFastReloading && !isFastReloadingCoolTime)
                {
                    isFastReloading = true;
                    playerSound.PlayOneShot(fastReloadSoundEffect);
                    GameObject effect = Instantiate(fastReloadEffect, transform) as GameObject;
                    Destroy(effect, setFastReloadingTime);
                    originReloading = gunControl.equippedGun.reloading;
                    gunControl.equippedGun.reloading *= 0.8f;
                    print("재장전 속도 : " + gunControl.equippedGun.reloading);
                }
            }
         
                if (Input.GetButton("Sprint"))       // shift 누르면 달리기
            {
                if(isShoot == false && !isShoot)
                {
                    moveSpeed = sprintSpeed;
                    isSprint = true;
                    isCrouch = false;
                }
              
            }
            else         // shift 떼면 다시 걷기
            {
                if (isCrouch)
                {
                    moveSpeed = crouchSpeed;
                    isSprint = false;
                    isCrouch = true;
                }
                else
                {
                    moveSpeed = walkSpeed;
                }
            }

            if (isThrowGrenade)                                             //수류탄 투척
            {
                if (new Vector2(point.x - transform.position.x, point.z - transform.position.z).magnitude < throwRange) 
                {
                    attackRange.transform.position = new Vector3(point.x, 0.01f, point.z);
                }
                if (Input.GetButtonDown("Fire"))
                {
					
                    //수류탄 위치 계산
                    Vector3 targetPosition;
                    if (new Vector2(point.x - transform.position.x, point.z - transform.position.z).magnitude < throwRange)             
                    {
                        targetPosition = point;
                    }
                    else
                    {
                        targetPosition = new Vector3(point.x- transform.position.x, 0 ,point.z - transform.position.z).normalized * throwRange;
                        targetPosition = new Vector3(targetPosition.x + transform.position.x, 0, targetPosition.z + transform.position.z);
                    }
                    Vector3 myPosition = gunControl.weponHold.position;
                    gunControl.ThrowGrenade(myPosition, targetPosition, 45, range);
                    isThrowGrenade = false;
                    Cursor.visible = true;
                    attackRange.gameObject.SetActive(false);
                    shootDelayTime = shootDelayTimeByGrenade;
                    isShootDelay = true;
                }
            }
            //무적스킬 사용시사용시 설정된 무적 시간만큼 isInvincibility = true;
            if (isInvincibility)                                                       
            {
                invincibilityTime -= Time.deltaTime;
                if(invincibilityTime <= 0)
                {
                    isInvincibility = false;
                    isInvincibilityCoolTime = true;
                    invincibilityTime = setInvincibilityTime;
                    print("무적 시간 종료");
                }
            }
            if (isFastReloading)
            {
                fastReloadingTime -= Time.deltaTime;
                if (fastReloadingTime <= 0)
                {
                    isFastReloading = false;
                    gunControl.equippedGun.reloading = originReloading;
                    isFastReloadingCoolTime = true;
                    fastReloadingTime = setFastReloadingTime;
                    print("빠른 재장전 종료 : " + gunControl.equippedGun.reloading);
                }
            }
            //무적 스킬 사용 종료후 설정된 쿨타임만큼 isInvincibilityCoolTime = true;
            if (isInvincibilityCoolTime)                                        
            {
                invincibilityCoolTime -= Time.deltaTime;
                if(invincibilityCoolTime <= 0)
                {
                    isInvincibilityCoolTime = false;
                    invincibilityCoolTime = setInvincibilityCoolTime;
                    print("무적 쿨타임 종료");
                }
            }
            if (isFastReloadingCoolTime)
            {
                fastReloadingCoolTime -= Time.deltaTime;
                if(fastReloadingCoolTime <= 0)
                {
                    isFastReloadingCoolTime = false;
                    fastReloadingCoolTime = setFastReloadingCoolTime;
                    print("빠른 재장전 쿨타임 종료");
                }
            }
            ShootDelay();
            if(gunControl.equippedGun!= null)
            {
				if (Input.GetButton("Fire") && !gunControl.equippedGun.isReloading && !isThrowGrenade && isAfterShootTarget)
                {

                    if (isShoot == true)
                    {
                        gunControl.Shoot();
                        isSprint = false;
                        moveSpeed = walkSpeed;
                    }
                    if (!isShootDelay)
                    {
                        gunControl.equippedGun.transform.LookAt(lookPoint + transform.position + new Vector3(0, 0, 0));
                        isShoot = true;
                    }
                }
                else
                {
                    isShoot = false;
                    if(gunControl.equippedGun != null)
                    {
                        gunControl.equippedGun.transform.rotation = Quaternion.Lerp(GetComponent<Gun_Control>().equippedGun.transform.rotation, GetComponentInParent<Gun_Control>().weponHold.transform.rotation, Time.deltaTime * 10f);
                    }
                }
            }

            Collider[] findObject = Physics.OverlapSphere(transform.position, assassinationRange);      //적이 암살가능 범위에 있는지 확인
            foreach (Collider col in findObject)                                                                                   //적의 뒤에 있을시 암살
            {
                if (col.tag.Equals("Enemy"))
                {
                    if (Vector3.Angle(col.transform.forward, col.transform.position - transform.position) < 30)
                    {
                        if (Input.GetButtonDown("Assassination"))
                        {
                            if (col.GetComponent<Enemy_Life>() != null)
                            {
                                Destroy(col);
                                transform.position = col.gameObject.transform.position+ col.gameObject.transform.forward.normalized * -0.2f;
                                transform.rotation = col.gameObject.transform.rotation;
                                col.GetComponent<Enemy_Life>().TakeHit(assassinationDamage, true);
                                GetComponentInChildren<Animator>().SetTrigger("Stabbing");
                                if(gunControl.equippedGun != null)
                                {
                                    gunControl.equippedGun.gameObject.SetActive(false);
                                }
                                knife.SetActive(true);
                                isControl = false;
                                Invoke("endAssassination", 2.5f);
                            }
                        }
                        Debug.Log("암살가능");
                    }
                }
            }

        }
        else
        {

        }
    }

    void endAssassination()
    {
        isControl = true;
        knife.SetActive(false);
        if (gunControl.equippedGun != null)
        {
            gunControl.equippedGun.gameObject.SetActive(true);
        }

    }

    public Vector3 GetMoveSpeed()                               //현재 움직이는 속도 값 전달
    {
        return deltaMoveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    void ShootDelay()
    {
        if (isShootDelay)
        {
            shootDelayTime -= Time.deltaTime;
            if (shootDelayTime <= 0)
            {
                isShootDelay = false;
                shootDelayTime = 1f;
                print("딜레이 종료");
            }
        }
    }
    public void PlaySound(AudioClip clip)
    {
        playerSound.PlayOneShot(clip);
    }

}

/*
 * 이동기 바뀌기전 마우스 클릭으로 이동
    public class Player_Char : MonoBehaviour {

    public float heal;
    public float startingHealth;
    public float maximumHealth;
    protected float health;
    public bool isSelect; //캐릭터가 선택되어있는지 확인 True 시 선택된 상태 False 시 선택 해제 상태
    public bool isAttack = false;
    bool isTarget = false;
    bool isCrouch = false;
    bool isReloading = false;
    Gun_Control gunControl;
    public float range;
    public float damage;                                        //현재 플레이어의 공격 데미지
    //Attack_Range attackRange;
    public GameObject attackRange;
    private Vector3 heightCorrectedPoint;
    private Collider targetCollider;
    private float targetRange;
    // Use this for initialization
    void Start () {
        gunControl = GetComponent<Gun_Control>();
        range = gunControl.equippedGun.range;
        damage = gunControl.equippedGun.damage;
        startingHealth = 80;
        health = startingHealth;
        maximumHealth = 100;
    }

    // Update is called once per frame
    void Update () {
        CheckSelect();
        CommendToCharacter();
    }

    void CheckSelect() // 선택되어있는 경우 레드로 선택되어 있지 않은 경우 블랙으로 OutLine 생성
    {
        if(isSelect == true)
        {
            GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color(1, 0, 0));
        }

        else
        {
            GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color(0, 0, 0));
        }
    }

    void CommendToCharacter() // 캐릭터의 전체적인 커맨드 명령을 체크하고 수행하는 함수
    {
        if (isSelect == true)
        {
             if (Input.GetMouseButton(1))                       //오른쪽 마우스 버튼 클릭시 그 지점까지 이동
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.CompareTag("Floor"))
                        {
                            SetDestination(hit.point);
                            MoveDestination();
                            if (hit.collider.CompareTag("Player_"))                         //@@@@@@@@@조건을 player끼리 충동했을때로
                            {
                                if ((hit.point - this.transform.position).magnitude < 0.5f)
                                {
                                    StopDestination();
                                }
                            }
                        }
                        if (hit.collider.CompareTag("Heal"))
                        {
                            Aid_Kit healObject = hit.collider.gameObject.GetComponent<Aid_Kit>();
                            if(healObject != null)
                            {
                                SetDestination(hit.collider.gameObject.transform.position);
                                MoveDestination();
                            }
                        }
                    }
                isTarget = false;
                }
            else if (Input.GetKeyDown(KeyCode.A))            //A키를 눌러 공격 범위 표시
            {
                attackRange.SetActive(true);
                isAttack = true;
            }
            else if (Input.GetKeyDown(KeyCode.C))            //C키를 눌러 앉기, 서기
            {
                if (!isCrouch)                                              //서있을 때, 앉기
                {
                    transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    isCrouch = true;
                }
                else                                                          //앉아있을 때, 서기
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    isCrouch = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.R))           //R키를 눌러 재장전
            {
                if (!gunControl.equippedGun.isReloading)
                {
                    //gunControl.Reload();
                }
            }
             else if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                gunControl.ChangeMainGun();  
                
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gunControl.ChangeSubGun();
            }
            if (Input.GetMouseButtonDown(0) && isAttack)
            {
                attackRange.SetActive(false);

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Enemy"))                   //적을 클릭 했을 때
                    {
                        targetCollider = hit.collider;
                        targetRange = (targetCollider.transform.position - transform.position).magnitude;
                        range = gunControl.equippedGun.range;          
                        isTarget = true;
                        isAttack = false;
                        attackRange.SetActive(false);
                     }
                }
            }
        }
        else
        {
            
        }
        if (isTarget&&!gunControl.equippedGun.isReloading)                                   //target 설정이 되고 재장전 중이 아니면 target을 공격
        {
            if (targetCollider != null)
            {
                targetRange = (targetCollider.transform.position - transform.position).magnitude;
                transform.LookAt(targetCollider.transform.position);
                if (targetRange > range)
                {
                    SetDestination(targetCollider.transform.position);
                    MoveDestination();
                }
                else
                {
                    StopDestination();
                    gunControl.Shoot();
                }
            }   
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "Heal")                                                           
        {
            Aid_Kit healObject = collider.gameObject.GetComponent<Aid_Kit>();
            if (healObject != null)
            {
                health += healObject.heal;
                if(health >= maximumHealth)
                {
                    health = maximumHealth;
                }
            }
            GameObject.Destroy(collider.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

    }

    public void SetDestination(Vector3 Position) // 해당위치(Vector3 Position)로 캐릭터 자동 이동
    {
        this.gameObject.GetComponent<NavMeshAgent>().SetDestination(Position);
    }
    public void StopDestination() // 가던 캐릭터 멈춤
    {
        this.gameObject.GetComponent<NavMeshAgent>().Stop();
    }
    public void MoveDestination() // 가던 캐릭터 이동 재개
    {
        this.gameObject.GetComponent<NavMeshAgent>().Resume();
    }
}
*/
