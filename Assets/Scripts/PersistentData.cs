using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistentData : MonoBehaviour {

    public enum LEVEL_TYPE
    {
        NIL,
        GRASS,
        DESERT,
        RUINS,
    }

    // ------------------------------------------------------ //
    // Access this class by using "PersistentData.m_Instance."
    // ------------------------------------------------------ //

    public static PersistentData m_Instance;

    public bool InitialLoad = false;
    public bool LoadFailed = false;
    public LEVEL_TYPE LevelToLoad;

    public string PreviousScene = "";

    // Stuff to store in persistent data
    public int CreditsAmount;
    public int DataAmount;
    public int FoodAmount;
    public int PowerAmount;
    public int PopulationAmount;

    public int CreditsCap;
    public int DataCap;
    public int FoodCap;
    public int PowerCap;
    public int PopulationCap;

    public int MilestoneIndex;
    public float MilestoneProgress;

    public bool TutorialOver = false;

    //Run through to check for wall in a for loop. 
    public List<SerializableVector2> BuildingGridPos = new List<SerializableVector2>();
    public List<string> BuildingName = new List<string>();

    // Use this for initialization
    void Awake () {

        DontDestroyOnLoad(gameObject);

        if (!m_Instance)
        {
            m_Instance = this;
        }
        else if (m_Instance != this)
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
	   
        if (!InitialLoad)
        {
            LoadData();
            InitialLoad = true;
        }

        // Clear data
        if (Input.GetKeyUp(KeyCode.X))
        {
            // Load whatever numbers here
            DataAmount = 0;
            CreditsAmount = 0;
            FoodAmount = 0;
            PopulationAmount = 0;
            PowerAmount = 0;

            BuildingGridPos.Clear();
            BuildingName.Clear();
        }
	}

#if UNITY_ANDROID

    void OnApplicationQuit() {
        SaveDate();
    }

    void OnApplicationPause() {
#if UNITY_EDITOR

#else
        SaveDate();
#endif
    }

#endif

        void OnDisable()
    {
        SaveDate();
    }

    PlayerData CopyData()
    {
        PlayerData returnData = new PlayerData();

        returnData.CreditsAmount = this.CreditsAmount;
        returnData.DataAmount = this.DataAmount;
        returnData.FoodAmount = this.FoodAmount;
        returnData.PowerAmount = this.PowerAmount;
        returnData.PopulationAmount = this.PopulationAmount;

        returnData.CreditsCap = this.CreditsCap;
        returnData.DataCap = this.DataCap;
        returnData.FoodCap = this.FoodCap;
        returnData.PowerCap = this.PowerCap;
        returnData.PopulationCap = this.PopulationCap;

        returnData.MilestoneIndex = this.MilestoneIndex;
        returnData.MilestoneProgress = this.MilestoneProgress;

        returnData.BuildingGridPos = this.BuildingGridPos;
        returnData.BuildingName = this.BuildingName;       

        return returnData;
    }

    void LoadData(PlayerData theData)
    {
        this.CreditsAmount = theData.CreditsAmount;
        this.DataAmount = theData.DataAmount;
        this.FoodAmount = theData.FoodAmount;
        this.PowerAmount = theData.PowerAmount;
        this.PopulationAmount = theData.PopulationAmount;

        this.CreditsCap = theData.CreditsCap;
        this.DataCap = theData.DataCap;
        this.FoodCap = theData.FoodCap;
        this.PowerCap = theData.PowerCap;
        this.PopulationCap = theData.PopulationCap;

        this.MilestoneIndex = theData.MilestoneIndex;
        this.MilestoneProgress = theData.MilestoneProgress;

        this.BuildingGridPos = theData.BuildingGridPos;
        this.BuildingName = theData.BuildingName;
    }

    public void SaveDate()
    {
        if (LevelToLoad == LEVEL_TYPE.NIL)
            return;

        string SaveFileName = "";
        switch (LevelToLoad)
        {
            case LEVEL_TYPE.GRASS:
                SaveFileName = "/GRASS_PersistentData.dat";
                break;

            case LEVEL_TYPE.DESERT:
                SaveFileName = "/DESERT_PersistentData.dat";
                break;

            case LEVEL_TYPE.RUINS:
                SaveFileName = "/RUINS_PersistentData.dat";
                break;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + SaveFileName);

        PlayerData data = CopyData();

        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if (LevelToLoad == LEVEL_TYPE.NIL)
            return;

        string SaveFileName = "";
        switch (LevelToLoad)
        {
            case LEVEL_TYPE.GRASS:
                SaveFileName = "/GRASS_PersistentData.dat";
                break;

            case LEVEL_TYPE.DESERT:
                SaveFileName = "/DESERT_PersistentData.dat";
                TutorialOver = true;
                break;

            case LEVEL_TYPE.RUINS:
                SaveFileName = "/RUINS_PersistentData.dat";
                TutorialOver = true;
                break;
        }

        if (File.Exists(Application.persistentDataPath + SaveFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + SaveFileName, FileMode.Open);
            PlayerData data = (PlayerData)(bf.Deserialize(file));

            LoadData(data);
            file.Close();
        }
        else
        {
            LoadFailed = true;
        }
    }
}

[System.Serializable]
class PlayerData
{
    public int CreditsAmount;
    public int DataAmount;
    public int FoodAmount;
    public int PowerAmount;
    public int PopulationAmount;

    public int CreditsCap;
    public int DataCap;
    public int FoodCap;
    public int PowerCap;
    public int PopulationCap;

    public int MilestoneIndex;
    public float MilestoneProgress;

    public List<SerializableVector2> BuildingGridPos;
    public List<string> BuildingName;
}

[System.Serializable]
public struct SerializableVector2
{
    public SerializableVector2(float x, float y)
    {
        f_x = x;
        f_y = y;
    }

    public SerializableVector2(Vector2 aVec2)
    {
        f_x = aVec2.x;
        f_y = aVec2.y;
    }

    public Vector2 GetVec2()
    {
        return new Vector2(f_x, f_y);
    }

    float f_x;
    float f_y;
}
