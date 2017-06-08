using UnityEngine;
using System.Collections;
using System;

public class CreditsProduction : BuildingAbstractBase {

    [Header("Production Values")]
    public int CreditsGain;

    [Header("Storage Values")]
    public int CreditsStorageCap;

    // Use this for initialization
    public override void Start()
    {
        if (!IsPreview)
            GameObject.Find("TownHall").GetComponent<CityManager>().AddCreditsProduction(CreditsGain);
        b_IsProduction = true;

        base.Start();
    }

    void OnDisable()
    {
        if (!IsPreview)
        {
            GameObject.Find("TownHall").GetComponent<CityManager>().AddCreditsProduction(-CreditsGain);
            base.Shutdown();
        }
    }

    public override void ProduceResource()
    {
        if (PersistentData.m_Instance.PowerAmount > 0)
            PersistentData.m_Instance.CreditsAmount += CreditsGain;
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
        return CreditsStorageCap;
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
        GameObject.Find("TownHall").GetComponent<CityManager>().AddCreditsProduction(CreditsGain);
        this.gameObject.GetComponent<Health>().Reset();
    }
}
