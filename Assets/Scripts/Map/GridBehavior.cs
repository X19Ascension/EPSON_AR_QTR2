using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;

public class GridBehavior : MonoBehaviour
{
    public GameObject Grid;
    public GameObject Wall;
    public GameObject LeftCWall;
    public GameObject LeftWall;
    public GameObject RightCWall;
    public GameObject RightWall;
    public GameObject Tower;
    public GameObject TownSpawn;

    public GameObject DataGenerator, CreditsGenerator, PowerGenerator, FoodGenerator, Housing, Storage, StandardTurret, SniperTurret, HeavyTurret;
    public GameObject ObstacleGrass, ObstacleDesert1, ObstacleDesert2, ObstacleDesert3, ObstacleDesert4, ObstacleRuins1, ObstacleRuins2, ObstacleRuins3, ObstacleRuins4; //Add to this when needed
    public GameObject DestroyedBuilding, DestroyedWall, DestroyedTurret;

    public ReadFromCSV CSVReader;

    Vector3 Offset;                             // Based on expansion level
    int i_SizeX = 11;                             // Based on expansion level
    int i_i_SizeX = 11;                             // Based on expansion level
    Vector3 WallOffsetX;                        // Based on expansion level
    Vector3 WallOffsetY;                        // Based on expansion level
    Vector3 TowerOffset;                        // Based on expansion level

    bool b_FirstLoad = false;

    List<List<Vector3>> GridList = new List<List<Vector3>>();

    // Update is called once per frame
    void Update()
    {
        if (PersistentData.m_Instance.InitialLoad && !b_FirstLoad)
        {
            GenerateMap(true);

            b_FirstLoad = true;
        }

        if (Input.GetMouseButtonUp(2))
            Reset();

        /*if (Input.GetMouseButtonUp(0))
        {
            Expansion();
        }*/
    }

    // Old Expansion Code
    /*public void Expansion()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if ((hit.collider.gameObject.name == "TownHall") &&
                PersistentData.m_Instance.DataAmount >= 50 &&
                PersistentData.m_Instance.CreditsAmount >= 50 &&
                PersistentData.m_Instance.PopulationAmount >= 10
                )
            {
                MigrateBuildings();
                GenerateMap(false);
                AllignWallTurrets();

                PersistentData.m_Instance.DataAmount -= 50;
                PersistentData.m_Instance.CreditsAmount -= 50;
                Debug.Log("CurrentPop: " + GameObject.Find("TownHall").GetComponent<TownHall>().CurrentPop);
            }
            else if (hit.collider.gameObject.name == "TownHall" &&
                        PersistentData.m_Instance.DataAmount >= 100 &&
                        PersistentData.m_Instance.CreditsAmount >= 100 &&
                        PersistentData.m_Instance.PopulationAmount >= 15)
            {
                MigrateBuildings();
                GenerateMap(false);
                AllignWallTurrets();

                PersistentData.m_Instance.DataAmount -= 100;
                PersistentData.m_Instance.CreditsAmount -= 100;
            }
            else if (hit.collider.gameObject.name == "TownHall" &&
                        PersistentData.m_Instance.DataAmount >= 200 &&
                        PersistentData.m_Instance.CreditsAmount >= 200 &&
                        PersistentData.m_Instance.PopulationAmount >= 20)
            {
                MigrateBuildings();
                GenerateMap(false);
                AllignWallTurrets();

                PersistentData.m_Instance.DataAmount -= 200;
                PersistentData.m_Instance.CreditsAmount -= 200;
            }
        }
    }*/

