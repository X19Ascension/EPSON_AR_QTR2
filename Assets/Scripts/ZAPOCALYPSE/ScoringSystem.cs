using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoringSystem : MonoBehaviour {

    [SerializeField]
    GridMap The_Grid;

    public int player_score;
    public List<int> scoreToAdd = new List<int>();

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (scoreToAdd != null)
        {
            player_score += scoreToAdd[0];  
            scoreToAdd.RemoveAt(0);
        }
	}

    public void CalculateScore(GameObject survivor, GameObject enemy)
    {
        Survivor.SURVIVOR_TYPE scoreType = survivor.GetComponent<Survivor>().entityType;
        switch (scoreType)
        {
            // Head Honcho Score
            case Survivor.SURVIVOR_TYPE.S_MELEE:
                scoreToAdd.Add(100);
                break;
            // Rifle Long Range more score
            case Survivor.SURVIVOR_TYPE.S_RIFLE:
                if (((enemy.transform.position - survivor.transform.position).sqrMagnitude) > 25)
                {
                    scoreToAdd.Add(100);
                }
                else
                    scoreToAdd.Add(50);
                break;
            // Shotgun far 
            case Survivor.SURVIVOR_TYPE.S_SHOTGUN:
                if (((enemy.transform.position - survivor.transform.position).sqrMagnitude) > 7)
                    scoreToAdd.Add(150);
                break;
            // 
            case Survivor.SURVIVOR_TYPE.S_MECHANIC:
                scoreToAdd.Add(25);
                break;

            //case Survivor.SURVIVOR_TYPE.S_MEDIC:

            //    break;
        }
    }
}