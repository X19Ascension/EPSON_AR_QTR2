using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour 
{

    public GameObject go;

    DestinationLog destinationLog;

    
    public enum DayNightCycles
    {
        S_DAY = 1,
        S_NIGHT,
    }

    public DayNightCycles DNC;
	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnMouseDown()
    {
        //if (go.active == false)
        //{
        //    go.SetActive(true);
        //}
        //else
        //{
        //    go.SetActive(false);
        //}
    }

    public void OpenMenu()
    {
        go.SetActive(true);
    }

    public void ExitMenu()
    {
        go.SetActive(false);
    }
}
