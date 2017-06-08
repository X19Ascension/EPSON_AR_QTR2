using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class UIDisplay : MonoBehaviour {

    public Text Data;
    public Text Credits;
    public Text Food;
    public Text Population;
    public Text Power;

    GameObject CityManager;

	// Use this for initialization
	void Start () {
        CityManager = GameObject.Find("TownHall");
	}
	
	// Update is called once per frame
	void Update () {
        if(CityManager.GetComponent<CityManager>().GetDataProductionRate() > 0)
            Data.text = " " + PersistentData.m_Instance.DataAmount.ToString() + 
                " / " + PersistentData.m_Instance.DataCap.ToString() + 
                " (+" + CityManager.GetComponent<CityManager>().GetDataProductionRate().ToString() + ")";
        else
            Data.text = " " + PersistentData.m_Instance.DataAmount.ToString() +
                " / " + PersistentData.m_Instance.DataCap.ToString() +
                " (" + CityManager.GetComponent<CityManager>().GetDataProductionRate().ToString() + ")";

        if(CityManager.GetComponent<CityManager>().GetCreditsProductionRate() > 0)
            Credits.text = " " + PersistentData.m_Instance.CreditsAmount.ToString() + 
                " / " + PersistentData.m_Instance.CreditsCap.ToString() + 
                " (+" + CityManager.GetComponent<CityManager>().GetCreditsProductionRate().ToString() + ")";
        else
            Credits.text = " " + PersistentData.m_Instance.CreditsAmount.ToString() +
                " / " + PersistentData.m_Instance.CreditsCap.ToString() +
                " (" + CityManager.GetComponent<CityManager>().GetCreditsProductionRate().ToString() + ")";

        if(CityManager.GetComponent<CityManager>().GetFoodProductionRate() > 0)
            Food.text = " " + PersistentData.m_Instance.FoodAmount.ToString() + 
                " / " + PersistentData.m_Instance.FoodCap.ToString() +
                " (+" + CityManager.GetComponent<CityManager>().GetFoodProductionRate().ToString() + ")";
        else
            Food.text = " " + PersistentData.m_Instance.FoodAmount.ToString() +
                " / " + PersistentData.m_Instance.FoodCap.ToString() +
                " (" + CityManager.GetComponent<CityManager>().GetFoodProductionRate().ToString() + ")";

        if(CityManager.GetComponent<CityManager>().GetPopulationProductionRate() > 0)
            Population.text = " " + PersistentData.m_Instance.PopulationAmount.ToString() + 
                " / " + PersistentData.m_Instance.PopulationCap.ToString() +
                " (+" + CityManager.GetComponent<CityManager>().GetPopulationProductionRate().ToString() + ")";
        else
            Population.text = " " + PersistentData.m_Instance.PopulationAmount.ToString() +
                " / " + PersistentData.m_Instance.PopulationCap.ToString() +
                " (" + CityManager.GetComponent<CityManager>().GetPopulationProductionRate().ToString() + ")";

        if(CityManager.GetComponent<CityManager>().GetPowerProductionRate() > 0)
            Power.text = " " + PersistentData.m_Instance.PowerAmount.ToString() + 
                " / " + PersistentData.m_Instance.PowerCap.ToString() +
                " (+" + CityManager.GetComponent<CityManager>().GetPowerProductionRate().ToString() + ")";
        else
            Power.text = " " + PersistentData.m_Instance.PowerAmount.ToString() +
                " / " + PersistentData.m_Instance.PowerCap.ToString() +
                " (" + CityManager.GetComponent<CityManager>().GetPowerProductionRate().ToString() + ")";
    }
}
