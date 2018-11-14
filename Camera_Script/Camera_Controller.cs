using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour {

    public float camera_Speed;

    public GameObject playerControllUnit;

    Quaternion destinationQuaternion;
    float cameraHeight;

    void Start () {
        transform.rotation = Quaternion.Euler(70f, 0f, 0);
        destinationQuaternion = transform.rotation;
        playerControllUnit = GameObject.FindGameObjectWithTag("Player_");
        playerControllUnit.GetComponent<Player_Char>().SetCamera(this.gameObject);
        cameraHeight = 15;


        Vector3 destinationPosition = new Vector3(playerControllUnit.transform.position.x + Mathf.Cos((-transform.eulerAngles.y - 90f) * Mathf.Deg2Rad) * 5,
           cameraHeight, playerControllUnit.transform.position.z + Mathf.Sin((-transform.eulerAngles.y - 90f) * Mathf.Deg2Rad) * 5);

        transform.position = destinationPosition;
    }

    void Update()
    {
        if(Input.mouseScrollDelta.y < 0)
        {
            if(18 >= cameraHeight)
            {
                cameraHeight -= Input.mouseScrollDelta.y * Time.deltaTime * 30;
            }
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            if (5 <= cameraHeight)
            {
                cameraHeight -= Input.mouseScrollDelta.y * Time.deltaTime * 30;
            }
        }

        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");

            if(mouseX > 0)
            {
                transform.RotateAround(playerControllUnit.transform.position , new Vector3(0, 1, 0), 100 * Time.deltaTime);
                //Debug.Log("반시계회전");
            }
            else if (mouseX < 0)
            {
                transform.RotateAround(playerControllUnit.transform.position, new Vector3(0, 1, 0), -100 * Time.deltaTime);
                //Debug.Log("시계회전");
            }
        }
    }
	
    void FixedUpdate()
    {
        FollowCameraToPlayer();
    }

    void FollowCameraToPlayer()
    {
        if(playerControllUnit != null)
        {
            Vector3 destinationPosition = new Vector3(playerControllUnit.transform.position.x + Mathf.Cos((-transform.eulerAngles.y - 90f) * Mathf.Deg2Rad) * 5,
            cameraHeight, playerControllUnit.transform.position.z + Mathf.Sin((-transform.eulerAngles.y - 90f) * Mathf.Deg2Rad) * 5);

            transform.position = destinationPosition;//= Vector3.Lerp(transform.position, destinationPosition, 10f * Time.deltaTime);

            transform.LookAt(playerControllUnit.transform.position);
            //Debug.Log(Vector3.Angle(new Vector3(), playerControllUnit.transform.position-transform.position));
        }
    }

  
}
