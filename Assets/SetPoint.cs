using UnityEngine;
using System.Collections;

public class SetPoint : MonoBehaviour {



    public GameObject Origin;
    public GameObject Floor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetallPoints()
    {
        Floor.transform.parent = Origin.transform;
        if(Floor.transform.childCount != 0)
        {
            for (int i = 1; i < Floor.transform.childCount; i++)
            {
                Floor.transform.GetChild(i).transform.parent = Floor.transform;
            }
        }
        else
        {
        }
    }
}
