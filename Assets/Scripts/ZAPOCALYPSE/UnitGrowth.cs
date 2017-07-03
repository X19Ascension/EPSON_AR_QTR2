using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitGrowth : MonoBehaviour {

    //private List<Survivor> survivors = new List<Survivor>();

    private int startDMG;
    public int maxDMG;

    private float startSpd;
    public float maxSpd;

    private float startReload;
    public float maxReload;

    public GameObject surv;
    private GameObject diff;
    private float EXPToLevel;

    private int healthScale;
    private int statLevel = 0;
    public bool Updated;

    WaveSpawner The_Spawner;                //! The Grid

    //private Survivor[] survivors;
    //Survivor The_Survivor;
    
    // Use this for initialization
    void Start () {
        surv = this.gameObject;//.GetComponent<Survivor>();
        Debug.Log(surv.name);
        The_Spawner = GameObject.Find("SpawnerPrefab").GetComponent<WaveSpawner>();
        //survivors.Add(surv);
        //this.gameObject.GetComponent<Survivor>().;
        diff = GameObject.Find("SpawnerPrefab");

        startDMG= surv.GetComponent<Survivor>().atkDmg;
        startSpd = surv.GetComponent<Survivor>().atkSpd;
        startReload = surv.GetComponent<Survivor>().reloadRate;
        Updated = false;

        EXPToLVL();
    }
	
	// Update is called once per frame
	void Update () {

        if (The_Spawner.waveEnded && !Updated)
        {
            surv.gameObject.GetComponent<UnitGrowth>().CalculateEXPGain();
            surv.SetActive(false);
        }
    }

    public void CalculateEXPGain()
    {
        //Debug.Log((int)diff.GetComponent<WaveSpawner>().diff);
        float EXPGain = (surv.GetComponent<Survivor>().timeActive * 0.2f) + surv.GetComponent<Survivor>().level * (int)diff.GetComponent<WaveSpawner>().diff;

        float totalEXP = surv.GetComponent<Survivor>().experiencePt + EXPGain;
        Debug.Log("Surv Time Active: " + surv.GetComponent<Survivor>().timeActive);
        Debug.Log("EXP To Level: " + EXPToLevel);
        Debug.Log("Total EXP Earned: " + totalEXP);

        if (totalEXP >= EXPToLevel)
        {
            while (totalEXP >= EXPToLevel)
            {
                Debug.Log(totalEXP + " and EXPTOLEVEL: " + EXPToLevel);
                Debug.Log(surv.name + "Level Up!");

                surv.GetComponent<Survivor>().level++;
                //EXPGain -= EXPToLevel;
                totalEXP -= EXPToLevel;
                surv.GetComponent<Survivor>().experiencePt = totalEXP;
                EXPToLVL();

                surv.GetComponent<Survivor>().timeActive = 0;

                if ((surv.GetComponent<Survivor>().level % 5) == 0)
                    UpdateStats();
            }
        }
        else
        {
            surv.GetComponent<Survivor>().experiencePt = totalEXP;
            Updated = true;
        }

    }

    private void EXPToLVL()
    {
        if (surv.GetComponent<Survivor>().level < 20)
            EXPToLevel = (surv.GetComponent<Survivor>().level * 15) + (((int)diff.GetComponent<WaveSpawner>().diff + 1)* 50);
        //EXPToLevel = 0.1f;
        else
            EXPToLevel = 99999;
    }

    public float EXPToLVL(Survivor pew)
    {
        if (pew.level < 20)
            return ((pew.level * 15) + (2) * 50);
        //EXPToLevel = 0.1f;
        else
            return 99999.0f;
    }

    private void UpdateStats()
    {
        statLevel++;

        float statScale = (statLevel * 0.2f);
        float maxHP = (surv.GetComponent<Survivor>().GetMaxHealth() * statScale) + surv.GetComponent<Survivor>().GetMaxHealth();
        surv.GetComponent<Survivor>().SetMaxHealth((int)maxHP);
        surv.GetComponent<Survivor>().SetHealth((int)maxHP);

        //Debug.Log("Max Health: " + maxHP);

        int dmg = maxDMG - startDMG;
        int dmgPerLvl = (int)(dmg * 0.25);
        surv.GetComponent<Survivor>().atkDmg += dmgPerLvl;

        //Debug.Log("Attack Dmg: " + dmgPerLvl);

        float spd = maxSpd - startSpd;
        float spdPerLvl = spd * 0.25f;
        surv.GetComponent<Survivor>().atkSpd += spdPerLvl;

        //Debug.Log("Attack Speed: " + spdPerLvl);

        float reload = maxReload- startReload;
        float reloadPerLvl = spd * 0.25f;
        surv.GetComponent<Survivor>().reloadRate += reloadPerLvl;

        //Debug.Log("Reload Rate: " + reloadPerLvl);
    }
}
