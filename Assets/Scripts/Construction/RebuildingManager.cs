using UnityEngine;
using System.Collections;

public class RebuildingManager : MonoBehaviour {

    public enum REBUILDING_STAGE
    {
        NOT_REBUILDING,
        SELECTION,
        REBUILDING,
        CLEAR,
    }

    public REBUILDING_STAGE RebuildingStage = REBUILDING_STAGE.NOT_REBUILDING;
    public GridBehavior theGridBehaviour;
    public GameObject RebuildPanel;

    public bool InputReceived = false;
    public bool StartRebuilding = false;

    DestroyedBuilding TargetObject;

	// Use this for initialization
	void Start () { 
	
	}
	
	// Update is called once per frame
	void Update () {
	
        switch (RebuildingStage)
        {
            case REBUILDING_STAGE.NOT_REBUILDING:
                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    // Check if clicking on destroyed building
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.tag.Contains("Destroyed"))
                        {
                            TargetObject = hit.collider.gameObject.GetComponent<DestroyedBuilding>();

                            RebuildingStage = REBUILDING_STAGE.SELECTION;

                            RebuildPanel.SetActive(true);
                            RebuildPanel.GetComponent<UIHoverOver>().ObjectToHoverOver = TargetObject.gameObject;
                        }
                    }
                }
                    break;

            case REBUILDING_STAGE.SELECTION:

                if (InputReceived)
                {
                    if (StartRebuilding)
                        RebuildingStage = REBUILDING_STAGE.REBUILDING;
                    else
                        RebuildingStage = REBUILDING_STAGE.CLEAR;
                }

                if (Input.GetMouseButtonUp(0) && !InputReceived)
                {
                    RebuildingStage = REBUILDING_STAGE.NOT_REBUILDING;
                    RebuildPanel.SetActive(false);
                    InputReceived = false;
                    StartRebuilding = false;
                }

                    break;

            case REBUILDING_STAGE.REBUILDING:

                // Spawn the building
                TargetObject.GetComponent<DestroyedBuilding>().Rebuild();

                RebuildingStage = REBUILDING_STAGE.NOT_REBUILDING;
                RebuildPanel.SetActive(false);
                InputReceived = false;
                StartRebuilding = false;
                break;

            case REBUILDING_STAGE.CLEAR:

                TargetObject.PreviousBuilding.GetComponent<Health>().HP = 0;
                Destroy(TargetObject.PreviousBuilding);
                Destroy(TargetObject.gameObject);

                RebuildingStage = REBUILDING_STAGE.NOT_REBUILDING;
                RebuildPanel.SetActive(false);
                InputReceived = false;
                StartRebuilding = false;

                break;
        }

	}

    public void SetToRebuild(bool status)
    {
        bool EnoughResource = false;
        if (!TargetObject.tag.Equals("Tower"))
        {
            //Buildings
            if (PersistentData.m_Instance.CreditsAmount >= TargetObject.GetComponent<BuildingAbstractBase>().ECost &&
                PersistentData.m_Instance.DataAmount >= TargetObject.GetComponent<BuildingAbstractBase>().DCost)
            {
                EnoughResource = true;
            }
        }
        else
        {
            //Turrets
            if (PersistentData.m_Instance.CreditsAmount >= TargetObject.GetComponent<BaseTurret>().ECost &&
                PersistentData.m_Instance.DataAmount >= TargetObject.GetComponent<BaseTurret>().DCost)
            {
                EnoughResource = true;
            }
        }

        if (status && !EnoughResource)
            return;

        InputReceived = true;
        StartRebuilding = status;
    }
}
