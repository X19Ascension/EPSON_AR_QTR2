using UnityEngine;
using System.Collections;

public enum ADD_ON_TYPE
{
    NONE,
    FOOD,
    POWER,
}

public class BuildingAddOn : MonoBehaviour
{
    public ADD_ON_TYPE theType;
    public int i_IncreaseAmount;

    public BuildingAddOn(ADD_ON_TYPE aType)
    {
        theType = aType;
    }

    void Start()
    {
        string typeStr = "";
        switch (theType)
        {
            case ADD_ON_TYPE.FOOD:
                GameObject.Find("TownHall").GetComponent<CityManager>().AddFoodProduction(i_IncreaseAmount);
                typeStr = "_Food";
                Debug.Log("Addon Food");
                break;

            case ADD_ON_TYPE.POWER:
                GameObject.Find("TownHall").GetComponent<CityManager>().AddPowerProduction(i_IncreaseAmount);
                typeStr = "_Power";
                Debug.Log("Addon Power");
                break;
        }

        SerializableVector2 FindVec2 = new SerializableVector2(this.GetComponentInParent<Residential>().GridPos);
        for (int i = 0; i < PersistentData.m_Instance.BuildingGridPos.Count; ++i)
        {
            if (FindVec2.Equals(PersistentData.m_Instance.BuildingGridPos[i]))
            {
                if (!PersistentData.m_Instance.BuildingName[i].Contains("_"))
                    PersistentData.m_Instance.BuildingName[i] += typeStr;
            }
        }
    }

    void Update()
    { }

    public bool IsCorrectType(ADD_ON_TYPE check)
    {
        return (check == theType);
    }

    public int GetIncreaseAmount()
    {
        return i_IncreaseAmount;
    }

    public void ProduceResource()
    {
        switch (theType)
        {
            case ADD_ON_TYPE.FOOD:
                PersistentData.m_Instance.FoodAmount += i_IncreaseAmount;
                break;

            case ADD_ON_TYPE.POWER:
                PersistentData.m_Instance.PowerAmount += i_IncreaseAmount;
                break;
        }
    }

}
