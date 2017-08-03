using UnityEngine;
using System.Collections;

public class ZZombie : Zombie {

    public enum ZZombieSTATE
    {
        S_IDLE = 1,
        S_CHASE,
        S_ATTACK,
        S_DEAD,
    }

    public ZZombieSTATE Zombiestate;
    public GameObject SpawnerGO;
    float f_movespeed;
    float f_attackRate;
    int i_Threatvalue;

    void Awake()
    {
        atkSpd = 0.2f;
        f_attackRate = atkSpd;
        this.moveSpd = 2.0f;
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
        GameObject go_targetEnemy = TargetToAttack();

        if (this.HP <= 0)
        {
            Zombiestate = ZZombieSTATE.S_DEAD;
        }
        switch (Zombiestate)
        {
            case ZZombieSTATE.S_IDLE:
                {
                    if(TargetToAttack() != null)
                    {
                        Zombiestate = ZZombieSTATE.S_CHASE;
                    }
                    break;
                }
            case ZZombieSTATE.S_CHASE:
                {
                    float Distance = Vector3.Distance(this.gameObject.transform.position, go_targetEnemy.gameObject.transform.position);
                    if (go_targetEnemy != null)
                    {
                        Vector3 dir = (go_targetEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        this.gameObject.transform.localPosition += dir * moveSpd * Time.deltaTime;
                    }
                    if (Distance < 0.2f)
                    {
                        Zombiestate = ZZombieSTATE.S_ATTACK;
                    }
                    break;
                }
            case ZZombieSTATE.S_ATTACK:
                {
                    if (go_targetEnemy != null)
                    {
                        AttackEnemy(go_targetEnemy);
                    }
                    else
                        Zombiestate = ZZombieSTATE.S_CHASE;

                     break;
                }
            case ZZombieSTATE.S_DEAD:
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
}
