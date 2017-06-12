using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 viewPortPos;

    public GameObject test;

    public GridMap The_Grid;


    // Use this for initialization
    void Start () {
        test = new GameObject();
        The_Grid = GameObject.Find("Grid Spawner").GetComponent<GridMap>();
	}
	
	// Update is called once per frame
	void Update () {
        viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
        The_Grid.ClamptoGrid(gameObject);
    }

    void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        if (viewPortPos.x < 0.1 && viewPortPos.y < 0.1)
        {
            Destroy(gameObject);
        }

        //The_Grid.ClamptoGrid(this.gameObject);
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
        }
    }
}
