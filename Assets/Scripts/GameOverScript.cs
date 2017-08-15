using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

    public Barrier Barrier1;
    public Barrier Barrier2;
    public Barrier Barrier3;
    public Barrier Barrier4;

    public Survivor Survivor1;
    public Survivor Survivor2;
    public Survivor Survivor3;
    public Survivor Survivor4;

    public GameObject GameOverPanel;
    public ScoringSystem finalScorepew;
    //public Image

    private GameObject[] Zombies;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Survivor1.GetHealth() <= 0 || Survivor2.GetHealth() <= 0 || Survivor3.GetHealth() <= 0 || Survivor4.GetHealth() <= 0)
        {
            Zombies = GameObject.FindGameObjectsWithTag("test");
            foreach (GameObject zomb in Zombies)
            {
                if (zomb.activeSelf)
                    Destroy(zomb);
            }
            finalScorepew.FinalScore();
            GameOverPanel.SetActive(true);
        }
	}
}
