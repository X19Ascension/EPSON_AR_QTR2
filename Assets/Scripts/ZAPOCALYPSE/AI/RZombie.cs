using UnityEngine;
using System.Collections;

public class RZombie : Zombie
{

    public enum RZombieSTATE
    {
        S_IDLE = 1,
        S_CHASE,
        S_ATTACK,
        S_DEAD,
    }

    public RZombieSTATE Zombiestate;
    public GameObject SpawnerGO;
    float f_movespeed;
    float f_attackRate;
    int i_Threatvalue;

    void Awake()
    {
        Zombiestate = RZombieSTATE.S_IDLE;
        f_movespeed = 2f;
        f_attackRate = 0.2f;
        i_Threatvalue = 3;
        atkRange = 30.0f;
    }
    // Use this for initialization
    void Start ()
    {
        atkSpd = 3.0f;
        f_attackRate = atkSpd;
        this.moveSpd = 0.5f;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public override void RunFSM()
    {
        GameObject go_targetedEnemy = TargetToAttack();

        if (this.HP <= 0)
        {
            Zombiestate = RZombieSTATE.S_DEAD;
        }
        switch (Zombiestate)
        {
            case RZombieSTATE.S_IDLE:
                {
                    if (TargetToAttack() != null)
                    {
                        Zombiestate = RZombieSTATE.S_CHASE;
                    }
                    break;
                }
            case RZombieSTATE.S_CHASE:
                {
                    float Distance = Vector3.Distance(this.gameObject.transform.position, go_targetedEnemy.gameObject.transform.position);
                    if (go_targetedEnemy != null && !(Distance < atkRange))
                    {
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;
                    }
                    break;
                }
            case RZombieSTATE.S_ATTACK:
                {
                    if (go_targetedEnemy != null)
                    {
                        AttackEnemy(go_targetedEnemy);
                    }
                    else
                        Zombiestate = RZombieSTATE.S_CHASE;
                    break;
                }
            case RZombieSTATE.S_DEAD:
                {
                    Destroy(this.gameObject);
                    break;
                }
        }

    }

    void AttackEnemy(GameObject target)
    {

    }
}
