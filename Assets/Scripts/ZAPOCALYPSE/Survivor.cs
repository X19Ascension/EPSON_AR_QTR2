using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Survivor : EntityBase
{

    public enum SURVIVOR_STATE
    {
        S_IDLE,
        S_ATTACK,
        S_HEAL,
        S_RELOAD,
        S_DEAD,
    }

    public enum SURVIVOR_TYPE
    {
        S_RIFLE,
        S_MELEE,
        S_SHOTGUN,
        S_MEDIC,
        S_MECHANIC,
    }

    public GameObject EProjectile;                                          //! Projectile used to shoot at Zombies

    //EntityBase test;
    public SURVIVOR_STATE survivorState = SURVIVOR_STATE.S_IDLE;            //! States of Survivor
    public SURVIVOR_TYPE entityType = SURVIVOR_TYPE.S_RIFLE;                //! Type of Survivor
    [HideInInspector]
    public float attackRate;                                                //! Attack Rate of Survivor
    public float reloadRate;                                                //! Reload Rate of Survivor
    private float reloadDt;
    private float idleDt = 5.0f;

    public int maxAmmo;                                                     //! Max Ammo Survivor Have
    private int currAmmo;                                                   //! Current Ammo Survivor Has

    [HideInInspector]
    public float experiencePt;                                              //! Experience Pt of Survivor to Level
    [HideInInspector]
    public float timeActive;                                              //! Experience Pt of Survivor to Level
    public int level;                                              //! Experience Pt of Survivor to Level

    public bool onFieldAnot;                                                //! To Check if Survivor is on Field

    GameObject Directionpoint;                                              //! To determine direction survivor shoot at when he at which grid.
    GridMap The_Grid;                                                       //! The Grid
    WaveSpawner The_Spawner;                                                //! The Grid

    void Awake()
    {
        Directionpoint = new GameObject();
        The_Grid = GameObject.Find("Grid Spawner").GetComponent<GridMap>();
        The_Spawner = GameObject.Find("SpawnerPrefab").GetComponent<WaveSpawner>();
    }

    // Use this for initialization
    void Start()
    {
        //i_maxHP = HP;
        currAmmo = maxAmmo;
        attackRate = GetAttackSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        m_TimerDT += Time.deltaTime;
        timeActive += Time.deltaTime;

        if (HP <= 0)
        {
            if (this.tag.Contains("Survivor"))
                Destroy(this.gameObject);
            else
                this.gameObject.SetActive(false);
        }

        if (m_TimerDT >= 2)
        {
            HP += (int)HPRegen;
            if (HP >= i_maxHP)
                HP = i_maxHP;

            m_TimerDT = 0;
        }


        //if (The_Spawner.waveEnded)
        //{
        //    gameObject.GetComponent<UnitGrowth>().CalculateEXPGain();
        //    //The_Spawner.waveNo++;
        //    The_Spawner.waveEnded = false;
        //}

        RunFSM();
    }

    public override void RunFSM()
    {
        GameObject m_TargetedEnemy = TargetToAttack();

        if (this.HP <= 0)
        {
            survivorState = SURVIVOR_STATE.S_DEAD;
        }
        switch (survivorState)
        {
            case SURVIVOR_STATE.S_IDLE:

                if (m_TargetedEnemy != null)
                {
                    survivorState = SURVIVOR_STATE.S_ATTACK;
                    idleDt = 5.0f;
                }

                if (currAmmo < maxAmmo && idleDt <= 0)
                    survivorState = SURVIVOR_STATE.S_RELOAD;
                else
                    idleDt -= Time.deltaTime;

                break;

            case SURVIVOR_STATE.S_ATTACK:
                
                if (m_TargetedEnemy != null)
                {
                    Vector3 dir = (m_TargetedEnemy.transform.position - this.gameObject.transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3.0f);
                    if (currAmmo > 0)
                        AttackEnemy(m_TargetedEnemy, dir, 20);
                    else
                        survivorState = SURVIVOR_STATE.S_RELOAD;
                }
                else
                    survivorState = SURVIVOR_STATE.S_IDLE;

                break;

            case SURVIVOR_STATE.S_RELOAD:
                

                if (reloadDt >= reloadRate)
                {
                    currAmmo = maxAmmo;
                    reloadDt = 0.0f;
                    idleDt = 5.0f;
                    survivorState = SURVIVOR_STATE.S_IDLE;
                }
                else
                    reloadDt += Time.deltaTime;

                break;

            case SURVIVOR_STATE.S_HEAL:

                if (m_TargetedEnemy == null)
                {
                }
                else if (GetClosestDestination() == null)
                    survivorState = SURVIVOR_STATE.S_IDLE;
                else
                    survivorState = SURVIVOR_STATE.S_ATTACK;


                break;

            case SURVIVOR_STATE.S_DEAD:
                Destroy(this.gameObject);
                break;
        }
    }

    GameObject TargetstoTrack()
    {

        return gameObject;
    }

    // Used when Attacking.
    GameObject TargetToAttack()
    {
        GameObject[] AllEntities = GameObject.FindGameObjectsWithTag("Entities");
        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("test");
        // Get available targets
        GameObject[] AvailableTargets = ((AllEntities.Union<GameObject>(AllEnemies))).ToArray<GameObject>();//GameObject.FindGameObjectsWithTag("Survivor");

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
            // Check for priority
            if (go.transform.FindChild("TargetPlaceHolder(Clone)"))
                return go;

            float dist = (go.transform.position - this.gameObject.transform.position).sqrMagnitude;
            if (dist < closestDist * closestDist)
            {
                closestDist = dist;
                closestGo = go;
            }
        }

        return closestGo;
    }

    // Ranged
    void AttackEnemy(GameObject target, Vector3 direction, int shellcount, bool spread = false)
    {
        attackRate -= Time.deltaTime;

        if (attackRate < 0)
        {
            Vector3 pew = this.gameObject.transform.position;
            GameObject bullet = null;

            if (entityType == SURVIVOR_TYPE.S_SHOTGUN)
            {
                for (int i = 0; i < shellcount; i++)
                {
                    float spreadpew = 0.1f;

                    float RandomX = Random.Range(-spreadpew, spreadpew);
                    float RandomZ = Random.Range(-spreadpew, spreadpew);

                    Vector3 offset = new Vector3(RandomX, 0, RandomZ);
                    direction = (offset + direction).normalized;
                    direction.Normalize();

                    //direction.y += 2;

                    bullet = Instantiate(EProjectile, pew, Quaternion.identity) as GameObject;
                    bullet.GetComponent<Rigidbody>().AddForce(direction * bullet.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
                    bullet.GetComponent<Projectile>().Sender = this.gameObject;
                    bullet.GetComponent<Projectile>().ProjectileLifetime = 0.085f;
                }
            }
            else
            {
                float spreadpew = 0.01f;

                float RandomX = Random.Range(-spreadpew, spreadpew);
                float RandomZ = Random.Range(-spreadpew, spreadpew);

                Vector3 offset = new Vector3(RandomX, 0, RandomZ);
                direction = (offset + direction).normalized;
                direction.Normalize();


                //direction.y += 2;
                //direction.y += 0.05f;
                bullet = Instantiate(EProjectile, this.gameObject.transform.position, Quaternion.identity) as GameObject;

                bullet.GetComponent<Rigidbody>().AddForce(direction * bullet.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
                bullet.GetComponent<Projectile>().Sender = this.gameObject;
            }
            bullet.transform.parent = this.transform.parent;

            //int health = target.GetComponent<EntityBase>().GetHealth() - this.GetAttackDamage();
            //target.GetComponent<EntityBase>().SetHealth(health);
            //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

            attackRate = this.GetAttackSpeed();

            currAmmo--;
        }
    }

    // Melee
    void AttackEnemy(GameObject target)
    {
        attackRate -= Time.deltaTime;

        if (attackRate < 0)
        {
            int health = target.GetComponent<EntityBase>().GetHealth() - this.GetAttackDamage();
            target.GetComponent<EntityBase>().SetHealth(health);

            attackRate = this.GetAttackSpeed();
            currAmmo--;
        }
    }

    // For survivor use
    protected GameObject GetClosestDestination()
    {
        GameObject[] AllEntities = GameObject.FindGameObjectsWithTag("Entities");
        //GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("test");
        // Get available targets
        GameObject[] AvailableTargets = ((AllEntities.Union<GameObject>(AllEnemies))).ToArray<GameObject>();//GameObject.FindGameObjectsWithTag("Survivor");
        List<GameObject> TargetsInRange = new List<GameObject>();

        foreach (GameObject go in AvailableTargets)
        {
            if ((go.transform.position - this.gameObject.transform.position).sqrMagnitude < atkRange * atkRange && go.GetComponent<EntityBase>().HP > 0)
            {
                TargetsInRange.Add(go);
            }
        }

        float closestDist = 999;
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

        return closestGo; // If none, it will return the null it was assigned with.
    }

    public override float GetAttackSpeed()
    {
        return atkSpd;
    }
    public override int GetAttackDamage()
    {
        return atkDmg;
    }
    public override int GetHealth()
    {
        return HP;
    }
    public override void SetHealth(int health)
    {
        HP = health;
    }
    public override float GetHealthRegen()
    {
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

    public void SetDirectionPoint(GameObject Direction)
    {
        Directionpoint = Direction;
    }
}
