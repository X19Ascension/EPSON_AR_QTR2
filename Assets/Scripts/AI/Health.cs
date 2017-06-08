using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int HP;
    public GameObject HPBarPlaneTemplate;
    public bool DeathAnimationDone = false;

    private int i_MaxHP;
    private GameObject m_HPBar;

	// Use this for initialization
	void Start () {
        i_MaxHP = HP;

        m_HPBar = Instantiate(HPBarPlaneTemplate, this.gameObject.transform.position, Quaternion.identity) as GameObject;
        m_HPBar.transform.SetParent(this.gameObject.transform);
        m_HPBar.transform.Translate(0, m_HPBar.transform.lossyScale.y * 5, 0);

        m_HPBar.SetActive(false);

        HPBarPlaneTemplate = m_HPBar;
	}
	
	// Update is called once per frame
	void Update () {

        m_HPBar.transform.LookAt(Camera.main.transform.position, -Vector3.up);
        m_HPBar.transform.Rotate(new Vector3(0, 180, 0));

        if (HP < i_MaxHP)
        {
            m_HPBar.SetActive(true);

            if (!this.gameObject.tag.Contains("Enemy"))
                DeathAnimationDone = true;

            if (HP <= 0 && DeathAnimationDone)
            {
                if (this.tag.Contains("Enemy"))
                    Destroy(this.gameObject);
                else
                    this.gameObject.SetActive(false);
            }
        }
	}

    void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.tag.Contains("FriendlyFire"))
        {
            if (col.gameObject.GetComponent<Projectile>().Sender.GetComponent<TeamHandler>().Team != this.gameObject.GetComponent<TeamHandler>().Team)
            {
                //if (col.gameObject.GetComponent<Projectile>().Sender.GetComponent<FSMBase>().GetTarget() == this.gameObject)
                {
                    TakeDamage(col.gameObject.GetComponent<Projectile>().Sender.GetComponent<FSMBase>().AttackDamage);
                    Destroy(col.gameObject);
                }
            }
        }
    }

    void RescaleHealthBar(int dmg)
    {
        float ScaleChange = (dmg / (float)i_MaxHP);
        m_HPBar.transform.localScale -= new Vector3(HPBarPlaneTemplate.transform.localScale.x * ScaleChange, 0, 0);
    }

    public void TakeDamage(int dmg)
    {
        RescaleHealthBar(dmg);

        HP -= dmg;
    }

    public void Reset()
    {
        HP = i_MaxHP;

        if (m_HPBar)
            m_HPBar.SetActive(false);
    }
}
