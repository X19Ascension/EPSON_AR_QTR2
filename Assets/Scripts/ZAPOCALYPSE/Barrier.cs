using UnityEngine;
using System.Collections;

public class Barrier : EntityBase {

    public GameObject barrier;
    // Use this for initialization
    void Start () {
        i_maxHP = HP;
	}
	
	// Update is called once per frame
	void Update () {
        if (HP < i_maxHP)
        {
            //i_maxHP.SetActive(true);

            if (HP <= 0)
            {
                //barrier.SetActive(false);
                //if (this.tag.Contains("Barrier"))
                    Destroy(this.gameObject);
                //else
                //    this.gameObject.SetActive(false);
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
}
