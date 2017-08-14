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

    private Animator anim;

    private float atkDist;
    private float pew;

    void Awake()
    {
        Zombiestate = RZombieSTATE.S_IDLE;
        f_movespeed = 5.0f;
        f_attackRate = 0.2f;
        i_Threatvalue = 3;
        atkRange = 999.0f;
        atkDist = 18.0f;
    }
    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
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
                        anim.SetBool("CHASE", true);
                        //Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        //dir.y = 0;
                        //this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        this.gameObject.transform.localPosition += dir * moveSpd * Time.deltaTime;

                        Quaternion lookRotation = Quaternion.LookRotation(dir);

                        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 5);
                    }
                    if (Distance < atkDist)
                    {
                        anim.SetBool("CHASE", false);
                        Zombiestate = RZombieSTATE.S_ATTACK;
                    }
                    break;
                }
            case RZombieSTATE.S_ATTACK:
                {
                    float Distance = Vector3.Distance(this.gameObject.transform.position, go_targetedEnemy.gameObject.transform.position);
                    if (go_targetedEnemy != null && Distance < atkDist)
                    {
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        Quaternion lookRotation = Quaternion.LookRotation(dir);
                        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 5);
                        AttackEnemy(dir);
                    }
                    else
                    {
                        anim.speed = 1.0f;
                        anim.SetBool("ATTACK", false);
                        Zombiestate = RZombieSTATE.S_CHASE;
                    }
                    break;
                }
            case RZombieSTATE.S_DEAD:
                {
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DIE"))
                    {
                        anim.SetTrigger("DIE");
                    }
                    //Debug.Log(anim.GetComponent<AnimationState>().normalizedTime);
                    pew += Time.deltaTime;
                    if (pew > 2.5f)
                    {
                        Destroy(this.gameObject);
                    }

                    break;
                }
        }

    }

    void AttackEnemy(Vector3 Direction)
    {
        attackRate -= Time.deltaTime;
        if(attackRate <=0)
        {
            anim.SetBool("ATTACK", true);
            anim.speed = 0.6f;
            Vector3 v3_bullet = this.gameObject.transform.position;
            GameObject go_bullet = null;

            go_bullet = Instantiate(Projectile, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            go_bullet.GetComponent<Rigidbody>().AddForce(Direction * go_bullet.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
            go_bullet.GetComponent<Projectile>().Sender = this.gameObject;
            go_bullet.GetComponent<Projectile>().ProjectileLifetime = 0.65f;
            go_bullet.transform.parent = this.transform.parent;
            attackRate = atkSpd;
        }
    }

}
