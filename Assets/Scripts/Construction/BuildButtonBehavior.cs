using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildButtonBehavior : MonoBehaviour
{
    public Button BuildDGenerator;
    public Button BuildEGenerator;
    public Button BuildFarm;
    public Button BuildGenerator;
    public Button BuildHousing;
    public Button BuildStorage;
    public Button BuildWall;

    public Button BuildStandard;
    public Button BuildSniper;
    public Button BuildHeavy;

    bool b_MenuOpen = false;
    float f_OffsetX = Screen.width * 0.08f;
    float f_OffsetY = Screen.height * 0.165f;

    // Use this for initialization
    void Start()
    {
        BuildDGenerator.interactable = false;
        BuildEGenerator.interactable = false;
        BuildFarm.interactable = false;
        BuildGenerator.interactable = false;
        BuildHousing.interactable = false;
        BuildStorage.interactable = false;
        BuildWall.interactable = false;
        BuildStandard.interactable = false;
        BuildSniper.interactable = false;
        BuildHeavy.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPress()
    {
        if (!b_MenuOpen)
        {
            //Buildings, expand left
            BuildGenerator.transform.Translate(-f_OffsetX, 0, 0);
            BuildGenerator.interactable = true;

            BuildFarm.transform.Translate(-(f_OffsetX * 2), 0, 0);
            BuildFarm.interactable = true;

            BuildHousing.transform.Translate(-(f_OffsetX * 3), 0, 0);
            BuildHousing.interactable = true;

            BuildDGenerator.transform.Translate(-(f_OffsetX * 4), 0, 0);
            BuildDGenerator.interactable = true;

            BuildEGenerator.transform.Translate(-(f_OffsetX * 5), 0, 0);
            BuildEGenerator.interactable = true;

            BuildStorage.transform.Translate(-(f_OffsetX * 6), 0, 0);
            BuildStorage.interactable = true;

            BuildWall.transform.Translate(-(f_OffsetX * 7), 0, 0);
            BuildWall.interactable = true;

            //Towers, expand up
            BuildStandard.transform.Translate(0, f_OffsetY, 0);
            BuildStandard.interactable = true;

            BuildSniper.transform.Translate(0, f_OffsetY * 2, 0);
            BuildSniper.interactable = true;

            BuildHeavy.transform.Translate(0, f_OffsetY * 3, 0);
            BuildHeavy.interactable = true;

        }
        else
        {
            //Buildings
            BuildGenerator.transform.Translate(f_OffsetX, 0, 0);
            BuildGenerator.interactable = true;

            BuildFarm.transform.Translate((f_OffsetX * 2), 0, 0);
            BuildFarm.interactable = true;

            BuildHousing.transform.Translate((f_OffsetX * 3), 0, 0);
            BuildHousing.interactable = true;

            BuildDGenerator.transform.Translate((f_OffsetX * 4), 0, 0);
            BuildDGenerator.interactable = true;

            BuildEGenerator.transform.Translate((f_OffsetX * 5), 0, 0);
            BuildEGenerator.interactable = true;

            BuildStorage.transform.Translate((f_OffsetX * 6), 0, 0);
            BuildStorage.interactable = true;

            BuildWall.transform.Translate((f_OffsetX * 7), 0, 0);
            BuildWall.interactable = true;

            //Towers

            BuildStandard.transform.Translate(0, -f_OffsetY, 0);
            BuildStandard.interactable = true;

            BuildSniper.transform.Translate(0, -(f_OffsetY * 2), 0);
            BuildSniper.interactable = true;

            BuildHeavy.transform.Translate(0, -(f_OffsetY * 3), 0);
            BuildHeavy.interactable = true;
        }

        b_MenuOpen = !b_MenuOpen;
    }

    public bool GetMenuOpen()
    {
        return b_MenuOpen;
    }
}