    public void GenerateMap(bool LoadBuildings)
    {
        // Re-generate grid
        GridList.Clear();

        //Find all Wall Objects
        GameObject[] WallToDelete = GameObject.FindGameObjectsWithTag("Wall");

        // Delete them
        foreach (GameObject go in WallToDelete)
        {
            Destroy(go);
        }

        // Find all Grid Objects
        GameObject[] GridToDelete = GameObject.FindGameObjectsWithTag("Grid");

        // Delete them
        foreach (GameObject go in GridToDelete)
        {
            Destroy(go);
        }

        // Reset Production Rates
        GameObject.Find("TownHall").GetComponent<CityManager>().SetDataProductionRate(1);
        GameObject.Find("TownHall").GetComponent<CityManager>().SetCreditsProductionRate(1);
        GameObject.Find("TownHall").GetComponent<CityManager>().SetFoodProductionRate(1);
        GameObject.Find("TownHall").GetComponent<CityManager>().SetPopulationProductionRate(1);
        GameObject.Find("TownHall").GetComponent<CityManager>().SetPowerProductionRate(1);

        // Calc offset
        // Offset = new Vector3((PersistentData.m_Instance.ExpansionLevel * -0.25f) - 0.25f, 0, (PersistentData.m_Instance.ExpansionLevel * -0.25f) - 0.25f); - Used for old expansion code
        Offset = new Vector3(-1.25f, 0, -1.25f);
        //WallOffsetX = new Vector3(-0.5f - (PersistentData.m_Instance.ExpansionLevel - 1) * 0.25f, 0, PersistentData.m_Instance.ExpansionLevel * -0.25f); - Used for old expansion code
        WallOffsetX = new Vector3(-0.5f, 0, -0.25f);
        //WallOffsetY = new Vector3(PersistentData.m_Instance.ExpansionLevel * -0.25f, 0, -0.5f - (PersistentData.m_Instance.ExpansionLevel - 1) * 0.25f); - Used for old expansion code
        WallOffsetY = new Vector3(-0.25f, 0, -0.5f);
        //TowerOffset = new Vector3(-0.5f - (PersistentData.m_Instance.ExpansionLevel - 1) * 0.25f, 0, -0.5f - (PersistentData.m_Instance.ExpansionLevel - 1) * 0.25f); - Used for old expansion code
        TowerOffset = new Vector3(-0.5f, 0, -0.5f);

        i_SizeX = 1 + (4 * 2) + 2;
        i_SizeX = 1 + (4 * 2) + 2;

        //Init list
        for (int i = 0; i < i_SizeX; i++)
        {
            GridList.Add(new List<Vector3>());
        }
               
        //Fill up list + Spawn Grids
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                {
                    GameObject go = Instantiate(Grid, new Vector3(i * 0.25f, 0, j * 0.25f) + Offset, Quaternion.identity) as GameObject;
                    go.SetActive(true);
                    go.transform.parent = TownSpawn.transform;
                }
                GridList[i].Add(new Vector3(i * 0.25f, 0, j * 0.25f) + Offset);
            }
        }

        if (PersistentData.m_Instance.LoadFailed || !LoadBuildings || PersistentData.m_Instance.BuildingName.Count == 0)
        {
            // Spawn Walls and Towers
            for (int i = 0; i < 3; i++) //(int i = 0; i < (i_SizeX - 2); i++) - Used for old expansion code
            {
                GameObject go;
                //Spawn Walls
                if (i == 2)
                {
                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(i * 0.25f, 0.05f, 0) + WallOffsetY) == new Vector2(j, k))
                            {
                                go = Instantiate(RightCWall, new Vector3(i * 0.25f, 0.05f, 0) + WallOffsetY, Quaternion.Euler(0, -90, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(i * 0.25f, 0.05f, 0) + WallOffsetY);
                                go.transform.parent = TownSpawn.transform;
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            }
                        }
                    }

                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(0, 0.05f, i * 0.25f) + WallOffsetX) == new Vector2(j, k))
                            {
                                go = Instantiate(LeftCWall, new Vector3(0.05f, 0.05f, i * 0.25f - 0.03f) + WallOffsetX, Quaternion.Euler(0, 0, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(0, 0.05f, i * 0.25f) + WallOffsetX);
                                go.transform.parent = TownSpawn.transform;
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            }
                        }
                    }


                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(i * 0.25f, 0.05f, 1) + WallOffsetY) == new Vector2(j, k))
                            {
                                go = Instantiate(LeftCWall, new Vector3(i * 0.25f, 0.05f, 0.95f) + WallOffsetY, Quaternion.Euler(0, 90, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(i * 0.25f, 0.05f, 1) + WallOffsetY);
                                go.transform.parent = TownSpawn.transform;
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            }
                        }
                    }


                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(1, 0.05f, i * 0.25f) + WallOffsetX) == new Vector2(j, k))
                            {
                                go = Instantiate(RightCWall, new Vector3(1, 0.05f, i * 0.25f - 0.03f) + WallOffsetX, Quaternion.Euler(0, 180, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(1, 0.05f, i * 0.25f) + WallOffsetX);
                                go.SetActive(true);
                                go.transform.parent = TownSpawn.transform;
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            }
                        }
                    }
                }

                if (i == 0) //(i == (i_SizeX - 2) - 1) - Used for old expansion code
                {
                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(i * 0.25f, 0.05f, 0) + WallOffsetY) == new Vector2(j, k))
                            {
                                go = Instantiate(LeftCWall, new Vector3(i * 0.25f, 0.05f, 0) + WallOffsetY, Quaternion.Euler(0, -90, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(i * 0.25f, 0.05f, 0) + WallOffsetY);
                                go.transform.parent = TownSpawn.transform;
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            }
                        }
                    }

                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(0, 0.05f, i * 0.25f) + WallOffsetX) == new Vector2(j, k))
                            {
                                go = Instantiate(RightCWall, new Vector3(0.05f, 0.05f, i * 0.25f - 0.03f) + WallOffsetX, Quaternion.Euler(0, 0, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(0, 0.05f, i * 0.25f) + WallOffsetX);
                                go.transform.parent = TownSpawn.transform;
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            }
                        }
                    }


                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(i * 0.25f, 0.05f, 1) + WallOffsetY) == new Vector2(j, k))
                            {
                                go = Instantiate(RightCWall, new Vector3(i * 0.25f, 0.05f, 0.95f) + WallOffsetY, Quaternion.Euler(0, 90, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(i * 0.25f, 0.05f, 1) + WallOffsetY);
                                go.transform.parent = TownSpawn.transform;
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            }
                        }
                    }


                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(1, 0.05f, i * 0.25f) + WallOffsetX) == new Vector2(j, k))
                            {
                                go = Instantiate(LeftCWall, new Vector3(1, 0.05f, i * 0.25f - 0.03f) + WallOffsetX, Quaternion.Euler(0, 180, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(1, 0.05f, i * 0.25f) + WallOffsetX);
                                go.SetActive(true);
                                go.transform.parent = TownSpawn.transform;
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            }
                        }
                    }
                }

                if (i == 1) //(i != (i_SizeX - 2) - 1 && i != 0 && PersistentData.m_Instance.ExpansionLevel == 1) - Used for old expansion code
                {
                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(i * 0.25f, 0.05f, 0) + WallOffsetY) == new Vector2(j, k))
                            {
                                go = Instantiate(Wall, new Vector3(i * 0.25f, 0.05f, 0) + WallOffsetY, Quaternion.Euler(0, -90, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(i * 0.25f, 0.05f, 0) + WallOffsetY);
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                                go.transform.parent = TownSpawn.transform;
                            }
                        }
                    }

                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(0, 0.05f, i * 0.25f) + WallOffsetX) == new Vector2(j, k))
                            {
                                go = Instantiate(Wall, new Vector3(0.05f, 0.05f, i * 0.25f - 0.03f) + WallOffsetX, Quaternion.Euler(0, 0, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(0, 0.05f, i * 0.25f) + WallOffsetX);
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                                go.transform.parent = TownSpawn.transform;
                            }
                        }
                    }

                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(i * 0.25f, 0.05f, 1) + WallOffsetY) == new Vector2(j, k))
                            {
                                go = Instantiate(Wall, new Vector3(i * 0.25f, 0.05f, 0.95f) + WallOffsetY, Quaternion.Euler(0, 90, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(i * 0.25f, 0.05f, 1) + WallOffsetY);
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                                go.transform.parent = TownSpawn.transform;
                            }
                        }
                    }

                    for (int j = 0; j < i_SizeX - 1; j++)
                    {
                        //Since it reads bottom-up, should check descending from 11
                        for (int k = 0; k < i_SizeX - 1; k++)
                        {
                            if (CSVReader.loadedMap[j, k] == 0 && GetGridPos(new Vector3(1, 0.05f, i * 0.25f) + WallOffsetX) == new Vector2(j, k))
                            {
                                go = Instantiate(Wall, new Vector3(1, 0.05f, i * 0.25f - 0.03f) + WallOffsetX, Quaternion.Euler(0, 180, 0)) as GameObject;
                                go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                                go.GetComponent<BuildingAbstractBase>().GridPos = GetGridPos(new Vector3(1, 0.05f, i * 0.25f) + WallOffsetX);
                                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                                go.transform.parent = TownSpawn.transform;
                            }
                        }
                    }
                }
            }

            //Spawn Towers
            //Bottom Left
            GameObject kek;
            for (int j = 0; j < i_SizeX - 1; j++)
            {
                //Since it reads bottom-up, should check descending from 11
                for (int k = 0; k < i_SizeX - 1; k++)
                {
                    if (CSVReader.loadedMap[j, k] == 0 && GetWithinGridPos(new Vector3(-0.25f, 0.05f, 0) + WallOffsetY) == new Vector2(j, k))
                    {
                        kek = Instantiate(Tower, new Vector3(-0.25f, 0.15f, -0.02f) + WallOffsetY, Quaternion.identity) as GameObject;
                        kek.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        kek.GetComponent<BuildingAbstractBase>().GridPos = GetWithinGridPos(new Vector3(-0.25f, 0.05f, 0) + WallOffsetY);
                        kek.transform.parent = TownSpawn.transform;
                        kek.GetComponent<BuildingAbstractBase>().IsPreview = false;
                    }
                }
            }


            //Top Right
            for (int j = 0; j < i_SizeX - 1; j++)
            {
                //Since it reads bottom-up, should check descending from 11
                for (int k = 0; k < i_SizeX - 1; k++)
                {
                    if (CSVReader.loadedMap[j, k] == 0 && GetWithinGridPos(new Vector3(1, 0.05f, 1) + TowerOffset) == new Vector2(j, k))
                    {
                        kek = Instantiate(Tower, new Vector3(1, 0.15f, 0.98f) + TowerOffset, Quaternion.identity) as GameObject;
                        kek.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        kek.GetComponent<BuildingAbstractBase>().GridPos = GetWithinGridPos(new Vector3(1, 0.05f, 1) + TowerOffset);
                        kek.transform.parent = TownSpawn.transform;
                        kek.GetComponent<BuildingAbstractBase>().IsPreview = false;
                    }
                }
            }

            //Top Left
            for (int j = 0; j < i_SizeX - 1; j++)
            {
                //Since it reads bottom-up, should check descending from 11
                for (int k = 0; k < i_SizeX - 1; k++)
                {
                    if (CSVReader.loadedMap[j, k] == 0 && GetWithinGridPos(new Vector3(-0.25f, 0.05f, 1) + WallOffsetY) == new Vector2(j, k))
                    {
                        kek = Instantiate(Tower, new Vector3(-0.25f, 0.15f, 0.96f) + WallOffsetY, Quaternion.identity) as GameObject;
                        kek.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        kek.GetComponent<BuildingAbstractBase>().GridPos = GetWithinGridPos(new Vector3(-0.25f, 0.05f, 1) + WallOffsetY);
                        kek.transform.parent = TownSpawn.transform;
                        kek.GetComponent<BuildingAbstractBase>().IsPreview = false;
                    }
                }
            }

            //Bottom Right
            for (int j = 0; j < i_SizeX - 1; j++)
            {
                //Since it reads bottom-up, should check descending from 11
                for (int k = 0; k < i_SizeX - 1; k++)
                {
                    if (CSVReader.loadedMap[j, k] == 0 && GetWithinGridPos(new Vector3(1, 0.05f, -0.25f) + WallOffsetX) == new Vector2(j, k))
                    {
                        kek = Instantiate(Tower, new Vector3(1, 0.15f, -0.27f) + WallOffsetX, Quaternion.identity) as GameObject;
                        kek.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        kek.GetComponent<BuildingAbstractBase>().GridPos = GetWithinGridPos(new Vector3(1, 0.05f, -0.25f) + WallOffsetX);
                        kek.transform.parent = TownSpawn.transform;
                        kek.GetComponent<BuildingAbstractBase>().IsPreview = false;
                    }
                }
            }
        }

        if (LoadBuildings)
        {
            //Check against CSV to place obstacles
            for (int i = 0; i < i_SizeX - 1; i++)
            {
                //Since it reads bottom-up, should check descending from 11
                for (int j = 0; j < i_SizeX - 1; j++)
                {
                    GameObject[] obs = GameObject.FindGameObjectsWithTag("Grid");
                    //Grass = 1 Kind
                    //Desert = 4 Kinds
                    //Ruins = 4 Kinds
                    for (int k = 0; k < obs.Length; k++)
                    {
                        if (CSVReader.loadedMap[i, j] != 0 && GetGridPos(obs[k].transform.position) == new Vector2(i, j))
                        {
                            Destroy(obs[k]);
                            if (CSVReader.loadedMap[i, j] != 50 && GetGridPos(obs[k].transform.position) == new Vector2(i, j))
                            {
                                if (PersistentData.m_Instance.LevelToLoad == PersistentData.LEVEL_TYPE.GRASS)
                                {
                                    GameObject go = Instantiate(ObstacleGrass, obs[k].transform.position, Quaternion.Euler(0, Random.Range(-3600, 3600), 0)) as GameObject;
                                    go.transform.parent = this.transform;
                                }
                                if (PersistentData.m_Instance.LevelToLoad == PersistentData.LEVEL_TYPE.DESERT)
                                {
                                    float randomVal = Random.value;
                                    if (randomVal < 0.25f)
                                    {
                                        GameObject go = Instantiate(ObstacleDesert1, obs[k].transform.position, Quaternion.Euler(0, Random.Range(-3600, 3600), 0)) as GameObject;
                                        go.transform.parent = this.transform;
                                    }
                                    else if (randomVal < 0.50f)
                                    {
                                        GameObject go = Instantiate(ObstacleDesert2, obs[k].transform.position, Quaternion.Euler(0, Random.Range(-3600, 3600), 0)) as GameObject;
                                        go.transform.parent = this.transform;
                                    }
                                    else if (randomVal < 0.75f)
                                    {
                                        GameObject go = Instantiate(ObstacleDesert3, obs[k].transform.position, Quaternion.Euler(0, Random.Range(-3600, 3600), 0)) as GameObject;
                                        go.transform.parent = this.transform;
                                    }
                                    else
                                    {
                                        GameObject go = Instantiate(ObstacleDesert4, obs[k].transform.position, Quaternion.Euler(0, Random.Range(-3600, 3600), 0)) as GameObject;
                                        go.transform.parent = this.transform;
                                    }
                                }
                                if (PersistentData.m_Instance.LevelToLoad == PersistentData.LEVEL_TYPE.RUINS)
                                {
                                    float randomVal = Random.value;
                                    if (randomVal < 0.25f)
                                    {
                                        GameObject go = Instantiate(ObstacleRuins1, obs[k].transform.position, Quaternion.Euler(0, Random.Range(-3600, 3600), 0)) as GameObject;
                                        go.transform.parent = this.transform;
                                    }
                                    else if (randomVal < 0.50f)
                                    {
                                        GameObject go = Instantiate(ObstacleRuins2, obs[k].transform.position, Quaternion.Euler(0, Random.Range(-3600, 3600), 0)) as GameObject;
                                        go.transform.parent = this.transform;
                                    }
                                    else if (randomVal < 0.75f)
                                    {
                                        GameObject go = Instantiate(ObstacleRuins3, obs[k].transform.position, Quaternion.Euler(0, Random.Range(-3600, 3600), 0)) as GameObject;
                                        go.transform.parent = this.transform;
                                    }
                                    else
                                    {
                                        GameObject go = Instantiate(ObstacleRuins4, obs[k].transform.position, Quaternion.Euler(0, Random.Range(-3600, 3600), 0)) as GameObject;
                                        go.transform.parent = this.transform;
                                    }
                                }
                            }                           
                        }
                    }
                }
            }
            // Spawn Buildings
            for (int i = 0; i < PersistentData.m_Instance.BuildingName.Count; ++i)
            {

                Vector2 temp = PersistentData.m_Instance.BuildingGridPos[i].GetVec2();

                Quaternion Rotate = new Quaternion();
                float VerticalDist = Mathf.Abs(temp.y - GridList.Count / 2);
                float HorizontalDist = Mathf.Abs(temp.x - GridList.Count / 2);

                if (VerticalDist >= HorizontalDist)
                {
                    // Bottom
                    if (temp.y < GridList.Count / 2)
                    {
                        Rotate = Quaternion.Euler(0, 270, 0);
                    }
                    // Top
                    if (temp.y > GridList.Count / 2)
                    {
                        Rotate = Quaternion.Euler(0, 90, 0);
                    }
                }
                else
                {
                    // Left
                    if (temp.x < GridList.Count / 2)
                    {
                        Rotate = Quaternion.Euler(0, 0, 0);
                    }
                    // Right
                    if (temp.x > GridList.Count / 2)
                    {
                        Rotate = Quaternion.Euler(0, 180, 0);
                    }
                }

                switch (PersistentData.m_Instance.BuildingName[i])
                {
                    case "CreditsGenerator":
                        GameObject go = Instantiate(CreditsGenerator, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "DataGenerator":
                        go = Instantiate(DataGenerator, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "Generator":
                        go = Instantiate(PowerGenerator, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "Farm":
                        go = Instantiate(FoodGenerator, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "Housing":
                        go = Instantiate(Housing, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "Storage":
                        go = Instantiate(Storage, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "Wall":
                        go = Instantiate(Wall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "LeftCWall":
                        go = Instantiate(LeftCWall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "LeftWall":
                        go = Instantiate(LeftWall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "RightCWall":
                        go = Instantiate(RightCWall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "RightWall":
                        go = Instantiate(RightWall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "ConnectorWall":
                        go = Instantiate(Tower, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.15f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();
                        break;

                    case "Housing_Food":
                        go = Instantiate(Housing, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();

                        go.transform.GetChild(2).gameObject.SetActive(true);
                        go.GetComponent<Residential>().theAddOn = go.transform.GetChild(2).gameObject.GetComponent<BuildingAddOn>();
                        break;

                    case "Housing_Power":
                        go = Instantiate(Housing, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();

                        go.transform.GetChild(1).gameObject.SetActive(true);
                        go.GetComponent<Residential>().theAddOn = go.transform.GetChild(1).gameObject.GetComponent<BuildingAddOn>();
                        break;

                    case "StandardTurret":
                        {
                            go = Instantiate(StandardTurret, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                            go.GetComponent<BaseTurret>().GridPos = temp;
                            go.transform.parent = this.transform;
                            go.GetComponent<BaseTurret>().IsPreview = false;
                            go.GetComponent<BaseTurret>().TurretActive = true;

                            MaterialSwitch[] ComponentList = go.GetComponentsInChildren<MaterialSwitch>();
                            foreach (MaterialSwitch ms in ComponentList)
                            {
                                ms.SwitchToSecondMaterial();
                            }
                            break;
                        }

                    case "SniperTurret":
                        {
                            go = Instantiate(SniperTurret, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                            go.GetComponent<BaseTurret>().GridPos = temp;
                            go.transform.parent = this.transform;
                            go.GetComponent<BaseTurret>().IsPreview = false;
                            go.GetComponent<BaseTurret>().TurretActive = true;

                            MaterialSwitch[] ComponentList = go.GetComponentsInChildren<MaterialSwitch>();
                            foreach (MaterialSwitch ms in ComponentList)
                            {
                                ms.SwitchToSecondMaterial();
                            }
                            break;
                        }

                    case "HeavyTurret":
                        {
                            go = Instantiate(HeavyTurret, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                            go.GetComponent<BaseTurret>().GridPos = temp;
                            go.transform.parent = this.transform;
                            go.GetComponent<BaseTurret>().IsPreview = false;
                            go.GetComponent<BaseTurret>().TurretActive = true;

                            MaterialSwitch[] ComponentList = go.GetComponentsInChildren<MaterialSwitch>();
                            foreach (MaterialSwitch ms in ComponentList)
                            {
                                ms.SwitchToSecondMaterial();
                            }
                            break;
                        }
                }

                // Wall Turrets
                if (PersistentData.m_Instance.BuildingName[i].Contains("+"))
                {
                    string[] StrArr = PersistentData.m_Instance.BuildingName[i].Split('+');

                    GameObject go;
                    GameObject WallObject = null;

                    // Wall Section
                    switch (StrArr[0])
                    {
                        case "Wall":
                            go = Instantiate(Wall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                            go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                            go.transform.parent = this.transform;
                            go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();

                            WallObject = go;
                            break;

                        case "LeftCWall":
                            go = Instantiate(LeftCWall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                            go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                            go.transform.parent = this.transform;
                            go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();

                            WallObject = go;
                            break;

                        case "LeftWall":
                            go = Instantiate(LeftWall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                            go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                            go.transform.parent = this.transform;
                            go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();

                            WallObject = go;
                            break;

                        case "RightCWall":
                            go = Instantiate(RightCWall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                            go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                            go.transform.parent = this.transform;
                            go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();

                            WallObject = go;
                            break;

                        case "RightWall":
                            go = Instantiate(RightWall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity * Rotate) as GameObject;
                            go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                            go.transform.parent = this.transform;
                            go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                            go.GetComponentInChildren<MaterialSwitch>().SwitchToSecondMaterial();

                            WallObject = go;
                            break;
                    }

                    // Turret Section
                    if (StrArr[1].Contains("Standard"))
                    {
                        go = Instantiate(GetComponent<ConstructionManager>().WallStandardTurret, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.15f, 0), Quaternion.identity * Rotate * Quaternion.Euler(0, -90, 0)) as GameObject;
                        go.GetComponent<BaseTurret>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BaseTurret>().IsPreview = false;
                        go.GetComponent<BaseTurret>().TurretActive = true;

                        MaterialSwitch[] ComponentList = go.GetComponentsInChildren<MaterialSwitch>();
                        foreach (MaterialSwitch ms in ComponentList)
                        {
                            ms.SwitchToSecondMaterial();
                        }

                        go.GetComponent<BaseTurret>().AttachedWall = WallObject.GetComponent<BuildingAbstractBase>();
                    }
                    else if (StrArr[1].Contains("Sniper"))
                    {
                        go = Instantiate(GetComponent<ConstructionManager>().WallSniperTurret, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.15f, 0), Quaternion.identity * Rotate * Quaternion.Euler(0, -90, 0)) as GameObject;
                        go.GetComponent<BaseTurret>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BaseTurret>().IsPreview = false;
                        go.GetComponent<BaseTurret>().TurretActive = true;

                        MaterialSwitch[] ComponentList = go.GetComponentsInChildren<MaterialSwitch>();
                        foreach (MaterialSwitch ms in ComponentList)
                        {
                            ms.SwitchToSecondMaterial();
                        }

                        go.GetComponent<BaseTurret>().AttachedWall = WallObject.GetComponent<BuildingAbstractBase>();
                    }
                    else if (StrArr[1].Contains("Heavy"))
                    {
                        go = Instantiate(GetComponent<ConstructionManager>().WallHeavyTurret, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.15f, 0), Quaternion.identity * Rotate * Quaternion.Euler(0, -90, 0)) as GameObject;
                        go.GetComponent<BaseTurret>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BaseTurret>().IsPreview = false;
                        go.GetComponent<BaseTurret>().TurretActive = true;

                        MaterialSwitch[] ComponentList = go.GetComponentsInChildren<MaterialSwitch>();
                        foreach (MaterialSwitch ms in ComponentList)
                        {
                            ms.SwitchToSecondMaterial();
                        }

                        go.GetComponent<BaseTurret>().AttachedWall = WallObject.GetComponent<BuildingAbstractBase>();
                    }
                }

                // Destroyed Buildings
                if (PersistentData.m_Instance.BuildingName[i].Contains("_"))
                {
                    string[] StrArr = PersistentData.m_Instance.BuildingName[i].Split('_');

                    GameObject go;

                    if ((StrArr[1].Contains("Standard") || StrArr[1].Contains("Sniper") || StrArr[1].Contains("Heavy")))
                    {
                        // Turret
                        go = Instantiate(DestroyedTurret, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BaseTurret>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BaseTurret>().IsPreview = false;
                    }
                    else if (StrArr[1].Contains("Wall"))
                    {
                        // Wall
                        go = Instantiate(DestroyedWall, GridList[(int)temp.x][(int)temp.y] + new Vector3(0, 0.05f, 0), Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                    }
                    else
                    {
                        // Building
                        go = Instantiate(DestroyedBuilding, GridList[(int)temp.x][(int)temp.y], Quaternion.identity) as GameObject;
                        go.GetComponent<BuildingAbstractBase>().GridPos = temp;
                        go.transform.parent = this.transform;
                        go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                    }

                    go.GetComponent<DestroyedBuilding>().PreviousName = StrArr[1];
                }
            }
        }
    }

    public void DeleteBuildings()
    {
        //Find all Tower Objects
        GameObject[] TowerToDelete = GameObject.FindGameObjectsWithTag("Tower");

        // Delete them
        foreach (GameObject go in TowerToDelete)
        {
            Destroy(go);
        }

        // Find all Buildings Objects
        GameObject[] ToDelete = GameObject.FindGameObjectsWithTag("Buildings");

        // Delete them
        foreach (GameObject go in ToDelete)
        {
            if (go.name.Equals("TownHall"))
                continue;

            Destroy(go);
        }
    }

    void Reset()
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

    // Function to handle re-alligning the buildings when the grid is expanded 
    public void MigrateBuildings()
    {
        GameObject[] AllBuildings = GameObject.FindGameObjectsWithTag("Buildings");
        GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");

        GameObject[] ToMigrate = (AllBuildings.Union<GameObject>(AllTowers)).ToArray<GameObject>();

        int i = 0;
        foreach (GameObject go in ToMigrate)
        {
            if (go.tag.Contains("Buildings"))
            {
                go.GetComponent<BuildingAbstractBase>().GridPos += new Vector2(1, 1);

                if (!go.name.Equals("TownHall"))
                {
                    PersistentData.m_Instance.BuildingGridPos[i] = new SerializableVector2(go.GetComponent<BuildingAbstractBase>().GridPos);
                    ++i;
                }
            }
            else
            {
                if (!go.GetComponent<BaseTurret>())
                    continue;

                if (!go.name.Contains("Wall"))
                {
                    go.GetComponent<BaseTurret>().GridPos += new Vector2(1, 1);

                    PersistentData.m_Instance.BuildingGridPos[i] = new SerializableVector2(go.GetComponent<BaseTurret>().GridPos);
                    ++i;
                }
                else
                {
                    if (go.GetComponent<BaseTurret>().GridPos.x == 0)
                    {
                        go.GetComponent<BaseTurret>().GridPos += new Vector2(0, 1);

                        PersistentData.m_Instance.BuildingGridPos[i] = new SerializableVector2(go.GetComponent<BaseTurret>().GridPos);
                        ++i;
                    }
                    else if (go.GetComponent<BaseTurret>().GridPos.x == i_SizeX - 1)
                    {
                        go.GetComponent<BaseTurret>().GridPos += new Vector2(2, 1);

                        PersistentData.m_Instance.BuildingGridPos[i] = new SerializableVector2(go.GetComponent<BaseTurret>().GridPos);
                        ++i;
                    }
                    else if(go.GetComponent<BaseTurret>().GridPos.y == 0)
                    {
                        go.GetComponent<BaseTurret>().GridPos += new Vector2(1, 0);

                        PersistentData.m_Instance.BuildingGridPos[i] = new SerializableVector2(go.GetComponent<BaseTurret>().GridPos);
                        ++i;
                    }
                    else if (go.GetComponent<BaseTurret>().GridPos.y == i_SizeX - 1)
                    {
                        go.GetComponent<BaseTurret>().GridPos += new Vector2(1, 2);

                        PersistentData.m_Instance.BuildingGridPos[i] = new SerializableVector2(go.GetComponent<BaseTurret>().GridPos);
                        ++i;
                    }
                }
            }
        }
    }

    // Function to reallign wall turrets
    void AllignWallTurrets()
    {
        GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");

        foreach (GameObject go in AllTowers)
        {
            if (go.tag.Contains("Tower"))
            {
                if (go.name.Contains("Wall"))
                {
                    Vector3 newVec = new Vector3(GridList[(int)go.GetComponent<BaseTurret>().GridPos.x][(int)go.GetComponent<BaseTurret>().GridPos.y].x, go.transform.position.y, GridList[(int)go.GetComponent<BaseTurret>().GridPos.x][(int)go.GetComponent<BaseTurret>().GridPos.y].z);
                    go.transform.position = newVec;
                }
            }
        }
    }

    public Vector2 GetGridSize()
    {
        return new Vector2(i_SizeX, i_SizeX);
    }

    public float GetGridCost(int x, int y)
    {
        for (int i = 0; i < PersistentData.m_Instance.BuildingGridPos.Count; ++i)
        {
            if (PersistentData.m_Instance.BuildingGridPos[i].Equals(new SerializableVector2(x, y)))
            {
                if (PersistentData.m_Instance.BuildingName[i].Contains("Destroyed"))
                {
                    return 1;
                }

                if (PersistentData.m_Instance.BuildingName[i].Contains("Wall"))
                {
                    return 10;
                }

                if (CSVReader.loadedMap[x,y].Equals(1))
                {
                    return -1;
                }
                
                return 999;
            }
        }

        if (CSVReader.loadedMap[x, y] == 1)
        {
            return -1;
        }

        return 1;
    }

    public Vector3 GetVec3Pos(int x, int y)
    {
        if (x < i_SizeX && x >= 0 && y < i_SizeX && y >= 0)
            return GridList[x][y];

        return new Vector3(0, 0, 0);
    }

    public Vector2 GetGridPos(Vector3 checkPos)
    {
        checkPos.y = 0;
        for (int i = 0; i < i_SizeX; ++i)
        {
            for (int j = 0; j < i_SizeX; ++j)
            {
                if (GridList[i][j].Equals(checkPos))
                {
                    return new Vector2(i, j);
                }
            }
        }

        return new Vector2(-1, -1);
    }

    public Vector2 GetWithinGridPos(Vector3 checkPos)
    {
        checkPos.y = 0;
        for (int i = 0; i < i_SizeX; ++i)
        {
            for (int j = 0; j < i_SizeX; ++j)
            {
                if (checkPos.x >= GridList[i][j].x - 0.125 && checkPos.z >= GridList[i][j].z - 0.125 && checkPos.x <= GridList[i][j].x + 0.125 && checkPos.z <= GridList[i][j].z + 0.125)
                {
                    return new Vector2(i, j);
                }
            }
        }

        return new Vector2(-1, -1);
    }

    public bool CheckIfInGridMap(Vector3 checkPos)
    {

        if (checkPos.x >= GridList[0][0].x - 0.125 && checkPos.z >= GridList[0][0].z - 0.125 && checkPos.x <= GridList[i_SizeX - 1][i_SizeX - 1].x + 0.125 && checkPos.z <= GridList[i_SizeX - 1][i_SizeX - 1].z + 0.125)
            return true;

        return false;
    }


}