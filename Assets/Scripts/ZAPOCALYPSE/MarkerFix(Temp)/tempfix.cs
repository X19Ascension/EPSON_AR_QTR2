using UnityEngine;
using System.Collections;

public class tempfix : MonoBehaviour {

    GridMap thegrid;
	// Use this for initialization
	void Start () {
        thegrid = GameObject.Find("Game Manager").GetComponent<GridMap>();
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //thegrid.ClamptoGrid(this.gameObject);
	
	}
}
