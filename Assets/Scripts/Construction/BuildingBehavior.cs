using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingBehavior : MonoBehaviour
{
    public GameObject building;
    public Sprite OGImage;
    private GameObject m_ConstructionManager;

    // Use this for initialization
    void Start()
    {
        m_ConstructionManager = GameObject.Find("GridManager");

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick()
    {
        if (!building.tag.Equals("Tower"))
        {
            if (PersistentData.m_Instance.CreditsAmount >= building.GetComponent<BuildingAbstractBase>().ECost &&
                PersistentData.m_Instance.DataAmount >= building.GetComponent<BuildingAbstractBase>().DCost)
            {
                m_ConstructionManager.GetComponent<ConstructionManager>().EnoughResource = true;
            }
            else
                m_ConstructionManager.GetComponent<ConstructionManager>().EnoughResource = false;
        }
        else
        {
            if (PersistentData.m_Instance.CreditsAmount >= building.GetComponent<BaseTurret>().ECost &&
                PersistentData.m_Instance.DataAmount >= building.GetComponent<BaseTurret>().DCost &&
                PersistentData.m_Instance.MilestoneIndex + 1 >= building.GetComponent<BaseTurret>().RequiredMilestoneLevel)
            {
                m_ConstructionManager.GetComponent<ConstructionManager>().EnoughResource = true;
            }
            else
                m_ConstructionManager.GetComponent<ConstructionManager>().EnoughResource = false;
        }

        if (m_ConstructionManager.GetComponent<ConstructionManager>().EnoughResource)
        {
            this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = OGImage;
            this.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            m_ConstructionManager.GetComponent<ConstructionManager>().SetBuildingToConstruct(building);
        }
        if(!m_ConstructionManager.GetComponent<ConstructionManager>().EnoughResource)
        {
            this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = OGImage;
            this.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        }       
    }

    public void OnExit()
    {
        this.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }
}