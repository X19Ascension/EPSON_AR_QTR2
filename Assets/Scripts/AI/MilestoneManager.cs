using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;

public class MilestoneManager : MonoBehaviour {

    [Tooltip("Size of both list must match")]
    [Header("Milestone List")]
    public List<float> EconomicMilestone = new List<float>();
    public List<float> DefensiveMilestone = new List<float>();
    public List<float> PopulationMilestone = new List<float>();

    [Header("")]
    public float BaseProgressIncrease;

    [Header("Object References")]
    public InvasionManager theInvasionManager;
    public Slider theMilestoneBar;
    public Text theMilestoneNumber;

    int i_CurrentMilestoneIdx = 0;

    float f_EconomicValue;
    float f_DefensiveValue;
    float f_PopulationValue;
    float f_ProgressValue;

    double d_Timer = 0.0;
    bool b_InitialLoad = false;

    // Use this for initialization
    void Start () {
        theMilestoneNumber.text = "1";
    }

    // Update is called once per frame
    void Update()
    {
        if (!b_InitialLoad && PersistentData.m_Instance.InitialLoad && !PersistentData.m_Instance.LoadFailed)
        { 
            i_CurrentMilestoneIdx = PersistentData.m_Instance.MilestoneIndex;
            f_ProgressValue = PersistentData.m_Instance.MilestoneProgress;

            theMilestoneNumber.text = (i_CurrentMilestoneIdx + 1).ToString();

            b_InitialLoad = true;
        }

        if (d_Timer > 0.1)
        {
            CalculateValue();
            CheckValues();

            d_Timer = 0.0;
        }
        else
        {
            d_Timer += Time.deltaTime;
        }

        if (theMilestoneBar.value < f_ProgressValue)
        {
            theMilestoneBar.value += Time.deltaTime * 50;
        }
        else if (theMilestoneBar.value > f_ProgressValue + theMilestoneBar.maxValue * 0.05)
        {
            theMilestoneBar.value -= Time.deltaTime * 50;
        }
    }

    void CalculateValue()
    {
        f_EconomicValue = 0;
        f_DefensiveValue = 0;

        GameObject[] AllBuildings = GameObject.FindGameObjectsWithTag("Buildings");
        GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");

        foreach (GameObject go in AllBuildings)
        {
            if (go.GetComponent<BuildingAbstractBase>().IsPreview || !go.activeSelf)
                continue;

            f_EconomicValue += go.GetComponent<BuildingAbstractBase>().EconomicValue;
            f_DefensiveValue += go.GetComponent<BuildingAbstractBase>().DefensiveValue;
        }

        foreach (GameObject go in AllTowers)
        {
            if (go.GetComponent<BaseTurret>().IsPreview || !go.activeSelf)
                continue;

            if (!go.GetComponent<BaseTurret>())
                continue;

            f_EconomicValue += go.GetComponent<BaseTurret>().EconomicValue;
            f_DefensiveValue += go.GetComponent<BaseTurret>().DefensiveValue;
        }

        f_PopulationValue = PersistentData.m_Instance.PopulationAmount;

        // Update UI
        // Calculate percentage
        if (i_CurrentMilestoneIdx < EconomicMilestone.Count)
        {
            float EconomicPercentage = ((f_EconomicValue - GetPrevEconomicValue()) / EconomicMilestone[i_CurrentMilestoneIdx]) * 100;
            float DefensivePercentage = ((f_DefensiveValue - GetPrevDefensiveValue()) / DefensiveMilestone[i_CurrentMilestoneIdx]) * 100;
            float PopulationPercentage = ((f_PopulationValue - GetPrevPopulationValue()) / PopulationMilestone[i_CurrentMilestoneIdx]) * 100;

            f_ProgressValue = (Mathf.Min(EconomicPercentage, 100) + Mathf.Min(DefensivePercentage, 100) + Mathf.Min(PopulationPercentage, 100)) / 3;
        }
        else
        {
            f_ProgressValue += BaseProgressIncrease;
        }
    }

    void CheckValues()
    {
        if (i_CurrentMilestoneIdx < EconomicMilestone.Count)
        {
            if (f_EconomicValue >= EconomicMilestone[i_CurrentMilestoneIdx] && f_DefensiveValue >= DefensiveMilestone[i_CurrentMilestoneIdx] && f_PopulationValue >= PopulationMilestone[i_CurrentMilestoneIdx])
            {
                theInvasionManager.StartAttack();
                theMilestoneBar.value = 0;
                theMilestoneNumber.text = (i_CurrentMilestoneIdx + 2).ToString();
                ++i_CurrentMilestoneIdx;
            }
        }
        else
        {
            if (theMilestoneBar.value >= theMilestoneBar.maxValue)
            {
                theInvasionManager.StartAttack();
                theMilestoneBar.value = 0;
                f_ProgressValue = 0;
                theMilestoneNumber.text = (i_CurrentMilestoneIdx + 2).ToString();
                ++i_CurrentMilestoneIdx;
            }
        }

        // Save to PersistentData
        PersistentData.m_Instance.MilestoneIndex = i_CurrentMilestoneIdx;
        PersistentData.m_Instance.MilestoneProgress = f_ProgressValue;
    }

    float GetPrevEconomicValue()
    {
        if (i_CurrentMilestoneIdx == 0)
            return 0;

        return EconomicMilestone[i_CurrentMilestoneIdx - 1];
    }

    float GetPrevDefensiveValue()
    {
        if (i_CurrentMilestoneIdx == 0)
            return 0;

        return DefensiveMilestone[i_CurrentMilestoneIdx - 1];
    }

    float GetPrevPopulationValue()
    {
        if (i_CurrentMilestoneIdx == 0)
            return 0;

        return PopulationMilestone[i_CurrentMilestoneIdx - 1];
    }

    public bool IsMilestonesCompleted()
    {
        return (i_CurrentMilestoneIdx >= EconomicMilestone.Count);
    }

    public void IncreaseBarValue(float amount)
    {
        this.theMilestoneBar.value += amount;
    }

    public float GetEconomicValue()
    {
        return f_EconomicValue;
    }

    public float GetDefensiveValue()
    {
        return f_DefensiveValue;
    }

    public float GetPopulationValue()
    {
        return f_PopulationValue;
    }

    public float GetEconomicMax()
    {
        return EconomicMilestone[i_CurrentMilestoneIdx];
    }

    public float GetDefensiveMax()
    {
        return DefensiveMilestone[i_CurrentMilestoneIdx];
    }

    public float GetPopulationMax()
    {
        return PopulationMilestone[i_CurrentMilestoneIdx];
    }

    public void Reset()
    {
        i_CurrentMilestoneIdx = 0;
        f_ProgressValue = 0;

        theMilestoneNumber.text = "1";
    }
}
