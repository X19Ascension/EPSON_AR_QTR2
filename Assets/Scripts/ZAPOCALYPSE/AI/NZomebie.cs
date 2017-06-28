using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        SetThreat(3);
        atkRange = 999;
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
                    float Distance = Vector3.Distance(this.gameObject.transform.position, go_targetedEnemy.gameObject.transform.position);
                    if (go_targetedEnemy != null)
                    {
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;
                    }
                    if (Distance < 2.0f)
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
                    spawnerGO.GetComponent<WaveSpawner>().maxAmount--;
                    spawnerGO.GetComponent<WaveSpawner>().killcount++;
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
}
