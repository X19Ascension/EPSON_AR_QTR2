using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {

    [Tooltip("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();     //! The number of Spawn Points in the Game
    [Tooltip("The current Wave Number")]
    int waveNo;                                                     //! The Current Wave number.
    public GameObject zombieGO;                                     //! Standard Zombie Game Object
    public GameObject fastzombieGO;                                 //! Fast Zombie Game Object
    public GameObject tankzombieGO;                                 //! Tank Zombie Game Object
    public GameObject rangezombieGO;                                //! Ranged Zombie Game Object
    public GameObject spawnerGO;                                    //! Spawner Game Object

    public int amtToKill4Tank;                                      //! Amount of Standard to kill for Tank to Spawn
    public int killcount;
    public float spawnValue = 910.0f;
    //public float spawnValue = 1.0f;

    public int minutePerWave;
    public int secondPerWave;

    // Difficulty Modifier
    public enum TEMP_DIFF
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


	// Use this for initialization
	void Start () {
        switch (diff)
        {
            case TEMP_DIFF.EASY:
                difficultyMod = 0.75f;
                break;
            case TEMP_DIFF.NORMAL:
                difficultyMod = 1.0f;
                break;
            case TEMP_DIFF.HARD:
                difficultyMod = 1.25f;
                break;
        }

        waveDuration = minutePerWave * 60 + secondPerWave;
    }
	
	// Update is called once per frame
	void Update () {
        testTimeDelta += Time.deltaTime;
        waveDuration -= Time.deltaTime;
        testTankSpawn += Time.deltaTime;
        //if (waveDuration > 0)
        //{
            if (testTimeDelta >= 1.5f)
            {
                SpawnZombie(GenerateSpawnPos());
                //testTimeDelta = 0f;
            }
            if (testTimeDelta >= 1.5f)
            {
                SpawnHorde(GenerateSpawnPos());
                testTimeDelta = 0f;
            }
            if (spawnerGO.GetComponent<WaveSpawner>().killcount >= amtToKill4Tank)
        {
            SpawnTankZombie(GenerateSpawnPos());
            spawnerGO.GetComponent<WaveSpawner>().killcount = 0;
            Debug.Log(spawnerGO.GetComponent<WaveSpawner>().killcount);
            //zombieGO.gameObject
        }
        //}
        //else
        //{
            // call game manager
        //}

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
            go.GetComponent<Zombie>().HP = (int)((float)(go.GetComponent<Zombie>().HP) * difficultyMod);
            go.GetComponent<Zombie>().i_maxHP = go.GetComponent<Zombie>().HP;
            if (go.GetComponent<Zombie>().atkDmg > 3)
                go.GetComponent<Zombie>().atkDmg = (int)((float)(go.GetComponent<Zombie>().atkDmg) * difficultyMod);
        }

    }

    void SpawnZombie(Vector3 spawnPos)
    {
        GameObject go = Instantiate(zombieGO, spawnPos, Quaternion.identity) as GameObject;
        TweakStats(go);
        go.transform.parent = this.transform.parent;
        spawnValue -= 1;
        Debug.Log("Zombie Spawn");
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
    }
}