using UnityEngine;
using System.Collections;
using System;

public class DestroyedBuilding : BuildingAbstractBase
{
    public GameObject PreviousBuilding;
    public string PreviousName;
    Quaternion PreviousRotation = Quaternion.identity;
    public ReadFromCSV CSVReader;

	// Use this for initialization
	public override void Start() {

        BuildingName += ("_" + PreviousBuilding.name);

        base.Start();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void Rebuild()
    {
        GameObject go = null;

        if (PreviousBuilding)
        {
            PreviousBuilding.SetActive(true);
            PreviousBuilding.GetComponent<BuildingAbstractBase>().Reset();
        }
        else
        {
            if (PreviousName.Contains("Wall"))
            {
                
                for (int i = 0; i < 11 * 11; ++i)
                {
                    Vector2 temp = PersistentData.m_Instance.BuildingGridPos[i].GetVec2();

                    float VerticalDist = Mathf.Abs(temp.y - CSVReader.loadedMap[11, 11] / 2);
                    float HorizontalDist = Mathf.Abs(temp.x - CSVReader.loadedMap[11, 11] / 2);

                    if (VerticalDist >= HorizontalDist)
                    {
                        // Bottom
                        if (temp.y < CSVReader.loadedMap[11, 11] / 2)
                        {
                            PreviousRotation = Quaternion.Euler(0, 0, 0);
                        }
                        // Top
                        if (temp.y > CSVReader.loadedMap[11, 11] / 2)
                        {
                            PreviousRotation = Quaternion.Euler(0, 0, 0);
                        }
                    }
                    else
                    {
                        // Left
                        if (temp.x < CSVReader.loadedMap[11, 11] / 2)
                        {
                            PreviousRotation = Quaternion.Euler(0, 0, 0);
                        }
                        // Right
                        if (temp.x > CSVReader.loadedMap[11, 11] / 2)
                        {
                            PreviousRotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
                }
                if (PreviousName.Contains("WallTower"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().Tower;
                    PreviousRotation = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().Tower.transform.rotation;
                }

                if (PreviousName.Contains("LeftCWall"))
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().LeftCWall;
                if (PreviousName.Contains("LeftWall"))
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().LeftWall;
                if (PreviousName.Contains("RightCWall"))
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().RightCWall;
                if (PreviousName.Contains("RightWall"))
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().RightWall;
                if (!PreviousName.Contains("C") && !PreviousName.Contains("Left") && !PreviousName.Contains("Right") && !PreviousName.Contains("Tower"))
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().Wall;

                go = Instantiate(PreviousBuilding, this.gameObject.transform.position, PreviousRotation) as GameObject;
            }
            else
            {
                Quaternion rotate = Quaternion.identity;

                if (PreviousName.Contains("DataGenerator"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().DataGenerator;
                }
                else if (PreviousName.Contains("CreditsGenerator"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().CreditsGenerator;
                }
                else if (PreviousName.Contains("Farm"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().FoodGenerator;
                }
                else if (PreviousName.Contains("Generator"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().PowerGenerator;
                }
                else if (PreviousName.Contains("Housing"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().Housing;
                }
                else if (PreviousName.Contains("Storage"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().Storage;
                }
                else if (PreviousName.Contains("Standard"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().StandardTurret;
                }
                else if (PreviousName.Contains("Sniper"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().SniperTurret;
                }
                else if (PreviousName.Contains("Heavy"))
                {
                    PreviousBuilding = GameObject.FindGameObjectWithTag("ImageTarget").GetComponentInChildren<GridBehavior>().HeavyTurret;
                }

                go = Instantiate(PreviousBuilding, this.gameObject.transform.position, rotate) as GameObject;
            }
      
            // Assign GridPos and Deduct cost
            if (!go.tag.Equals("Tower"))
            {
                go.GetComponent<BuildingAbstractBase>().GridPos = this.GridPos;
                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                PersistentData.m_Instance.CreditsAmount -= go.GetComponent<BuildingAbstractBase>().ECost;
                PersistentData.m_Instance.DataAmount -= go.GetComponent<BuildingAbstractBase>().DCost;
                go.GetComponent<BuildingAbstractBase>().Reset();

            }
            else
            {
                go.GetComponent<BaseTurret>().GridPos = this.GridPos;
                go.GetComponent<BaseTurret>().IsPreview = false;
                PersistentData.m_Instance.CreditsAmount -= go.GetComponent<BaseTurret>().ECost;
                PersistentData.m_Instance.DataAmount -= go.GetComponent<BaseTurret>().DCost;
                go.GetComponent<BaseTurret>().Reset();

                go.GetComponent<BaseTurret>().TurretActive = true;
            }

            go.transform.parent = GameObject.Find("GridManager").transform;

            MaterialSwitch[] ComponentList = go.GetComponentsInChildren<MaterialSwitch>();
            foreach (MaterialSwitch ms in ComponentList)
            {
                ms.SwitchToSecondMaterial();
            }

            // Special case for Housing
            if (go.name.Contains("Housing"))
            {
                go.GetComponent<AddOnConstruction>().theConstructionManager = this.gameObject.GetComponent<ConstructionManager>();
                go.GetComponent<AddOnConstruction>().theHoverScript = this.gameObject.GetComponent<ConstructionManager>().theAddOnPanel.GetComponent<UIHoverOver>();
            }
        }
        Destroy(this.gameObject);
    }

    public override void ProduceResource()
    {
        
    }

    public override int GetPopulationCount()
    {
        return 0;
    }

    public override int GetPopulationCap()
    {
        return 0;
    }

    public override int GetCreditsStorageCap()
    {
        return 0;
    }

    public override int GetDataStorageCap()
    {
        return 0;
    }

    public override int GetPowerStorageCap()
    {
        return 0;
    }

    public override int GetFoodStorageCap()
    {
        return 0;
    }

    public override void Reset()
    {
        throw new NotImplementedException();
    }
}