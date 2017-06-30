using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

// Essentially Game Control is a singleton in C#
public class GameControl: MonoBehaviour {

    [Tooltip("Number Of Enemies")]
    //public List<GameObject> enemies = new List<GameObject>();     //! The number of Spawn Points in the Game
    //public List<GameObject> survivors = new List<GameObject>();

    public GameObject[] enemies;
    public GameObject[] survivors;

    public float HP_Rifle;              //! HP of the Rifler
    public float EXP_Rifle;             //! EXP of the Rifler
    public float LVL_Rifle;             //! Level of the rifler <- probaby the only essential one in stats as stats will be recalculated.
    public float durationUp_Rifle;      //! Duration of the rifler active
    public float range_Rifle;           //! Range of the Rifler

    public float HP_Shotgun;            //! HP of the Shotgunner
    public float EXP_Shotgun;           //! EXP of the Shotgunner
    public float LVL_Shotgun;           //! Level of the Shotgunner
    public float durationUp_Shotgun;    //! Duration of the shotgunner active
    public float range_Shotgun;         //! Range of the Shotgunner

    public float HP_Melee;              //! HP Of the Melee
    public float EXP_Melee;             //! EXP of the Melee
    public float LVL_Melee;             //! Level of the Melee
    public float durationUp_Melee;      //! Duration of the melee active
    public float range_Melee;           //! Range of the Melee

    public float HP_Engineer;           //! HP of the Engineer
    public float EXP_Engineer;          //! EXP of the Engineer
    public float LVL_Engineer;          //! Level of the Engineer
    public float durationUp_Engineer;   //! Duration of the Engineer active
    public float range_Engineer;        //! Range of the Engineer

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

    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("test");
        survivors = GameObject.FindGameObjectsWithTag("Survivor");
    }

    // GUI Display onto Unity Screen
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Health: " + health);
        GUI.Label(new Rect(10, 20, 250, 30), "Experience: " + experience);
        GUI.Label(new Rect(10, 30, 250, 30), "Rifle Health: " + HP_Rifle);
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerDataInfo.dat");

        PlayerTestData data = new PlayerTestData();
        data.health = health;
        data.experience= experience;
        data.Scene_Enemies = enemies;

        foreach (GameObject pew in survivors)
        {
            switch (pew.GetComponent<Survivor>().entityType)
            {
                case Survivor.SURVIVOR_TYPE.S_RIFLE:
                    data.HP_Rifle = pew.GetComponent<Survivor>().GetHealth();
                    data.EXP_Rifle = pew.GetComponent<Survivor>().experiencePt; 
                    data.LVL_Rifle = pew.GetComponent<Survivor>().level;
                    data.durationUp_Rifle = pew.GetComponent<Survivor>().timeActive;
                    data.range_Rifle = pew.GetComponent<Survivor>().atkRange;
                    break;
                case Survivor.SURVIVOR_TYPE.S_SHOTGUN:
                    data.HP_Shotgun = pew.GetComponent<Survivor>().GetHealth();
                    data.EXP_Shotgun = pew.GetComponent<Survivor>().experiencePt;
                    data.LVL_Shotgun = pew.GetComponent<Survivor>().level;
                    data.durationUp_Shotgun = pew.GetComponent<Survivor>().timeActive;
                    data.range_Shotgun = pew.GetComponent<Survivor>().atkRange;
                    break;
                case Survivor.SURVIVOR_TYPE.S_MELEE:
                    data.HP_Melee = pew.GetComponent<Survivor>().GetHealth();
                    data.EXP_Melee = pew.GetComponent<Survivor>().experiencePt;
                    data.LVL_Melee = pew.GetComponent<Survivor>().level;
                    data.durationUp_Melee = pew.GetComponent<Survivor>().timeActive;
                    data.range_Melee = pew.GetComponent<Survivor>().atkRange;
                    break;
                case Survivor.SURVIVOR_TYPE.S_MECHANIC:
                    data.HP_Engineer = pew.GetComponent<Survivor>().GetHealth();
                    data.EXP_Engineer = pew.GetComponent<Survivor>().experiencePt;
                    data.LVL_Engineer = pew.GetComponent<Survivor>().level;
                    data.durationUp_Engineer = pew.GetComponent<Survivor>().timeActive;
                    data.range_Engineer = pew.GetComponent<Survivor>().atkRange;
                    break;
            }
        }

        //data.Scene_Enemies = enemies;

        bf.Serialize(file, data);
        file.Close();
    }

    // Load the file by opening and deserializing the data and allow assignment of values;
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
            data.Scene_Enemies = enemies;

            foreach (GameObject survInfo in survivors)
            {
                switch (survInfo.GetComponent<Survivor>().entityType)
                {
                    case Survivor.SURVIVOR_TYPE.S_RIFLE:
                        survInfo.GetComponent<Survivor>().SetHealth(data.HP_Rifle);
                        survInfo.GetComponent<Survivor>().experiencePt = data.EXP_Rifle;
                        survInfo.GetComponent<Survivor>().level = data.LVL_Rifle;
                        survInfo.GetComponent<Survivor>().timeActive = data.durationUp_Rifle;
                        survInfo.GetComponent<Survivor>().atkRange = data.range_Rifle;
                        break;
                    case Survivor.SURVIVOR_TYPE.S_SHOTGUN:
                        survInfo.GetComponent<Survivor>().SetHealth(data.HP_Shotgun);
                        survInfo.GetComponent<Survivor>().experiencePt = data.EXP_Shotgun;
                        survInfo.GetComponent<Survivor>().level = data.LVL_Shotgun;
                        survInfo.GetComponent<Survivor>().timeActive = data.durationUp_Shotgun;
                        survInfo.GetComponent<Survivor>().atkRange = data.range_Shotgun;
                        break;
                    case Survivor.SURVIVOR_TYPE.S_MELEE:
                        survInfo.GetComponent<Survivor>().SetHealth(data.HP_Melee);
                        survInfo.GetComponent<Survivor>().experiencePt = data.EXP_Melee;
                        survInfo.GetComponent<Survivor>().level = data.LVL_Melee;
                        survInfo.GetComponent<Survivor>().timeActive = data.durationUp_Melee;
                        survInfo.GetComponent<Survivor>().atkRange = data.range_Melee;
                        break;
                    case Survivor.SURVIVOR_TYPE.S_MECHANIC:
                       survInfo.GetComponent<Survivor>().SetHealth(data.HP_Engineer);
                       survInfo.GetComponent<Survivor>().experiencePt = data.EXP_Engineer;
                       survInfo.GetComponent<Survivor>().level = data.LVL_Engineer;
                       survInfo.GetComponent<Survivor>().timeActive = data.durationUp_Engineer;
                        survInfo.GetComponent<Survivor>().atkRange = data.range_Engineer;
                        break;
                }
            }

        }

        SceneChange pew = new SceneChange();
        pew.ChangePlayGame();
    }
}

