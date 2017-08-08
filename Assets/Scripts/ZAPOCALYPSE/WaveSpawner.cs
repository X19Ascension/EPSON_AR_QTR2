using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;

public class WaveSpawner : MonoBehaviour {

    [Tooltip("Spawn Units or Not")]
    [Header("Units to Spawn")]
    public bool spawnZombie;                                        //! Spawn Normal Zombie or Not, true for spawn
    public bool spawnGroupZombie;                                   //! Spawn Grouped Zombie or Not, true for spawn
    public bool spawnArmoredZombie;                                 //! Spawn Armored Zombie or Not, true for spawn
    public bool spawnRangedZombie;                                  //! Spawn Ranged Zombie or Not, true for spawn
    public bool spawnTankZombie;                                    //! Spawn Tank Zombie or Not, true for spawn

    [Header("Spawning Percentages")]
    public float chanceToSpawnGroup = 100;                          //! Percentage Rate of Spawning Group (Set to 0 if want normal spawn)
    public float chanceToSpawnArmored = 100;                        //! Percentage Rate of Spawning Armored (Set to 0 if want normal spawn)
    public float chanceToSpawnRanged = 100;                         //! Percentage Rate of Spawning Ranged (Set to 0 if want normal spawn)
    public float chanceToSpawnTank = 100;                           //! Percentage Rate of Spawning Tank (Set to 0 if want normal spawn)

