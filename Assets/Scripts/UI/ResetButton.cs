using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

    public ReadFromCSV CSVReader;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        // Clear data
        // Load whatever numbers here
        PersistentData.m_Instance.DataAmount = 0;
        PersistentData.m_Instance.CreditsAmount = 0;
        PersistentData.m_Instance.FoodAmount = 0;
        PersistentData.m_Instance.PopulationAmount = 0;
        PersistentData.m_Instance.PowerAmount = 0;

        GameObject.Find("TownHall").GetComponent<TownHall>().CurrentPop = 0;

        PersistentData.m_Instance.BuildingGridPos.Clear();
        PersistentData.m_Instance.BuildingName.Clear();

        GameObject.Find("MilestoneManager").GetComponent<MilestoneManager>().Reset();

        PersistentData.m_Instance.MilestoneIndex = 0;
        PersistentData.m_Instance.MilestoneProgress = 0;

        //Regenerate Grid
        GameObject.Find("GridManager").GetComponent<GridBehavior>().DeleteBuildings();
        GameObject.Find("GridManager").GetComponent<GridBehavior>().GenerateMap(false);

        //Check against CSV to place obstacles
        for (int i = 0; i < 11; i++)
        {
            //Since it reads bottom-up, should check descending from 11
            for (int j = 0; j < 11; j++)
            {
                GameObject[] obs = GameObject.FindGameObjectsWithTag("Grid");
                for (int k = 0; k < obs.Length; k++)
                {
                    if (CSVReader.loadedMap[i, j] != 0 && GameObject.Find("GridManager").GetComponent<GridBehavior>().GetGridPos(obs[k].transform.position) == new Vector2(i, j))
                    {
                            Destroy(obs[k]);
                    }
                }
            }
        }
    }
}
