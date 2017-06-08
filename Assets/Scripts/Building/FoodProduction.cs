using UnityEngine;
using System.Collections;
using System;

public class FoodProduction : BuildingAbstractBase
{
    [Header("Production Values")]
    public int FoodGain;

    [Header("Storage Values")]
    public int FoodStorageCap;

    // Use this for initialization
    public override void Start()
    {
        if (!IsPreview)
            GameObject.Find("TownHall").GetComponent<CityManager>().AddFoodProduction(FoodGain);
        b_IsProduction = true;

        base.Start();
    }

    void OnDisable()
    {
        if (!IsPreview)
        {
            GameObject.Find("TownHall").GetComponent<CityManager>().AddFoodProduction(-FoodGain);
            base.Shutdown();
        }
    }

    public override void ProduceResource()
    {
        if (PersistentData.m_Instance.PowerAmount > 0)
            PersistentData.m_Instance.FoodAmount += FoodGain;
        if (PersistentData.m_Instance.FoodAmount < 0)
            PersistentData.m_Instance.FoodAmount = 0;
    }

    public override int GetPopulationCount()
    {
        return 0;
    }

    public override int GetPopulationCap()
    {
        return 0;
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
        return FoodStorageCap;
    }

    public override void Reset()
    {
        GameObject.Find("TownHall").GetComponent<CityManager>().AddFoodProduction(FoodGain);
        this.gameObject.GetComponent<Health>().Reset();
    }
}
