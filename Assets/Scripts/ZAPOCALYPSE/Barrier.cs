using UnityEngine;
using System.Collections;

public class Barrier : EntityBase {
    Mesh mesh;
    public Mesh barrier_state_damaged;
    public Mesh barrier_state_destroyed;
    public GameObject barrier;
    private HealthBar testHealth;
    // Use this for initialization
    void Start () {
        i_maxHP = HP;
        //mesh = GetComponent<MeshFilter>().sharedMesh;
        //testHealth = this.gameObject.GetComponent<HealthBar>();
    }
	
	// Update is called once per frame
	void Update () {
        //testHealth.B_RescaleHealthBar(HP);
        if (HP < i_maxHP)
        {
            //i_maxHP.SetActive(true);
            if (HP <= 250 && HP > 0)
            {
                GetComponent<MeshFilter>().sharedMesh = barrier_state_damaged;
            }
            if (HP <= 0)
            {
                GetComponent<MeshFilter>().sharedMesh = barrier_state_destroyed;
                GetComponent<BoxCollider>().enabled = false;
            }
        }
        
    }

    public override void RunFSM()
    {
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

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "EnemyBullet" && col.gameObject.GetComponent<Projectile>().Sender.tag == "test")
        {
            TakeDamage(col.gameObject.GetComponent<Projectile>().Sender.GetComponent<RZombie>().GetAttackDamage());
            //testHealth.B_RescaleHealthBarDamage(col.gameObject.GetComponent<Projectile>().Sender.GetComponent<RZombie>().GetAttackDamage());
            Destroy(col.gameObject);
        }
    }
}
