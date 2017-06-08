using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class UISwitchOff : MonoBehaviour {

    public List<GameObject> ListToSwitchOff; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SwitchOff()
    {
        foreach (GameObject go in ListToSwitchOff)
        {
            if (go)
                go.SetActive(false);
        }
    }

    public void SwitchOn()
    {
        foreach (GameObject go in ListToSwitchOff)
        {
            if (go)
                go.SetActive(true);
        }
    }
}
