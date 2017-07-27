using UnityEngine;

public class NZomebie : Zombie {
    [Header("Entity Blood Splatter")]
    public GameObject blood;
    public GameObject blood1;
    public GameObject blood2;

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
    float pew;
    int i_Threatvalue;
    private Animator anim;

    public AudioClip[] zombieSound;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        Zombiestate = NZombie_STATE.S_IDLE;
        f_movespeed = 2f;
        f_attackRate = 0.2f;
        SetThreat(3);
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

        this.moveSpd = Random.Range(1.0f, 2.5f);
       // this.gameObject.transform.parent = GameObject.Find("TerrainSpawn").transform;
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
            //Destroy(this.gameObject);

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
                    if (go_targetedEnemy == null)
                    {
                        Zombiestate = NZombie_STATE.S_IDLE;
                    }
                    anim.SetBool("Chase", true);

                    float Distance = Vector3.Distance(this.gameObject.transform.position, go_targetedEnemy.gameObject.transform.position);
                    if (go_targetedEnemy != null)
                    {
                        if (!source.isPlaying)
                            source.PlayOneShot(zombieSound[Random.Range(0, 2)], 0.2F);
                        Vector3 dir = (go_targetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                        dir.y = 0;
                        this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;

                        Quaternion lookRotation = Quaternion.LookRotation(dir);

                        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 5);

                    }
                    if (Distance < 0.5f)
                    {
                        Zombiestate = NZombie_STATE.S_ATTACK; 
                        anim.SetBool("Chase", false);
                    }
                    break;
                }
            case NZombie_STATE.S_ATTACK:
                {
                    float Distance = Vector3.Distance(this.gameObject.transform.position, go_targetedEnemy.gameObject.transform.position);
                    if (go_targetedEnemy != null && Distance < 0.5f) 
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
                    gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
                    {
                        anim.SetTrigger("Die");
                        source.PlayOneShot(zombieSound[6], 0.2F);
                    }
                    //Debug.Log(anim.GetComponent<AnimationState>().normalizedTime);
                    pew += Time.deltaTime;
                    if (pew > 1.5f)
                    {

                        spawnerGO.GetComponent<WaveSpawner>().maxAmount--;
                        spawnerGO.GetComponent<WaveSpawner>().killcount++;
                        SpawnBlood();
                        Destroy(this.gameObject);
                    }

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
                source.PlayOneShot(zombieSound[Random.Range(3, 5)], 0.2F);
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

        Random.Range(0, 2);

        Instantiate(blood, bloodpos, Quaternion.identity);
    }
}
