using UnityEngine;
using System.Collections;

public class CityManager : MonoBehaviour {

    [Tooltip("Time taken before one 'tick' occurs (in secs)")]
    public double TickTime;

    private int i_DataProductionRate;
    private int i_CreditsProductionRate;
    private int i_FoodProductionRate;
    private int i_PopulationProductionRate;
    private int i_PowerProductionRate;

    private int i_PowerConsumptionRate;
    private int i_FoodConsumptionRate;

    private double d_Timer;
    private int i_DataCap;
    private int i_CreditsCap;
    private int i_FoodCap;
    private int i_PopulationCount;
    private int i_PopulationCap;
    private int i_PowerCap;

    public bool GeneratorBuilt;
    public bool FarmBuilt;
    public bool HousingBuilt;
    public bool DEGeneratorBuilt;

    // Use this for initialization
    void Start () {
        d_Timer = 0.0;
        i_PopulationCount = 0;
        i_PopulationCap = 0;
    }
	
	// Update is called once per frame
	void Update () {

        d_Timer += Time.deltaTime;

        if (d_Timer > TickTime)
        {
            GameObject[] AllBuildings = GameObject.FindGameObjectsWithTag("Buildings");

            int PowerConsumingCount = 0;

            foreach (GameObject go in AllBuildings)
            {
                if (go.GetComponent<BuildingAbstractBase>().IsPreview)
                    continue;

                go.GetComponent<BuildingAbstractBase>().ProduceResource();

                // Calculate Population Count and Capacity
                if (go.GetComponent<BuildingAbstractBase>().GetIsResidential())
                {
                    i_PopulationCount += go.GetComponent<BuildingAbstractBase>().GetPopulationCount();
                    i_PopulationCap += go.GetComponent<BuildingAbstractBase>().GetPopulationCap();
                }

                // Calculate Storage values
                if (go.GetComponent<BuildingAbstractBase>().GetIsStorage())
                {
                    i_CreditsCap += go.GetComponent<BuildingAbstractBase>().GetCreditsStorageCap();
                    i_DataCap += go.GetComponent<BuildingAbstractBase>().GetDataStorageCap();
                    i_PowerCap += go.GetComponent<BuildingAbstractBase>().GetPowerStorageCap();
                    i_FoodCap += go.GetComponent<BuildingAbstractBase>().GetFoodStorageCap();
                }

                if (go.name != "TownHall" && go.name != "Generator" && go.name != "Storage")
                    PowerConsumingCount++;
            }

            // Calculate Consumption
            i_PowerConsumptionRate = (PowerConsumingCount / 4);
            i_FoodConsumptionRate = (i_PopulationCount / 4);

            // Assign values to PersistentData
            PersistentData.m_Instance.PopulationAmount = i_PopulationCount;
            PersistentData.m_Instance.CreditsAmount = Mathf.Min(PersistentData.m_Instance.CreditsAmount, i_CreditsCap);
            PersistentData.m_Instance.DataAmount = Mathf.Min(PersistentData.m_Instance.DataAmount, i_DataCap);
            PersistentData.m_Instance.PowerAmount = Mathf.Min(PersistentData.m_Instance.PowerAmount - i_PowerConsumptionRate, i_PowerCap);
            PersistentData.m_Instance.FoodAmount = Mathf.Min(PersistentData.m_Instance.FoodAmount - i_FoodConsumptionRate, i_FoodCap);

            PersistentData.m_Instance.CreditsCap = i_CreditsCap;
            PersistentData.m_Instance.DataCap = i_DataCap;
            PersistentData.m_Instance.PowerCap = i_PowerCap;
            PersistentData.m_Instance.FoodCap = i_FoodCap;
            PersistentData.m_Instance.PopulationCap = i_PopulationCap;

            i_CreditsCap = 0;
            i_DataCap = 0;
            i_PowerCap = 0;
            i_FoodCap = 0;
            i_PopulationCap = 0;
            i_PopulationCount = 0;

            d_Timer = 0.0;
        }

        if (Input.GetKeyUp("h"))
        {
            PersistentData.m_Instance.CreditsAmount = PersistentData.m_Instance.CreditsCap;
            PersistentData.m_Instance.DataAmount = PersistentData.m_Instance.DataCap;
            PersistentData.m_Instance.PowerAmount = PersistentData.m_Instance.PowerCap;
            PersistentData.m_Instance.FoodAmount = PersistentData.m_Instance.FoodCap;
            PersistentData.m_Instance.PopulationAmount = PersistentData.m_Instance.PopulationCap;
        }
	}

    public int GetDataCap() { return i_DataCap; }
    public int GetCreditsCap() { return i_CreditsCap; }
    public int GetFoodCap() { return i_FoodCap; }
    public int GetPopCap() { return i_PopulationCap; }
    public int GetPopCount() { return i_PopulationCount; }
    public int GetPowerCap() { return i_PowerCap; }

    public void AddDataProduction(int DataGain) { i_DataProductionRate += DataGain; }
    public void AddCreditsProduction(int CreditsGain) { i_CreditsProductionRate += CreditsGain; }
    public void AddFoodProduction(int FoodGain) { i_FoodProductionRate += FoodGain; }
    public void AddPopulationProduction(int PopulationGain) { i_PopulationProductionRate += PopulationGain; }
    public void AddPowerProduction(int PowerGain) { i_PowerProductionRate += PowerGain; }

    public int GetDataProductionRate() { return i_DataProductionRate; }
    public int GetCreditsProductionRate() { return i_CreditsProductionRate; }
    public int GetFoodProductionRate() { return i_FoodProductionRate - i_FoodConsumptionRate; }
    public int GetPopulationProductionRate() { return i_PopulationProductionRate; }
    public int GetPowerProductionRate() { return i_PowerProductionRate - i_PowerConsumptionRate; }

    public void SetDataProductionRate(int i) { i_DataProductionRate = i; }
    public void SetCreditsProductionRate(int i) { i_CreditsProductionRate = i; }
    public void SetFoodProductionRate(int i) { i_FoodProductionRate = i; }
    public void SetPopulationProductionRate(int i) { i_PopulationProductionRate = i; }
    public void SetPowerProductionRate(int i) { i_PowerProductionRate = i; }

}
