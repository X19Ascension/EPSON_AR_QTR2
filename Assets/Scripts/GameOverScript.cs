using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

    public Barrier Barrier1;
    public Barrier Barrier2;
    public Barrier Barrier3;
    public Barrier Barrier4;

    public GameObject GameOverPanel;
    //public Image

    private GameObject[] Zombies;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Barrier1.GetHealth() <= 0 || Barrier2.GetHealth() <= 0 || Barrier3.GetHealth() <= 0 || Barrier4.GetHealth() <= 0)
        {
            Zombies = GameObject.FindGameObjectsWithTag("test");
            foreach (GameObject zomb in Zombies)
            {
                if (zomb.activeSelf)
                    Destroy(zomb);
            }

            GameOverPanel.SetActive(true);
        }
	}
}
