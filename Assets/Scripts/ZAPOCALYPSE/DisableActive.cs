using UnityEngine;
using System.Collections;

public class DisableActive : MonoBehaviour {

    public GameObject[] objectsToDisable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void DisableObjects()
    {
        foreach (GameObject pew in objectsToDisable)
        {
            if (pew.activeSelf)
                pew.SetActive(false);
        }
    }
}
