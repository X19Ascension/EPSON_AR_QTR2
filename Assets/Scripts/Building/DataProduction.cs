using UnityEngine;
using System.Collections;
using System;

public class DataProduction : BuildingAbstractBase {

    [Header("Production Values")]
    public int DataGain;

    [Header("Storage Values")]
    public int DataStorageCap;

    // Use this for initialization
    public override void Start()
    {
        if (!IsPreview)
            GameObject.Find("TownHall").GetComponent<CityManager>().AddDataProduction(DataGain);

        b_IsProduction = true;

        base.Start();
    }

    void OnDisable()
    {
        if (!IsPreview)
        {
            GameObject.Find("TownHall").GetComponent<CityManager>().AddDataProduction(-DataGain);
            base.Shutdown();
        }
    }

    public override void ProduceResource()
    {
        if(PersistentData.m_Instance.PowerAmount > 0)
            PersistentData.m_Instance.DataAmount += DataGain;
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
        return DataStorageCap;
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
        GameObject.Find("TownHall").GetComponent<CityManager>().AddDataProduction(DataGain);
        this.gameObject.GetComponent<Health>().Reset();
    }
}
