using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MarkerCheck : MonoBehaviour {

    List<GameObject> CheckObjects;

	// Use this for initialization
	void Start () {

        CheckObjects = new List<GameObject>();
        if (SceneManager.GetActiveScene().name.Equals("LevelSelect"))
        {
            CheckObjects = GameObject.FindGameObjectsWithTag("ImageTarget").ToList<GameObject>();
        }
        else
        {
            CheckObjects.Add(GameObject.FindGameObjectWithTag("ImageTarget"));
        }
    }
	
	// Update is called once per frame
	void Update () {

        bool ObjectsAreDisabled = true;

        foreach (GameObject go in CheckObjects)
        {
            if (go.GetComponentInChildren<Renderer>().enabled)
            {
                ObjectsAreDisabled = false;
                break;
            }
        }

        if (ObjectsAreDisabled)
        {
            this.gameObject.GetComponent<Image>().enabled = true;
            this.gameObject.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<Image>().enabled = false;
            this.gameObject.GetComponentInChildren<Text>().enabled = false;
        }
    }
}
