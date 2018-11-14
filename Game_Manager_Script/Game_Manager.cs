using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour {

    public GameObject player;
	public GameObject acquireArisaka;
	public GameObject acquireMp18;
    private GameObject playerPoisition;
    public Camera mainCamera;
    public GameObject stageCrowds;
    public GameObject streetCrowds;
    public GameObject enemy;
    public GameObject point1;
    public GameObject point2;
    public GameObject assassinTarget;
    public GameObject assassinPoint;
    public GameObject assassinTarget2;
    public GameObject assassinPoint2;
	public float interrogateScore;
    public GameObject shootTarget;
    public GameObject getToTheCarPoint;
    [Header("Creat Enemy")]
    public GameObject enemyCreatPoint1;
    public GameObject enemyCreatPoint1_1;
    public GameObject enemyCreatPoint2;
    public GameObject enemyCreatPoint2_1;
	public GameObject enemyCreatPoint3;
	public GameObject enemyCreatPoint3_1;
    public GameObject enemyCreatPoint4;
    public GameObject enemyCreatPoint4_1;
    public GameObject enemyCreatPoint5;
    public GameObject enemyCreatPoint5_1;

    private GameObject creatEnemy;
    public GameObject targetCar;
    public GameObject ExplodeCar;
    public GameObject runAwayPoint;
    public GameObject endingScene;
    public GameObject runAwayPointBlock;
    //public ParticleControlPlayable b;
    //public ControlPlayableAsset a;
	public Canvas gameOverCanvas;
    public Canvas tutorialCanvas;
    private bool isTutorial;
    public GameObject tutorialImage;
    public Sprite[] tutorialImages;
    public IEnumerator tutorial;
    private GameObject playerModel;

    public bool isGoToPoint2;
    public bool isGoToAssassionPoint2;
    public bool isGoToAssassionPoint2_2;
    public bool isGoToZoomInTarget;
    public bool isShootTarget;
    public bool isFinishedShootTargetTimelines;
    public bool isGetToTheCar;
	public bool isExplodeCar;
    public bool isMissionComplete;
    public bool isRunAway;
	public bool isGameOver;
    public List<PlayableDirector> playableDirectors;
    public List<TimelineAsset> timelines;
    private int timelineCount;
    private int enemyCount;
	private int plusEnemyCount;
	private int plusEnemy;
	public int carInFlag;
    private bool isPause = true;
    private bool isTargetCarExplodeTutorial;
	public float initCameraFOV;

    public AudioClip[] audioClips;
	void Update () {
        PressedEsc();
	}

    IEnumerator Start()
    {
        playerModel = player.GetComponentInChildren<PlayerModel>().gameObject;
        mainCamera.enabled = true;
		initCameraFOV = mainCamera.fieldOfView;
		mainCamera.fieldOfView = initCameraFOV;
        player.GetComponent<Player_Char>().isControl = false;
        stageCrowds.SetActive(true);
        playableDirectors[0].Play(timelines[0]);            //GettingStarted timeline
        timelineCount++;
        yield return StartCoroutine("GoToPoint1");
        player.GetComponent<Player_Char>().isControl = true;
        yield return StartCoroutine("CollidePoint2");
        yield return StartCoroutine("AssassinTarget");
        yield return StartCoroutine("AssassinTarget2");
        yield return StartCoroutine("GoToInterrogate");
        yield return StartCoroutine("GoToZoomInTarget");
        yield return StartCoroutine("ZoomInTarget");
        yield return StartCoroutine("ShootTarget");
        yield return StartCoroutine("GetToTheCar");
        yield return StartCoroutine("PlayerExit");
        



    }
    void PressedEsc()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                Time.timeScale = 0;
                isPause = false;
                player.GetComponent<Player_Char>().isControl = false;


            }
            else
            {
                Time.timeScale = 1;
                isPause = true;
                player.GetComponent<Player_Char>().isControl = true;

            }


        }
    }
    IEnumerator GoToPoint1()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 targetPos = new Vector2(point1.transform.position.x, point1.transform.position.z);
        while (Vector2.Distance(playerPos, targetPos) >=0.1f)
        {
            player.GetComponent<Player_Char>().deltaMoveSpeed = player.transform.forward * 0.5f;
            playerPos = Vector2.MoveTowards(playerPos, targetPos, 5f*Time.deltaTime);
            player.transform.position = new Vector3(playerPos.x, player.transform.position.y, playerPos.y);
            yield return null;
        }
		mainCamera.transform.rotation = Quaternion.Euler(70f, 0f, 0);
        Destroy(point1);
        //tutorialImage.GetComponent<Image>().sprite = tutorialImages[0];
        //tutorialCanvas.gameObject.SetActive(true);

        //PutIntutorialImageToShowTutorialCanvas(tutorialImages[0]);
        //yield return StartCoroutine(tutorial);
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[0]));         //초반 튜토리얼
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[1]));
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[2]));
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[3]));
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[4]));

    }
    IEnumerator CollidePoint2()
    {
        while (!isGoToPoint2)
        {
            yield return null;
        }
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[5]));             //암살 튜토리얼

        mainCamera.GetComponent<Camera_Controller>().playerControllUnit = assassinTarget;
        assassinTarget.GetComponent<Enemy_AI>().reconnaissancePoint = new Transform[1];
        assassinTarget.GetComponent<Enemy_AI>().reconnaissancePoint[0] = assassinPoint.transform;

    }
    IEnumerator AssassinTarget()
    {
        while (!assassinTarget.GetComponent<Enemy_Life>().dead)
        {
            if ((new Vector2(assassinTarget.transform.position.x - assassinPoint.transform.position.x,
                assassinTarget.transform.position.z - assassinPoint.transform.position.z)).magnitude <= 10f || assassinTarget.GetComponent<Enemy_Life>().dead)
            {
                mainCamera.GetComponent<Camera_Controller>().playerControllUnit = player;
            }
            yield return null;
        }
        mainCamera.GetComponent<Camera_Controller>().playerControllUnit = player;
		Instantiate (acquireArisaka, assassinTarget.transform.position, assassinTarget.transform.rotation);
        Destroy(assassinPoint);
        //Destroy(assassinTarget);
    }
    

    IEnumerator AssassinTarget2()
    {
        while (!isGoToAssassionPoint2)
        {
            yield return null;
        }
        while (isGoToAssassionPoint2 && !isGoToAssassionPoint2_2)
        {
            mainCamera.GetComponent<Camera_Controller>().playerControllUnit = assassinTarget2;
            yield return new WaitForSeconds(3f);
            isGoToAssassionPoint2_2 = true;
        }
        while (isGoToAssassionPoint2 && !assassinTarget2.GetComponent<Enemy_Life>().dead && isGoToAssassionPoint2_2)
        {
            mainCamera.GetComponent<Camera_Controller>().playerControllUnit = player;
            yield return null;
        }
        //Instantiate (acquireMp18, assassinTarget2.transform.position, assassinTarget2.transform.rotation);
        stageCrowds.SetActive(true);
        Destroy(assassinPoint2.GetComponent<Collider>());
        Destroy(assassinPoint2.gameObject, 10f);
        mainCamera.GetComponent<Camera_Controller>().playerControllUnit = player;
        yield return new WaitForSeconds(6f);
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[7]));             //심문 튜토리얼

    }

    IEnumerator GoToInterrogate()
    {
        while (false)
        {
            yield return null;
        }
    }
    IEnumerator GoToZoomInTarget()
    {
        while (!isGoToZoomInTarget)
        {
            yield return null;
        }
    }
    IEnumerator ZoomInTarget()
    {
        playableDirectors[0].Play(timelines[1]);        //TargetZoomIn timeline
        timelineCount++;
        Destroy(streetCrowds.gameObject);
        while (playableDirectors[0].state != PlayState.Paused)
        {
            player.GetComponent<Player_Char>().isControl = false;
            yield return null;
        }
        player.GetComponent<Player_Char>().isControl = true;
        shootTarget.SetActive(true);
		player.GetComponent<Player_Char> ().isAfterShootTarget = true;
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[6]));             //총기 튜토리얼
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[8]));             //구급상자 튜토리얼


    }

    IEnumerator ShootTarget()
    {
        while (!player.GetComponent<Player_Char>().isShoot && !isShootTarget)
        {
            
            yield return null;
        }
        while (player.GetComponent<Player_Char>().isShoot && !isFinishedShootTargetTimelines)
        {
            if (!isShootTarget)
            {
                isShootTarget = true;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            stageCrowds.GetComponent<AudioSource>().Pause();
            stageCrowds.GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
            stageCrowds.GetComponent<AudioSource>().PlayOneShot(audioClips[1]);
            playableDirectors[0].Play(timelines[2]);             //RunAwayTarget
            timelineCount++;
			StageCrowdsSetPaths();
            while (playableDirectors[0].state != PlayState.Paused)
            {
                player.GetComponent<Player_Char>().isControl = false;
                yield return null;
            }
			mainCamera.fieldOfView = initCameraFOV;
            player.GetComponent<Player_Char>().isControl = true;
            yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[9]));             //수류탄 튜토리얼

            isFinishedShootTargetTimelines = true;
            playableDirectors[0].Play(timelines[3]);            //RunAwayTarget2
            Enemy_AI.isCombat = true;
            Destroy(shootTarget.gameObject);

			plusEnemy = 5-(int)(interrogateScore * 5f);
			print ("심문 점수 :" + interrogateScore);
			print ("추가 소환될 적 "+plusEnemyCount);

            while (enemyCount <= 5)
            {
                creatEnemy = Instantiate(enemy, enemyCreatPoint1.transform.position, enemyCreatPoint1.transform.rotation);
                creatEnemy.GetComponent<Enemy_AI>().reconnaissancePoint[0] = enemyCreatPoint1_1.transform;
                creatEnemy.GetComponent<Enemy_AI>().aiState = Enemy_AI.AI_State.Fight;
                creatEnemy = Instantiate(enemy, enemyCreatPoint2.transform.position, enemyCreatPoint2.transform.rotation);
                creatEnemy.GetComponent<Enemy_AI>().reconnaissancePoint[0] = enemyCreatPoint2_1.transform;
                creatEnemy.GetComponent<Enemy_AI>().aiState = Enemy_AI.AI_State.Fight;
                if (plusEnemyCount <= plusEnemy) {
					creatEnemy = Instantiate(enemy, enemyCreatPoint3.transform.position, enemyCreatPoint1.transform.rotation);
					creatEnemy.GetComponent<Enemy_AI>().reconnaissancePoint[0] = enemyCreatPoint3_1.transform;
                    creatEnemy.GetComponent<Enemy_AI>().aiState = Enemy_AI.AI_State.Fight;

                }
                plusEnemyCount++;
                enemyCount++;
                yield return new WaitForSeconds(5f);
            }





            while (playableDirectors[0].state != PlayState.Paused)
            {
                yield return null;
            }
            GameObject.Find("MiniMapCamera").GetComponent<MiniMapCamera_Controller>().targetIcons[1].SetActive(false);      //도망간 Target의 아이콘 제거
            yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[11]));        //타겜 암살 실패 설명


        }
    }

    IEnumerator GetToTheCar()
    {
        getToTheCarPoint.SetActive(true);
        runAwayPoint.SetActive(true);
        while (!isGetToTheCar)
        {
            yield return null;
        }
        if (isGetToTheCar)
        {
            playableDirectors[0].Play(timelines[4]);                        //GetToTheCar
            //player.GetComponent<Player_Char>().isControl = false;
            while (playableDirectors[0].state != PlayState.Paused && !isMissionComplete || isRunAway)
            {
                if (playableDirectors[0].time < 12.0f)
                {
                    //적이 공격을 잠시 멈춰야함
                    player.GetComponent<Player_Life>().isCutScene = true;
                    Enemy_AI.isCombat = false;
                    player.GetComponent<Gun_Control>().grenadeCapacity = 5;
                    player.GetComponent<Gun_Control>().isGrenadeEmpty = false;
                }
                else
                {
                    if (!isTargetCarExplodeTutorial)
                    {
                        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[10]));             //타겟차 폭파 튜토리얼
                        isTargetCarExplodeTutorial = true;
                    }
                    player.GetComponent<Player_Life>().isCutScene = false;
                    Enemy_AI.isCombat = true;

                }
//                print (isGameOver);
				if (isGameOver) 
				{
                    //게임오버씬
                    //print("차 폭파 실패로 게임 오버");
					gameOverCanvas.gameObject.SetActive(true);
                    yield return null;
				}

				while (isExplodeCar)
                {
                    isMissionComplete = true;
                    playableDirectors[0].Pause();
					switch (carInFlag)
                    {

                        case 1:
                            //print("폭파 깃발 "+targetCar.GetComponentInChildren<CarExplosion>().kindOfFlag);
                            playerModel.SetActive(false);		            //그냥 플레이어 비활성화시 오류
                            playableDirectors[0].Play(timelines[5]);
                            break;
                        case 2:
                            //print("폭파 깃발 " + targetCar.GetComponentInChildren<CarExplosion>().kindOfFlag);
                            playerModel.SetActive(false);		            //그냥 플레이어 비활성화시 오류
                            playableDirectors[0].Play(timelines[6]);
                            break;
					    case 0:
						    print ("깃발 없는 폭파");
                            playerModel.SetActive(false);		           //그냥 플레이어 비활성화시 오류
                            //playableDirectors[0].Play(timelines[7]);
                            break;
                        default:
                            break;
                    }
					Enemy_AI.isCombat = false;

                    break;
                }


				if (!isGameOver) {

                    yield return null;
				}
            }
		}
    }

    IEnumerator PlayerExit()
    {
		Destroy(runAwayPointBlock.gameObject);
        while (playableDirectors[0].state != PlayState.Paused)             
        {

            player.GetComponent<Player_Char>().isControl = false;
            player.GetComponent<Player_Life>().isCutScene = true;
            yield return null;
        }
		mainCamera.fieldOfView = initCameraFOV;
        //폭파 컷신 끝
        yield return StartCoroutine(ShowTutorialCanvas(tutorialImages[12]));
        playerModel.SetActive(true);		//그냥 플레이어 비활성화시 오류
        Destroy(ExplodeCar.gameObject);
        player.GetComponent<Player_Char>().isControl = true;
        player.GetComponent<Player_Life>().isCutScene = false;
		Enemy_AI.isCombat = true;
        enemyCount = 0;
        while (enemyCount < 5)
        {
            creatEnemy = Instantiate(enemy, enemyCreatPoint4.transform.position, enemyCreatPoint4.transform.rotation);
            creatEnemy.GetComponent<Enemy_AI>().reconnaissancePoint[0] = enemyCreatPoint4_1.transform;
            creatEnemy.GetComponent<Enemy_AI>().aiState = Enemy_AI.AI_State.Fight;
            creatEnemy = Instantiate(enemy, enemyCreatPoint5.transform.position, enemyCreatPoint5.transform.rotation);
            creatEnemy.GetComponent<Enemy_AI>().reconnaissancePoint[0] = enemyCreatPoint5_1.transform;
            creatEnemy.GetComponent<Enemy_AI>().aiState = Enemy_AI.AI_State.Fight;
            enemyCount++;
            Enemy_AI.isCombat = true;

            yield return new WaitForSeconds(0.5f);
        }


        while (!isRunAway)
        {
            yield return null;
        }
        GameObject[] destoryEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i =0; i< destoryEnemies.Length; i++)
        {
            GameObject.Destroy(destoryEnemies[i].gameObject);
        }
        GameObject.Find("SoundManager").GetComponent<Sound_Manager>().OffTheVolume();
        playableDirectors[0].Play(timelines[8]);            //엔딩씬 시작
        playerModel.SetActive(false);                           //그냥 플레이어 비활성화시 오류
        player.GetComponent<AudioListener>().enabled = false;
		Enemy_AI.isCombat = false;
		print ("플레이 감사1");

        while (playableDirectors[0].state != PlayState.Paused)
        {
            player.GetComponent<Player_Char>().isControl = false;
            player.GetComponent<Player_Life>().isCutScene = true;
            yield return null;
        }
        //player.GetComponent<AudioListener>().enabled = true;
        playerModel.SetActive(true);                           //그냥 플레이어 비활성화시 오류
        print("플레이 감사2");
		//Application.LoadLevel("Ending");

		//mainCamera.GetComponent<Fade>()._fadeSetting._fadeOut = true;
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Ending", UnityEngine.SceneManagement.LoadSceneMode.Additive);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Ending");
    }

    /*
    IEnumerator RunAwayTarget()
    {

    }*/


    private void StageCrowdsSetPaths()
    {
        Crowd_AI[] runAwayCrowds = stageCrowds.GetComponentsInChildren<Crowd_AI>();
        PlayableDirector[] crowdsPlayable = stageCrowds.GetComponentsInChildren<PlayableDirector>();
        for(int i = 0; i<crowdsPlayable.Length; i++)
        {
            crowdsPlayable[i].Pause();
        }
        for (int i = 0; i < runAwayCrowds.Length; i++)
        {
            runAwayCrowds[i].SetRunAwayPaths();
        }
    }
    
    void PutIntutorialImageToShowTutorialCanvas(Sprite tutorialImage)
    {
        tutorial = ShowTutorialCanvas(tutorialImage);
    }



    IEnumerator ShowTutorialCanvas(Sprite image)
    {
        //print("캔버스 코루틴 들어감");
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            //print("캔버스 활성화");
            tutorialImage.GetComponent<Image>().sprite = image;
            tutorialCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            player.GetComponent<Player_Char>().isControl = false;
            yield return null;
        }
        //print("캔버스 코루틴 빠져나옴");
        Time.timeScale = 1;
        player.GetComponent<Player_Char>().isControl = true;
        tutorialCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }
}
