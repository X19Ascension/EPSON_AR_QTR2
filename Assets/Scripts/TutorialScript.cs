using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialScript : MonoBehaviour {

    public Image CharC;
    public Sprite CharCNormal;
    public Sprite CharCHappy;
    public Sprite CharCSad;
    public Image TextBoxC;
    bool b_CTalking;

    public Image CharL;
    public Sprite CharLNormal;
    public Sprite CharLHappy;
    public Sprite CharLSad;
    public Image TextBoxL;
    bool b_LTalking;

    public Text Name;
    public Text Dialogue;

    public BuildButtonBehavior BuildMenu;
    public Canvas CityCanvas;
    public Image TutorialBlack;

    public ReadFromCSV CSVReader;

    // Use this for initialization
    void Start () {
        if (PersistentData.m_Instance.TutorialOver == true)
        {
            BuildMenu.BuildWall.gameObject.SetActive(true);
            BuildMenu.BuildStorage.gameObject.SetActive(true);
            BuildMenu.BuildSniper.gameObject.SetActive(true);
            BuildMenu.BuildHeavy.gameObject.SetActive(true);
            SwitchToGameplay();
        }
        else
        {
            BuildMenu.BuildDGenerator.gameObject.SetActive(false);
            BuildMenu.BuildEGenerator.gameObject.SetActive(false);
            BuildMenu.BuildHousing.gameObject.SetActive(false);
            BuildMenu.BuildFarm.gameObject.SetActive(false);
            BuildMenu.BuildWall.gameObject.SetActive(false);
            BuildMenu.BuildStorage.gameObject.SetActive(false);
            BuildMenu.BuildSniper.gameObject.SetActive(false);
            BuildMenu.BuildStandard.gameObject.SetActive(false);
            BuildMenu.BuildHeavy.gameObject.SetActive(false);
            Name.transform.Translate(0, Screen.height / 19, 0);
            Dialogue.transform.Translate(0, Screen.height / 20, 0);
            StartReset();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == -1)
            FadeIn();
        if (Input.GetMouseButtonUp(0) && CityCanvas.sortingOrder == 0)
            GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().SetContinueCheck(true);
        if (Input.GetMouseButtonUp(2))
            Reset();

        if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 4)
            GameObject.Find("TutorialBlack").GetComponent<FadeInOut>().FadeIn();

        if (GameObject.Find("ZoomIn").GetComponent<ZoomInOut>().theState == ZoomInOut.ZOOM_STATE.NORMAL)
        {
            //Build Checks for Generator,
            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 12 && !GameObject.Find("GridManager").GetComponent<ConstructionManager>().GeneratorBuilt)
                SwitchToCity();
            //Farm, 
            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 18 && !GameObject.Find("GridManager").GetComponent<ConstructionManager>().FarmBuilt)
            {
                BuildMenu.BuildFarm.gameObject.SetActive(true);
                SwitchToCity();
            }
            //Housing,
            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 21 && !GameObject.Find("GridManager").GetComponent<ConstructionManager>().HouseBuilt)
            {
                BuildMenu.BuildHousing.gameObject.SetActive(true);
                SwitchToCity();

            }
            //D/E Generators,
            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 24 && !GameObject.Find("GridManager").GetComponent<ConstructionManager>().DEGeneratorsBuilt)
            {
                BuildMenu.BuildDGenerator.gameObject.SetActive(true);
                BuildMenu.BuildEGenerator.gameObject.SetActive(true);
                SwitchToCity();

            }
            //and Turrets
            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 31 && !GameObject.Find("GridManager").GetComponent<ConstructionManager>().TurretBuilt)
            {
                BuildMenu.BuildStandard.gameObject.SetActive(true);
                SwitchToCity();
            }
        }

        //Switch characters and boxes
        if (!GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().GetContinueCheck() == true)
        {
            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 12 && GameObject.Find("GridManager").GetComponent<ConstructionManager>().GeneratorBuilt ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 18 && GameObject.Find("GridManager").GetComponent<ConstructionManager>().FarmBuilt ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 24 && GameObject.Find("GridManager").GetComponent<ConstructionManager>().DEGeneratorsBuilt ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 31 && GameObject.Find("GridManager").GetComponent<ConstructionManager>().TurretBuilt)
            {
                CityCanvas.sortingOrder = 0;
                //GameObject.Find("TutorialBlack").SetActive(true);
                SwitchToC();
                SwitchToTutorial();
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().SetContinueCheck(true);
            }
            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 21 && GameObject.Find("GridManager").GetComponent<ConstructionManager>().HouseBuilt)
            {
                CityCanvas.sortingOrder = 0;
                //GameObject.Find("TutorialBlack").SetActive(true);
                SwitchToL();
                SwitchToTutorial();
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().SetContinueCheck(true);
            }
        }
        else
        {

            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 1 && b_CTalking == true ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 8 && b_CTalking == true ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 13 && b_CTalking == true ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 17 && b_CTalking == true ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 27 && b_CTalking == true)
                SwitchToL();

            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 4 && b_LTalking == true ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 16 && b_LTalking == true ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 30 && b_LTalking == true ||
                GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 39 && b_LTalking == true)
                SwitchToC();

            if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage >= 39)
            {
                BuildMenu.BuildWall.gameObject.SetActive(true);
                BuildMenu.BuildStorage.gameObject.SetActive(true);
                BuildMenu.BuildSniper.gameObject.SetActive(true);
                BuildMenu.BuildHeavy.gameObject.SetActive(true);
                SwitchToCity();
                PersistentData.m_Instance.TutorialOver = true;
            }
        }
    }

    void Reset()
    {
        // Clear data
        // Load whatever numbers here
        PersistentData.m_Instance.DataAmount = 0;
        PersistentData.m_Instance.CreditsAmount = 0;
        PersistentData.m_Instance.FoodAmount = 0;
        PersistentData.m_Instance.PopulationAmount = 0;
        PersistentData.m_Instance.PowerAmount = 0;

        GameObject.Find("TownHall").GetComponent<TownHall>().CurrentPop = 0;

        PersistentData.m_Instance.BuildingGridPos.Clear();
        PersistentData.m_Instance.BuildingName.Clear();

        GameObject.Find("MilestoneManager").GetComponent<MilestoneManager>().Reset();

        PersistentData.m_Instance.MilestoneIndex = 0;
        PersistentData.m_Instance.MilestoneProgress = 0;

        //Regenerate Grid
        GameObject.Find("GridManager").GetComponent<GridBehavior>().DeleteBuildings();
        GameObject.Find("GridManager").GetComponent<GridBehavior>().GenerateMap(false);

        //Check against CSV to place obstacles
        for (int i = 0; i < 11; i++)
        {
            //Since it reads bottom-up, should check descending from 11
            for (int j = 0; j < 11; j++)
            {
                GameObject[] obs = GameObject.FindGameObjectsWithTag("Grid");
                for (int k = 0; k < obs.Length; k++)
                {
                    if (CSVReader.loadedMap[i, j] != 0 && GameObject.Find("GridManager").GetComponent<GridBehavior>().GetGridPos(obs[k].transform.position) == new Vector2(i, j))
                    {
                        Destroy(obs[k]);
                    }
                }
            }
        }
    }


    void SwitchToL()
    {
        var tempBoxC = TextBoxC.color;
        var tempBoxL = TextBoxL.color;
        var tempCharC = CharC.color;
        var tempCharL = CharL.color;

        tempBoxL.a = 1.5f;
        tempBoxC.a = 0;
        tempCharL.a = 1.5f;
        tempCharC.a = 0.4f;

        TextBoxC.color = tempBoxC;
        TextBoxL.color = tempBoxL;
        CharC.color = tempCharC;
        CharL.color = tempCharL;

        Name.text = "Los";
        Name.transform.Translate(new Vector3(-758, -2, 0));
        b_LTalking = true;
        b_CTalking = false;

        if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 1)
            CharL.sprite = CharLHappy;
        if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 8 ||
            GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 17 ||
            GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 35)

            CharL.sprite = CharLSad;
        else
            CharL.sprite = CharLNormal;
    }

    void SwitchToC()
    {
        var tempBoxC = TextBoxC.color;
        var tempBoxL = TextBoxL.color;
        var tempCharC = CharC.color;
        var tempCharL = CharL.color;

        tempBoxL.a = 0;
        tempBoxC.a = 1.5f;
        tempCharL.a = 0.4f;
        tempCharC.a = 1.5f;

        TextBoxC.color = tempBoxC;
        TextBoxL.color = tempBoxL;
        CharC.color = tempCharC;
        CharL.color = tempCharL;

        Name.text = "Corr";
        Name.transform.Translate(new Vector3(758, 2, 0));
        b_CTalking = true;
        b_LTalking = false;

        if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 12 ||
            GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage == 16)
            CharC.sprite = CharCHappy;
        else
            CharC.sprite = CharCNormal;
    }

    void FadeIn()
    {
        var tempBoxC = TextBoxC.color;
        var tempCharC = CharC.color;
        var tempCharL = CharL.color;

        var tempName = Name.color;
        var tempDialogue = Dialogue.color;

        if (tempName.a < 1.5)
        {
            tempCharL.a += Time.deltaTime;
            tempCharC.a += Time.deltaTime;
            tempBoxC.a += Time.deltaTime;
            tempName.a += Time.deltaTime;
            tempDialogue.a += Time.deltaTime;

            TextBoxC.color = tempBoxC;
            CharC.color = tempCharC;
            CharL.color = tempCharL;
            Name.color = tempName;
            Dialogue.color = tempDialogue;
        }

        Name.text = "Corr";
        b_CTalking = true;
    }

    void SwitchToCity()
    {
        CityCanvas.sortingOrder = 2;

        var tempBoxC = TextBoxC.color;
        var tempBoxL = TextBoxL.color;
        var tempCharC = CharC.color;
        var tempCharL = CharL.color;
        var tempName = Name.color;
        var tempDialogue = Dialogue.color;

        tempBoxL.a = 0f;
        tempBoxC.a = 0;
        tempCharL.a = 0f;
        tempCharC.a = 0f;
        tempName.a = 0f;
        tempDialogue.a = 0f;

        TextBoxC.color = tempBoxC;
        TextBoxL.color = tempBoxL;
        CharC.color = tempCharC;
        CharL.color = tempCharL;
        Name.color = tempName;
        Dialogue.color = tempDialogue;
        Lodsofemone();
        TutorialBlack.gameObject.SetActive(false);
    }

    void SwitchToGameplay()
    {
        CityCanvas.sortingOrder = 2;
        this.gameObject.SetActive(false);
    }

    void SwitchToTutorial()
    {
        var tempName = Name.color;
        var tempDialogue = Dialogue.color;

        tempName.a = 1.5f;
        tempDialogue.a = 1.5f;

        Name.color = tempName;
        Dialogue.color = tempDialogue;

        return;
    }

    void StartReset()
    {
        // Clear data
        // Load whatever numbers here
        PersistentData.m_Instance.CreditsAmount = PersistentData.m_Instance.CreditsCap;
        PersistentData.m_Instance.DataAmount = PersistentData.m_Instance.DataCap;
        PersistentData.m_Instance.PowerAmount = PersistentData.m_Instance.PowerCap;
        PersistentData.m_Instance.FoodAmount = PersistentData.m_Instance.FoodCap;
        PersistentData.m_Instance.PopulationAmount = PersistentData.m_Instance.PopulationCap;

        GameObject.Find("TownHall").GetComponent<TownHall>().CurrentPop = PersistentData.m_Instance.PopulationCap;

        PersistentData.m_Instance.BuildingGridPos.Clear();
        PersistentData.m_Instance.BuildingName.Clear();

        GameObject.Find("MilestoneManager").GetComponent<MilestoneManager>().Reset();

        PersistentData.m_Instance.MilestoneIndex = 0;
        PersistentData.m_Instance.MilestoneProgress = 0;

        //Delete Grids!
        for (int i = 0; i < 11; i++)
        {
            //Since it reads bottom-up, should check descending from 11
            for (int j = 0; j < 11; j++)
            {
                GameObject[] obs = GameObject.FindGameObjectsWithTag("Grid");
                for (int k = 0; k < obs.Length; k++)
                {
                    if (CSVReader.loadedMap[i, j] != 0 && GameObject.Find("GridManager").GetComponent<GridBehavior>().GetGridPos(obs[k].transform.position) == new Vector2(i, j))
                    {
                        Destroy(obs[k]);
                    }
                }
            }
        }
    }

    void Lodsofemone()
    {
        if (GameObject.Find("DialogueText").GetComponent<AnimatedDialog>().ConvoStage < 40)
        {
            PersistentData.m_Instance.CreditsAmount = PersistentData.m_Instance.CreditsCap;
            PersistentData.m_Instance.DataAmount = PersistentData.m_Instance.DataCap;
            PersistentData.m_Instance.PowerAmount = PersistentData.m_Instance.PowerCap;
            PersistentData.m_Instance.FoodAmount = PersistentData.m_Instance.FoodCap;
            PersistentData.m_Instance.PopulationAmount = PersistentData.m_Instance.PopulationCap;

            GameObject.Find("TownHall").GetComponent<TownHall>().CurrentPop = PersistentData.m_Instance.PopulationCap;
        }
    }
}