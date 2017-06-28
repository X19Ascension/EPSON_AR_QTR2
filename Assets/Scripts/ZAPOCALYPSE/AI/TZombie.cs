using UnityEngine;
using System.Collections;

public class TZombie : Zombie
{
    public enum TZombieSTATE
    {
        S_IDLE = 1,
        S_CHASE,
        S_ATTACK,
        S_DEAD,
    }

    public TZombieSTATE Zombiestate;
    public GameObject SpawnerGO;
    float f_movespeed;
    float f_attackRate;
    int i_Threatvalue;

    void Awake()
    {
        Zombiestate = TZombieSTATE.S_IDLE;
        f_movespeed = 0.8f;
        f_attackRate = 0.9f;
        i_Threatvalue = 5;
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
        GameObject go_targetedEnemy = TargetToAttack();

        if (this.HP <= 0)
        {
            Zombiestate = TZombieSTATE.S_DEAD;
        }
        switch (Zombiestate)
        {
            case TZombieSTATE.S_IDLE:
                {
                    if (TargetToAttack() != null)
                    {
                        Zombiestate = TZombieSTATE.S_CHASE;
                    }
                    break;
                }
            case TZombieSTATE.S_CHASE:
                {
                    if (go_targetedEnemy != null)
                    {
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;
                    }
                    else if (TargetToAttack() == null)
                    {
                        Zombiestate = TZombieSTATE.S_IDLE;
                    }
                    else
                    {
                        Zombiestate = TZombieSTATE.S_ATTACK;
                    }
                    break;
                }
            case TZombieSTATE.S_ATTACK:
                {
                    if (go_targetedEnemy != null)
                    {
                        AttackEnemy(go_targetedEnemy);
                    }
                    else
                        Zombiestate = TZombieSTATE.S_CHASE;
                    break;
                }
            case TZombieSTATE.S_DEAD:
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
