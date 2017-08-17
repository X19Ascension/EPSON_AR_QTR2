using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mechanic : Survivor {

    public enum MechanicSTATE
    {
        S_IDLE,
        S_PANIC,
        S_SEARCH,
        S_REPAIR,
        S_DEAD,
    }
    
    public MechanicSTATE Mechastate;
    private Slider ESlider;

    float f_Attackdamage;
    private Animator anim;

    void Awake()
    {
        base.Awake();
    }
	// Use this for initialization
	void Start ()
    {
        base.Start();
        ESlider = GameObject.FindGameObjectWithTag("EHP").GetComponent<Slider>();
        GameObject.FindGameObjectWithTag("MeleeLVL").GetComponent<UnitGrowthResult>().Unit = this.gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        base.Update();
        Regenerate();
        RunFSM();
    }

    void ScaleHP()
    {
        ESlider.value = ((float)HP / (float)i_maxHP);
    }


    public override void RunFSM()
    {
        if(HP <= 0 || Ustate == UnitState.S_DEAD)
        {
            Mechastate = MechanicSTATE.S_DEAD;
        }
        switch (Mechastate)
        {
            case MechanicSTATE.S_IDLE:
                {
                    if (Enemynear(PanicRange))
                    {
                        target = GetNearestTarget();
                        Mechastate = MechanicSTATE.S_PANIC;
                    }
                    else if(SearchRepairTarget() == null)
                    {
                        anim.SetTrigger("IDLE");
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
                    {
                        anim.SetTrigger("SHOVE");
                        ShoveEnemy(target);
                    }
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
                    anim.SetBool("DIE", true);
                    DestroyGO();
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
        anim.SetTrigger("REPAIR");
        target.GetComponent<Barrier>().HP += this.atkDmg;
        if(target.GetComponent<Barrier>().HP > target.GetComponent<Barrier>().GetMaxHealth())
        {
            target.GetComponent<Barrier>().HP = target.GetComponent<Barrier>().GetMaxHealth();
            Mechastate = MechanicSTATE.S_IDLE;
        }
    }
}
