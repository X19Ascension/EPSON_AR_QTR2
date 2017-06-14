using UnityEngine;
using System.Collections;

public class Rifle : Survivor
{
    enum Rifle_State
    {
        S_IDLE = 1,
        S_PANIC,
        S_ATTACK,
        S_DEAD,
    }

    GameObject target;
    Rifle_State riflestate;

    void Awake()
    {
        target = new GameObject();
        riflestate = Rifle_State.S_IDLE;
    }

	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        RunFSM();
	}


    void RunFSM()
    {
        if(this.HP <= 0 )
        {
            riflestate = Rifle_State.S_DEAD;
        }
        switch (riflestate)
        {
            case Rifle_State.S_IDLE:
                {
                    break;
                    
                }
            case Rifle_State.S_PANIC:
                {
                    break;
                }
            case Rifle_State.S_ATTACK:
                {
                    break;
                }
            case Rifle_State.S_DEAD:
                {
                    Destroy(this.gameObject);
                    break;
                }
        }

    }
}
