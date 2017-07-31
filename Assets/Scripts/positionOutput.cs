using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class positionOutput : MonoBehaviour {

    public Text debugText;

    public GameObject marker;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        float Distance = Vector3.Distance(this.gameObject.transform.position, marker.gameObject.transform.position);
        debugText.text = "Dist: " + Distance;
        //Debug.Log(this.gameObject.transform.position);

    }
}
