using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Melee : Survivor
{
    public enum Melee_State
    {
        S_IDLE = 1,
        S_SEARCH,
        S_ATTACK,
        S_DEAD,
    }

    private Animator Anim;

    public GameObject target;
    public Melee_State meleestate;
    private Slider CCSlider;

    void Awake()
    {
        Anim = GetComponent<Animator>();
        target = null;
        meleestate = Melee_State.S_IDLE;
        Ustate = UnitState.S_HEALTHY;
    }
    // Use this for initialization
    void Start ()
    {
        CCSlider = GameObject.FindGameObjectWithTag("CCHP").GetComponent<Slider>();
        this.atkRange = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Regenerate();
        RunFSM();
        RunDeathDoor();
        ScaleHP();
    }

    void ScaleHP()
    {
        CCSlider.value = ((float)HP / (float)i_maxHP);
    }

    public override void RunFSM()
    {
        if (this.HP <= 0)
        {
            meleestate = Melee_State.S_DEAD;
        }
        switch (meleestate)
        {
            case Melee_State.S_IDLE:
                {
                    if (this.Enemynear(this.atkRange))
                    {
                        meleestate = Melee_State.S_SEARCH;
                        Anim.SetTrigger("IDLE");
                    }
                    break;
                }
            case Melee_State.S_SEARCH:
                {
                    target = SelectTarget(this.atkRange,this.transform.position);
                    meleestate = Melee_State.S_ATTACK;
                    break;
                }
            case Melee_State.S_ATTACK:
                {
                    if (target != null && target.activeSelf)
                    { 
                        Attackenemy(target);
                        Anim.SetTrigger("ATTACK");
                    }
                    else
                    {
                        meleestate = Melee_State.S_IDLE;
                    }
                    break;
                }
            case Melee_State.S_DEAD:
                {
                    Anim.SetBool("DIE", true);
                    DestroyGO();
                    break;
                }
        }

    }

   void RunDeathDoor()
    {
        switch (Ustate)
        {
            case UnitState.S_HEALTHY:
                {
                    if (this.HP == 0)
                    {
                        DeathDoorStats();
                        Ustate = UnitState.S_DEATHDOOR;
                    }
                    break;
                }
            case UnitState.S_DEATHDOOR:
                {
                    if (this.HP > (this.i_maxHP * 0.4))
                    {
                        ReturnStats();
                        Ustate = UnitState.S_HEALTHY;
                    }
                    else
                    {
                        if (DeathDoor())
                        {
                            meleestate = Melee_State.S_DEAD;
                        }
                        else
                        {

                        }
                    }
                    break;
                }
        }
    }

    void Attackenemy(GameObject target)
    {
        attackRate -= Time.deltaTime;
        if (attackRate <= 0)
        {
            int tempHP = target.GetComponent<Zombie>().HP;
            tempHP -= atkDmg;
            target.GetComponent<Zombie>().SetHealth(tempHP);
            attackRate = atkSpd;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }


}
