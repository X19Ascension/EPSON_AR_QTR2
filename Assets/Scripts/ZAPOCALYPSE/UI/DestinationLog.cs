using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestinationLog : MonoBehaviour 
{
    private List<string> EventLog = new List<string>();
    private string guiText = "";

    public int maxlines = 10;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void AddEvent(string eventString)
    {
        EventLog.Add(eventString);

        if(EventLog.Count >= maxlines)
        {
            EventLog.RemoveAt(0);
        }
    }
}


