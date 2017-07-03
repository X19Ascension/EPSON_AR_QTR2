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

    public LEVEL currLevel = LEVEL.PLAY;

    void Awake()
    {
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

                break;
            case LEVEL.PAUSE:

                break;
            case LEVEL.UPGRADE:
                upgradeGO.SetActive(true);

                break;
        }
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
        
        foreach (GameObject surv in The_Survivors)
        {
            surv.SetActive(true);
            surv.GetComponent<UnitGrowth>().Updated = false;
        }
        currLevel = LEVEL.PLAY;
    }
}
