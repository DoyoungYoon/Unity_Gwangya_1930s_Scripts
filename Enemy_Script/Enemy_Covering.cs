using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Covering : MonoBehaviour {

    public GameObject cover;
    private Enemy_AI ai;

	// Use this for initialization
	void Start () {
        StartCoroutine(FindCover());
        ai = GetComponent<Enemy_AI>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void ResetCover()
    {
        cover = null;
        StartCoroutine(FindCover());
    }

    public Vector3 FindCoverPoint()
    {
        Vector3 result = new Vector3();
        if (cover != null)
        {
            result = cover.transform.position - ai.targetCharacter.transform.position;
            result.Normalize();
            result += cover.transform.position;
            result.y = transform.position.y;
            result.Scale(new Vector3(transform.localScale.x, 1, transform.localScale.z));
        }
        return result;
    }

    IEnumerator FindCover()
    {
        while(cover == null)
        {
            Collider[] tempObject = Physics.OverlapSphere(transform.position, 10);
            for (int i = 0; i < tempObject.Length; i++)
            {
                if (tempObject[i].CompareTag("Cover") || tempObject[i].CompareTag("HalfCover"))
                {
                    try
                    {
                        if (tempObject[i].GetComponent<Cover>().isCovering == false)
                        {
                            cover = tempObject[i].gameObject;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(tempObject[i].name);
                    }
                    
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
