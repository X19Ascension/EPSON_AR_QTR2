using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConstructionManager : MonoBehaviour {

    public enum CONSTRUCTION_STAGE
    {
        NOT_CONSTRUCTING,
        SELECTION_BUILDING,
        SELECTION_POSITION,
        CONSTRUCTION,
        SELECTION_ADDON,
    }

    public CONSTRUCTION_STAGE ConstructionStage = CONSTRUCTION_STAGE.NOT_CONSTRUCTING;
    public GridBehavior theGridBehaviour;
    public GameObject theAddOnPanel;

    [Header("Special Building Replacements")]
    public GameObject WallStandardTurret;
    public GameObject WallSniperTurret;
    public GameObject WallHeavyTurret;

    public GameObject RightCWall;
    public GameObject RightWall;
    public GameObject LeftCWall;
    public GameObject LeftWall;
    public GameObject WallTower;

    public bool GeneratorBuilt;
    public bool FarmBuilt;
    public bool HouseBuilt;
    public bool DGeneratorBuilt;
    public bool EGeneratorBuilt;
    public bool DEGeneratorsBuilt;
    public bool TurretBuilt;

    private GameObject m_ObjectToConstruct = null;
    private GameObject m_OldObjectToConstruct = null;   // Used when needing to switch from a variant back to the standard model (wall turret to normal turret)
    private GameObject m_PreviewObject = null;
    private ADD_ON_TYPE m_AddOnToConstruct;
    private Vector3 m_SpawnLocation;
    private Vector3 m_oldPos;
    private Quaternion m_SpawnRotation;
    string top = "Top", bottom = "Bot", left = "Left", right = "Right";
    private enum WallSide
    {
        Empty = 0,
        Top = 1,
        Bot = 2,
        Right = 3,
        Left = 4
    }

    WallSide Side;

    public bool EnoughResource = false;
    private GameObject m_WallObject;    // Used to assign wall object to wall turrets

    double d_Timer;

	// Use this for initialization
	void Start () {
	//TownHall's GridPos is (4,4);
	}
	
	// Update is called once per frame
	void Update () {

        switch (ConstructionStage)
        {
            case CONSTRUCTION_STAGE.NOT_CONSTRUCTING:

                if (!GameObject.Find("BuildMenu"))
                    break;

                if (GameObject.Find("BuildMenu").GetComponent<BuildButtonBehavior>().GetMenuOpen())
                    ConstructionStage = CONSTRUCTION_STAGE.SELECTION_BUILDING;
                        
                break;

            case CONSTRUCTION_STAGE.SELECTION_BUILDING:

                d_Timer = 0.0;

                if (!GameObject.Find("BuildMenu").GetComponent<BuildButtonBehavior>().GetMenuOpen())
                    ConstructionStage = CONSTRUCTION_STAGE.NOT_CONSTRUCTING;

                if (m_ObjectToConstruct != null)
                    ConstructionStage = CONSTRUCTION_STAGE.SELECTION_POSITION;
                break;

            case CONSTRUCTION_STAGE.SELECTION_POSITION:

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Preview Building
                if (Physics.Raycast(ray, out hit))
                { 
                    // Check if the object is either a grid or wall
                    if ((hit.collider.gameObject.name == "Grid(Clone)" || hit.collider.gameObject.name.Contains("Wall")))
                    {
                        Vector2 checkPos = theGridBehaviour.GetWithinGridPos(hit.collider.gameObject.transform.position);
                        string checkName = "";
                        for (int i = 0; i < PersistentData.m_Instance.BuildingGridPos.Count; ++i)
                        {
                            if (PersistentData.m_Instance.BuildingGridPos[i].Equals(new SerializableVector2(checkPos)))
                            {
                                checkName = PersistentData.m_Instance.BuildingName[i];
                                break;
                            }
                        }

                        if (hit.collider.gameObject.tag.Contains("Wall"))
                            m_WallObject = hit.collider.gameObject;

                        // Check if the spot is already taken
                        if (!PersistentData.m_Instance.BuildingGridPos.Contains(new SerializableVector2(checkPos)))
                        {
                            // If spot is not taken, show preview building
                            // Switch m_ObjectToConstruct back to regular variant
                            if (!hit.collider.gameObject.name.Contains("Wall"))
                            {
                                if (m_ObjectToConstruct.name.Contains("Wall") && m_ObjectToConstruct.tag.Equals("Tower") ||
                                    m_ObjectToConstruct.name.Contains("Wall") && !m_ObjectToConstruct.name.Equals("Wall") && top == "Top" && bottom == "Bot" && left == "Left" && right == "Right")
                                {
                                    m_ObjectToConstruct = m_OldObjectToConstruct;
                                }
                            }
                        }
                        else if (hit.collider.gameObject.name.Contains("Wall") && !hit.collider.gameObject.name.Contains("Turret"))
                        {
                            if (m_PreviewObject != null)
                            {
                                Destroy(m_PreviewObject);
                                m_PreviewObject = null;
                            }

                            // If spot is taken by a wall, preview wall turret
                            // Switch m_ObjectToConstruct to wall variant
                            if (hit.collider.gameObject.name.Contains("Wall") && !(m_ObjectToConstruct.name.Contains("Wall")))
                            {
                                if (m_ObjectToConstruct.name.Contains("Standard"))
                                {
                                    m_OldObjectToConstruct = m_ObjectToConstruct;
                                    m_ObjectToConstruct = WallStandardTurret;
                                }
                                else if (m_ObjectToConstruct.name.Contains("Sniper"))
                                {
                                    m_OldObjectToConstruct = m_ObjectToConstruct;
                                    m_ObjectToConstruct = WallSniperTurret;
                                }
                                else if (m_ObjectToConstruct.name.Contains("Heavy"))
                                {
                                    m_OldObjectToConstruct = m_ObjectToConstruct;
                                    m_ObjectToConstruct = WallHeavyTurret;
                                }
                            }
                        }
                        // Wall Check Code
                        Vector2 TopCheck = theGridBehaviour.GetGridPos(hit.collider.gameObject.transform.position) + new Vector2(0, 1);
                        Vector2 BotCheck = theGridBehaviour.GetGridPos(hit.collider.gameObject.transform.position) + new Vector2(0, -1);
                        Vector2 LeftCheck = theGridBehaviour.GetGridPos(hit.collider.gameObject.transform.position) + new Vector2(-1, 0);
                        Vector2 RightCheck = theGridBehaviour.GetGridPos(hit.collider.gameObject.transform.position) + new Vector2(1, 0);
                        for (int idx = 0; idx < PersistentData.m_Instance.BuildingGridPos.Count; ++idx)
                        {
                            if (TopCheck.Equals(PersistentData.m_Instance.BuildingGridPos[idx].GetVec2()))
                            {
                                top = PersistentData.m_Instance.BuildingName[idx];
                            }
                            if (BotCheck.Equals(PersistentData.m_Instance.BuildingGridPos[idx].GetVec2()))
                            {
                                bottom = PersistentData.m_Instance.BuildingName[idx];
                            }
                            if (LeftCheck.Equals(PersistentData.m_Instance.BuildingGridPos[idx].GetVec2()))
                            {
                                left = PersistentData.m_Instance.BuildingName[idx];
                            }
                            if (RightCheck.Equals(PersistentData.m_Instance.BuildingGridPos[idx].GetVec2()))
                            {
                                right = PersistentData.m_Instance.BuildingName[idx];
                            }
                        }
                        //Switch to Wall Towers
                        if (top.Contains("Wall") && left.Contains("Wall") && m_ObjectToConstruct.name.Equals("Wall") ||
                            top.Contains("Wall") && right.Contains("Wall") && m_ObjectToConstruct.name.Equals("Wall") ||
                            bottom.Contains("Wall") && left.Contains("Wall") && m_ObjectToConstruct.name.Equals("Wall") ||
                            bottom.Contains("Wall") && right.Contains("Wall") && m_ObjectToConstruct.name.Equals("Wall"))
                        {
                            if (!top.Contains("WallTower") && !bottom.Contains("WallTower") && !right.Contains("WallTower") && !left.Contains("WallTower"))
                            {
                                m_OldObjectToConstruct = m_ObjectToConstruct;
                                m_ObjectToConstruct = WallTower;
                                top = "Top";
                                bottom = "Bot";
                                left = "Left";
                                right = "Right";
                            }
                        }
                        //Switch to Corner Walls
                        else if (top.Contains("WallTower") && Side == WallSide.Left ||
                                 bottom.Contains("WallTower") && Side == WallSide.Right ||
                                 right.Contains("WallTower") && Side == WallSide.Top ||
                                 left.Contains("WallTower") && Side == WallSide.Bot)
                        {
                            m_OldObjectToConstruct = m_ObjectToConstruct;
                            m_ObjectToConstruct = RightCWall;
                            top = "Top";
                            bottom = "Bot";
                            left = "Left";
                            right = "Right";
                        }
                        else if (top.Contains("WallTower") && Side == WallSide.Right ||
                                 bottom.Contains("WallTower") && Side == WallSide.Left ||
                                 right.Contains("WallTower") && Side == WallSide.Bot ||
                                 left.Contains("WallTower") && Side == WallSide.Top)
                        {
                            m_OldObjectToConstruct = m_ObjectToConstruct;
                            m_ObjectToConstruct = LeftCWall;
                            top = "Top";
                            bottom = "Bot";
                            left = "Left";
                            right = "Right";
                        }
                        //Switch to Side Walls
                        else if (top.Contains("Wall") && !top.Contains("WallTower") && Side == WallSide.Right ||
                                 bottom.Contains("Wall") && !bottom.Contains("WallTower") && Side == WallSide.Left ||
                                 right.Contains("Wall") && !right.Contains("WallTower") && Side == WallSide.Bot ||
                                 left.Contains("Wall") && !left.Contains("WallTower") && Side == WallSide.Top)
                        {
                            m_OldObjectToConstruct = m_ObjectToConstruct;
                            m_ObjectToConstruct = RightWall;
                            top = "Top";
                            bottom = "Bot";
                            left = "Left";
                            right = "Right";
                        }
                        else if (top.Contains("Wall") && !top.Contains("WallTower") && Side == WallSide.Left ||
                                 bottom.Contains("Wall") && !bottom.Contains("WallTower") && Side == WallSide.Right ||
                                 right.Contains("Wall") && !right.Contains("WallTower") && Side == WallSide.Top ||
                                 left.Contains("Wall") && !left.Contains("WallTower") && Side == WallSide.Bot)
                        {
                            m_OldObjectToConstruct = m_ObjectToConstruct;
                            m_ObjectToConstruct = LeftWall;
                            top = "Top";
                            bottom = "Bot";
                            left = "Left";
                            right = "Right";
                        }

                        if (m_ObjectToConstruct.tag.Contains("Building") && hit.collider.gameObject.tag.Contains("Wall"))
                            return;

                        if (hit.collider.gameObject.name.Contains("WallTower"))
                            return;

                        //if (hit.collider.gameObject.tag.Contains("Tower"))
                        //    return;

                        // Create Preview Building if one does not exist
                        if ((m_PreviewObject == null && m_ObjectToConstruct.tag.Contains("Tower")) || (m_PreviewObject == null && !PersistentData.m_Instance.BuildingGridPos.Contains(new SerializableVector2(checkPos))))
                        {
                            m_PreviewObject = Instantiate(m_ObjectToConstruct, hit.collider.gameObject.transform.position + new Vector3(0, 0.1f, 0), hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, -90, 0))) as GameObject;
                        }
                        // Move the preview building
                        else
                        {
                            m_oldPos = m_PreviewObject.transform.position;
                            m_PreviewObject.transform.position = hit.collider.gameObject.transform.position; // + new Vector3(0, 0.1f, 0)                                  
                            if (m_PreviewObject.name.Contains("Wall") && m_PreviewObject.tag.Contains("Tower"))
                                m_PreviewObject.transform.rotation = hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, -90, 0));
                            if (m_PreviewObject.tag.Equals("Wall") && !m_PreviewObject.name.Equals("WallTower(Clone)")) //m_PreviewObject.name.Contains("Wall") &&
                            {
                                if (m_PreviewObject.transform.position.z < m_oldPos.z)
                                {
                                    m_PreviewObject.transform.rotation = hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, -90, 0));
                                    Side = WallSide.Bot;
                                }
                                if (m_PreviewObject.transform.position.z > m_oldPos.z)
                                {
                                    m_PreviewObject.transform.rotation = hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, 90, 0));
                                    Side = WallSide.Top;
                                }
                                if (m_PreviewObject.transform.position.x > m_oldPos.x)
                                {
                                    m_PreviewObject.transform.rotation = hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, 180, 0));
                                    Side = WallSide.Right;
                                }
                                if (m_PreviewObject.transform.position.x < m_oldPos.x)
                                {
                                    m_PreviewObject.transform.rotation = hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, 0, 0));
                                    Side = WallSide.Left;
                                }
                            }
                            else if (m_PreviewObject.name.Equals("WallTower(Clone)"))
                                m_PreviewObject.transform.rotation = hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, 0, 0));
                            else
                                m_PreviewObject.transform.rotation = hit.collider.gameObject.transform.rotation;
                        }
                    }
                }
                else
                {
                    if (m_PreviewObject != null)
                    {
                        Destroy(m_PreviewObject);
                        m_PreviewObject = null;
                    }
                }


                d_Timer += Time.deltaTime;
                if (Input.GetMouseButtonUp(0) && d_Timer > 0.1)
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        // Check if ray is hiting a grid or the preview itself
                        if ((hit.collider.gameObject.name == "Grid(Clone)" || hit.collider.gameObject.name.Contains("Wall")) || hit.collider.gameObject.Equals(m_PreviewObject))
                        {
                            // Check if the location is valid
                            Vector2 checkPos = theGridBehaviour.GetWithinGridPos(hit.collider.gameObject.transform.position);
                            string checkName = "";
                            for (int i = 0; i < PersistentData.m_Instance.BuildingGridPos.Count; ++i)
                            {
                                if (PersistentData.m_Instance.BuildingGridPos[i].Equals(new SerializableVector2(checkPos)))
                                {
                                    checkName = PersistentData.m_Instance.BuildingName[i];
                                    break;
                                }
                            }

                            if (!PersistentData.m_Instance.BuildingGridPos.Contains(new SerializableVector2(checkPos))
                                || PersistentData.m_Instance.BuildingGridPos.Contains(new SerializableVector2(checkPos)) && checkName.Contains("Wall") && !(checkName.Contains("Standard") || checkName.Contains("Sniper") || checkName.Contains("Heavy")) && m_ObjectToConstruct.tag.Contains("Tower"))
                            {
                                // Check if the player has enough resources
                                if (EnoughResource)
                                {
                                    ConstructionStage = CONSTRUCTION_STAGE.CONSTRUCTION;
                                    m_SpawnLocation = hit.collider.gameObject.transform.position;

                                    if (m_ObjectToConstruct.name.Contains("Wall") && m_ObjectToConstruct.tag.Equals("Tower"))
                                        m_SpawnRotation = hit.collider.gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, -90, 0));
                                    else
                                        m_SpawnRotation = hit.collider.gameObject.transform.rotation;
                                }
                            }
                    
                        }
                        else
                        {
                            ConstructionStage = CONSTRUCTION_STAGE.SELECTION_BUILDING;
                            m_ObjectToConstruct = null;
                        }
                    }
                    else
                    {
                        ConstructionStage = CONSTRUCTION_STAGE.SELECTION_BUILDING;
                        m_ObjectToConstruct = null;
                    }
                }
                break;

            case CONSTRUCTION_STAGE.CONSTRUCTION:
                // Destroy the preview
                if (m_PreviewObject != null)
                {
                    Destroy(m_PreviewObject);
                    m_PreviewObject = null;
                }
                GameObject go;
                // Spawn the building
                //Adjust height for walls,
                if (m_ObjectToConstruct.name.Contains("Wall") && !m_ObjectToConstruct.name.Contains("WallTower"))
                    go = Instantiate(m_ObjectToConstruct, m_SpawnLocation + new Vector3(0, 0.05f, -0.025f), m_SpawnRotation /*m_ObjectToConstruct.transform.rotation*/) as GameObject;
                //Wall Towers,
                else if (m_ObjectToConstruct.name.Contains("WallTower"))
                    go = Instantiate(m_ObjectToConstruct, m_SpawnLocation + new Vector3(0, 0.15f, -0.025f), m_SpawnRotation /*m_ObjectToConstruct.transform.rotation*/) as GameObject;
                //And normal buildings
                else
                    go = Instantiate(m_ObjectToConstruct, m_SpawnLocation + new Vector3(0, 0.1f, -0.025f), m_SpawnRotation /*m_ObjectToConstruct.transform.rotation*/) as GameObject;

                // Assign GridPos and Deduct cost
                if (!m_ObjectToConstruct.tag.Equals("Tower"))
                {
                    go.GetComponent<BuildingAbstractBase>().GridPos = theGridBehaviour.GetWithinGridPos(m_SpawnLocation);
                    go.GetComponent<BuildingAbstractBase>().IsPreview = false;

                    if (m_ObjectToConstruct.tag.Contains("Wall"))
                    {
                        Vector2 TopCheck = theGridBehaviour.GetGridPos(go.transform.position) + new Vector2(0, 1);
                        Vector2 BotCheck = theGridBehaviour.GetGridPos(go.transform.position) + new Vector2(0, -1);
                        Vector2 LeftCheck = theGridBehaviour.GetGridPos(go.transform.position) + new Vector2(-1, 0);
                        Vector2 RightCheck = theGridBehaviour.GetGridPos(go.transform.position) + new Vector2(1, 0);
                        for (int idx = 0; idx < PersistentData.m_Instance.BuildingGridPos.Count; ++idx)
                        {
                            if (TopCheck.Equals(PersistentData.m_Instance.BuildingGridPos[idx].GetVec2()))
                            {
                                top = PersistentData.m_Instance.BuildingName[idx];
                            }
                            if (BotCheck.Equals(PersistentData.m_Instance.BuildingGridPos[idx].GetVec2()))
                            {
                                bottom = PersistentData.m_Instance.BuildingName[idx];
                            }
                            if (LeftCheck.Equals(PersistentData.m_Instance.BuildingGridPos[idx].GetVec2()))
                            {
                                left = PersistentData.m_Instance.BuildingName[idx];
                            }
                            if (RightCheck.Equals(PersistentData.m_Instance.BuildingGridPos[idx].GetVec2()))
                            {
                                right = PersistentData.m_Instance.BuildingName[idx];
                            }
                            if (top.Contains("Wall") && !top.Contains("Tower"))
                            {
                                go.transform.Rotate(0, 0, 0);
                            }
                            if (bottom.Contains("Wall") && !top.Contains("Tower"))
                            {
                                go.transform.Rotate(0, 90, 0);
                            }
                        }
                    }

                    if (m_ObjectToConstruct.name == ("Generator"))
                        GeneratorBuilt = true;
                    if (m_ObjectToConstruct.name == ("Farm"))
                        FarmBuilt = true;
                    if (m_ObjectToConstruct.name == ("Housing"))
                        HouseBuilt = true;
                    if (m_ObjectToConstruct.name == ("DataGenerator"))
                        DGeneratorBuilt = true;
                    if (m_ObjectToConstruct.name == ("CreditsGenerator"))
                        EGeneratorBuilt = true;
                    if (DGeneratorBuilt && EGeneratorBuilt)
                        DEGeneratorsBuilt = true;
                        
                    PersistentData.m_Instance.CreditsAmount -= go.GetComponent<BuildingAbstractBase>().ECost;
                    PersistentData.m_Instance.DataAmount -= go.GetComponent<BuildingAbstractBase>().DCost;

                    go.GetComponent<BoxCollider>().enabled = true;

                }
                else
                {
                    go.GetComponent<BaseTurret>().GridPos = theGridBehaviour.GetWithinGridPos(m_SpawnLocation);
                    go.GetComponent<BaseTurret>().IsPreview = false;
                    PersistentData.m_Instance.CreditsAmount -= m_ObjectToConstruct.GetComponent<BaseTurret>().ECost;
                    PersistentData.m_Instance.DataAmount -= m_ObjectToConstruct.GetComponent<BaseTurret>().DCost;

                    if (m_ObjectToConstruct.name.Contains("Standard"))
                        TurretBuilt = true;

                    go.GetComponent<BaseTurret>().TurretActive = true;
                    go.GetComponent<BoxCollider>().enabled = true;
                }

                // Set the grid behaviour to be the parent
                go.transform.parent = theGridBehaviour.transform;

                // Cause the building to switch from the preview material to its final one
                MaterialSwitch[] ComponentList = go.GetComponentsInChildren<MaterialSwitch>();
                foreach (MaterialSwitch ms in ComponentList)
                {
                    ms.SwitchToSecondMaterial();
                }

                // Special case for Housing
                if (go.name.Contains("Housing"))
                {
                    go.GetComponent<AddOnConstruction>().theConstructionManager = this;
                    go.GetComponent<AddOnConstruction>().theHoverScript = theAddOnPanel.GetComponent<UIHoverOver>();
                }

                // Send to RandomEventManager
                EventBase.TRIGGER_TYPE aTrigger = EventBase.TRIGGER_TYPE.BUILD_CREDIT;

                switch (m_ObjectToConstruct.name)
                {
                    case "Generator": aTrigger = EventBase.TRIGGER_TYPE.BUILD_GENERATOR; break;
                    case "DataGenerator": aTrigger = EventBase.TRIGGER_TYPE.BUILD_DATA; break;
                    case "CreditsGenerator": aTrigger = EventBase.TRIGGER_TYPE.BUILD_CREDIT; break;
                    case "Farm": aTrigger = EventBase.TRIGGER_TYPE.BUILD_FARM; break;
                    case "Housing": aTrigger = EventBase.TRIGGER_TYPE.BUILD_HOUSING; break;
                    case "Storage": aTrigger = EventBase.TRIGGER_TYPE.BUILD_STORAGE; break;
                }

                if (m_ObjectToConstruct.tag.Equals("Tower"))
                    aTrigger = EventBase.TRIGGER_TYPE.BUILD_TURRET;

                GameObject.Find("RandomEventSystem").GetComponent<RandomEventManager>().RunRandomCheck(aTrigger);


                // Assign attached wall object
                if (m_ObjectToConstruct.name.Contains("Wall") && (m_ObjectToConstruct.name.Contains("Standard") || m_ObjectToConstruct.name.Contains("Sniper") || m_ObjectToConstruct.name.Contains("Heavy")))
                {
                    go.GetComponent<BaseTurret>().AttachedWall = m_WallObject.GetComponent<BuildingAbstractBase>();
                }

                ConstructionStage = CONSTRUCTION_STAGE.NOT_CONSTRUCTING;
                m_ObjectToConstruct = null;

                break;

            case CONSTRUCTION_STAGE.SELECTION_ADDON:
                if (m_AddOnToConstruct != ADD_ON_TYPE.NONE)
                {
                    switch (m_AddOnToConstruct)
                    {
                        case ADD_ON_TYPE.POWER:
                            theAddOnPanel.GetComponent<UIHoverOver>().ObjectToHoverOver.transform.GetChild(1).gameObject.SetActive(true);
                            theAddOnPanel.GetComponent<UIHoverOver>().ObjectToHoverOver.GetComponent<Residential>().theAddOn = theAddOnPanel.GetComponent<UIHoverOver>().ObjectToHoverOver.transform.GetChild(1).gameObject.GetComponent<BuildingAddOn>();
                            break;

                        case ADD_ON_TYPE.FOOD:
                            theAddOnPanel.GetComponent<UIHoverOver>().ObjectToHoverOver.transform.GetChild(2).gameObject.SetActive(true);
                            theAddOnPanel.GetComponent<UIHoverOver>().ObjectToHoverOver.GetComponent<Residential>().theAddOn = theAddOnPanel.GetComponent<UIHoverOver>().ObjectToHoverOver.transform.GetChild(2).gameObject.GetComponent<BuildingAddOn>();
                            break;
                    }

                    ConstructionStage = CONSTRUCTION_STAGE.NOT_CONSTRUCTING;
                    m_AddOnToConstruct = ADD_ON_TYPE.NONE;
                    theAddOnPanel.SetActive(false);
                }
                else
                {
                    if (Input.GetMouseButtonUp(0))
                    {

                        ConstructionStage = CONSTRUCTION_STAGE.NOT_CONSTRUCTING;
                        m_AddOnToConstruct = ADD_ON_TYPE.NONE;
                        theAddOnPanel.SetActive(false);
                    }
                }
                break;
        }
	}

    public void WallChecker(GameObject NewWall)
    {
        //Vector2 Wall1Pos = theGridBehaviour.GetWithinGridPos(hit.collider.gameObject.transform.position);
        //theGridBehaviour.CheckGridTop(theGridBehaviour.GetGridPos(hit.collider.gameObject.transform.position));
        //Check which side the new wall is facing
        switch (Side)
        {
            case WallSide.Top:
                {
                    ////Wall on left, nothing on right. Turns into RightCWall.
                    //if (theGridBehaviour.CheckGridLeft(theGridBehaviour.GetGridPos(NewWall.transform.position)) && !theGridBehaviour.CheckGridRight(theGridBehaviour.GetGridPos(NewWall.transform.position)))
                    //{
                    //    m_ObjectToConstruct = RightCWall;
                    //    //Destroy(NewWall);
                    //    //GameObject go = Instantiate(m_ObjectToConstruct, NewWall.transform.position + new Vector3(0, 0.1f, 0), NewWall.transform.rotation * Quaternion.Euler(new Vector3(0, 90, 0))) as GameObject;
                    //}
                    //Walls on perpendicular Sides
                    
                    break;
                }
            case WallSide.Bot:
                break;
            case WallSide.Right:
                break;
            case WallSide.Left:
                break;
        }
    }

    public void SetBuildingToConstruct(GameObject theBuilding)
    {
        //Check if enough resources

        if (EnoughResource)
            m_ObjectToConstruct = theBuilding;
    }

    public void SetAddOnToConstruct(string aTypeStr)
    {
        switch (aTypeStr)
        {
            case "Food":
                m_AddOnToConstruct = ADD_ON_TYPE.FOOD;
                break;

            case "Power":
                m_AddOnToConstruct = ADD_ON_TYPE.POWER;
                break;
        }
    }
}
