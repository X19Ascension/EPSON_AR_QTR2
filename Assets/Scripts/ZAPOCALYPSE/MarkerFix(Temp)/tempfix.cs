using UnityEngine;
using System.Collections;

public class tempfix : MonoBehaviour {
    public GameObject Origin;

    GridMap thegrid;
	// Use this for initialization
	void Start () {
        thegrid = GameObject.Find("Game Manager").GetComponent<GridMap>();
        this.transform.parent = Origin.transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //thegrid.ClamptoGrid(this.gameObject);
       //this.transform.position = Origin.transform.position ;
       //transform.localRotation = Origin.transform.rotation;
	
	}
}
