using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DayNightCycle : MonoBehaviour 
{
    
    public GameObject go_Canvas; //This is the canvas

    InputHandler IH;

    DestinationLog DestinationLOG_1;

    WaveSpawner wavespawner;

    int i_Supplies;

    //To initialise Destinations for opening stuff
    [SerializeField]
    GameObject go_Destination_1;
    [SerializeField]
    GameObject go_Destination_2;
    [SerializeField]
    GameObject go_Destination_3;
    [SerializeField]
    GameObject go_Destination_4;
    [SerializeField]
    GameObject StartWaveButton;

    DestinationLog destinationLog;

    [SerializeField]
    GameObject[] go_CanvasList = new GameObject[4];

    List<GameObject> l_DestinationList;
    GameObject[] go_DestinationList;
    
    enum MenuState
    {
        NO_MENU_OPEN = 1,
        MENU_OPEN_1, // 2
        MENU_OPEN_2, // 3
        MENU_OPEN_3, // 4
        MENU_OPEN_4, // 5
    }

    public enum DayNightCycles
    {
        S_DAY = 1,
        S_NIGHT,
    }

    public DayNightCycles DNC;
    [SerializeField]
    MenuState MS;
	// Use this for initialization
	void Start () 
    {
       IH =  GameObject.Find("Game Manager").GetComponent<InputHandler>();
       MS = MenuState.NO_MENU_OPEN;
       go_DestinationList = new GameObject[4];
       l_DestinationList = new List<GameObject>();
       l_DestinationList.Add(go_Destination_1);
       l_DestinationList.Add(go_Destination_2); 
       l_DestinationList.Add(go_Destination_3);
       l_DestinationList.Add(go_Destination_4);
       for (int i = 0; i < l_DestinationList.Count; i++ )
       {
           AssignObjectid(l_DestinationList[i], i + 1);
       }
       go_DestinationList = l_DestinationList.ToArray();

       wavespawner = GameObject.Find("SpawnerPrefab").GetComponent<WaveSpawner>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(wavespawner.secondPerWave == 0)
        {
            DNC = DayNightCycles.S_DAY;
        }
        if(DNC == DayNightCycles.S_DAY)
        {
            if (IH.go_Selected != null)
            {
                for (int i = 0; i < go_DestinationList.Length; i++)
                {
                    if (IH.go_Selected == go_DestinationList[i])
                    {
                        MS = (MenuState)(i + 2);
                        IH.go_Selected = null;
                    }
                }
            }
            DisplayMenu();
        }
        else
        {
            StartWaveButton.SetActive(false);
        }
       
	}

    void DisplayMenu()
    {
        if(MS != MenuState.NO_MENU_OPEN)
        {
            go_Canvas.SetActive(true);
        }
        else
        {
            DisableMenu();
        }
        switch (MS)
        {
            case MenuState.NO_MENU_OPEN:
                {
                    ExitMenu();
                    break;
                }
            case MenuState.MENU_OPEN_1:
                {
                    ExitMenu();
                    go_CanvasList[0].SetActive(true);
                    break;
                }
            case MenuState.MENU_OPEN_2:
                {
                    ExitMenu();
                    go_CanvasList[1].SetActive(true);
                    break;
                }
            case MenuState.MENU_OPEN_3:
                {
                    ExitMenu();
                    go_CanvasList[2].SetActive(true);
                    break;
                }
            case MenuState.MENU_OPEN_4:
                {
                    ExitMenu();
                    go_CanvasList[3].SetActive(true);
                    break;
                }
        }
    }

    void ExitMenu()
    {
        for(int i = 0 ; i< go_CanvasList.Length; i++)
        {
            if(go_CanvasList[i].active)
            {
                go_CanvasList[i].SetActive(false);
            }
        }
    }

    public void DisableMenu()
    {
        ExitMenu();
       // go_Canvas.SetActive(false);
        MS = MenuState.NO_MENU_OPEN;
    }

   
    void AssignObjectid(GameObject go, int id)
    {
        go.GetComponent<TouchableObject>().Assignid(id);
    }

    void StartWave()
    {
        DNC = DayNightCycles.S_NIGHT;
    }

    void Search()
    {

    }
}
