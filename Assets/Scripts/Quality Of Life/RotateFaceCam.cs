using UnityEngine;
using System.Collections;

public class RotateFaceCam : MonoBehaviour {

    public GameObject ObjectToRotate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ObjectToRotate.transform.LookAt(Camera.main.transform.position);
        ObjectToRotate.transform.Rotate(new Vector3(0, 180, 0));
    }
}
