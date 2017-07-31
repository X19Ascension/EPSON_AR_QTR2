using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMap : MonoBehaviour {
    
    //Objects for MOdels, etc
    public GameObject Original;
    [SerializeField]
    GameObject Grid_Empty;
    public GameObject Grid_Barricade_Front;
    public GameObject Grid_Barricade_Back;
    public GameObject Grid_Barricade_Left;
    public GameObject Grid_Barricade_Right;
    public GameObject HealthBarTemp;


    //Object that holds all directions for targeting
    GameObject Directions;
    //Empty GameObject for organizing purposes in Scene
    GameObject Grid_Encapsulate;
    //Empty GameObject for Organizing purposes in scene
    GameObject TargetGrid_Encapsulate;

    //Grid Size that can be changed to meet desres
    public int i_GridSize_X = 6;
    public int i_GridSize_Y = 6;

    //Object to hold all Squares to hold a Grid like system
    private GameObject[,] Grid;
    public GameObject[,] Target_Grid;
    //Exceptions of the grid above, to hold other items i.e Barricades
    List<GameObject> Grid_Exceptions;
    //Offsets for the barricades
    Vector3 Offset, Front_Offset, Back_Offset, Left_Offset, Right_Offset;

    string[] S_Directions;
  

    void Awake()
    {
        Grid_Exceptions = new List<GameObject>();

        Grid_Encapsulate = new GameObject();
        Grid_Encapsulate.gameObject.name = "The Grid";

        TargetGrid_Encapsulate = new GameObject();
        TargetGrid_Encapsulate.gameObject.name = "Targeting Grid";

        Directions = new GameObject();
        Directions.gameObject.name = "Directions";

        Grid = new GameObject[i_GridSize_X, i_GridSize_Y];
        Target_Grid = new GameObject[3, 3];

        Offset = new Vector3((i_GridSize_X - 1) * -.5f, 0, (i_GridSize_Y - 1) * -.5f);
        Front_Offset = new Vector3(-1, 0, 0);
        Back_Offset = new Vector3(1, 0, 0);
        Left_Offset = new Vector3(0, 0, -1);
        Right_Offset = new Vector3(0, 0, 1);


        S_Directions = new string[] { "North West", "West", "South West", "South", "South East", "East", "North East", "North" };

        BuildGrid();

        BuildTargetingPoints();
    }

    // Use this for initialization
    void Start()
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
                GameObject gridplane = (GameObject)Instantiate(Grid_Empty, new Vector3(x, 0, z) + Offset, Quaternion.identity);
                gridplane.gameObject.name = "X: " + (x + 1).ToString() + "Z: " + (z + 1).ToString();
                gridplane.gameObject.tag = "Empty Grid";
                gridplane.transform.parent = Grid_Encapsulate.transform;
                Grid[x, z] = gridplane;
                if ((x == 0 || x == i_GridSize_X - 1) || (z == 0 || z == i_GridSize_Y - 1))
                {
                    Grid_Exceptions.Add(gridplane);
                }

            }
        }
        Vector3 Front_Midpoint = Grid[0, 0].transform.position + ((Grid[0, i_GridSize_Y - 1].transform.position - Grid[0, 0].transform.position) / 2);
        GameObject Front_3 = (GameObject)Instantiate(Grid_Barricade_Front);
        Front_3.transform.position = Front_Midpoint + Front_Offset;
        Front_3.transform.parent = Grid_Encapsulate.transform;
        Front_3.gameObject.name = "Barrier_Front";
        Front_3.gameObject.tag = "Barrier";

        Vector3 Back_Midpoint = Grid[i_GridSize_X - 1, 0].transform.position + ((Grid[i_GridSize_X - 1,i_GridSize_Y - 1].transform.position - Grid[i_GridSize_X - 1, 0].transform.position) / 2);
        GameObject Back_3 = (GameObject)Instantiate(Grid_Barricade_Back);
        Back_3.transform.position = Back_Midpoint + Back_Offset;
        Back_3.transform.parent = Grid_Encapsulate.transform;
        Back_3.gameObject.name = "Barrier_Back";
        Back_3.gameObject.tag = "Barrier";

        Vector3 Left_Midpoint = Grid[0, 0].transform.position + ((Grid[i_GridSize_X - 1,0].transform.position - Grid[0, 0].transform.position) / 2);
        GameObject Left_3 = (GameObject)Instantiate(Grid_Barricade_Left);
        Left_3.transform.position = Left_Midpoint + Left_Offset;
        Left_3.transform.parent = Grid_Encapsulate.transform;
        Left_3.gameObject.name = "Barrier_Left";
        Left_3.gameObject.tag = "Barrier";

        Vector3 Right_Midpoint = Grid[0, i_GridSize_Y - 1].transform.position + ((Grid[i_GridSize_X - 1, i_GridSize_Y - 1].transform.position - Grid[0, i_GridSize_Y - 1].transform.position) / 2);
        GameObject Right_3 = (GameObject)Instantiate(Grid_Barricade_Right);
        Right_3.transform.position = Right_Midpoint + Right_Offset;
        Right_3.transform.parent = Grid_Encapsulate.transform;
        Right_3.gameObject.name = "Barrier_Right";
        Right_3.gameObject.tag = "Barrier";

        Grid_Encapsulate.transform.parent = Original.transform;
    }

    void BuildTargetingPoints()
    {
        
        for (int i = 0; i < 8; i++) 
        {
            GameObject point_test = (GameObject)Instantiate(Grid_Empty);
            point_test.transform.parent = Directions.transform;
            Directions.transform.rotation = Quaternion.Euler(0, i * 45, 0);
            point_test.transform.position = new Vector3(30, 0, 0);
            point_test.transform.localRotation = Quaternion.Euler(0, 0, 0) ;
            point_test.gameObject.tag = "DirectionPoint";
            point_test.gameObject.name = S_Directions[i];
            Directions.transform.parent = Original.transform;

        }
    }
    

    public void ClamptoGrid(GameObject go)
    {
        float radius = 100.0f;
        GameObject PointtoClamp = null;
        List<GameObject> NearestGridList = new List<GameObject>();
        for (int x = 0; x < i_GridSize_X; x++)
        {
            for (int z = 0; z < i_GridSize_Y; z++)
            {
                if (x == 0 || z == 0 || x == i_GridSize_X - 1 || z == i_GridSize_Y - 1)
                {
                    Grid_Exceptions.Add(Grid[x, z]);
                }
                else if (Vector3.Distance(Grid[x, z].transform.position, go.transform.position) <= radius)
                {
                    NearestGridList.Add(Grid[x, z]);
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
                //go.gameObject.transform.parent = Original.transform;
                //Grid_Exceptions.Add(NearestGridList[i]);
            }
        }

    }

    public void SetTargetRange(GameObject go)
    {
        //Exception List, kinda
        for (int x = 0; x < i_GridSize_X; x++) 
        {
            for (int z = 0; z < i_GridSize_Y; z++) 
            {
                if (go.transform.position == Grid[x, z].transform.position)
                {
                    if (x == 1)
                    {
                        if ((z == 1) || (z == (i_GridSize_Y - 2)))
                        {
                            if (z == 1)
                            {
                                go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("South East"));
                            }
                            else
                            {
                                go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("South West"));
                            }
                        }
                        else
                        {
                            go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("South"));
                        }
                    }
                    else if (x == i_GridSize_X - 2)
                    {
                        if (z == 1 || z == i_GridSize_Y - 2)
                        {
                            if (z == 1)
                            {
                                go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("North East"));
                            }
                            else if (z == i_GridSize_Y - 2) 
                            {
                                go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("North West"));
                            }
                        }
                        else
                        {
                            go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("North"));
                        }
                    }
                    else if (z == 1 || z == i_GridSize_Y - 2)
                    {
                        if(z == 1)
                        {
                            go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("East"));
                        }
                        else
                        {
                            go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("West"));
                        }
                    }
                    else
                    {
                        if (x < i_GridSize_X / 2 && z < i_GridSize_Y / 2)
                        {
                            go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("North"));
                        }
                        if (x > i_GridSize_X / 2 && z > i_GridSize_Y / 2)
                        {
                            go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("South"));
                        }
                        if (x < i_GridSize_X / 2 && z > i_GridSize_Y / 2)
                        {
                            go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("West"));
                        }
                        if (x > i_GridSize_X / 2 && z < i_GridSize_Y / 2)
                        {
                            go.GetComponent<Survivor>().SetDirectionPoint(GameObject.Find("East"));
                        }
                    }
                }
            }
        }
    }
}
