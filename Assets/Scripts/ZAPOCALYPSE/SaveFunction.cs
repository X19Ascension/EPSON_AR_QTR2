using UnityEngine;
using System.Collections;

public class SaveFunction : MonoBehaviour {

    private GameControl gameControl;
    // Use this for initialization
    void Start () {
       gameControl = GameObject.Find("GameControl").GetComponent<GameControl>();
    }	

    public void SaveData()
    {
        gameControl.Save();
    }

    public void LoadData()
    {
        gameControl.Load();
    }
}
