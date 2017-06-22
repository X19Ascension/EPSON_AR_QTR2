using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;

public class WaveSpawner : MonoBehaviour {

    [Tooltip("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();     //! The number of Spawn Points in the Game
    [Tooltip("The current Wave Number")]
    public int waveNo;                                              //! The Current Wave number.
    public int maxWaveNo;                                           //! The Max No. Wave number.

    public GameObject zombieGO;                                     //! Standard Zombie Game Object
    public GameObject fastzombieGO;                                 //! Fast Zombie Game Object
    public GameObject tankzombieGO;                                 //! Tank Zombie Game Object
    public GameObject rangezombieGO;                                //! Ranged Zombie Game Object
    public GameObject spawnerGO;                                    //! Spawner Game Object

    public int amtToKill4Tank;                                      //! Amount of Standard to kill for Tank to Spawn
    public int killcount;                                           //! Total Kill count player has
    public float spawnValue = 150.0f;                               //! Spawn Value for each wave.

    public int minutePerWave;                                       //! Wave Time Limit in minutes. 
    public int secondPerWave;                                       //! Wave Time Limit in seconds.

    private float randomModifier;                                   //! randomModifier
    private float randomSpawnTimer = 1.5f;

    public int limitAmount;
    [HideInInspector]
    public int maxAmount;

    public CSVLoadLevel CSVReader;

    private int currSpawnPt;

    // Difficulty Modifier
    public enum TEMP_DIFF                                           //! Temporary Difficulty Setting
    {
        EASY = 0,
        NORMAL,
        HARD,
    }

    public TEMP_DIFF diff = TEMP_DIFF.NORMAL;
    private float difficultyMod;

    private float testTimeDelta;
    private float testTankSpawn;
    private float waveDuration;
    private float waveDurationSave;

    public bool waveEnded;

    // Use this for initialization
    void Start () {
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

        waveDuration = minutePerWave * 60 + secondPerWave;
        waveDurationSave = waveDuration;
        waveEnded = false;
    }
	
	// Update is called once per frame
	void Update () {
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
                        SpawnZombie(LoadSpawnPoint());
                        currSpawnPt++;
                        if (currSpawnPt > 15)
                            currSpawnPt = 0;

                        //SpawnZombie(GenerateSpawnPos());
                        randomSpawnTimer = Random.Range(1.5f, 2.2f);

                        testTimeDelta = 0f;
                    }
                    //if (testTankSpawn >= 6.0f && spawnerGO.GetComponent<WaveSpawner>().killcount >= 10)
                    //{
                    //    for (int i = 0; i < 5; i++)
                    //        SpawnHorde(GenerateSpawnPos());

                    //    testTankSpawn = 0f;
                    //}
                    //if (spawnerGO.GetComponent<WaveSpawner>().killcount >= amtToKill4Tank)
                    //{
                    //    SpawnTankZombie(GenerateSpawnPos());
                    //    spawnerGO.GetComponent<WaveSpawner>().killcount = 0;
                    //    //zombieGO.gameObject
                    //}
                }
                
            }
        }
        else if (waveNo <= maxWaveNo && waveEnded == false)
        {
            waveNo++;
            waveEnded = true;
            Debug.Log("Wave " + waveNo + " Ended");

            LevelManagement pewpew = GameObject.Find("testupgrade").GetComponent<LevelManagement>();
            pewpew.ChangeLevel(LevelManagement.LEVEL.UPGRADE);
            // Handle These Elsewhere
            //waveDuration = waveDurationSave;
            //waveEnded = false;
        }

    }

    protected bool CheckAnyAlive()
    {
        GameObject[] AllEntities = GameObject.FindGameObjectsWithTag("Entities");
        GameObject[] AllSurvivors = GameObject.FindGameObjectsWithTag("Survivor");
        // Get available targets
        GameObject[] AvailableTargets = ((AllEntities.Union<GameObject>(AllSurvivors))).ToArray<GameObject>();//GameObject.FindGameObjectsWithTag("Survivor");

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
            randomModifier = Random.Range(0.7f, 1.3f);
            go.GetComponent<Zombie>().HP = (int)((float)(go.GetComponent<Zombie>().HP) * difficultyMod);
            go.GetComponent<Zombie>().i_maxHP = go.GetComponent<Zombie>().HP;
            if (go.GetComponent<Zombie>().atkDmg > 3)
                go.GetComponent<Zombie>().atkDmg = (int)((float)(go.GetComponent<Zombie>().atkDmg) * difficultyMod);
            go.GetComponent<Zombie>().moveSpd *= randomModifier;
        }

    }

    void SpawnZombie(Vector3 spawnPos)
    {
        GameObject go = Instantiate(zombieGO, spawnPos, Quaternion.identity) as GameObject;
        TweakStats(go);
        go.transform.parent = this.transform.parent;
        spawnValue -= 1;
        Debug.Log("Zombie Spawn");
        spawnerGO.GetComponent<WaveSpawner>().maxAmount++;
        //Debug.Log(spawnerGO.GetComponent<WaveSpawner>().maxAmount);
    }

    void SpawnHorde(Vector3 spawnPos)
    {
        int randAmt = Random.Range(3, 6);

        for (int i = 0; i < randAmt; i++)
        {
            spawnPos.x += (i + 1) * 2;
            GameObject go = Instantiate(zombieGO, spawnPos, Quaternion.identity) as GameObject;
            TweakStats(go);
            go.transform.parent = this.transform.parent;
            spawnerGO.GetComponent<WaveSpawner>().maxAmount++;
        }
        spawnValue -= 1;
        Debug.Log("Horde Spawn");
        
        //testTimeDelta = 0f;
    }

    void SpawnTankZombie(Vector3 spawnPos)
    {
        GameObject go = Instantiate(tankzombieGO, spawnPos, Quaternion.identity) as GameObject;
        TweakStats(go);
        go.transform.parent = this.transform.parent;

        Debug.Log("Tank Zombie Spawn");
        spawnerGO.GetComponent<WaveSpawner>().maxAmount++;
    }

    public void SetWaveDuration()
    {
        waveDuration = waveDurationSave;
    }

    public Vector3 LoadSpawnPoint()
    {
        //if (waveEnded)
        //{
        for (int j = 0; j < (5 - 1); j++)
        {
            for (int k = 0; k < (17 - 1); k++)
            {
                    if (j == waveNo && k == currSpawnPt)
                    {
                    //Debug.Log(CSVReader.loadedMap[j, i]);
                    if (CSVReader.loadedMap[k, j] >= 0)
                        {
                            //Debug.Log(CSVReader.loadedMap[i, j]);
                            Vector3 temp;
                            temp = spawnPoints[CSVReader.loadedMap[k, j]].transform.position;
                            return temp;
                        }
                    }
                }
            }
        //}

        return new Vector3(0, 0, 0);
    }
}