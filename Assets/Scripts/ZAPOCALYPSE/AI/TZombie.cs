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
    private Animator anim;

    public AudioClip[] zombieSound;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        Zombiestate = TZombieSTATE.S_IDLE;
        f_movespeed = 0.8f;
        f_attackRate = 0.9f;
        i_Threatvalue = 5;
        atkRange = 999;
    }
	// Use this for initialization
	void Start ()
    {
        testHealth = this.gameObject.GetComponent<HealthBar>();
        anim = GetComponent<Animator>();
        atkSpd = 0.2f;
        f_attackRate = atkSpd;
        this.moveSpd = 1.5f;

        this.moveSpd = Random.Range(1.0f, 2.0f);
        // this.gameObject.transform.parent = GameObject.Find("TerrainSpawn").transform;
        //SpawnerGO = GameObject.Find("SpawnerPrefab");
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

                        Quaternion lookRotation = Quaternion.LookRotation(dir);

                        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 5);
                        if (!source.isPlaying)
                            source.PlayOneShot(zombieSound[0], 1F);
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
                    if (!source.isPlaying)
                        source.PlayOneShot(zombieSound[3], 1F);
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
            if (!source.isPlaying)
                source.PlayOneShot(zombieSound[1], 1F);
            int health = target.GetComponent<EntityBase>().GetHealth() - this.GetAttackDamage();
            target.GetComponent<EntityBase>().SetHealth(health);

            f_attackRate = this.GetAttackSpeed();
        }
    }

}
