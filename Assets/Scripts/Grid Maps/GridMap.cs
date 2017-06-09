using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMap : MonoBehaviour {
    
    GameObject plane;
    [SerializeField]
    GameObject Grid_Empty;
    [SerializeField]
    GameObject Grid_Barricade;
    public GameObject Grid_Barricade_Front;
    public GameObject Grid_Barricade_Back;
<<<<<<< HEAD
    public GameObject Grid_Barricade_Left;
    public GameObject Grid_Barricade_Right;
=======
    public GameObject HealthBarTemp;
>>>>>>> 46fa943da66c655752c0cb52c2079305128899e0

    GameObject Grid_Parent;
    
    public int i_GridSize_X = 6;
    public int i_GridSize_Y = 6;

    private GameObject[,] Grid;
    Vector3 Offset, Front_Offset, Back_Offset, Left_Offset, Right_Offset;
   
  

    void Awake()
    {
        Grid = new GameObject[i_GridSize_X, i_GridSize_Y];
        Offset = new Vector3((i_GridSize_X - 1) * -.5f, 0, (i_GridSize_Y - 1) * -.5f);
        Front_Offset = new Vector3(-1, 0, 0);
        Back_Offset = new Vector3(1, 0, 0);
        Left_Offset = new Vector3(0, 0, -1);
        Right_Offset = new Vector3(0, 0, 1);

<<<<<<< HEAD
        BuildGrid();
=======
        for (int x = 0; x < i_GridSize_X; x++)
        {
            for (int z = 0; z < i_GridSize_Y; z++) 
            {
                if((x == 0 || x == i_GridSize_X -1) || (z == 0 || z == i_GridSize_Y - 1))
                {
                    GameObject gridplane = (GameObject)Instantiate(Grid_Barricade, new Vector3(x * 0.25f, 0, z * 0.25f) + Offset, Quaternion.identity);
                    gridplane.gameObject.name = "Barricade";
                    gridplane.gameObject.GetComponent<Health>().HPBarPlaneTemplate = HealthBarTemp;
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
        
>>>>>>> 46fa943da66c655752c0cb52c2079305128899e0
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
     

	}

    public void BuildGrid()
    {
        for (int x = 0; x < i_GridSize_X; x++)
        {
            for (int z = 0; z < i_GridSize_Y; z++)
            {
                //if((x == 0 || x == i_GridSize_X -1))
                //{
                //    GameObject gridplane = (GameObject)Instantiate(Grid_Barricade_Front, new Vector3(x * 0.25f, 0, z * 0.25f) + Offset, Quaternion.identity);
                //    gridplane.gameObject.name = "Barricade";
                //    Grid[x, z] = gridplane;
                //}
                //else if (z == 0 || z == i_GridSize_Y - 1)
                //{
                //    GameObject gridplane = (GameObject)Instantiate(Grid_Barricade_Left, new Vector3(x * 0.25f, 0, z * 0.25f) + Offset, Quaternion.identity);
                //    gridplane.gameObject.name = "Barricade";
                //    Grid[x, z] = gridplane;
                //}
                //else
                {
                    GameObject gridplane = (GameObject)Instantiate(Grid_Empty, new Vector3(x, 0, z) + Offset, Quaternion.identity);
                    gridplane.gameObject.name = "Empty Grid";
                    Grid[x, z] = gridplane;
                }

            }
        }
        Vector3 Front_Midpoint = Grid[0, 0].transform.position + ((Grid[0, i_GridSize_Y - 1].transform.position - Grid[0, 0].transform.position) / 2);
        GameObject Front_3 = (GameObject)Instantiate(Grid_Barricade_Front, Front_Midpoint + Front_Offset, Quaternion.identity);
        Front_3.gameObject.name = "Barrier";

        Vector3 Back_Midpoint = Grid[i_GridSize_X - 1, 0].transform.position + ((Grid[i_GridSize_X - 1,i_GridSize_Y - 1].transform.position - Grid[i_GridSize_X - 1, 0].transform.position) / 2);
        GameObject Back_3 = (GameObject)Instantiate(Grid_Barricade_Back, Back_Midpoint + Back_Offset, Quaternion.identity);
        Back_3.gameObject.name = "Barrier";

        Vector3 Left_Midpoint = Grid[0, 0].transform.position + ((Grid[i_GridSize_X - 1,0].transform.position - Grid[0, 0].transform.position) / 2);
        GameObject Left_3 = (GameObject)Instantiate(Grid_Barricade_Left, Left_Midpoint + Left_Offset, Quaternion.identity);
        Left_3.gameObject.name = "Barrier";

        Vector3 Right_Midpoint = Grid[0, i_GridSize_Y - 1].transform.position + ((Grid[i_GridSize_X - 1, i_GridSize_Y - 1].transform.position - Grid[0, i_GridSize_Y - 1].transform.position) / 2);
        GameObject Right_3 = (GameObject)Instantiate(Grid_Barricade_Right, Right_Midpoint + Right_Offset, Quaternion.identity);
        Right_3.gameObject.name = "Barrier";

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
