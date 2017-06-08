using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMap : MonoBehaviour {
    
    GameObject plane;
    [SerializeField]
    GameObject Grid_Empty;
    [SerializeField]
    GameObject Grid_Barricade;
    public GameObject Grid_Barricade_Left;
    public GameObject Grid_Barricade_Front;
    public GameObject Grid_Barricade_Right;
    public GameObject Grid_Barricade_Back;

    GameObject Grid_Parent;
    
    public int i_GridSize_X = 6;
    public int i_GridSize_Y = 6;

    private GameObject[,] Grid;
    Vector3 Offset;
   
  

    void Awake()
    {
        Grid = new GameObject[i_GridSize_X, i_GridSize_Y];
        Offset = new Vector3((i_GridSize_X - 1) * -.125f, 0, (i_GridSize_Y - 1) * -.125f);

        for (int x = 0; x < i_GridSize_X; x++)
        {
            for (int z = 0; z < i_GridSize_Y; z++) 
            {
                if((x == 0 || x == i_GridSize_X -1) || (z == 0 || z == i_GridSize_Y - 1))
                {
                    GameObject gridplane = (GameObject)Instantiate(Grid_Barricade, new Vector3(x * 0.25f, 0, z * 0.25f) + Offset, Quaternion.identity);
                    gridplane.gameObject.name = "Barricade";
                    Grid[x, z] = gridplane;
                }
                else
                {
                    GameObject gridplane = (GameObject)Instantiate(Grid_Empty, new Vector3(x * 0.25f, 0, z * 0.25f) + Offset, Quaternion.identity);
                    gridplane.gameObject.name = "Empty Grid";
                    Grid[x, z] = gridplane;
                }
                
            }
        }
        
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
     

	}

    public void ClamptoGrid(GameObject go)
    {
        float radius = .5f;
        GameObject PointtoClamp = null;
        List<GameObject> NearestGridList = new List<GameObject>();
        for (int x = 0; x < i_GridSize_X; x++)
        {
            for (int z = 0; z < i_GridSize_Y; z++)
            {
                if (Grid[x, z].gameObject.name == "Empty Grid")
                {
                    if (Vector3.Distance(Grid[x, z].transform.position, go.transform.position) <= radius)
                    {
                        NearestGridList.Add(Grid[x, z]);
                    }
                }
            }

        }
        //Run through List to check which position is the nearest
        for (int i = 0; i < NearestGridList.Count; i++)
        {
            float tempdistance;
            tempdistance = Vector3.Distance(NearestGridList[i].transform.position, go.transform.position);
            if (tempdistance < radius)
            {
                radius = tempdistance;
                PointtoClamp = NearestGridList[i];
            }
        }
        //Clamps the unit's position down to the nearest grid
        for (int i = 0; i < NearestGridList.Count; i++)
        {
            float tempdistance;
            tempdistance = Vector3.Distance(NearestGridList[i].transform.position, go.transform.position);
            if (tempdistance == radius)
            {
                go.transform.position = PointtoClamp.transform.position;
            }
        }

    }
}
