//! Brief description.

//! Detailed description 
//! starts here.

using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;

public abstract class EntityBase : MonoBehaviour 
{

    [Header("Entity Attack Stats")]
    public float atkRange = 1;                              ///! Attack Range of Unit
    [Tooltip("Amount of delay between attacks in secs")]
    public float atkSpd = 1;                                ///! Attack Speed of Unit
    public int atkDmg = 1;                                  ///! Attack Damage of Unit

    [Header("Entity Health Stats")]
    public int HP;                                          ///! Health of Unit
    [Tooltip("Amount Health Regen Per Second")]
    public float HPRegen;                                   ///! Health Regeneration of Unit
    public int i_maxHP;                                    ///! Max Health of Unit, Set when HP is set
    [HideInInspector]
    public float m_TimerDT;

    public GameObject HPBarPlaneTemplate;
    private GameObject m_HPBar;

    // Use this for initialization
    void Start () {
        i_maxHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        m_TimerDT += Time.deltaTime;
    }
    
    //public abstract int UpdateFSM();            ///! process the updates
    public abstract void RunFSM();                      ///! act upon any change in behaviour   
    public abstract float GetAttackSpeed();             ///! Returns Attack Speed of Unit
    public abstract int GetAttackDamage();              ///! Returns Attack Damage of Unit
    public abstract int GetHealth();                    ///! Returns Health of Unit
    public abstract int GetMaxHealth();                 ///! Returns Max Health of Unit
    public abstract void SetHealth(int health);         ///! Sets Health of Unit
    public abstract void SetMaxHealth(int maxhealth);   ///! Sets Max Health of Unit
    public abstract float GetHealthRegen();             ///! Returns Health of Unit
}
