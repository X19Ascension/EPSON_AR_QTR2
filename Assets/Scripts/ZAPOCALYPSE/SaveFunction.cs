using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveFunction : MonoBehaviour {

    private GameControl gameControl;

    private List<GameObject> survivorss = new List<GameObject>();
    private GameObject[] survivors;
    private GameObject demWaveSpawn;

    // Use this for initialization
    void Start () {
       gameControl = GameObject.Find("GameControl").GetComponent<GameControl>();
        demWaveSpawn = GameObject.Find("SpawnerPrefab");
    }	

    public void SaveData()
    {
        survivors = GameObject.FindGameObjectsWithTag("Survivor");
        survivorss = survivors.ToList();
        gameControl.Save(survivorss, demWaveSpawn.GetComponent<WaveSpawner>().waveNo);
    }

    public void LoadData()
    {
        gameControl.Load();
    }
}
