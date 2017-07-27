using UnityEngine;
using System.Collections;

public class RZombie : Zombie
{
    public GameObject Projectile;
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
        testHealth = this.gameObject.GetComponent<HealthBar>();
        atkSpd = 3.0f;
        f_attackRate = atkSpd;
        this.moveSpd = 0.5f;
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
                    if (go_targetedEnemy != null && (Distance < atkRange))
                    {
                        //Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        //dir.y = 0;
                        //this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;

                        Quaternion lookRotation = Quaternion.LookRotation(dir);

                        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 5);
                    }
                    break;
                }
            case RZombieSTATE.S_ATTACK:
                {
                    if (go_targetedEnemy != null)
                    {
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;

                        AttackEnemy(dir);
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

    void AttackEnemy(Vector3 Direction)
    {
        attackRate -= Time.deltaTime;
        if(attackRate <=0)
        {
            Vector3 v3_bullet = this.gameObject.transform.position;
            GameObject go_bullet = null;

            go_bullet = Instantiate(Projectile, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            go_bullet.GetComponent<Rigidbody>().AddForce(Direction * go_bullet.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
            go_bullet.transform.parent = this.transform.parent;
            attackRate = atkSpd;
        }
    }

}
