using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlacementManager : MonoBehaviour {

    public enum PLACEMENT_STATE
    {
        NOT_PLACING = 0,
        SELECTING_POSITION,
        PLACEMENT,
    }
    public Button b_rifle;
    public Button b_shotgun;
    public Button b_melee;
    public Button b_medic;
    public Button b_mechanic;

    public PLACEMENT_STATE placementState = PLACEMENT_STATE.NOT_PLACING;
    public Survivor.SURVIVOR_TYPE survivorType; 
    public GameObject medic;
    public GameObject rifle;
    public GameObject shotgun;
    public GameObject melee;
    public GameObject mechanic;

    public int onField;

    public string chosenOne;
    public bool buttonClicked;

    private GameObject m_ObjectToPlace = null;
    private GameObject m_ObjectPreview = null;

    public GridMap theGrid;

    public GameObject helpTip;



    private Vector3 screenPoint;
    private Vector3 offset;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        switch (placementState)
        {
            case PLACEMENT_STATE.NOT_PLACING:
                if (buttonClicked)
                {
                    helpTip.SetActive(true);
                    placementState = PLACEMENT_STATE.SELECTING_POSITION;
                }
                break;
            case PLACEMENT_STATE.SELECTING_POSITION:
                //m_ObjectPreview;
                switch (survivorType)
                {
                    case Survivor.SURVIVOR_TYPE.S_RIFLE:
                        m_ObjectToPlace = rifle;
                        break;
                    case Survivor.SURVIVOR_TYPE.S_SHOTGUN:
                        m_ObjectToPlace = shotgun;
                        break;
                    case Survivor.SURVIVOR_TYPE.S_MELEE:
                        m_ObjectToPlace = melee;
                        break;
                    case Survivor.SURVIVOR_TYPE.S_MEDIC:
                        m_ObjectToPlace = medic;
                        break;
                    case Survivor.SURVIVOR_TYPE.S_MECHANIC:
                        m_ObjectToPlace = mechanic;
                        break;
                }


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Preview Building
                if (Physics.Raycast(ray, out hit))
                {
                    if ((m_ObjectPreview == null && m_ObjectToPlace.tag.Contains("Survivor")))
                    {
                        m_ObjectPreview = Instantiate(m_ObjectToPlace, hit.collider.gameObject.transform.position + new Vector3(0, 0.1f, 0), hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, -90, 0))) as GameObject;
                    }

                    //theGrid.ClamptoGrid(m_ObjectPreview);

                    if (hit.collider.gameObject.tag == "Empty Grid")
                    {
                        if (Input.GetMouseButtonDown(0) == true)
                        {
                            // Create Preview Building if one does not exist
                            if ((/*m_ObjectPreview == null &&*/ m_ObjectToPlace.tag.Contains("Survivor")))
                            {
                                m_ObjectToPlace = Instantiate(m_ObjectToPlace, hit.collider.gameObject.transform.position + new Vector3(0, 0.1f, 0), hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, -90, 0))) as GameObject;
                            }

                            onField++;
                            helpTip.SetActive(false);
                            buttonClicked = false;
                            placementState = PLACEMENT_STATE.NOT_PLACING;
                            theGrid.ClamptoGrid(m_ObjectToPlace);
                            m_ObjectToPlace = null;
                            m_ObjectPreview = null;
                        }
                    }
                }
                if (Input.GetKey(KeyCode.Escape))
                {
                    helpTip.SetActive(false);
                    placementState = PLACEMENT_STATE.NOT_PLACING;
                    buttonClicked = false;
                    m_ObjectPreview = null;
                }
                break;
            case PLACEMENT_STATE.PLACEMENT:


                break;
        }
	}


    public void PlaceRifle()
    {
        survivorType = Survivor.SURVIVOR_TYPE.S_RIFLE;
        buttonClicked = true;
    }
    public void PlaceMedic()
    {
        survivorType = Survivor.SURVIVOR_TYPE.S_MEDIC;
        buttonClicked = true;
    }
    public void PlaceShotgun()
    {
        survivorType = Survivor.SURVIVOR_TYPE.S_SHOTGUN;
        buttonClicked = true;
    }
    public void PlaceMelee()
    {
        survivorType = Survivor.SURVIVOR_TYPE.S_MELEE;
        buttonClicked = true;
    }
    public void PlaceMechanic()
    {
        survivorType = Survivor.SURVIVOR_TYPE.S_MECHANIC;
        buttonClicked = true;
    }
}
