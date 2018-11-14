using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera_Controller : MonoBehaviour {

    private GameObject playerControllUnit;
    Vector3 player;
    public GameObject[] targetIcons;
    public GameObject[] IconArrows;
    public float boundaryX;
    public float boundaryZ;

    void Start()
    {
        playerControllUnit = GameObject.FindGameObjectWithTag("Player_");
        StartCoroutine("MiniMapCameraFollowToPlayer");
    }

    IEnumerator MiniMapCameraFollowToPlayer() {
        while (playerControllUnit != null)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, player, 0.5f);
            for(int i =0; i<targetIcons.Length; i++)
            {
                player = new Vector3(playerControllUnit.transform.position.x,transform.position.y,playerControllUnit.transform.position.z);

                if (targetIcons[i] != null)
                {
                    Vector3 IconRot = new Vector3(targetIcons[i].transform.position.x - transform.position.x,0f, targetIcons[i].transform.position.z - transform.position.z); 
                    targetIcons[i].transform.rotation = Quaternion.LookRotation(IconRot, Vector3.up);
                    if (targetIcons[i].transform.position.x - transform.position.x > boundaryX)
                    {
                        IconArrows[i].SetActive(true);
                        if (targetIcons[i].transform.position.z - transform.position.z > boundaryZ)
                        {
                            targetIcons[i].GetComponentInChildren<Target_Icon>().transform.position = new Vector3(transform.position.x + boundaryX, targetIcons[i].transform.position.y+20f, transform.position.z + boundaryZ);
                        }
                        else if (targetIcons[i].transform.position.z - transform.position.z < -boundaryZ)
                        {
                            targetIcons[i].GetComponentInChildren<Target_Icon>().transform.position = new Vector3(transform.position.x + boundaryX, targetIcons[i].transform.position.y+20f, transform.position.z - boundaryZ);
                        }
                        else
                        {
                            targetIcons[i].GetComponentInChildren<Target_Icon>().transform.position = new Vector3(transform.position.x + boundaryX, targetIcons[i].transform.position.y + 20f, targetIcons[i].transform.position.z);
                        }
                    }
                    else if(targetIcons[i].transform.position.x - transform.position.x < -boundaryX)
                    {
                        IconArrows[i].SetActive(true);
                        if (targetIcons[i].transform.position.z - transform.position.z > boundaryZ)
                        {
                            targetIcons[i].GetComponentInChildren<Target_Icon>().transform.position = new Vector3(transform.position.x - boundaryX, targetIcons[i].transform.position.y + 20f, transform.position.z + boundaryZ);
                        }
                        else if (targetIcons[i].transform.position.z - transform.position.z < -boundaryZ)
                        {
                            targetIcons[i].GetComponentInChildren<Target_Icon>().transform.position = new Vector3(transform.position.x - boundaryX, targetIcons[i].transform.position.y + 20f, transform.position.z - boundaryZ);
                        }
                        else
                        {
                            targetIcons[i].GetComponentInChildren<Target_Icon>().transform.position = new Vector3(transform.position.x - boundaryX, targetIcons[i].transform.position.y + 20f, targetIcons[i].transform.position.z);
                        }
                    }
                    else
                    {
                        IconArrows[i].SetActive(true);
                        if (targetIcons[i].transform.position.z - transform.position.z > boundaryZ)
                        {
                            targetIcons[i].GetComponentInChildren<Target_Icon>().transform.position = new Vector3(targetIcons[i].transform.position.x, targetIcons[i].transform.position.y + 20f, transform.position.z + boundaryZ);
                        }
                        else if (targetIcons[i].transform.position.z - transform.position.z < -boundaryZ)
                        {
                            targetIcons[i].GetComponentInChildren<Target_Icon>().transform.position = new Vector3(targetIcons[i].transform.position.x, targetIcons[i].transform.position.y + 20f, transform.position.z - boundaryZ);
                        }
                        else
                        {
                            IconArrows[i].SetActive(false);
                        }
                    }
                }
            }
            
            yield return null;
        }
        
	}	
}
