using UnityEngine;
using System.Collections;
using System;

public class Storage : BuildingAbstractBase {

    [Header("Storage Values")]
    public int DataStorageCap;
    public int CreditsStorageCap;
    public int PowerStorageCap;
    public int FoodStorageCap;

    // Use this for initialization
    public override void Start()
    { 
        b_IsStorage = true;

        PersistentData.m_Instance.DataCap += DataStorageCap;
        PersistentData.m_Instance.CreditsCap += CreditsStorageCap;
        PersistentData.m_Instance.PowerCap += PowerStorageCap;
        PersistentData.m_Instance.FoodCap += FoodStorageCap;

        base.Start();
    }

    void OnDisable()
    {
        if (!IsPreview)
            base.Shutdown();

        if (GetComponent<Health>().HP <= 0)
        {
            PersistentData.m_Instance.DataCap -= DataStorageCap;
            PersistentData.m_Instance.CreditsCap -= CreditsStorageCap;
            PersistentData.m_Instance.PowerCap -= PowerStorageCap;
            PersistentData.m_Instance.FoodCap -= FoodStorageCap;
        }
    }

    public override void ProduceResource()
    {
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
        PersistentData.m_Instance.DataCap += DataStorageCap;
        PersistentData.m_Instance.CreditsCap += CreditsStorageCap;
        PersistentData.m_Instance.PowerCap += PowerStorageCap;
        PersistentData.m_Instance.FoodCap += FoodStorageCap;

        this.gameObject.GetComponent<Health>().Reset();

    }
}
