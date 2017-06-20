using UnityEngine;
using System.Collections;

public class NZomebie : Zombie {

    public enum NZombie_STATE
    {
        S_IDLE = 1,
        S_CHASE,
        S_ATTACK,
        S_DEAD,
    }

    public NZombie_STATE Zombiestate;
    public GameObject SpawnerGO;
    float f_movespeed;
    float f_attackRate;
    int i_Threatvalue;

    void Awake()
    {
        Zombiestate = NZombie_STATE.S_IDLE;
        f_movespeed = 2f;
        f_attackRate = 0.2f;
        i_Threatvalue = 3;
    }

	// Use this for initialization
	void Start ()
    {
        atkSpd = 0.2f;
        f_attackRate = atkSpd;
        this.moveSpd = 1.5f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        RunFSM();
	}

    public override void RunFSM()
    {
        GameObject go_targetedEnemy = TargetToAttack();

        if (this.HP <= 0) 
        {
            Zombiestate = NZombie_STATE.S_DEAD;
        }
        switch (Zombiestate)
        {
            case NZombie_STATE.S_IDLE:
                {
                    if(TargetToAttack() != null)
                    {
                        Zombiestate = NZombie_STATE.S_CHASE;
                    }
                    break;
                }
            case NZombie_STATE.S_CHASE:
                {
                    if(go_targetedEnemy != null && TargetToAttack() != null)
                    {
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;
                    }
                    else if(TargetToAttack() == null)
                    {
                        Zombiestate = NZombie_STATE.S_IDLE;
                    }
                    else
                    {
                        Zombiestate = NZombie_STATE.S_ATTACK;
                    }
                    break;
                }
            case NZombie_STATE.S_ATTACK:
                {
                    if (go_targetedEnemy != null)
                    {
                        AttackEnemy(go_targetedEnemy);
                    }
                    else
                        Zombiestate = NZombie_STATE.S_CHASE;
                    break;
                }
            case NZombie_STATE.S_DEAD:
                {
                    Destroy(this.gameObject);
                    break;
                }
        }

    }

    void AttackEnemy(GameObject target)
    {
        f_attackRate -= Time.deltaTime;

        if (f_attackRate < 0)
        {
            int health = target.GetComponent<EntityBase>().GetHealth() - this.GetAttackDamage();
            target.GetComponent<EntityBase>().SetHealth(health);

            f_attackRate = this.GetAttackSpeed();
        }
    }

    void TakeDamage(int damage)
    {
        int health = this.GetHealth() - damage;
        this.SetHealth(health);
    }


    void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.tag.Contains("FriendlyFire"))
        //Debug.Log(col.gameObject.tag);
        if (col.gameObject.GetComponent<Projectile>().Sender.tag == "Survivor" && col.gameObject.tag == "Bullet")
        {
            //if (col.gameObject.GetComponent<Projectile>().Sender.GetComponent<FSMBase>().GetTarget() == this.gameObject)
            {
                TakeDamage(col.gameObject.GetComponent<Projectile>().Sender.GetComponent<Survivor>().GetAttackDamage());
                Destroy(col.gameObject);
            }
        }

    }
}
