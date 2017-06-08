using UnityEngine;
using System.Collections;
using System;

public class Residential : BuildingAbstractBase {

    public BuildingAddOn theAddOn;

    [Header("Population Values")]
    public int CurrentPop;
    public int PopulationGain;
    public int PopulationCap;

    private bool b_PopulationFull;
    private bool b_PopulationGrowing;

    // Use this for initialization
    public override void Start()
    {
        if (!IsPreview)
            GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(PopulationGain);
        theAddOn = null;
        b_IsResidential = true;

        base.Start();
    }

    void OnDisable()
    {
        if (!IsPreview)
        {
            if(b_PopulationFull != true)
                GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(-PopulationGain);
            base.Shutdown();
        }
    }

    public override void ProduceResource()
    {
        if (PersistentData.m_Instance.FoodAmount <= 0)
            CurrentPop = Mathf.Min(--CurrentPop, PopulationCap);
        else
        {
            if(b_PopulationGrowing == false)
            CurrentPop = Mathf.Min(++CurrentPop, PopulationCap);
        }

        if (CurrentPop == PopulationCap && b_PopulationFull == false)
        {
            GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(-1);
            b_PopulationFull = true;
            Debug.Log("CurrentPop : " + CurrentPop);
        }

        if (CurrentPop < PopulationCap && b_PopulationFull)
        {
            GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(1);
            b_PopulationFull = false;
        }

        if (GameObject.Find("TownHall").GetComponent<CityManager>().GetFoodProductionRate() < 0 && PersistentData.m_Instance.FoodCap * 0.25 == PersistentData.m_Instance.FoodAmount)
        {
            GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(-1);
            b_PopulationGrowing = true;
        }

        if (theAddOn != null)
            theAddOn.ProduceResource();
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
        return 0;
    }

    public override int GetDataStorageCap()
    {
        return 0;
    }

    public override int GetPowerStorageCap()
    {
        return 0;
    }

    public override int GetFoodStorageCap()
    {
        return 0;
    }

    public override void Reset()
    {
        GameObject.Find("TownHall").GetComponent<CityManager>().AddPopulationProduction(1);
        theAddOn = null;

        this.gameObject.GetComponent<Health>().Reset();
        CurrentPop = 0;

    }
}