    [Header("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();     //! The number of Spawn Points in the Game
    [Tooltip("The current Wave Number")]
    [Header("Current Wave No.")]
    public int waveNo;                                              //! The Current Wave number.
    public int maxWaveNo;                                           //! The Max No. Wave number.

    [Header("Unit's Game Object")]
    public GameObject zombieGO;                                     //! Standard Zombie Game Object
    public GameObject armoredzombieGO;                                     //! Standard Armored Zombie Game Object
    public GameObject fastzombieGO;                                 //! Fast Zombie Game Object
    public GameObject tankzombieGO;                                 //! Tank Zombie Game Object
    public GameObject rangezombieGO;                                //! Ranged Zombie Game Object
    public GameObject spawnerGO;                                    //! Spawner Game Object

    [Header("Conditions for Tank Spawn")]
    public int amtToKill4Tank;                                      //! Amount of Standard to kill for Tank to Spawn
    public int killcount;                                           //! Total Kill count player has

    [Header("Spawn Value for a wave")]
    public float spawnValue = 9999.0f;                               //! Spawn Value for each wave.

    public int minutePerWave;                                       //! Wave Time Limit in minutes. 
    public int secondPerWave;                                       //! Wave Time Limit in seconds.

    private float randomModifier;                                   //! randomModifier
    private float randomSpawnTimer = 1.5f;                          //! Random Spawn Value

    public int limitAmount;                                         //! Amount of Zombies allowed at each time
    [HideInInspector]
    public int maxAmount;                                           //! Max Amount of Zombies allowed

    public CSVLoadLevel CSVReader;                                  //! To read and load the CSV file for spawning

    private int currSpawnPt;                                        //! Current Spawn Point of the Spawner

    // Difficulty Modifier
    public enum TEMP_DIFF                                           //! Temporary Difficulty Setting
    {
        EASY = 0,
        NORMAL,
        HARD,
    }

    public TEMP_DIFF diff = TEMP_DIFF.NORMAL;
    private float difficultyMod;                                    //! Difficulty Modifier for Zombies Health

    private float testTimeDelta;                                    //! Rate of spawning between zombies. //Default
    private float testTankSpawn;                                    //! Debug Test value
    [HideInInspector]
    public float waveDuration;                                     //! Duration of Wave passed
    [HideInInspector]
    public float waveDurationSave;                                 //! For Gamecontrol Saving

    public bool waveEnded;                                          //! Check if wave has ended.

    public GameObject OriginPoint;                                  //! Spawner's Point of Origin

    private GameControl gameControl;                                //! Game Control to load/save data
    public GameObject OriginPOint;//! Spawner Game Object

    // Use this for initialization
    void Start () {
        OriginPOint = GameObject.Find("CityTerrain");
       // this.transform.parent = OriginPOint.transform;

        gameControl = GameObject.Find("GameControl").GetComponent<GameControl>();
        spawnerGO.GetComponent<WaveSpawner>().maxAmount = 0;
        switch (diff)
        {
            case TEMP_DIFF.EASY:
                difficultyMod = 0.75f;
                maxWaveNo = 12;
                break;
            case TEMP_DIFF.NORMAL:
                difficultyMod = 1.0f;
                maxWaveNo = 15;
                break;
            case TEMP_DIFF.HARD:
                difficultyMod = 1.25f;
                maxWaveNo = 17;
                break;
        }
        if (gameControl != null)
            difficultyMod = gameControl.difficulty;

        waveDuration = minutePerWave * 60 + secondPerWave;
        waveDurationSave = waveDuration;
        waveEnded = false;
        if (gameControl == null)
            waveNo = 0;
        else
            waveNo = gameControl.waveNo - 1;

        chanceToSpawnGroup *= 0.01f;
        chanceToSpawnArmored *= 0.01f;
        chanceToSpawnRanged *= 0.01f;
        chanceToSpawnTank *= 0.01f;
    }
	
	// Update is called once per frame
	void Update () {
        SetOriginPoint();
        testTimeDelta += Time.deltaTime;
        waveDuration -= Time.deltaTime;
        //Debug.Log(waveDuration);

        testTankSpawn += Time.deltaTime;

        if (waveDuration > 0)
        {
            if (CheckAnyAlive() == true && spawnerGO.GetComponent<WaveSpawner>().maxAmount < limitAmount)
            {
                if (spawnValue >= 0)
                {
                    if (testTimeDelta >= randomSpawnTimer)
                    {

                        for (int i = 0; i < currSpawnPt; i++)
                            SpawnZombie(LoadSpawnPoint());

                        if (currSpawnPt > 5/* && (chanceToSpawnGroup > Random.Range(0, 1))*/)
                            SpawnHorde(LoadSpawnPoint());

                        //if (currSpawnPt > 8/* && (chanceToSpawnArmored > Random.Range(0, 1))*/)
                        //    SpawnArmoredZombie(LoadSpawnPoint());

                        if (currSpawnPt > 11/* && (chanceToSpawnRanged > Random.Range(0, 1))*/)
                            SpawnRangedZombie(LoadSpawnPoint());

                        //SpawnZombie(GenerateSpawnPos());
                        randomSpawnTimer = Random.Range(2.5f, 3.2f);

                        if (currSpawnPt > 14)
                            currSpawnPt = Random.Range(8,14);

                        testTimeDelta = 0f;
                        currSpawnPt++;



                    }
                    if (spawnerGO.GetComponent<WaveSpawner>().killcount >= amtToKill4Tank && spawnTankZombie && waveNo >= 6 )
                    {
                        SpawnTankZombie(GenerateSpawnPos());
                        spawnerGO.GetComponent<WaveSpawner>().killcount = 0;
                    }
                }
                
            }
        }
        else if (waveNo <= maxWaveNo && waveEnded == false)
        {
            waveNo++;
            waveEnded = true;
            spawnValue = 9999.0f;
            Debug.Log("Wave " + waveNo + " Ended");

            LevelManagement pewpew = GameObject.Find("testupgrade").GetComponent<LevelManagement>();
            pewpew.ChangeLevel(LevelManagement.LEVEL.UPGRADE);

            // Handle These Elsewhere
            //waveDuration = waveDurationSave;
            //waveEnded = false;
        }

    }

    // Goes through all the list to check if any survivor is alive, if nothing is alive, don't spawn it.
    protected bool CheckAnyAlive()
    {
        GameObject[] AllEntities = GameObject.FindGameObjectsWithTag("Entities");
        GameObject[] AllSurvivors = GameObject.FindGameObjectsWithTag("Survivor");
        GameObject[] AllBarriers = GameObject.FindGameObjectsWithTag("Barrier");
        // Get available targets
        GameObject[] AvailableTargets = ((AllEntities.Union<GameObject>(AllSurvivors)).Union<GameObject>(AllBarriers)).ToArray<GameObject>();//GameObject.FindGameObjectsWithTag("Survivor");

        if (AvailableTargets != null)
            return true;

        return false; // If none, it will return the null it was assigned with.
    }

    Vector3 GenerateSpawnPos()
    {
        int randSpawn = Random.Range(0, 4);
        Vector3 temp;
        temp = spawnPoints[randSpawn].transform.position;
        

        if (randSpawn == 0 || randSpawn == 1)
            temp.x = Random.Range(-30, 30);
        else
            temp.z = Random.Range(-30, 30);

        return temp;
    }

    void TweakStats(GameObject go)
    {
        if (diff != TEMP_DIFF.NORMAL)
        {
            if (difficultyMod <= 0)
                difficultyMod = 1;
            randomModifier = Random.Range(0.7f, 1.3f);
            go.GetComponent<Zombie>().HP = (int)((float)(go.GetComponent<Zombie>().HP + (waveNo * 6)) * difficultyMod);
            go.GetComponent<Zombie>().i_maxHP = go.GetComponent<Zombie>().HP;
            if (go.GetComponent<Zombie>().atkDmg > 3)
                go.GetComponent<Zombie>().atkDmg = (int)((float)(go.GetComponent<Zombie>().atkDmg + waveNo) * difficultyMod);
            go.GetComponent<Zombie>().moveSpd *= randomModifier;
        }

    }

    void SpawnZombie(Vector3 spawnPos)
    {
        if (spawnZombie == true)
        {
            GameObject go = Instantiate(zombieGO, spawnPos, Quaternion.identity) as GameObject;
            TweakStats(go);
            GameObject it = GameObject.FindGameObjectWithTag("ImageTarget");
            go.transform.SetParent(it.transform);
            spawnValue -= 1;
            //Debug.Log("Zombie Spawn");
            spawnerGO.GetComponent<WaveSpawner>().maxAmount++;
            //Debug.Log(spawnerGO.GetComponent<WaveSpawner>().maxAmount);
        }
    }

    void SpawnHorde(Vector3 spawnPos)
    {
        if (spawnGroupZombie == true)
        {
            int randAmt = Random.Range(3, 6);

            for (int i = 0; i < randAmt; i++)
            {
                spawnPos.x = Random.Range(-3.0f, 3.0f);
                spawnPos.z = Random.Range(-3.0f, 3.0f);
                GameObject go = Instantiate(fastzombieGO, spawnPos, Quaternion.identity) as GameObject;
                GameObject it = GameObject.FindGameObjectWithTag("ImageTarget");
                go.transform.SetParent(it.transform);
                TweakStats(go);
                spawnerGO.GetComponent<WaveSpawner>().maxAmount++;
            }
            spawnValue -= 1;
            //Debug.Log("Horde Spawn");
        }
        //testTimeDelta = 0f;
    }

    void SpawnTankZombie(Vector3 spawnPos)
    {
        if (spawnTankZombie == true)
        {
            GameObject go = Instantiate(tankzombieGO, spawnPos, Quaternion.identity) as GameObject;
            go.gameObject.transform.parent = OriginPoint.transform;
            TweakStats(go);
            GameObject it = GameObject.FindGameObjectWithTag("ImageTarget");
            go.transform.SetParent(it.transform);
            Debug.Log("Tank Zombie Spawn");
            spawnerGO.GetComponent<WaveSpawner>().maxAmount++;
        }
    }

    void SpawnRangedZombie(Vector3 spawnPos)
    {
        if (spawnRangedZombie == true)
        {
            GameObject go = Instantiate(rangezombieGO, spawnPos, Quaternion.identity) as GameObject;
            go.gameObject.transform.parent = OriginPoint.transform;
            TweakStats(go);
            GameObject it = GameObject.FindGameObjectWithTag("ImageTarget");
            go.transform.SetParent(it.transform);
            Debug.Log("Ranged Zombie Spawn");
            spawnerGO.GetComponent<WaveSpawner>().maxAmount++;
        }
    }

    void SpawnArmoredZombie(Vector3 spawnPos)
    {
        if (spawnArmoredZombie == true)
        {
            GameObject go = Instantiate(armoredzombieGO, spawnPos, Quaternion.identity) as GameObject;
            go.gameObject.transform.parent = OriginPoint.transform;
            TweakStats(go);
            go.transform.parent = this.transform.parent;

            Debug.Log("Armored Zombie Spawn");
            spawnerGO.GetComponent<WaveSpawner>().maxAmount++;
        }
    }

    public void SetWaveDuration()
    {
        waveDuration = waveDurationSave;
    }

    public Vector3 LoadSpawnPoint()
    {
        //if (waveEnded)
        //{
        for (int j = 0; j < (15 - 1); j++)
        {
            for (int k = 0; k < (17 - 1); k++)
            {
                if (j == waveNo && k == currSpawnPt)
                {
                    //Debug.Log(CSVReader.loadedMap[j, i]);
                    if (CSVReader.loadedMap[k, j] >= 0)
                    {
                        //Debug.Log("The X: " + k + " - The Y: " + j);
                        Vector3 temp;
                        temp = spawnPoints[CSVReader.loadedMap[k, j]].transform.position;
                        temp.x += Random.Range(-6.5f, 6.5f);
                        temp.z += Random.Range(-6.5f, 6.5f);
                        return temp;
                    }
                }
                }
            }
        //}

        return new Vector3(0, 0, 0);
    }

    void SetOriginPoint()
    {
        for (int i = 0; i < OriginPoint.transform.childCount; i++) 
        {
            OriginPoint.transform.GetChild(i).gameObject.transform.parent = OriginPoint.transform;
        }
    }

    public int  GetSpawnPoint()
    {
        return currSpawnPt;
    }
}