using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tempfix : MonoBehaviour {
    public GameObject Origin;
    public Text text;
    GridMap thegrid;
	// Use this for initialization
	void Start () {
        thegrid = GameObject.Find("Game Manager").GetComponent<GridMap>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(this.gameObject.GetComponent<Defau);
	
	}
}
