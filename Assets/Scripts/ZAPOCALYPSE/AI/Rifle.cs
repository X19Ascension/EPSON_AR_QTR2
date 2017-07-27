using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rifle : Survivor
{ 
    public enum Rifle_State
    {
        S_IDLE = 1,
        S_PANIC,
        S_SEARCH,
        S_SWITCHSEARCH,
        S_ATTACK,
        S_DEAD,
    }

    private Animator Anim;
    public GameObject target;
    public Vector3 V3_targetpos;
    int i_targetSurroundings;
    public Rifle_State riflestate;

    private GameControl gameControl;

    public AudioClip shootSound;
    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        target = null;
        riflestate = Rifle_State.S_IDLE;
        Ustate = UnitState.S_HEALTHY;
    }

	void Start ()
    {
        gameControl = GameObject.Find("GameControl").GetComponent<GameControl>();
        Anim = GetComponent<Animator>();

        if (gameControl != null)
            LoadFromGameControl();

        //this.atkRange = 25.0f;
        this.atkRange = atkRange * this.transform.localScale.x * 0.2f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeActive += Time.deltaTime;
        RunFSM();
        RunDeathDoor();
        Regenerate();
	}

    void LoadFromGameControl()
    {
        //Debug.Log(gameControl.range_Rifle);
        this.atkRange = gameControl.range_Rifle;//25.0f;
        this.timeActive = gameControl.durationUp_Rifle;
        this.experiencePt = gameControl.EXP_Rifle;
        this.HP = gameControl.HP_Rifle;
        this.level = gameControl.LVL_Rifle;
    }


    public override void RunFSM()
    {
        //target = SelectTarget(this.atkRange);
        if (this.HP <= 0 )
        {
           // riflestate = Rifle_State.S_DEAD;
        }
        switch (riflestate)
        {
            case Rifle_State.S_IDLE:
                {
                    //if(this.Enemynear(0.5f))
                    if (this.Enemynear(atkRange))
                    {
                        riflestate = Rifle_State.S_SEARCH;
                    }
                    else
                    {
                        Anim.SetTrigger("IDLE");
                    }
                    break;
                    
                }
            case Rifle_State.S_PANIC:
                {
                    if (target != null)
                    {
                        Anim.SetTrigger("SHOVE");
                        ShoveEnemy(target);
                    }
                    else
                    {
                        riflestate = Rifle_State.S_IDLE;
                    }
                    break;
                }
            case Rifle_State.S_SEARCH:
                {
                    target = SelectTarget(this.atkRange, this.transform.position) ;
                    if(target !=null)
                    {
                        riflestate = Rifle_State.S_ATTACK;
                    }
                    else riflestate = Rifle_State.S_SEARCH;
                    break;
                }
            case Rifle_State.S_SWITCHSEARCH:
                {
                    target = SwitchTarget(V3_targetpos);
                    riflestate = Rifle_State.S_ATTACK;
                    break;
                }
            case Rifle_State.S_ATTACK:
                {
                    if (target != null) 
                    {
                           //Anim.SetTrigger("Attack");
                           Vector3 V3_Direction = (target.transform.position - this.transform.position).normalized;
                        Attackenemy(V3_Direction);
                        V3_targetpos = target.transform.position;
                        i_targetSurroundings = target.GetComponent<Zombie>().CheckSurroundings();
                        Anim.SetTrigger("ATTACK");
                    }
                    else
                    {
                        if(i_targetSurroundings != 0)
                        {
                            riflestate = Rifle_State.S_SWITCHSEARCH;
                        }
                        else
                            riflestate = Rifle_State.S_IDLE;
                    }
                    break;
                }
            case Rifle_State.S_DEAD:
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
                    if(this.HP == 0)
                    {
                        DeathDoorStats();
                        Ustate = UnitState.S_DEATHDOOR;
                    }
                    break;
                }
            case UnitState.S_DEATHDOOR:
                {
                    if(this.HP > (this.i_maxHP * 0.4))
                    {
                        ReturnStats();
                        Ustate = UnitState.S_HEALTHY;
                    }
                    else
                    {
                        if(DeathDoor())
                        {
                            riflestate = Rifle_State.S_DEAD;
                        }
                        else
                        {
                            
                        }
                    }
                    break;
                }
        }

    }

    void Attackenemy(Vector3 Direction)
    {
        attackRate -= Time.deltaTime;
        if (attackRate <= 0)
        {
            source.PlayOneShot(shootSound, 0.2F);
            Vector3 pew = this.gameObject.transform.position;
            GameObject bullet = null;
                //direction.y += 2;
                //direction.y += 0.05f;
            bullet = Instantiate(EProjectile, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            
            bullet.GetComponent<Rigidbody>().AddForce(Direction * bullet.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
            bullet.GetComponent<Projectile>().Sender = this.gameObject;
            bullet.transform.parent = this.transform.parent;
            attackRate = atkSpd;
            //source.Play();
            
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }
}
