using UnityEngine;
using System.Collections;

public class Medic : Survivor
{
    enum Medic_State
    {
        S_IDLE = 1,
        S_PANIC,
        S_HEAL,
        S_DEAD,
    }

    GameObject Targettoheal;
    Medic_State medicstate;
    float Healtimer;
    void Awake()
    {
        Targettoheal = new GameObject();
        medicstate = Medic_State.S_IDLE;
        Healtimer = attackRate;
    }
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        RunFSM();
	}

    public override void RunFSM()
    {
        if (this.GetHealth() <= 0) 
        {
            medicstate = Medic_State.S_DEAD;
        }
        switch (medicstate)
        {
            case (Medic_State.S_IDLE):
                {
                    if(this.Enemynear())
                    {
                        medicstate = Medic_State.S_PANIC;
                    }
                    else if(Checkhurtunit())
                    {
                        Targettoheal = SearchUnit();
                        medicstate = Medic_State.S_HEAL;
                    }
                    break;

                }
            case (Medic_State.S_PANIC):
                {
                    ShoveZombie();
                    if(!this.Enemynear())
                    {
                        medicstate = Medic_State.S_IDLE;
                    }
                    break;
                }
            case (Medic_State.S_HEAL):
                {
                    HealUnit(Targettoheal);
                    if(!Checkhurtunit())
                    {
                        medicstate = Medic_State.S_IDLE;
                    }
                    break;
                }
            case (Medic_State.S_DEAD):
                {
                    Destroy(this.gameObject);
                    break;
                }
        }

    }
    
    bool Checkhurtunit()
    {
        GameObject[] Friendlyunits = GameObject.FindGameObjectsWithTag("Survivor");
        for (int i = 0; i < Friendlyunits.Length; i++) 
        {
            if(Friendlyunits[i].GetComponent<Survivor>().GetEnmity() != 1)
            {
                return true;
            }
        }
        return false;
    }
    GameObject SearchUnit()
    {
        GameObject go = new GameObject();
        GameObject[] Friendlyunits = GameObject.FindGameObjectsWithTag("Survivor");
        go = Friendlyunits[0];
        float checkenmity = Friendlyunits[0].GetComponent<Survivor>().GetEnmity();
        for (int i = 0; i<Friendlyunits.Length; i ++)
        {
            if(checkenmity > Friendlyunits[i].GetComponent<Survivor>().GetEnmity())
            {
                go = Friendlyunits[i];
                checkenmity = Friendlyunits[i].GetComponent<Survivor>().GetEnmity();
            }
        }
        return go;
    }

    void HealUnit(GameObject target)
    {
        Healtimer -= Time.deltaTime;
        if (Healtimer <= 0) 
        {
            target.GetComponent<Survivor>().HP += this.GetAttackDamage();
            Healtimer = attackRate;
        }
        
    }

    void ShoveZombie()
    {

    }
}
