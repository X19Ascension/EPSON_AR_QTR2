using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnUnitTest : MonoBehaviour {

    [SerializeField]
    GridMap The_Grid;

    public GameObject test_unit;
    List<GameObject> Unit_List;
    void Awake()
    {
        Unit_List = new List<GameObject>();
        GameObject unit = (GameObject)Instantiate(test_unit, new Vector3(Random.Range(0, 0.5f), 0, Random.Range(0, 0.5f)),Quaternion.identity);
        //initialises the units, all of them
        //Temporary add testunit
        Unit_List.Add(unit);
    }

	// Use this for initialization
	void Start ()
    {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i <= Unit_List.Count; i++)
        {
            The_Grid.ClamptoGrid(Unit_List[i]);
        }
    }
}
