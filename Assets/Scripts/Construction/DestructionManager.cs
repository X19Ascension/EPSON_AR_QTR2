using UnityEngine;
using System.Collections;

public class DestructionManager : MonoBehaviour {
    public enum DESTRUCTION_STAGE
    {
        NOT_DESTROYING,
        SELECTION
    }

    public DESTRUCTION_STAGE DestructionStage = DESTRUCTION_STAGE.NOT_DESTROYING;
    public GridBehavior theGridBehaviour;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        switch (DestructionStage)
        {
            case DESTRUCTION_STAGE.NOT_DESTROYING:
                if (!GameObject.Find("DeleteButton"))
                    break;

                if (GameObject.Find("DeleteButton").GetComponent<DestroyButton>().GetDestroy())
                {
                    DestructionStage = DESTRUCTION_STAGE.SELECTION;
                }

                break;
            case DESTRUCTION_STAGE.SELECTION:
                if(!GameObject.Find("DeleteButton").GetComponent<DestroyButton>().GetDestroy())
                    DestructionStage = DESTRUCTION_STAGE.NOT_DESTROYING;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;


                if (Physics.Raycast(ray, out hit))
                {
                    MaterialSwitch[] ComponentList = hit.collider.gameObject.GetComponentsInChildren<MaterialSwitch>();
                    if (hit.collider.gameObject.tag == "Buildings" &&
                        hit.collider.gameObject.name != "TownHall" ||
                        hit.collider.gameObject.tag == "Tower" ||
                        hit.collider.gameObject.tag == "Wall")
                    {                        
                        foreach (MaterialSwitch ms in ComponentList)
                        {
                            ms.SwitchToThirdMaterial();
                        }
                        if (Input.GetMouseButtonUp(0))
                        {
                            Destroy(hit.collider.gameObject);
                            
                            //If Buildings
                            if (hit.collider.gameObject.tag == "Buildings" && hit.collider.gameObject.name != "TownHall" || hit.collider.gameObject.tag == "Wall")
                            {
                                PersistentData.m_Instance.DataAmount += hit.collider.gameObject.GetComponent<BuildingAbstractBase>().DCost / 5;
                                PersistentData.m_Instance.CreditsAmount += hit.collider.gameObject.GetComponent<BuildingAbstractBase>().ECost / 5;
                                //Remove from Persistent Data
                                for (int i = 0; i < PersistentData.m_Instance.BuildingName.Count; ++i)
                                {
                                    if (hit.collider.gameObject.GetComponent<BuildingAbstractBase>().GridPos == PersistentData.m_Instance.BuildingGridPos[i].GetVec2())
                                    {
                                        PersistentData.m_Instance.BuildingGridPos.RemoveAt(i);
                                        PersistentData.m_Instance.BuildingName.RemoveAt(i);
                                    }
                                }                            
                            }

                            //If Turrets
                            if (hit.collider.gameObject.tag == "Tower")
                            {
                                PersistentData.m_Instance.DataAmount += hit.collider.gameObject.GetComponent<BaseTurret>().DCost / 5;
                                PersistentData.m_Instance.CreditsAmount += hit.collider.gameObject.GetComponent<BaseTurret>().ECost / 5;
                                //Remove from Persistent Data
                                for (int i = 0; i < PersistentData.m_Instance.BuildingName.Count; ++i)
                                {
                                    if (hit.collider.gameObject.GetComponent<BaseTurret>().GridPos == PersistentData.m_Instance.BuildingGridPos[i].GetVec2())
                                    {
                                        PersistentData.m_Instance.BuildingGridPos.RemoveAt(i);
                                        PersistentData.m_Instance.BuildingName.RemoveAt(i);
                                    }
                                }
                            }

                            DestructionStage = DESTRUCTION_STAGE.NOT_DESTROYING;
                            GameObject.Find("DeleteButton").GetComponent<DestroyButton>().SetDestroy(false);
                        }
                    }
                    else
                    {
                        ComponentList = gameObject.GetComponentsInChildren<MaterialSwitch>();
                        DestructionStage = DESTRUCTION_STAGE.NOT_DESTROYING;
                        foreach (MaterialSwitch ms in ComponentList)
                        {
                            ms.SwitchToSecondMaterial();

                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    DestructionStage = DESTRUCTION_STAGE.NOT_DESTROYING;
                    GameObject.Find("DeleteButton").GetComponent<DestroyButton>().SetDestroy(false);
                }
                break;
        }
    }
}
