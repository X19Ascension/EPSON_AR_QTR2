using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DifficultyChoose : MonoBehaviour {

    public GameObject DifficultyChooser;
    public GameControl gameControl;

	// Use this for initialization
	void Start () {
        gameControl = GameObject.Find("GameControl").GetComponent<GameControl>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DifficultySelect()
    {
        DifficultyChooser.SetActive(true);
    }

    public void chooseEasy()
    {
        gameControl.difficulty = 0.75f;
    }

    public void chooseNormal()
    {
        gameControl.difficulty = 1.0f;
    }

    public void chooseHard()
    {
        gameControl.difficulty = 1.25f;
    }
}
