using UnityEngine;
using System.Collections;

public class HaxButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        PersistentData.m_Instance.CreditsAmount = PersistentData.m_Instance.CreditsCap;
        PersistentData.m_Instance.DataAmount = PersistentData.m_Instance.DataCap;
        PersistentData.m_Instance.PowerAmount = PersistentData.m_Instance.PowerCap;
        PersistentData.m_Instance.FoodAmount = PersistentData.m_Instance.FoodCap;
        PersistentData.m_Instance.PopulationAmount = PersistentData.m_Instance.PopulationCap;

        GameObject.Find("TownHall").GetComponent<TownHall>().CurrentPop = PersistentData.m_Instance.PopulationCap;
    }
}
