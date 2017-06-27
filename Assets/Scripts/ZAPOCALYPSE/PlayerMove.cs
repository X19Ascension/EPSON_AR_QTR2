using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 viewPortPos;

    public GameObject test;

    public GridMap The_Grid;
    private PlacementManager placementManage;


    // Use this for initialization
    void Start () {
        test = new GameObject();
        The_Grid = GameObject.Find("Grid Spawner").GetComponent<GridMap>();
	}

    // Update is called once per frame
    void Update()
    {
        viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
        The_Grid.ClamptoGrid(gameObject);

        offset.Set(0.5f, 0.5f, 0.5f);
        Vector3 screenPt = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPt.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        //transform.position = curPosition;
        Vector3 test = curPosition;
        curPosition += offset;
        test -= offset;

        if (Input.GetMouseButtonDown(1) && ((transform.position.x < curPosition.x && transform.position.x > test.x) &&
            (transform.position.z < curPosition.z && transform.position.z > test.z)))// && 
        {
                Destroy(gameObject);
                placementManage = GameObject.Find("PlaceSurvivors").GetComponent<PlacementManager>();
                placementManage.onField -= 1;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
    }

    void OnMouseDrag()
    {
        Debug.Log("Mouse Up");
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        transform.position = curPosition;
        //The_Grid.ClamptoGrid(this.gameObject);
    }
    
    void OnMouseUp()
    {
        if (viewPortPos.x < 0.1 && viewPortPos.y < 0.1)
        {
            Destroy(gameObject);
            placementManage = GameObject.Find("PlaceSurvivors").GetComponent<PlacementManager>();
            placementManage.onField -= 1;
            Debug.Log(placementManage.onField);
        }
    }
}
