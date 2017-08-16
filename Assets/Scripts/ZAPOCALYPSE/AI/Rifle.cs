using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    [Header("Rifle Specific Statistics")]
    public Rifle_State riflestate;
    private Animator Anim;
    int i_targetSurroundings;

    private GameControl gameControl;

    #region SOUND
    public AudioClip shootSound;
    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    private Slider GSlider;
    #endregion

    void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
        riflestate = Rifle_State.S_IDLE;
    }

	void Start ()
    {
        base.Start();
        GSlider = GameObject.FindGameObjectWithTag("GHP").GetComponent<Slider>();
        GameObject.FindGameObjectWithTag("RifleLVL").GetComponent<UnitGrowthResult>().Unit = this.gameObject;
        gameControl = GameObject.Find("GameControl").GetComponent<GameControl>();
        Anim = GetComponent<Animator>();

        if (gameControl != null)
            LoadFromGameControl();

        //this.atkRange = 25.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        base.Update();
        RunFSM();
        ScaleHP();
    }

    void ScaleHP()
    {
        GSlider.value = ((float)HP / (float)i_maxHP);
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
        if (this.HP <= 0  || Ustate == UnitState.S_DEAD)
        {
            riflestate = Rifle_State.S_DEAD;
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
                    Anim.SetTrigger("IDLE");
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
                    if (target != null && target.activeSelf) 
                    {
                        //Anim.SetTrigger("Attack");
                        
                        Vector3 V3_Direction = (target.transform.position - this.transform.position).normalized;
                        Quaternion lookRotation = Quaternion.LookRotation(V3_Direction);
                        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 5);

                        if (currAmmo > 0)
                        {

                            Attackenemy(V3_Direction);
                            i_targetSurroundings = target.GetComponent<Zombie>().CheckSurroundings();
                            Anim.SetTrigger("ATTACK");
                        }
                        else
                        {
                            Anim.SetTrigger("RELOAD");
                            Reload();
                        }
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
            pew.y += 0.5f;
            bullet = Instantiate(EProjectile, pew, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(Direction * bullet.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
            bullet.GetComponent<Projectile>().Sender = this.gameObject;
            bullet.transform.parent = this.transform.parent;
            attackRate = atkSpd;
            //source.Play();
            currAmmo--;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }
}
