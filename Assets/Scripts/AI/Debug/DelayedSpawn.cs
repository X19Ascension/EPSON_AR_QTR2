using UnityEngine;
using System.Collections;

public class DelayedSpawn : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if (gameObject.activeInHierarchy)
        {
            gameObject.GetComponent<FSMBase>().enabled = false;
            gameObject.GetComponent<Pathfinder>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.O))
        {
            gameObject.GetComponent<FSMBase>().enabled = true;
            gameObject.GetComponent<Pathfinder>().enabled = true;
        }

	}
}
