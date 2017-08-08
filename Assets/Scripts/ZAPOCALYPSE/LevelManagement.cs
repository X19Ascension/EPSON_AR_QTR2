using UnityEngine;
using System.Collections;

public class LevelManagement : MonoBehaviour {

    public GameObject upgradeGO;

    public enum LEVEL
    {
         PLAY = 0,
         PAUSE,
         UPGRADE,
    }

    WaveSpawner The_Spawner;                                                //! The Grid
    GameObject[] The_Survivors;
    GameObject[] The_Enemies;
    bool findfirst;

    public LEVEL currLevel = LEVEL.PLAY;

    void Awake()
    {
        findfirst = true;
        The_Spawner = GameObject.Find("SpawnerPrefab").GetComponent<WaveSpawner>();
        The_Survivors = GameObject.FindGameObjectsWithTag("Survivor");
    }

	// Use this for initialization
	void Start () {

        
	
	}
	
	// Update is called once per frame
	void Update () {
	    switch (currLevel)
        {
            case LEVEL.PLAY:
                upgradeGO.SetActive(false);
                if (!findfirst)
                    ActivateEnemies();
                break;
            case LEVEL.PAUSE:

                break;
            case LEVEL.UPGRADE:
                upgradeGO.SetActive(true);
                DeactivateEnemies();
                break;
        }
	}

    void DeactivateEnemies()
    {
        if (findfirst)
        {
            The_Enemies = GameObject.FindGameObjectsWithTag("test");
            findfirst = false;
        }

        foreach (GameObject enem in The_Enemies)
        {
            if (enem.activeSelf)
            {
                //surv.SetActive(true);
                enem.SetActive(false);
            }
        }
    }

    void ActivateEnemies()
    {
        findfirst = true;
        foreach (GameObject enem in The_Enemies)
        {
            if (!enem.activeSelf)
            {
                //surv.SetActive(true);
                enem.SetActive(true);
            }
        }
        findfirst = true;
    }

    public void ChangeLevel(LEVEL nextLevel)
    {
        currLevel = nextLevel;
    }

    public void ContinuePlay()
    {
        upgradeGO.SetActive(false);
        The_Spawner.SetWaveDuration();
        The_Spawner.waveEnded = false;
        //The_Spawner.waveNo--;

        foreach (GameObject surv in The_Survivors)
        {
            if (surv != null)
            {
                //surv.SetActive(true);
                surv.GetComponent<UnitGrowth>().Updated = false;
            }
        }
        currLevel = LEVEL.PLAY;
    }
}
