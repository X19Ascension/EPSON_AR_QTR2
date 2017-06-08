using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class InvasionManager : MonoBehaviour {

    // Detremined by player milestones at first, then perodically

    public List<EnemySpawner> Spawners = new List<EnemySpawner>();
    public int EnemyStrength = 1;
    public int MaxStrength;

    [Header("Base Spawn Values")]
    public float BaseMeleeEnemy;
    public float BaseRangedEnemy;
    public float BaseTankEnemy;

    [Header("Object References")]
    public MilestoneManager theMilestoneManager;
    public ZoomInOut theZoomInOutScript;
    public GameObject AlertPanel;

    int i_PreparedForceStrength = 0;
    bool b_StartAttack = false;
    bool b_AttackOngoing = false;

	// Use this for initialization
	void Start () {

        // TEMP CODE
        if (!theZoomInOutScript)
            theZoomInOutScript = GameObject.Find("CityCanvas").GetComponentInChildren<ZoomInOut>();
	}

    // Update is called once per frame
    void Update() {

        // Prepare Enemy Strength
        if (i_PreparedForceStrength != EnemyStrength)
        {
            PrepareEenmy();
        }

        // Check if to launch attack
        if (!b_StartAttack)
            b_StartAttack = CheckIfStartAttack();
        else
        {
            // Zooom Out
            if (theZoomInOutScript.theState == ZoomInOut.ZOOM_STATE.NORMAL)
            {
                theZoomInOutScript.DoZoomInOut();
                AlertPanel.SetActive(true);

                theZoomInOutScript.gameObject.transform.parent.transform.FindChild("Pause").gameObject.GetComponent<Image>().enabled = false;
            }

            // Switch on Spawners
            if (theZoomInOutScript.theState == ZoomInOut.ZOOM_STATE.ZOOMED_OUT)
            {
                foreach (EnemySpawner es in Spawners)
                    es.gameObject.SetActive(true);

                b_StartAttack = false;
                b_AttackOngoing = true;

                theZoomInOutScript.gameObject.GetComponent<Image>().enabled = false;
                theZoomInOutScript.enabled = false;
            }
        }

        // Debug
        if (Input.GetKeyUp(KeyCode.K))
        {
            GameObject[] ToKill = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject go in ToKill)
                Destroy(go);
        }

        // Check to Zoom back in
        if (b_AttackOngoing)
        {
            bool EnemySpawned = true;
            foreach (EnemySpawner es in Spawners)
            {
                if (!es.GetEnemySpawned())
                {
                    EnemySpawned = false;
                    break;
                }
            }

            // Zoom In when all enemies killed
            if (EnemySpawned)
            {
                GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                if (AllEnemies.Length <= 0)
                {
                    EnemyStrength = Mathf.Min(++EnemyStrength, MaxStrength);

                    theZoomInOutScript.gameObject.GetComponent<Image>().enabled = true;
                    theZoomInOutScript.enabled = true;

                    theZoomInOutScript.gameObject.transform.parent.transform.FindChild("Pause").gameObject.GetComponent<Image>().enabled = true;

                    theZoomInOutScript.DoZoomInOut();
                    b_AttackOngoing = false;
                    AlertPanel.SetActive(false);

                    // Heal all buildings
                    GameObject[] AllBuildings = GameObject.FindGameObjectsWithTag("Buildings");
                    GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");
                    GameObject[] AllWalls = GameObject.FindGameObjectsWithTag("Wall");

                    GameObject[] AvailableTargets = ((AllBuildings.Union<GameObject>(AllTowers)).Union<GameObject>(AllWalls)).ToArray<GameObject>();

                    foreach (GameObject go in AvailableTargets)
                    {
                        go.GetComponent<Health>().Reset();
                    }

                    foreach (EnemySpawner es in Spawners)
                    {
                        es.GetComponent<EnemySpawner>().SetEnemySpawned(false);
                    }
                }
            }
        }
    }

    // Function to check if an attack should be launched
    public bool CheckIfStartAttack()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            PrepareEenmy();
            return true;
        }

        if (theMilestoneManager.IsMilestonesCompleted())
        {
            // Do checks/calc for periodical attacks
        }

        return false;
    }

    // Function to fill up the spawners with units
    void PrepareEenmy()
    {
        // Determin how many units to spawn
        int MeleeCount = 0;
        int RangedCount = 0;
        int TankCount = 0;

        MeleeCount = (int)(BaseMeleeEnemy * EnemyStrength);
        RangedCount = (int)(BaseRangedEnemy * EnemyStrength);
        TankCount = (int)(BaseTankEnemy * EnemyStrength);

        // Special Condition for first wave and when no melee spawns
        if (EnemyStrength == 1 && MeleeCount == 0)
            MeleeCount = 1;

        // Set the spawners spawnlist
        foreach (EnemySpawner es in Spawners)
            es.SetUnits(MeleeCount, RangedCount, TankCount);

        i_PreparedForceStrength = EnemyStrength;
    }

    public void StartAttack()
    {
        b_StartAttack = true;
    }

    public bool GetIsAttacking()
    { return b_AttackOngoing; }
}
