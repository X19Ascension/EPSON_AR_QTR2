using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Zombie : EntityBase {
    private ScoringSystem scoring;
    public HealthBar testHealth;

    //EntityBase test;
    public GameObject spawnerGO;
    public GameObject OriginPOint;//! Spawner Game Object

    public float moveSpd;
    [HideInInspector]
    public float attackRate;
    int Threat;

    [SerializeField]
    protected List<GameObject> GO_SpawnPointFollower;
    [SerializeField]
    protected List<Vector3> V3_SpawnPointFollower;

    void Awake()
    {
        atkRange = 999;
        testHealth = this.gameObject.GetComponent<HealthBar>();
    }

    // Use this for initialization
    void Start()
    {
        i_maxHP = HP;
        attackRate = GetAttackSpeed();
        scoring = GameObject.Find("ScoringText").GetComponent<ScoringSystem>();
        OriginPOint = GameObject.Find("TownSpawn");
        GO_SpawnPointFollower = new List<GameObject>();
        V3_SpawnPointFollower = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        scoring = GameObject.Find("ScoringText").GetComponent<ScoringSystem>();
        testHealth.RescaleHealthBar(HP);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if (HP <= 0)
        {
            if (this.tag.Contains("test"))
            {
                spawnerGO.GetComponent<WaveSpawner>().maxAmount--;
                spawnerGO.GetComponent<WaveSpawner>().killcount++;
                Destroy(this.gameObject);

            }
            else
                this.gameObject.SetActive(false);
        }

        Regenerate();

        RunFSM();
    }

    public override void RunFSM()
    {

    }

    // Used when Attacking.
    protected GameObject TargetToAttack()
    {
        GameObject[] AllEntities = GameObject.FindGameObjectsWithTag("Entities");
        GameObject[] AllSurvivors = GameObject.FindGameObjectsWithTag("Survivor");
        GameObject[] AllBarriers = GameObject.FindGameObjectsWithTag("Barrier");
        // Get available targets
        GameObject[] AvailableTargets = ((AllEntities.Union<GameObject>(AllSurvivors)).Union<GameObject>(AllBarriers)).ToArray<GameObject>();//GameObject.FindGameObjectsWithTag("Survivor");

        List<GameObject> TargetsInRange = new List<GameObject>();

        foreach (GameObject go in AvailableTargets)
        {
            if ((go.transform.position - this.gameObject.transform.position).sqrMagnitude < atkRange * atkRange && go.GetComponent<EntityBase>().HP > 0)
            {
                TargetsInRange.Add(go);
            }
        }


        // Search for closest one
        float closestDist = 999999;
        GameObject closestGo = null;

        foreach (GameObject go in TargetsInRange)
        {
            float dist = (go.transform.position - this.gameObject.transform.position).sqrMagnitude;
            if (dist < closestDist * closestDist)
            {
                closestDist = dist;
                closestGo = go;
            }
        }

        return closestGo;
    }
    
    protected void TakeDamage(int damage)
    {
        testHealth.RescaleHealthBarDamage(damage);
        int health = this.GetHealth() - damage;
        this.SetHealth(health);
    }

    void Regenerate()
    {
        m_TimerDT += Time.deltaTime;

        if (m_TimerDT >= 1)
        {
            testHealth.RescaleHealthBarHeal((int)HPRegen);
            HP += (int)HPRegen;
            if (HP >= i_maxHP)
                HP = i_maxHP;

            m_TimerDT = 0;
        }
    }

    public override float GetAttackSpeed() {
        return atkSpd;
    }
    public override int GetAttackDamage() {
        return atkDmg;
    }
    public override int GetHealth() {
        return HP;
    }
    public override void SetHealth(int health)
    {
        HP = health;
    }
    public override float GetHealthRegen() {
        return HPRegen;
    }
    public override int GetMaxHealth()
    {
        return i_maxHP;
    }
    public override void SetMaxHealth(int maxhealth)
    {
        i_maxHP = maxhealth;
    }


    public void SetThreat(int threat)
    {
        Threat = threat;
    }

    public int GetThreat()
    {
        return Threat;
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Colliding With: " + col.gameObject.name);
        //// If the object we hit is the enemy
        //if (col.gameObject.tag == "test")
        //{
        //    // force is how forcefully we will push the player away from the enemy.
        //    float force = 3;

        //        // Calculate Angle Between the collision point and the player
        //        Vector3 dir = col.contacts[0].point - transform.position;
        //        dir = -dir.normalized;
        //}


        scoring = GameObject.FindGameObjectWithTag("Scoring").GetComponent<ScoringSystem>();
        //if (col.gameObject.tag.Contains("FriendlyFire"))
        //Debug.Log(col.gameObject.tag);
        if (col.gameObject.GetComponent<Projectile>().Sender.tag == "Survivor" && col.gameObject.tag == "Bullet")
        {
            //if (col.gameObject.GetComponent<Projectile>().Sender.GetComponent<FSMBase>().GetTarget() == this.gameObject)
            {
                if (col.gameObject.GetComponent<Projectile>().Sender.name == "Rifle_Final(Clone)" || col.gameObject.GetComponent<Projectile>().Sender.name == "Rifle_Final")
                {
                    //Debug.Log((col.gameObject.GetComponent<Projectile>().ProjectileLifetime / 0.65f));
                    //Debug.Log((col.gameObject.GetComponent<Projectile>().ProjectileLifetime / 3.0f));
                    int damage = (int)(col.gameObject.GetComponent<Projectile>().Sender.GetComponent<Survivor>().GetAttackDamage() * (col.gameObject.GetComponent<Projectile>().ProjectileLifetime / 3.0f));
                    if (col.gameObject.GetComponent<Projectile>().ProjectileLifetime > Random.Range(0, 20))
                    {
                        damage *= 2;
                        Debug.Log("Crit boi");
                    }

                    TakeDamage(damage);
                    //if (HP <= 0)
                        scoring.CalculateScore(col.gameObject.GetComponent<Projectile>().Sender, this.gameObject);
                    Destroy(col.gameObject);

                }
                //if (col.gameObject.GetComponent<Projectile>().Sender.name == "Rifle_Final(Clone)")
                else
                {
                    TakeDamage(col.gameObject.GetComponent<Projectile>().Sender.GetComponent<Survivor>().GetAttackDamage());
                    //if (HP <= 0)
                        scoring.CalculateScore(col.gameObject.GetComponent<Projectile>().Sender, this.gameObject);
                    Destroy(col.gameObject);
                }
            }
        }
    }

    void OnCollisionStay(Collision colInfo)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void OnCollisionExit(Collision colInfo)
    {
        Debug.Log("Colliding With: " + colInfo.gameObject.name);
        if (colInfo.gameObject.tag == "test" && colInfo.gameObject.tag == "Bullet")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }

    protected void ChoosePointSequence(GameObject target)
    {
        int Spawnpoint = spawnerGO.GetComponent<WaveSpawner>().GetSpawnPoint();
        Debug.Log(Spawnpoint.ToString());
        Spawnpoint += 1;
        if (Spawnpoint > 4)
        {
            Spawnpoint = 4;
        }
        PickSpawnPointSequence(Spawnpoint);
        if (target != null)
        {
            V3_SpawnPointFollower.Add(target.transform.position);
        }
    }

    protected void PickSpawnPointSequence(int SpawnPoint)
    {
        string temp = SpawnPoint.ToString();
        GO_SpawnPointFollower = GameObject.FindGameObjectsWithTag("SpawnPoint" + temp).ToList();
        for (int i = 0; i < 10; i++)
        {
            Vector3 temp2 = GO_SpawnPointFollower[i].transform.position;
            temp2 += new Vector3(Random.Range(1, 4), 0, Random.Range(1, 4));
            V3_SpawnPointFollower.Add(temp2);
        }
    }

    protected void ChaseSequence()
    {
        int index = 0;

        float Distance = Vector3.Distance(this.gameObject.transform.position, V3_SpawnPointFollower[index]);
        FollowPath(V3_SpawnPointFollower[index]);
        
        if (this.transform.position == V3_SpawnPointFollower[index] && index != V3_SpawnPointFollower.Count && index < V3_SpawnPointFollower.Count)
        {
            index += 1;
            FollowPath(V3_SpawnPointFollower[index]);

        }
    }

    protected void FollowPath(Vector3 V3)
    {
        Vector3 Dir = (V3 - this.transform.position).normalized;
        this.gameObject.transform.localPosition += Dir * moveSpd * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(Dir);
        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    public int CheckSurroundings()
    {
        GameObject[] Zombies = GameObject.FindGameObjectsWithTag("test");
        List<GameObject> Surrounded = new List<GameObject>();
        foreach(GameObject go in Zombies)
        {
            if (Vector3.Distance(go.transform.position, this.transform.position) < 5f)
            {
                Surrounded.Add(go);
            }
        }
        if(Surrounded.Count < 1)
        {
            return Surrounded.Count;
        }
        return 0;
    }
}
