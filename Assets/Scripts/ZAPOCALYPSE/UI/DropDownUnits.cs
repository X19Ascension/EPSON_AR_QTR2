using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class DropDownUnits : MonoBehaviour {

    enum AvailableUnits
    {
        Shotgun_1 = 1,
        Gunner_1,
        Melee_1,
        Mechanic_1,
    }
    bool[] ChosenUnits;
    Dictionary<AvailableUnits, bool> PopulationList;
    
    
    [SerializeField]
    Dropdown dropdown;

	// Use this for initialization
	void Start () 
    {
        PopulateList();
        PopulationList[AvailableUnits.Shotgun_1] = false;
        PopulationList[AvailableUnits.Gunner_1] = false;
        PopulationList[AvailableUnits.Melee_1] = false;
        PopulationList[AvailableUnits.Mechanic_1] = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void PopulateList()
    {
        string[] Enumnames = Enum.GetNames(typeof (AvailableUnits));
        List<string> names = new List<string>(Enumnames);
        dropdown.AddOptions(names);
    }

    public void PopulationIndexChanged(int index)
    {
        AvailableUnits AU = (AvailableUnits)index;
        PopulationList[AU] = true;
        dropdown.ClearOptions();
        string[] tempnames = new string[4];
        for (int i = 0; i < PopulationList.Count; i++) 
        {
            AvailableUnits AU_ = (AvailableUnits)index;
            if(PopulationList[AU_] == false)
            {
                tempnames.SetValue(AU_.ToString(), i);
            }
        }
        List<string> names = new List<string>(tempnames);
        dropdown.AddOptions(names);
    }
   
}
