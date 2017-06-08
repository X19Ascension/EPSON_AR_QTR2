using UnityEngine;
using System.Collections;

public abstract class BuildingAbstractBase : MonoBehaviour {

    [Header("BuildingAbstractBase Values")]
    [Tooltip("Position in the grid, bottom left grid is (0,0)")]
    public Vector2 GridPos;
    public string BuildingName;
    public bool IsPreview = true;

    public string Description;
    public int DCost;
    public int ECost;

    [Header("Milestone Values")]
    public float EconomicValue;
    public float DefensiveValue;

    protected bool b_IsResidential = false;
    protected bool b_IsProduction = false;
    protected bool b_IsStorage = false;
    protected bool b_IsTownHall = false;

    // Use this for initialization
    public virtual void Start()
    {
        if (!BuildingName.Equals("TownHall") && !CheckIfExist() && !IsPreview)
        {
            PersistentData.m_Instance.BuildingGridPos.Add(new SerializableVector2(GridPos.x, GridPos.y));
            PersistentData.m_Instance.BuildingName.Add(BuildingName);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool GetIsResidential()
    {
        return b_IsResidential;
    }

    public bool GetIsProduction()
    {
        return b_IsProduction;
    }

    public bool GetIsStorage()
    {
        return b_IsStorage;
    }

    public abstract void ProduceResource();
    public abstract int GetPopulationCount();
    public abstract int GetPopulationCap();
    public abstract int GetCreditsStorageCap();
    public abstract int GetDataStorageCap();
    public abstract int GetPowerStorageCap();
    public abstract int GetFoodStorageCap();
    public abstract void Reset();

    bool CheckIfExist()
    {
        if (PersistentData.m_Instance.BuildingGridPos.Contains(new SerializableVector2(GridPos.x, GridPos.y)))
        {
            return true;
        }

        return false;
    }

    protected void Shutdown()
    {
        // Check if building is destroyed or just being re-generated, make sure it isn't TownHall
        if (GetComponent<Health>().HP <= 0 && !BuildingName.Equals("TownHall"))
        {
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
}
