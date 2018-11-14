using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crowd_AI : MonoBehaviour {

    

    public Transform[] Paths;
    public Transform[] RunAwayPaths;
    private NavMeshAgent _nav;
    public float acceptDistance;
    private int targetPathID;
    public bool isRunAway;

    public NavMeshAgent nav
    {
        get { return _nav; }
        //set { _nav = value; }
    }

    void Start() {
        _nav = GetComponent<NavMeshAgent>();
        targetPathID = 0;
        if (Paths[targetPathID] != null )
        {
            _nav.SetDestination(Paths[targetPathID].position);
        }
        StartCoroutine("GoToPoint");
    }

    private void OnEnable()
    {
        _nav = GetComponent<NavMeshAgent>();
        targetPathID = 0;
        if (Paths[targetPathID] != null)
        {
            _nav.SetDestination(Paths[targetPathID].position);
        }
        StartCoroutine("GoToPoint");
    }

    IEnumerator GoToPoint()
    {
        while (true)
        {
            if (Paths[targetPathID] != null)
            {
				if (Vector3.Distance(transform.position, _nav.destination) < acceptDistance)
                {
					targetPathID++;
					//print (targetPathID);
                    if (targetPathID >= Paths.Length)
                    {
                        targetPathID = 0;
                    }

                    _nav.SetDestination(Paths[targetPathID].position);
                    if (isRunAway)
                    {
						if(Vector3.Distance(transform.position,RunAwayPaths[RunAwayPaths.Length-1].transform.position)<= acceptDistance)
                        {
                            Destroy(this.gameObject);
                        }
						Destroy (this.gameObject, 30f);
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void SetRunAwayPaths()
    {
        for(int i = 0; i < RunAwayPaths.Length; i++)
        {
            Paths[i] = RunAwayPaths[i];
            isRunAway = true;
        }

    }
}
