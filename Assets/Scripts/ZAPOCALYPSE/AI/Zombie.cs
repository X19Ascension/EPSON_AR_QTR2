using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Zombie : EntityBase {
    
    //EntityBase test;
    public GameObject spawnerGO;                                    //! Spawner Game Object

    public float moveSpd;
    [HideInInspector]
    public float attackRate;
    int Threat;

    void Awake()
    {
        atkRange = 999;
    }

    // Use this for initialization
    void Start()
    {
        i_maxHP = HP;
        attackRate = GetAttackSpeed();
    }

    // Update is called once per frame
    void Update()
    {

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
        int health = this.GetHealth() - damage;
        this.SetHealth(health);
    }

    void Regenerate()
    {
        m_TimerDT += Time.deltaTime;

        if (m_TimerDT >= 1)
        {
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
        //if (col.gameObject.tag.Contains("FriendlyFire"))
        //Debug.Log(col.gameObject.tag);
        if (col.gameObject.GetComponent<Projectile>().Sender.tag == "Survivor" && col.gameObject.tag == "Bullet")
        {
            //if (col.gameObject.GetComponent<Projectile>().Sender.GetComponent<FSMBase>().GetTarget() == this.gameObject)
            {
                if (col.gameObject.GetComponent<Projectile>().Sender.name == "Rifle_Final(Clone)")
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
                    Destroy(col.gameObject);
                }
                else
                {
                    TakeDamage(col.gameObject.GetComponent<Projectile>().Sender.GetComponent<Survivor>().GetAttackDamage());
                    Destroy(col.gameObject);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, atkRange);
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
