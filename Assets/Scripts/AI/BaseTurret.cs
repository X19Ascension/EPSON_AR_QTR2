using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class BaseTurret : FSMBase {

    public enum TURRET_STATE
    {
        IDLE,
        ATTACKING,
    }

    public enum TURRET_TYPE
    {
        STANDARD,
        SNIPER,
        HEAVY,
    }

    [Header("Tower Variables")]
    public TURRET_STATE CurrentState;
    public GameObject ProjectilePrefab;
    public TURRET_TYPE TurretType;
    public bool TurretActive = false;
    public BuildingAbstractBase AttachedWall;

    [Header("Building Variables")]
    public string BuildingName;
    public bool IsPreview = true;
    public int RequiredMilestoneLevel = 1;

    [Header("Milestone Values")]
    public float EconomicValue;
    public float DefensiveValue;

    public string FlavourName;
    public string Description;
    public int DCost;
    public int ECost;

    double d_Timer;

    // Use this for initialization
    void Start() {

        if (!IsPreview && gameObject.name.Contains("Wall"))
        {
            for (int i = 0; i < PersistentData.m_Instance.BuildingGridPos.Count; ++i)
            {
                if (GridPos.Equals((PersistentData.m_Instance.BuildingGridPos[i]).GetVec2()) && !PersistentData.m_Instance.BuildingName[i].Contains("+")/* && !gameObject.name.Contains(PersistentData.m_Instance.BuildingName[i])*/)
                {
                    PersistentData.m_Instance.BuildingName[i] += "+" + gameObject.name;
                    break;
                }
            }
        }
        else
        {

            if (!IsPreview && !CheckIfExist())
            {
                PersistentData.m_Instance.BuildingGridPos.Add(new SerializableVector2(GridPos.x, GridPos.y));
                PersistentData.m_Instance.BuildingName.Add(BuildingName);
            }
        }
    }

    public override void Sense()
    {
        Message temp = ReadFromMessageBoard();
        if (temp != null)
        {
            CurrentMessage = temp;
        }

        // Search for target
        if (!m_TargetedEnemy)
            SerchForTarget();

        if (BuildingName.Contains("Wall") && (!AttachedWall || !AttachedWall.gameObject.activeInHierarchy))
        {
            GetComponent<Health>().HP = 0;
        }

    }

    public override int Think()
    {
        switch (CurrentState)
        {
            case TURRET_STATE.IDLE:

                if (m_TargetedEnemy)
                    return (int)TURRET_STATE.ATTACKING;

                return (int)TURRET_STATE.IDLE;

            case TURRET_STATE.ATTACKING:

                if (m_TargetedEnemy.GetComponent<Health>().HP <= 0)
                {
                    m_TargetedEnemy = null;
                    return (int)TURRET_STATE.IDLE;
                }
                return (int)TURRET_STATE.ATTACKING;
        }

        return -1;
    }

    public override void Act(int value)
    {
        if (!TurretActive)
            return;

        switch (value)
        {
            case (int)TURRET_STATE.IDLE:
                CurrentState = TURRET_STATE.IDLE;

                // Make turret head rotate
                this.gameObject.transform.GetChild(1).transform.RotateAround(this.gameObject.transform.position, Vector3.up, (5 + Random.Range(0, 5)) * Time.deltaTime);
                break;

            case (int)TURRET_STATE.ATTACKING:
                CurrentState = TURRET_STATE.ATTACKING;

                // Make turret head aim at enemy
                Vector3 dir = (m_TargetedEnemy.transform.position- this.gameObject.transform.GetChild(1).transform.position).normalized;
                dir.y = 0;
                Quaternion lookRotation = Quaternion.LookRotation(dir);

                this.gameObject.transform.GetChild(1).transform.rotation = Quaternion.Slerp(this.gameObject.transform.GetChild(1).transform.rotation, lookRotation /** Quaternion.Euler(0, 90, 0)*/, Time.deltaTime * 5);

                // Check if can fire
                if (d_Timer > AttackSpeed)
                {
                    //this.gameObject.transform.GetChild(1).transform.LookAt(m_TargetedEnemy.transform.position);

                    //if (Vector3.Dot(this.gameObject.transform.GetChild(1).transform.rotation.eulerAngles, dir) > 0.75f)
                    float dot = Vector3.Dot(this.gameObject.transform.GetChild(1).transform.forward, dir);
                    if (dot > 0.75)
                    {
                        if (TurretType != TURRET_TYPE.HEAVY)
                        {
                            dir = (m_TargetedEnemy.transform.position - this.gameObject.transform.GetChild(1).transform.position).normalized;

                            GameObject go = Instantiate(ProjectilePrefab, this.gameObject.transform.GetChild(1).transform.position, lookRotation * Quaternion.Euler(90, 0, 0)) as GameObject;
                            go.GetComponent<Rigidbody>().AddForce(dir * go.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
                            go.GetComponent<Projectile>().Sender = this.gameObject;
                        }
                        else
                        {
                            // Fire 1st projectile
                            dir = (m_TargetedEnemy.transform.position - this.gameObject.transform.GetChild(1).transform.GetChild(0).transform.position).normalized;

                            GameObject go = Instantiate(ProjectilePrefab, this.gameObject.transform.GetChild(1).transform.GetChild(0).transform.position, lookRotation * Quaternion.Euler(90, 90, 90)) as GameObject;
                            go.GetComponent<Rigidbody>().AddForce(dir * go.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
                            go.GetComponent<Projectile>().Sender = this.gameObject;

                            // Fire 2nd projectile
                            dir = (m_TargetedEnemy.transform.position - this.gameObject.transform.GetChild(1).transform.GetChild(1).transform.position).normalized;

                            go = Instantiate(ProjectilePrefab, this.gameObject.transform.GetChild(1).transform.GetChild(1).transform.position, lookRotation * Quaternion.Euler(90, 90, 90)) as GameObject;
                            go.GetComponent<Rigidbody>().AddForce(dir * go.GetComponent<Projectile>().ProjectileSpeed, ForceMode.Impulse);
                            go.GetComponent<Projectile>().Sender = this.gameObject;
                        }
                    }

                    d_Timer = 0.0;
                }
                else
                    d_Timer += Time.deltaTime;
                break;
        }
    }

    public override void ProcessMessage()
    {

    }

    void SerchForTarget()
    {
        // Get available targets
        GameObject[] AvailableTargets = GameObject.FindGameObjectsWithTag("Enemy");

        List<GameObject> TargetsInRange = new List<GameObject>();

        foreach (GameObject go in AvailableTargets)
        {
            if ((go.transform.position - this.gameObject.transform.position).sqrMagnitude < AttackRange * AttackRange && go.GetComponent<Health>().HP > 0)
            {
                TargetsInRange.Add(go);
            }
        }


        // Search for closest one
        float closestDist = 9999999;
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

        m_TargetedEnemy = closestGo;
    }

    bool CheckIfExist()
    {
        if (PersistentData.m_Instance.BuildingGridPos.Contains(new SerializableVector2(GridPos.x, GridPos.y)))
        {
            return true;
        }

        return false;
    }

    void OnDisable()
    {
        if (!IsPreview)
        {
            Shutdown();
        }
    }

    void Shutdown()
    {
        // Check if building is destroyed or just being re-generated
        if (GetComponent<Health>().HP <= 0)
        {
            // Special case for wall turrets
            if (this.name.Contains("Wall"))
                return;

            //Remove from Persistent Data
            for (int i = 0; i < PersistentData.m_Instance.BuildingName.Count; ++i)
            {
                if (this.GridPos == PersistentData.m_Instance.BuildingGridPos[i].GetVec2())
                {
                    PersistentData.m_Instance.BuildingGridPos.RemoveAt(i);
                    PersistentData.m_Instance.BuildingName.RemoveAt(i);
                }
            }
        }
    }

    public void Reset()
    {
        CurrentState = TURRET_STATE.IDLE;
        this.gameObject.GetComponent<Health>().Reset();
        m_TargetedEnemy = null;
    }

    public override float GetMoveSpeed()
    {
        return 0;
    }
}
