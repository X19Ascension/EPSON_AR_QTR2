using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;

public class EnemySpawner : MonoBehaviour {

    [Tooltip("Amount of delay between spawns in secs")]
    public float SpawnRate;
    public bool RandomSpawnTime;
    public float SpawnRateRandomMax;
    public Vector2 SpawnAreaMin;
    public Vector2 SpawnAreaMax;

    [Header("Unit Templates")]
    public GameObject Melee;
    public GameObject Ranged;
    public GameObject Tank;

    Dictionary<GameObject, int> SpawnList = new Dictionary<GameObject, int>(); // Key is enemy to spawm, value is number of enemies

    double d_Timer = 0.0;
    int i_Idx = 0;
    bool b_EnemySpawned = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float FinalSpawnRate = SpawnRate;
        if (RandomSpawnTime)
            FinalSpawnRate += Random.Range(0, SpawnRateRandomMax);

        if (d_Timer > SpawnRate)
        {
            if (i_Idx >= SpawnList.Count)
            {
                this.gameObject.SetActive(false);
                i_Idx = 0;
                return;
            }

            var entry = SpawnList.ElementAt(i_Idx);
            if (entry.Value > 0)
            {
                Vector3 RandLocation = new Vector3(Random.Range(SpawnAreaMin.x, SpawnAreaMax.x), 0.05f, Random.Range(SpawnAreaMin.y, SpawnAreaMax.y));
                Vector3 SpawnLocation = this.gameObject.transform.position + RandLocation;

                GameObject go = Instantiate(entry.Key, SpawnLocation, Quaternion.identity) as GameObject;
                SpawnList[entry.Key] = entry.Value - 1;

                GameObject it = GameObject.FindGameObjectWithTag("ImageTarget");
                go.transform.SetParent(it.transform);

                go.GetComponent<Pathfinder>().theGridMap = it.GetComponentInChildren<GridBehavior>();

                b_EnemySpawned = true;
            }
            else if (entry.Value == 0)
            {
                ++i_Idx;
            }

            d_Timer = 0.0;
        }
        else
        {
            d_Timer += Time.deltaTime;
        }
	}

    public void SetUnits(int melee = 0, int ranged = 0, int tank = 0)
    {
        // Empty any leftover
        SpawnList.Clear();

        SpawnList.Add(Tank, tank);
        SpawnList.Add(Melee, melee);
        SpawnList.Add(Ranged, ranged);

    }

    public void SetEnemySpawned(bool status)
    {
        b_EnemySpawned = status;
    }

    public bool GetEnemySpawned()
    {
        return b_EnemySpawned;
    }
}
