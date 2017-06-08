using UnityEngine;
using System.Collections;
using System;

public class TownHall : BuildingAbstractBase {

    [Header("Production Values")]
    public int DataGain;
    public int CreditsGain;
    public int PowerGain;
    public int FoodGain;

    [Header("Storage Values")]
    public int DataStorageCap;
    public int CreditsStorageCap;
    public int PowerStorageCap;
    public int FoodStorageCap;

    [Header("Population Values")]
    public int CurrentPop;
    public int PopulationCap;

    private bool b_PopulationFull;

	// Use this for initialization
	public override void Start () {

        GameObject.Find("TownHall").GetComponent<CityManager>().AddDataProduction(DataGain);
        GameObject.Find("TownHall").GetComponent<CityManager>().AddCreditsProduction(CreditsGain);
        GameObject.Find("TownHall").GetComponent<CityManager>().AddFoodProduction(FoodGain);
        GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(1);
        GameObject.Find("TownHall").GetComponent<CityManager>().AddPowerProduction(PowerGain);

        b_IsProduction = true;
        b_IsResidential = true;
        b_IsStorage = true;

        b_PopulationFull = false;

        IsPreview = false;

        base.Start();
    }

    public override void ProduceResource()
    {
        if (PersistentData.m_Instance.PowerAmount < 0)
        {
            CreditsGain = 0;
            DataGain = 0;
            FoodGain = 0;
        }
        PersistentData.m_Instance.CreditsAmount += CreditsGain;
        PersistentData.m_Instance.DataAmount += DataGain;
        PersistentData.m_Instance.FoodAmount += FoodGain;
        if (PersistentData.m_Instance.FoodAmount < 0)
            PersistentData.m_Instance.FoodAmount = 0;
        PersistentData.m_Instance.PowerAmount += PowerGain;
        if (PersistentData.m_Instance.PowerAmount < 0)
            PersistentData.m_Instance.PowerAmount = 0;

        CurrentPop = Mathf.Min(++CurrentPop, PopulationCap);
        if (CurrentPop == PopulationCap && b_PopulationFull == false)
        {
            GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(-1);
            b_PopulationFull = true;
            //Debug.Log("CurrentPop : " + CurrentPop);
        }

        if (CurrentPop < PopulationCap && b_PopulationFull)
        {
            GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(1);
            b_PopulationFull = false;
        }
    }

    public override int GetPopulationCount()
    {
        return CurrentPop;
    }

    public override int GetPopulationCap()
    {
        return PopulationCap;
    }

    public override int GetCreditsStorageCap()
    {
        return CreditsStorageCap;
    }

    public override int GetDataStorageCap()
    {
        return DataStorageCap;
    }

    public override int GetPowerStorageCap()
    {
        return PowerStorageCap;
    }

    public override int GetFoodStorageCap()
    {
        return FoodStorageCap;
    }

    public override void Reset()
    {
        GameObject.Find("TownHall").GetComponent<CityManager>().AddDataProduction(DataGain);
        GameObject.Find("TownHall").GetComponent<CityManager>().AddCreditsProduction(CreditsGain);
        GameObject.Find("TownHall").GetComponent<CityManager>().AddFoodProduction(FoodGain);
        GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(1);
        GameObject.Find("TownHall").GetComponent<CityManager>().AddPowerProduction(PowerGain);

        this.gameObject.GetComponent<Health>().Reset();
    }
}
