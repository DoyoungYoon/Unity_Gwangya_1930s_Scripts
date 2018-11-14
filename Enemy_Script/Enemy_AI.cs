using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour {


    //전투상태인가?
    public static bool isCombat = false;
    static bool fightMusicPlay = false;



    public enum AI_State
    {
        Reconnaissance, Suspicion, Search, Fight
    }

    public bool aiActive;

    private NavMeshAgent nav;

    public Transform[] reconnaissancePoint;
    public AI_State aiState;
    public float eyeSight;

    private float ShootDistance = 10;
    private float acceptDistance = 2f;

    public GameObject targetCharacter;
    Vector3 suspicionPosition;

    int reconnaissancePoint_Count = 0;

    //현재 총을 쏘고있는가?
    private bool isShoot;
    //엄폐관련
    private Enemy_Covering coverManager;
    private bool isCovering;
    public int coverPercentage;

    private bool stopMotion = false;

    // Use this for initialization
    void Start () {
        nav = GetComponent<NavMeshAgent>();

        if (reconnaissancePoint.Length > 0)
        {
            try
            {
                nav.SetDestination(reconnaissancePoint[reconnaissancePoint_Count].position);
            }
            catch(System.Exception e)
            {
                nav.SetDestination(transform.position);
            }
        }

        aiState = AI_State.Reconnaissance;
        isShoot = false;
        isCovering = false;

        coverManager = GetComponent<Enemy_Covering>();
        suspicionPosition = new Vector3();
    }

    public Vector3 GetVelocity()
    {
        Vector3 result = nav.velocity;
        return result;
    }

    private void Update()
    {
        if (aiActive == true)
        {
            if (aiState.Equals(AI_State.Reconnaissance))
            {
                if (reconnaissancePoint.Length > 0)
                {
                    ReconnaissanceProc();
                }
                if(isCombat == true)
                {
                    FindPlayer();
                }
            }
            else if (aiState.Equals(AI_State.Search))
            {
                SearchPlayer();
            }
            else if (aiState.Equals(AI_State.Fight))
            {
                FightToPlayer();
            }
        }

        if(isCombat == true)
        {
            if(fightMusicPlay == false)
            {
                //재생
                GameObject.Find("SoundManager").GetComponent<Sound_Manager>().PlayBgmSound(1);
                fightMusicPlay = true;
            }
            else
            {

            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
       
    }

    public NavMeshAgent GetNav()
    {
        return nav;
    }

    void ReconnaissanceProc()
    {
        Vector3 chkPosition = transform.position - new Vector3(0, 1, 0);
        
        if (Vector3.Distance(chkPosition, nav.pathEndPosition) < acceptDistance)
        {
            if(stopMotion == false)
            {
                stopMotion = true;
                Invoke("SetNextDestination", 3f);
            }
        }
        
        
    }

    void SetNextDestination()
    {
        //print(reconnaissancePoint_Count);
        reconnaissancePoint_Count++;
        if (reconnaissancePoint_Count >= reconnaissancePoint.Length)
        {
            reconnaissancePoint_Count = 0;
        }

        if (reconnaissancePoint[reconnaissancePoint_Count] != null)
        {
            nav.SetDestination(reconnaissancePoint[reconnaissancePoint_Count].position);
        }
        stopMotion = false;
    }

    void FindPlayer()
    {
        Collider[] findObject = Physics.OverlapSphere(transform.position, eyeSight);

        foreach(Collider col in findObject)
        {
            if (col.tag.Equals("Player_"))
            {
                if(Vector3.Angle(transform.forward, col.transform.position - transform.position) < 25)
                {
                    Debug.Log("Player 의심");
                    aiState = AI_State.Suspicion;
                    nav.SetDestination(transform.position);
                    suspicionPosition = col.transform.position;
                    Invoke("ToSearch", 1.5f);

                    nav.speed = 5;
                }
            }
        }
    }

    void SearchPlayer()
    {
        Collider[] findObject = Physics.OverlapSphere(transform.position, eyeSight);

        foreach (Collider col in findObject)
        {
            if (col.tag.Equals("Player_"))
            {
               

                if (Vector3.Angle(transform.forward, col.transform.position - transform.position) < 15)
                {
                    Debug.Log("Player 공격");
                    aiState = AI_State.Fight;
                    targetCharacter = col.gameObject;
                    



                    int rand = Random.Range(0, 100);
                    if(rand < coverPercentage)
                    {
                        isCovering = true;

                        if(coverManager.cover == null)
                        {
                            isCovering = false;
                        }
                    }
                    else
                    {
                        isCovering = false;
                    }

                    
                    ShootDistance = Random.Range(8, 12);


                    Collider[] enemys = Physics.OverlapSphere(transform.position, 45);

                    for (int i = 0; i < enemys.Length; i++)
                    {
                        if (enemys[i].gameObject.Equals(gameObject) == false && enemys[i].CompareTag("Enemy"))
                        {
                            enemys[i].GetComponent<Enemy_AI>().SetTarget(targetCharacter);
                        }
                    }
                }
            }
        }
        
        if(Vector3.Distance(transform.position, nav.destination) < 1.5f)
        {
            aiState = AI_State.Reconnaissance;
            nav.speed = 2.5f;
        }
    }

    void ToSearch()
    {
        aiState = AI_State.Search;
        nav.SetDestination(suspicionPosition);
    }

    void FightToPlayer()
    {
        if (targetCharacter != null)
        {
            if (isCovering)
            {
                isShoot = false;
                nav.SetDestination(coverManager.FindCoverPoint());
                if (Vector3.Distance(transform.position, nav.pathEndPosition) < 1.2f)
                {
                    isCovering = false;
                }
            }
            else
            {
                if(isCombat == true)
                {
                    if (Vector3.Distance(transform.position, targetCharacter.transform.position) < ShootDistance)
                    {
                        transform.LookAt(targetCharacter.transform);

                        GetComponent<Enemy_Shoot>().Shoot();
                        nav.SetDestination(transform.position);
                        isShoot = true;
                    }
                    else
                    {
                        nav.SetDestination(targetCharacter.transform.position);
                        isShoot = false;
                    }

                    if (Physics.Raycast(transform.position, targetCharacter.transform.position))
                    {

                    }
                }
                else
                {
                    nav.SetDestination(transform.position);
                }
                
            }
        }
    }

    public bool GetIsShoot()
    {
        return isShoot;
    }

    public void SetTarget(GameObject target)
    {
        if(aiState != AI_State.Fight && aiState != AI_State.Search)
        {
            targetCharacter = target;
            suspicionPosition = target.transform.position;
            ToSearch();
            nav.speed = 5f;
        }
    }

    public void Dead()
    { 
        nav.isStopped = true;
    }
}