[Serializable] // To ensure that this is Serializable and able to convert into a binary data file
class PlayerTestData
{
    // Learning abit on player data.
    public float health;
    public float experience;

    public int HP_Rifle;              //! HP of the Rifler
    public float EXP_Rifle;             //! EXP of the Rifler
    public int LVL_Rifle;             //! Level of the rifler <- probaby the only essential one in stats as stats will be recalculated.
    public float durationUp_Rifle;      //! Duration of the rifler active
    public float range_Rifle;           //! Range of the Rifler
    

    public int HP_Shotgun;            //! HP of the Shotgunner
    public float EXP_Shotgun;           //! EXP of the Shotgunner
    public int LVL_Shotgun;           //! Level of the Shotgunner
    public float durationUp_Shotgun;    //! Duration of the shotgunner active
    public float range_Shotgun;         //! Range of the Shotgunner

    public int HP_Melee;              //! HP Of the Melee
    public float EXP_Melee;             //! EXP of the Melee
    public int LVL_Melee;             //! Level of the Melee
    public float durationUp_Melee;      //! Duration of the melee active
    public float range_Melee;           //! Range of the Melee

    //public float HP_Medic;
    //public float EXP_Medic;
    //public float LVL_Medic;
    //public float durationUp_Rifle;    //! Duration of the rifler active

    public int HP_Engineer;           //! HP of the Engineer
    public float EXP_Engineer;          //! EXP of the Engineer
    public int LVL_Engineer;          //! Level of the Engineer
    public float durationUp_Engineer;   //! Duration of the Engineer active
    public float range_Engineer;        //! Range of the Engineer

    //! Storing a List of Enemies so that the HP, position and other important data values are being recorded.
    //public List<GameObject> Scene_Enemies = new List<GameObject>();
    public GameObject[] Scene_Enemies;

    public float EXP_Player;
    public float test;

    //    public float 


}