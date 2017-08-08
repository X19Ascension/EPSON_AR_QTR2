using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnUnitTest : MonoBehaviour {

    public List<GameObject> survivorType = new List<GameObject>();     //! The number of Spawn Points in the Game

    [SerializeField]
    GridMap The_Grid;

    List<GameObject> Unit_List;
    void Awake()
    {
        float test = 1.5f;
        Unit_List = new List<GameObject>();
        GameObject unit = (GameObject)Instantiate(survivorType[0], new Vector3(1.5f, 0, 1.5f),Quaternion.identity);

        //initialises the units, all of them
        //Temporary add testunit
        Unit_List.Add(unit);
        unit = (GameObject)Instantiate(survivorType[1], new Vector3(-0.5f, 0, 1.5f), Quaternion.identity);
        Unit_List.Add(unit);
        unit = (GameObject)Instantiate(survivorType[2], new Vector3(0.5f, 0, 1.5f), Quaternion.identity); 
        Unit_List.Add(unit);
    }

	// Use this for initialization
	void Start ()
    {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < Unit_List.Count; i++)
        {
            The_Grid.ClamptoGrid(Unit_List[i]);
            The_Grid.SetTargetRange(Unit_List[i]);
        }
    }
}
