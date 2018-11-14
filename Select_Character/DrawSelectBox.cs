using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



public static class Utils
{
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color) //외곽선을 위한 사각형 Draw 함수
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color) //셀렉트(선택) 박스 외곽선 GUI 생성 함수
    {
        // Top
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2) //셀렉트(선택) 박스 GUI 생성 함수
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2) //선택영역 추출
    {
        var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
        var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }
}

public class DrawSelectBox : MonoBehaviour {

    bool isSelecting = false;
    Vector3 mousePosition1;

    public GameObject[] playerControllUnits;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Vector3.Distance(mousePosition1, Input.mousePosition) > 0.1f){ // 셀렉트 영역이 넓을때
                for (int i = 0; i < playerControllUnits.Length; i++)
                {

                    if (IsWithinSelectionBounds(playerControllUnits[i])) // 바운딩 박스 내부에 오브젝트가 존재할경울 True
                    {
                        playerControllUnits[i].GetComponent<Player_Char>().isSelect = true;

                    }

                    else // 바운딩 박스 내부에 오브젝트가 존재할경울 False
                    {
                        playerControllUnits[i].GetComponent<Player_Char>().isSelect = false;
                    }
                }
            }
            else// 한번 클릭시
            {
                for (int i = 0; i < playerControllUnits.Length; i++)
                {
                    if (playerControllUnits[i].GetComponent<Player_Char>().isAttack == true)            //A 눌렀을시 선택 취소를 하지않음
                    {

                    }
                    else
                    {
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.CompareTag("Player_"))
                            {
                                playerControllUnits[i].GetComponent<Player_Char>().isSelect = false;
                                hit.collider.gameObject.GetComponent<Player_Char>().isSelect = true;
                            }
                        }
                    }
                }
            }
            isSelecting = false;
        }
        SelectButtonPlayer();
    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting) // 마우스가 클릭되어 있는 순간에만 체크하도록 설정
            return false;

        var camera = Camera.main;
        var viewportBounds =
            Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition); // 선택영역의 바운드 박스를 생성하여 저장

        //Debug.Log(gameObject);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position)); // 생성된 바운드 박스 내에 오브젝트의 Position 이 포함되는지 체크하여 리턴
    }

    void OnGUI()//실질적으로 선택영역 박스 GUI Draw
    {
        if (isSelecting)  // 마우스 버튼을 클릭할때 True 놓을때 False로 True일때만 셀렉트 박스를 그려줌
        {
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    void SelectButtonPlayer()                                //1,2,3,4 키 누르면 각 Player로 카메라 전환
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for (int i = 0; i < playerControllUnits.Length; i++)
            {
                playerControllUnits[i].GetComponent<Player_Char>().isSelect = false;
            }
                playerControllUnits[0].GetComponent<Player_Char>().isSelect = true;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            for (int i = 0; i < playerControllUnits.Length; i++)
            {
                playerControllUnits[i].GetComponent<Player_Char>().isSelect = false;
            }
            playerControllUnits[1].GetComponent<Player_Char>().isSelect = true;
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            for (int i = 0; i < playerControllUnits.Length; i++)
            {
                playerControllUnits[i].GetComponent<Player_Char>().isSelect = false;
            }
            playerControllUnits[2].GetComponent<Player_Char>().isSelect = true;
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            for (int i = 0; i < playerControllUnits.Length; i++)
            {
                playerControllUnits[i].GetComponent<Player_Char>().isSelect = false;
            }
            playerControllUnits[3].GetComponent<Player_Char>().isSelect = true;
        }

    }

}
