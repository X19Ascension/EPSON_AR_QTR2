using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveFunction : MonoBehaviour {

    private GameControl gameControl;

    private List<GameObject> survivorss = new List<GameObject>();
    private GameObject[] survivors;
    // Use this for initialization
    void Start () {
       gameControl = GameObject.Find("GameControl").GetComponent<GameControl>();
        
    }	

    public void SaveData()
    {
        survivors = GameObject.FindGameObjectsWithTag("Survivor");
        survivorss = survivors.ToList();
        gameControl.Save(survivorss);
    }

    public void LoadData()
    {
        gameControl.Load();
    }
}
