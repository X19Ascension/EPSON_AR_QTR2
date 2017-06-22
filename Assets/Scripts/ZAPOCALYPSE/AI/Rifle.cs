using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rifle : Survivor
{ 
    public enum Rifle_State
    {
        S_IDLE = 1,
        S_PANIC,
        S_ATTACK,
        S_DEAD,
    }

    public GameObject target;
    public Rifle_State riflestate;

    void Awake()
    {
        target = null;
        riflestate = Rifle_State.S_IDLE;
    }

	void Start ()
    {
        this.atkRange = 50.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        RunFSM();
	}


    public override void RunFSM()
    {
        if(this.HP <= 0 )
        {
            riflestate = Rifle_State.S_DEAD;
        }
        switch (riflestate)
        {
            case Rifle_State.S_IDLE:
                {
                    //if(this.Enemynear(0.5f))
                    if(this.Enemynear(atkRange))
                    {
                        target = SelectTarget(this.atkRange);
                        riflestate = Rifle_State.S_ATTACK;
                    }
                    break;
                    
                }
            case Rifle_State.S_PANIC:
                {
                    break;
                }
            case Rifle_State.S_ATTACK:
                {
                    if (target != null) 
                    {
                        Vector3 V3_Direction = (target.transform.position - this.transform.position).normalized;
                        Attackenemy(V3_Direction);
                    }
                    else
                    {
                        riflestate = Rifle_State.S_IDLE;
                    }
                    break;
                }
            case Rifle_State.S_DEAD:
                {
                    Destroy(this.gameObject);
                    break;
                }
        }

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }
}
