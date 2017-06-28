using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class GameControl: MonoBehaviour {

    [Tooltip("Number Of Enemies")]
    public List<GameObject> enemies = new List<GameObject>();     //! The number of Spawn Points in the Game

    public static GameControl m_instance;

    public float health;
    public float experience;

	// Use this for initialization
	void Awake () {
        if (m_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            m_instance = this;
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }
        
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Health: " + health);
        GUI.Label(new Rect(10, 20, 250, 30), "Experience: " + experience);
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerDataInfo.dat");

        PlayerTestData data = new PlayerTestData();
        data.health = health;
        data.experience= experience;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerDataInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerDataInfo.dat", FileMode.Open);
            PlayerTestData data = (PlayerTestData)bf.Deserialize(file);
            file.Close();

            health = data.health;
            experience = data.experience;
        }

        SceneChange pew = new SceneChange();
        pew.ChangePlayGame();
    }
}

[Serializable]
class PlayerTestData
{
    public float health;
    public float experience;

    public float HP_Rifle;
    public float EXP_Rifle;
    public float LVL_Rifle;

    public float HP_Shotgun;
    public float EXP_Shotgun;
    public float LVL_Shotgun;

    public float HP_Melee;
    public float EXP_Melee;
    public float LVL_Melee;

    public float HP_Medic;
    public float EXP_Medic;
    public float LVL_Medic;

    public float HP_Engineer;
    public float EXP_Engineer;
    public float LVL_Engineer;

    public List<GameObject> Scene_Enemies = new List<GameObject>();
    //    public float 


}