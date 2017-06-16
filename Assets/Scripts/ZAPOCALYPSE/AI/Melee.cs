using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Melee : Survivor
{
    public enum Melee_State
    {
        S_IDLE = 1,
        S_ATTACK,
        S_DEAD,
    }

    public GameObject target;
    public Melee_State meleestate;

    void Awake()
    {
        target = null;
        meleestate = Melee_State.S_IDLE;
    }
    // Use this for initialization
    void Start ()
    {
        this.atkRange = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        RunFSM();
    }

    public override void RunFSM()
    {
        if (this.HP <= 0)
        {
            meleestate = Melee_State.S_DEAD;
        }
        switch (meleestate)
        {
            case Melee_State.S_IDLE:
                {
                    if (this.Enemynear(this.atkRange))
                    {
                        target = SelectTarget();
                        meleestate = Melee_State.S_ATTACK;
                    }
                    break;
                }
            case Melee_State.S_ATTACK:
                {
                    if (target != null)
                    {
                        Vector3 V3_Direction = (target.transform.position - this.transform.position).normalized;
                        Attackenemy(V3_Direction);
                    }
                    else
                    {
                        meleestate = Melee_State.S_IDLE;
                    }
                    break;
                }
            case Melee_State.S_DEAD:
                {
                    Destroy(this.gameObject);
                    break;
                }
        }

    }

    GameObject SelectTarget()
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
                    <= 45 && Vector3.Distance(go.transform.position, this.transform.position) <= atkRange)
                {
                    Targetables.Add(go);
                }
            }

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
        return null;
    }

    void Attackenemy(Vector3 Direction)
    {
        attackRate -= Time.deltaTime;
        if (attackRate <= 0)
        {
            Vector3 pew = this.gameObject.transform.position;
            GameObject bullet = null;
            //direction.y += 2;
            //direction.y += 0.05f;
            bullet = Instantiate(EProjectile, this.gameObject.transform.position, Quaternion.identity) as GameObject;

            bullet.GetComponent<Rigidbody>().AddForce(Direction * bullet.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
            bullet.GetComponent<Projectile>().Sender = this.gameObject;
            bullet.transform.parent = this.transform.parent;
            attackRate = atkSpd;
        }
    }
}
