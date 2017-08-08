using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public GameObject HPBarPlaneTemplate;
    private GameObject m_HPBar;


    // Use this for initialization
    void Start () {
        m_HPBar = Instantiate(HPBarPlaneTemplate, this.gameObject.transform.position, Quaternion.identity) as GameObject;
        m_HPBar.transform.localScale += new Vector3(2.0f, 0.4f, 0);
        m_HPBar.transform.SetParent(this.gameObject.transform);
        m_HPBar.transform.Translate(0, m_HPBar.transform.lossyScale.y * 12, 0);
        
        m_HPBar.SetActive(false);

        HPBarPlaneTemplate = m_HPBar;
    }
	
	// Update is called once per frame
	void Update () {
        m_HPBar.transform.LookAt(Camera.main.transform.position, -Vector3.up);
        m_HPBar.transform.Rotate(new Vector3(0, 180, 0));

        if (this.gameObject.tag == "test" && this.gameObject.GetComponent<Zombie>().HP < this.gameObject.GetComponent<Zombie>().i_maxHP)
            m_HPBar.SetActive(true);
        else if (this.gameObject.tag == "Barrier" && this.gameObject.GetComponent<Barrier>().HP < this.gameObject.GetComponent<Barrier>().i_maxHP)
            m_HPBar.SetActive(true);
    }

    // Autistic Functions, forgive me.
    public void RescaleHealthBar(int val)
    {
        float ScaleChange = (val / (float)this.gameObject.GetComponent<Zombie>().GetMaxHealth());
        m_HPBar.transform.localScale -= new Vector3(HPBarPlaneTemplate.transform.localScale.x * ScaleChange, 0, 0);
    }
    public void RescaleHealthBarDamage(int damage)
    {
        float ScaleChange = (damage / (float)this.gameObject.GetComponent<Zombie>().GetMaxHealth());
        m_HPBar.transform.localScale -= new Vector3(HPBarPlaneTemplate.transform.localScale.x * ScaleChange, 0, 0);
    }
    public void RescaleHealthBarHeal(int heal)
    {
        float ScaleChange = (heal / (float)this.gameObject.GetComponent<Zombie>().GetMaxHealth());
        m_HPBar.transform.localScale += new Vector3(HPBarPlaneTemplate.transform.localScale.x * ScaleChange, 0, 0);
    }

    //// Another Autistic Functions.
    //public void B_RescaleHealthBar(int val)
    //{
    //    float ScaleChange = (val / (float)this.gameObject.GetComponent<Barrier>().GetMaxHealth());
    //    m_HPBar.transform.localScale -= new Vector3(HPBarPlaneTemplate.transform.localScale.x * ScaleChange, 0, 0);
    //}
    //public void B_RescaleHealthBarDamage(int damage)
    //{
    //    float ScaleChange = (damage / (float)this.gameObject.GetComponent<Barrier>().GetMaxHealth());
    //    m_HPBar.transform.localScale -= new Vector3(HPBarPlaneTemplate.transform.localScale.x * ScaleChange, 0, 0);
    //}
    //public void B_RescaleHealthBarHeal(int heal)
    //{
    //    float ScaleChange = (heal / (float)this.gameObject.GetComponent<Barrier>().GetMaxHealth());
    //    m_HPBar.transform.localScale += new Vector3(HPBarPlaneTemplate.transform.localScale.x * ScaleChange, 0, 0);
    //}
}
