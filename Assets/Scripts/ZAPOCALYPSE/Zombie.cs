using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Zombie : EntityBase {

    public enum ZOMBIE_STATE
    {
        Z_IDLE,
        Z_CHASE,
        Z_ATTACK,
        Z_DEAD,
    }

    public enum ZOMBIE_TYPE
    {
        Z_STANDARD,
        Z_ZEALOUS,
        Z_TANKY,
        Z_RANGED,
    }

    //EntityBase test;
    public ZOMBIE_STATE zombieState = ZOMBIE_STATE.Z_IDLE;
    public ZOMBIE_TYPE zombieType = ZOMBIE_TYPE.Z_STANDARD;
    public GameObject spawnerGO;                                    //! Spawner Game Object

    public float moveSpd;
    [HideInInspector]
    public float attackRate;

    // Use this for initialization
    void Start()
    {
        i_maxHP = HP;
        attackRate = GetAttackSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        m_TimerDT += Time.deltaTime;

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


        if (m_TimerDT >= 1)
        {
            HP += (int)HPRegen;
            //this.gameObject.GetComponent<HealthBar>().RescaleHealthBarHeal((int)HPRegen);

            if (HP >= i_maxHP)
            {
                HP = i_maxHP;
                //this.gameObject.GetComponent<HealthBar>().RescaleHealthBar(i_maxHP);
            }

            m_TimerDT = 0;
        }

        RunFSM();
    }

    public override void RunFSM()
    {
        GameObject m_TargetedEnemy = TargetToAttack();

        switch (zombieState)
        {
            case ZOMBIE_STATE.Z_IDLE:

                if (GetClosestDestination() != null)
                    zombieState = ZOMBIE_STATE.Z_CHASE;

                if (this.HP <= 0)
                    zombieState = ZOMBIE_STATE.Z_DEAD;

                break;
            case ZOMBIE_STATE.Z_CHASE:

                if (this.HP <= 0)
                    zombieState = ZOMBIE_STATE.Z_DEAD;

                if (m_TargetedEnemy == null && GetClosestDestination() != null) 
                {
                    GameObject targetToChase = GetClosestDestination();
                    // Translate
                    Vector3 dir = (targetToChase.transform.position - this.gameObject.transform.position).normalized;
                    dir.y = 0;
                    this.gameObject.transform.position += dir * moveSpd * Time.deltaTime;

                    // Rotate
                    Quaternion lookRotation = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
                }
                else if (GetClosestDestination() == null)
                    zombieState = ZOMBIE_STATE.Z_IDLE;  
                else
                    zombieState = ZOMBIE_STATE.Z_ATTACK;
                

                break;
            case ZOMBIE_STATE.Z_ATTACK:

                if (this.HP <= 0)
                    zombieState = ZOMBIE_STATE.Z_DEAD;

                if (m_TargetedEnemy != null)
                    AttackEnemy(m_TargetedEnemy);
                else
                    zombieState = ZOMBIE_STATE.Z_CHASE;

                break;
            case ZOMBIE_STATE.Z_DEAD:
                Destroy(this.gameObject);
                break;
        }
    }

    // Used when Attacking.
    GameObject TargetToAttack()
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

    void AttackEnemy(GameObject target)
    {
        attackRate -= Time.deltaTime;

        if (attackRate < 0)
        {
            int health = target.GetComponent<EntityBase>().GetHealth() - this.GetAttackDamage();
            target.GetComponent<EntityBase>().SetHealth(health);

            attackRate = this.GetAttackSpeed();
        }
    }

    void TakeDamage(int damage)
    {
        int health = this.GetHealth() - damage;
        this.SetHealth(health);
        this.gameObject.GetComponent<HealthBar>().RescaleHealthBarDamage(damage);
    }

    // For enemy use
    protected GameObject GetClosestDestination()
    {
        GameObject[] AllEntities = GameObject.FindGameObjectsWithTag("Entities");
        GameObject[] AllSurvivors = GameObject.FindGameObjectsWithTag("Survivor");
        GameObject[] AllBarriers = GameObject.FindGameObjectsWithTag("Barrier");
        // Get available targets
        GameObject[] AvailableTargets = ((AllEntities.Union<GameObject>(AllSurvivors)).Union<GameObject>(AllBarriers)).ToArray<GameObject>();//GameObject.FindGameObjectsWithTag("Survivor");

        float closestDist = float.MaxValue;
        GameObject closestGo = null;

        foreach (GameObject go in AvailableTargets)
        {
            float dist = (go.transform.position - this.gameObject.transform.position).sqrMagnitude;
            if (dist < closestDist * closestDist)
            {
                closestDist = dist;
                closestGo = go;
            }
        }

        return closestGo; // If none, it will return the null it was assigned with.
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


    void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.tag.Contains("FriendlyFire"))
        //Debug.Log(col.gameObject.tag);
            if (col.gameObject.GetComponent<Projectile>().Sender.tag == "Survivor" && col.gameObject.tag == "Bullet")
            {
                //if (col.gameObject.GetComponent<Projectile>().Sender.GetComponent<FSMBase>().GetTarget() == this.gameObject)
                {
                    TakeDamage(col.gameObject.GetComponent<Projectile>().Sender.GetComponent<Survivor>().GetAttackDamage());
                    
                    Destroy(col.gameObject);
                }
            }
        
    }
}
