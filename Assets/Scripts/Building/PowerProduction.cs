using UnityEngine;
using System.Collections;
using System;

public class PowerProduction : BuildingAbstractBase {

    [Header("Production Values")]
    public int PowerGain;

    [Header("Storage Values")]
    public int PowerStorageCap;

    // Use this for initialization
    public override void Start()
    {
        if (!IsPreview)
            GameObject.Find("TownHall").GetComponent<CityManager>().AddPowerProduction(PowerGain);

        b_IsProduction = true;

        base.Start();
    }

    void OnDisable()
    {
        if (!IsPreview)
        {
            GameObject.Find("TownHall").GetComponent<CityManager>().AddPowerProduction(-PowerGain);
            base.Shutdown();
        }
    }

    public override void ProduceResource()
    {
        PersistentData.m_Instance.PowerAmount += PowerGain;
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
        return PowerStorageCap;
    }

    public override int GetFoodStorageCap()
    {
        return 0;
    }

    public override void Reset()
    {
        GameObject.Find("TownHall").GetComponent<CityManager>().AddPowerProduction(PowerGain);
        this.gameObject.GetComponent<Health>().Reset();
    }
}
