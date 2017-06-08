using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class EventOutcome : MonoBehaviour {

    public enum OUTCOME_TYPE
    {
        NO_OUTCOME,
        AFFECT_RESOURCE,
        DESTROY_BUILDING,
    }

    public enum RESOURCE
    {
        NONE,
        DATA,
        CREDIT,
        POWER,
        FOOD,
        POPULATION,
    }

    public OUTCOME_TYPE OutcomeType;
    public RESOURCE AffectedResource;

    [Header("")]
    public float TriggerChance = 100;
    public int Amount;
    public GameObject TargetObject;

    public string Description;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DoOutcome()
    {
        GameObject[] BuildingList;

        switch (OutcomeType)
        {
            case OUTCOME_TYPE.AFFECT_RESOURCE:

                switch (AffectedResource)
                {
                    case RESOURCE.CREDIT: PersistentData.m_Instance.CreditsAmount += Amount; break;
                    case RESOURCE.DATA: PersistentData.m_Instance.DataAmount += Amount; break;
                    case RESOURCE.FOOD: PersistentData.m_Instance.FoodAmount += Amount; break;
                    case RESOURCE.POWER: PersistentData.m_Instance.PowerAmount += Amount; break;
                    case RESOURCE.POPULATION:

                        BuildingList = GameObject.FindGameObjectsWithTag("Buildings");
                        List<GameObject> ResidentialList = new List<GameObject>();

                        foreach (GameObject go in BuildingList)
                        {
                            if (!go.GetComponent<BuildingAbstractBase>().GetIsResidential())
                                continue;

                            ResidentialList.Add(go);
                        }

                        int rand = Random.Range(0, ResidentialList.Count - 1);

                        // Special Case for TownHall
                        if (ResidentialList[rand].name.Contains("TownHall"))
                            ResidentialList[rand].GetComponent<TownHall>().CurrentPop += Amount;
                        else
                            ResidentialList[rand].GetComponent<Residential>().CurrentPop += Amount;

                        break;
                }
                break;

            case OUTCOME_TYPE.DESTROY_BUILDING:

                BuildingList = GameObject.FindGameObjectsWithTag("Buildings");
                List<GameObject> TargetList = new List<GameObject>();

                foreach (GameObject go in BuildingList)
                {
                    if (!go.name.Contains(TargetObject.name))
                        continue;

                    TargetList.Add(go);
                }

                if (TargetList.Count == 0)
                    break;

                while (TargetList.Count > 0 && Amount > 0)
                {
                    int rand = Random.Range(0, TargetList.Count - 1);
                    TargetList[rand].GetComponent<Health>().HP = 0;
                    TargetList.RemoveAt(rand);

                    Amount--;
                }

                break;
        }
    }
}
