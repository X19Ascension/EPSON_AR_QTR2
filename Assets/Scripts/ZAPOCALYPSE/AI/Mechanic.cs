using UnityEngine;
using System.Collections;

public class Mechanic : Survivor {

    public enum MechanicSTATE
    {
        S_IDLE,
        S_PANIC,
        S_SEARCH,
        S_REPAIR,
        S_DEAD,
    }

    public GameObject target;
    public MechanicSTATE Mechastate;

    float f_Attackdamage;

    
    void Awake()
    {

    }
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public override void RunFSM()
    {
        if(HP <= 0 )
        {
            Mechastate = MechanicSTATE.S_DEAD;
        }
        switch (Mechastate)
        {
            case MechanicSTATE.S_IDLE:
                {
                    if(Enemynear(PanicRange))
                    {
                        target = GetNearestTarget();
                        Mechastate = MechanicSTATE.S_PANIC;
                    }
                    else if(SearchRepairTarget() == null)
                    {
                        
                    }
                    else
                    {
                        Mechastate = MechanicSTATE.S_SEARCH;
                    }
                    break;
                }
            case MechanicSTATE.S_PANIC:
                {
                    if (target != null)
                        ShoveEnemy(target);
                    else
                    {
                        Mechastate = MechanicSTATE.S_IDLE;
                    }
                    break;
                }
            case MechanicSTATE.S_SEARCH:
                {
                    target = SearchRepairTarget();
                    if(target == null)
                    {
                        Mechastate = MechanicSTATE.S_IDLE;
                    }
                    else
                    {
                        Mechastate = MechanicSTATE.S_REPAIR;
                    }
                    break;
                }
            case MechanicSTATE.S_REPAIR:
                {
                    if(target == null)
                    {
                        Mechastate = MechanicSTATE.S_IDLE;
                    }
                    else
                        RepairTarget(target);

                    break;
                }
            case MechanicSTATE.S_DEAD:
                {
                    Destroy(this);
                    break;
                }
        }

    }
    
    GameObject SearchRepairTarget()
    {
        GameObject[] AllBarriers = GameObject.FindGameObjectsWithTag("Barrier");
        if(AllBarriers.Length != 0)
        {
            GameObject tempobject = AllBarriers[0];
            foreach(GameObject go in AllBarriers)
            {
                if(go.GetComponent<Barrier>().HP < tempobject.GetComponent<Barrier>().HP)
                {
                    tempobject = go;
                }
            }
            return tempobject;
        }


        return null;
    }

    void RepairTarget(GameObject Target)
    {
        target.GetComponent<Barrier>().HP += this.atkDmg;
        if(target.GetComponent<Barrier>().HP > target.GetComponent<Barrier>().GetMaxHealth())
        {
            target.GetComponent<Barrier>().HP = target.GetComponent<Barrier>().GetMaxHealth();
            Mechastate = MechanicSTATE.S_IDLE;
        }
    }
}
