using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NZomebie : Zombie {
    public GameObject blood;

    public enum NZombie_STATE
    {
        S_IDLE = 1,
        S_CHASE,
        S_ATTACK,
        S_DEAD,
    }

    public NZombie_STATE Zombiestate;
     GameObject SpawnerGO;
    float f_movespeed;
    float f_attackRate;
    int i_Threatvalue;
    private Animator anim;

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
        anim = GetComponent<Animator>();
        atkSpd = 0.2f;
        f_attackRate = atkSpd;
        this.moveSpd = 1.5f;

        this.moveSpd = Random.Range(1.0f, 2.5f);
        this.gameObject.transform.parent = GameObject.Find("TerrainSpawn").transform;
        SpawnerGO = GameObject.Find("SpawnerPrefab");
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
            Destroy(this.gameObject);

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
                    anim.SetBool("Chase", true);
                    float Distance = Vector3.Distance(this.gameObject.transform.position, go_targetedEnemy.gameObject.transform.position);
                    if (go_targetedEnemy != null)
                    {
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;

                        Quaternion lookRotation = Quaternion.LookRotation(dir);

                        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 5);

                    }
                    if (Distance < 2.0f)
                    {
                        Zombiestate = NZombie_STATE.S_ATTACK; 
                        anim.SetBool("Chase", false);
                    }
                    break;
                }
            case NZombie_STATE.S_ATTACK:
                {
                    if (go_targetedEnemy != null)
                    {
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        Quaternion lookRotation = Quaternion.LookRotation(dir);

                        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 5);

                        anim.SetTrigger("Attack");
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
                    SpawnBlood();
                    anim.SetTrigger("Die");
                    if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
                    {
                        Destroy(this.gameObject);
                    }
                    
                    Destroy(this.gameObject, 1f);
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

    void DestroyThis()
    {
        Destroy(this.gameObject, 3);
    }

    void SpawnBlood()
    {
        Vector3 bloodpos = new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z);
        Instantiate(blood, bloodpos, Quaternion.identity);
    }
}
