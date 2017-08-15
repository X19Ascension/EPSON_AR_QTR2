using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour {

    public Image DecayThing;
    public AudioClip scoreSound;
    private AudioSource source;
    public Text scoreText;
    public Text scoreMultiplierText;
    public Text killFeedbackText;
    public Text player_scoreText;
    public Text final_scoreText;
    public int player_score;
    public List<int> scoreToAdd;

    private float m_textdt;

    public int killStreak;
    private float scoreMultiplier;
    public float maxscoreMultiplier;                //! Setting Max Score Multiplier
    private float multiplierDecayRate;
    private float decayDt;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        scoreToAdd = new List<int>();
    }

    // Use this for initialization
    void Start () {
        maxscoreMultiplier = 4;
        ResetStreak();
    }
	
	// Update is called once per frame
	void Update () {
        if (decayDt >= 0.0f)
            ScoreMultiplier();
        else
            ResetStreak();

        if (killFeedbackText.text != "")
            m_textdt -= Time.deltaTime;

        if (m_textdt <= 0.0f)
            killFeedbackText.text = "";

        if (scoreToAdd != null && scoreToAdd.Count != 0)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(scoreSound, 5F);
            }
            AddScore();
            player_scoreText.text = "Score: " + player_score;
            m_textdt = 2.0f;
            scoreToAdd.RemoveAt(0);
        }
    }

    public void FinalScore()
    {
        final_scoreText.text = "Final Score: " + player_score;
    }

    void AddScore()
    {
        scoreText.text = "+" + (scoreToAdd[0] * scoreMultiplier) + " Points";
        player_score += (int)(scoreToAdd[0] * scoreMultiplier);
    }

    void ResetStreak()
    {
        killStreak = 1;
        scoreMultiplier = 1.0f;
        multiplierDecayRate = 1.0f;
        scoreMultiplierText.text = "x" + scoreMultiplier;
        DecayThing.fillAmount = 0;
    }

    void ScoreMultiplier()
    {
        decayDt -= (Time.deltaTime * multiplierDecayRate);
        DecayThing.fillAmount = decayDt / 5;
    }

    void AddMultiplier()
    {
        if (scoreMultiplier < maxscoreMultiplier)
        {
            multiplierDecayRate += 0.05f;
            scoreMultiplier += 0.1f;
            scoreMultiplier = Mathf.Round(scoreMultiplier * 100f) / 100f;
            scoreMultiplierText.text = "x" + scoreMultiplier;
        }
    }

    public void CalculateScore(GameObject survivor, GameObject enemy)
    {
        Survivor.SURVIVOR_TYPE scoreType = survivor.GetComponent<Survivor>().entityType;
        if (enemy.GetComponent<Zombie>().HP <= 0)
        {
            AddMultiplier();
            killStreak++;
            decayDt = 5.0f;
            enemy.GetComponent<BoxCollider>().enabled = false;
            switch (scoreType)
            {
                // Head Honcho Score
                case Survivor.SURVIVOR_TYPE.S_MELEE:
                    killFeedbackText.text = "SLASHIN'!!\n";
                    scoreToAdd.Add(100);
                    break;
                // Rifle Long Range more score
                case Survivor.SURVIVOR_TYPE.S_RIFLE:
                    if (((enemy.transform.position - survivor.transform.position).sqrMagnitude) > 25)
                    {
                        killFeedbackText.text = "QUICK KILL!!\n";
                        scoreToAdd.Add(30);
                    }
                    else
                    {
                        killFeedbackText.text = "GOOD KILL\n";
                        scoreToAdd.Add(10);
                    }
                    break;
                // Shotgun far 
                case Survivor.SURVIVOR_TYPE.S_SHOTGUN:
                    if (((enemy.transform.position - survivor.transform.position).sqrMagnitude) > 7)
                    {
                        killFeedbackText.text = "IMPOSSIBLE KILL\n";
                        scoreToAdd.Add(40);
                    }
                    else
                    {
                        killFeedbackText.text = "GREAT KILL\n";
                        scoreToAdd.Add(20);
                    }
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
}