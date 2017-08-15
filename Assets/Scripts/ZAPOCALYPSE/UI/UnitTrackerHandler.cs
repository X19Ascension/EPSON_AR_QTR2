using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitTrackerHandler : MonoBehaviour {

    Vuforia.DefaultTrackableEventHandler test;
    [SerializeField]
    GameObject viewable;

    public Text text;
	// Use this for initialization
	void Start ()
    {
        test = gameObject.GetComponent<Vuforia.DefaultTrackableEventHandler>();
        viewable = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    //if(test.ReturnCheck())
     //   {
     //       transform.GetChild(0).gameObject.SetActive(true);
     //       Debug.Log("This Works" + " " + viewable.name);
     //       text.text = "This Works" + " " + viewable.name;


     //   }
     //   else
     //   {
     //       transform.GetChild(0).gameObject.SetActive(false);
     //       Debug.Log("This is OFF" + " " + viewable.name);
     //       text.text = "This is OFF" + " " + viewable.name;
     //   }
	}
}
