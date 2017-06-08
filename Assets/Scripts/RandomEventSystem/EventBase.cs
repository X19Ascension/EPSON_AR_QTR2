using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class EventBase : MonoBehaviour {

    public enum TRIGGER_TYPE
    {
        BUILD_GENERATOR,
        BUILD_DATA,
        BUILD_CREDIT,
        BUILD_FARM,
        BUILD_HOUSING,
        BUILD_STORAGE,
        BUILD_TURRET,
    }

    public TRIGGER_TYPE Trigger;
    public float TriggerChance;
    public string Title;
    public string Description;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
