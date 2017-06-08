using UnityEngine;
using System.Collections;

public class DebugDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(1))
        { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.Equals(this.gameObject))
                {
                    this.GetComponent<Health>().HP = -1;
                }
            }
        }

    }
}
