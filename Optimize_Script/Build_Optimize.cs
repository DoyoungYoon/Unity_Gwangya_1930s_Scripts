using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Optimize : MonoBehaviour {

    public static float viewDistance = 40;


    private GameObject player;
    private GameObject[] builds;
    
	// Use this for initialization
	void Start () {
        if(GameObject.FindGameObjectWithTag("Player_") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player_");
        }
        builds = GameObject.FindGameObjectsWithTag("Builds");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            for (int i = 0; i < builds.Length; i++)
            {
                try
                {
                    if (Vector3.Distance(builds[i].transform.position, player.transform.position) > viewDistance)
                    {
                        builds[i].SetActive(false);
                    }
                    else
                    {
                        builds[i].SetActive(true);
                    }
                }
                catch (System.Exception e)
                {
                    
                }
            }
        }
    }
}
