using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Survivor : EntityBase
{
    
    public enum SURVIVOR_TYPE
    {
        S_RIFLE,
        S_MELEE,
        S_SHOTGUN,
        S_MEDIC,
        S_MECHANIC,
    }

    public GameObject EProjectile;

    //EntityBase test;
    public SURVIVOR_TYPE entityType = SURVIVOR_TYPE.S_RIFLE;
    [HideInInspector]
    public float attackRate;

    [SerializeField]
    GameObject Directionpoint;
    GridMap The_Grid;

    
    float f_enmity;

    void Awake()
    {
        Directionpoint = new GameObject();
        The_Grid = GameObject.Find("Grid Spawner").GetComponent<GridMap>();
    }

    // Use this for initialization
    void Start()
    {
        //i_maxHP = HP;
        attackRate = GetAttackSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        m_TimerDT += Time.deltaTime;
        

        if (m_TimerDT >= 2)
        {
            HP += (int)HPRegen;
            if (HP >= i_maxHP)
                HP = i_maxHP;

            m_TimerDT = 0;
        }

        //RunFSM();

        f_enmity = this.HP / this.i_maxHP;
    }

    public override void RunFSM()
    {
        
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
        }
    }

    void ShoveEnemy(GameObject target)
    {

    }

    public GameObject SelectTarget(float Radius)
    {
        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("test");
        List<GameObject> Targetables = new List<GameObject>();
        List<GameObject> TargetList = new List<GameObject>();
        if (AllEnemies != null)
        {
            foreach (GameObject go in AllEnemies)
            {
                if (go == null)
                {
                    continue;
                }
                else if (Vector3.Angle(go.transform.position - this.transform.position, this.GetDirection().transform.position - this.transform.position)
                    <= 45 && Vector3.Distance(go.transform.position, this.transform.position) <= Radius)
                {
                    Targetables.Add(go);
                }
                else
                    continue;
            }

            if (Targetables.Count > 0)
            {
                foreach (GameObject go in Targetables)
                {
                    for (int i = 0; i < go.GetComponent<Zombie>().GetThreat(); i++)
                    {
                        TargetList.Add(go);
                    }
                }
                int targetindex = Random.Range(0, TargetList.Count);

                return TargetList[targetindex];
            }

        }
        return null;
    }


    public bool Enemynear(float distance)
    {
        float closetdistance = 900;
        GameObject[] EnemyList = GameObject.FindGameObjectsWithTag("test");
        foreach(GameObject go in EnemyList)
        {
            float dist = Vector3.Distance(go.transform.position,this.gameObject.transform.position);
            if(dist < closetdistance)
            {
                closetdistance = dist;
            }
        }
        if(closetdistance < 50)
        {
            return true;
        }
        return false;
    }

    public float GetEnmity()
    {
        return f_enmity;
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
    public GameObject GetDirection()
    {
        return Directionpoint;
    }



    
}
