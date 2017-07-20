using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour {

    [SerializeField]
    GridMap The_Grid;

    public Text scoreText;
    public Text player_scoreText;
    public int player_score;
    public List<int> scoreToAdd = new List<int>();

    private float m_textdt;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (m_textdt > 0.0f)
            m_textdt -= Time.deltaTime;

        if (scoreToAdd != null && scoreToAdd[0] > 0)
        {
            scoreText.text += scoreToAdd[0] + "Points";
            player_score += scoreToAdd[0];  
            scoreToAdd.RemoveAt(0);
            player_scoreText.text = "Score: " + player_score;
            m_textdt = 2.0f;
        }

        if (m_textdt <= 0.0f)
            scoreText.text = null;
    }

    public void CalculateScore(GameObject survivor, GameObject enemy)
    {
        scoreText.text = null;
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
                    scoreText.text += "QUICK KILL!!\n";
                    scoreToAdd.Add(100);
                }
                else
                {
                    scoreText.text += "GOOD KILL\n";
                    scoreToAdd.Add(50);
                }
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