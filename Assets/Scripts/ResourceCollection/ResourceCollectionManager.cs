using UnityEngine;
using System.Collections;

public enum RESOURCE_COLLECTON_STAGE
{
    NOT_COLLECTING,
    SCANNING,
    COLLECTING,
    DONE,
}

public class ResourceCollectionManager : MonoBehaviour {

    public GameObject CollectorText;

    private RESOURCE_COLLECTON_STAGE m_CurrentStage;
    private double d_CollectionTime;                    // Time taken to collect resources, determined by number of resources found.
    private int i_CreditsAmount, i_DataAmount;           // Amount of the resources to be collected, determined by number of resources found, will be sent to the PersistentData once completed. Names subject to change.
    private bool b_DoOnce;
    private string s_OrigText;

	// Use this for initialization
	void Start () {

        m_CurrentStage = RESOURCE_COLLECTON_STAGE.NOT_COLLECTING;
        b_DoOnce = true;

        s_OrigText = CollectorText.GetComponent<TextMesh>().text;

    }
	
	// Update is called once per frame
	void Update () {

        switch (m_CurrentStage)
        {
            // Do Nothing
            case RESOURCE_COLLECTON_STAGE.NOT_COLLECTING:
                return;

            // Ask user to scan various objects 
            case RESOURCE_COLLECTON_STAGE.SCANNING:
                DoScanning();
                break;

            // Collects resource based on the objects/props found
            case RESOURCE_COLLECTON_STAGE.COLLECTING:
                DoCollection();
                break;

            // Collection done, ask user to collect
            case RESOURCE_COLLECTON_STAGE.DONE:
                DoDone();
                break;
        }
	}

    void DoPlacement()
    {
        if (b_DoOnce)
        {
            CollectorText.GetComponent<TextMesh>().text = "Place target";
            b_DoOnce = false;
        }
    }

    void DoScanning()
    {
        if (b_DoOnce)
        {
            GameObject.Find("SmartTerrain").GetComponent<CustomSmartTerrainEventHandler>().StartTerrainGeneration();

            CollectorText.GetComponent<TextMesh>().text = "Scanning";
            b_DoOnce = false;
        }
    }

    void DoCollection()
    {
        if (b_DoOnce)
        {
            GameObject.Find("SmartTerrain").GetComponent<CustomSmartTerrainEventHandler>().StopTerrainGeneration();
            GameObject.Find("SmartTerrain").GetComponent<CustomSmartTerrainEventHandler>().ReplaceTemplates();

            CollectorText.GetComponent<TextMesh>().text = "Collecting";

            // Calculate Time for collection and amount collected
            GameObject[] GeneratedObjects = GameObject.FindGameObjectsWithTag("Resources");

            foreach (GameObject go in GeneratedObjects)
            {
                if (go.name.Contains("Resource_Credit"))
                {
                    i_CreditsAmount += go.GetComponent<Resource>().ResourceGain;
                    d_CollectionTime += go.GetComponent<Resource>().TimePerResource;
                }

                if (go.name.Contains("Resource_Data"))
                {
                    i_DataAmount += go.GetComponent<Resource>().ResourceGain;
                    d_CollectionTime += go.GetComponent<Resource>().TimePerResource;
                }
            }
            b_DoOnce = false;
        }

        // Countdown Time  
        d_CollectionTime -= Time.deltaTime;
        CollectorText.GetComponent<TextMesh>().text = "Collecting" + '\n' + "Time Left: " + (int)d_CollectionTime;

        if (d_CollectionTime <= 0)
        {
            IncrementStage();
            PersistentData.m_Instance.CreditsAmount += i_CreditsAmount;
            PersistentData.m_Instance.DataAmount += i_DataAmount;
            d_CollectionTime = 0;
        }
    }

    void DoDone()
    {
        if (b_DoOnce)
        {
            GameObject.Find("SmartTerrain").GetComponent<CustomSmartTerrainEventHandler>().ResetTerrainGeneration();

            i_CreditsAmount = 0;
            i_DataAmount = 0;

            CollectorText.GetComponent<TextMesh>().text = "Finished";
            b_DoOnce = false;
        }
        else
        {
            m_CurrentStage = RESOURCE_COLLECTON_STAGE.NOT_COLLECTING;
            CollectorText.GetComponent<TextMesh>().text = s_OrigText;
        }

    }



    public void IncrementStage()
    {
        m_CurrentStage++;
        b_DoOnce = true;
    }

    private void SetStage(RESOURCE_COLLECTON_STAGE newStage)
    {
        m_CurrentStage = newStage;
    }

    public RESOURCE_COLLECTON_STAGE GetStage()
    {
        return m_CurrentStage;
    }

}
