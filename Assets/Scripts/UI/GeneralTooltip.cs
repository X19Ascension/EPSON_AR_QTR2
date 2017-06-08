using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeneralTooltip : MonoBehaviour
{
    public GameObject item;

    public Image Tooltip;
    public Text Name;
    public Text Description;
    public Text DCost;
    public Text ECost;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Tooltip.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25, 0);
    }

    public void OnHover()
    {
        Tooltip.gameObject.SetActive(true);

        if (item.tag == "Buildings" || item.tag == "Wall")
        {
            //Readjust to be visible
            Tooltip.GetComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
            Name.text = item.name.ToString();
            Description.text = "";
            DCost.text = item.GetComponent<BuildingAbstractBase>().DCost.ToString();
            ECost.text = item.GetComponent<BuildingAbstractBase>().ECost.ToString();
        }

        if (item.tag == "Tower")
        {
            //Readjust to be visible
            Tooltip.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
            Name.text = item.GetComponent<BaseTurret>().FlavourName.ToString() + " Lv[" + item.GetComponent<BaseTurret>().RequiredMilestoneLevel.ToString() + "]";
            Description.text = "";
            DCost.text = item.GetComponent<BaseTurret>().DCost.ToString();
            ECost.text = item.GetComponent<BaseTurret>().ECost.ToString();
        }

        if (item.name == "MilestoneManager")
        {
            //Uses a custom version to check values
            Tooltip.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            Name.text = "Progress Bar";
            Description.text = "Econ.        " + item.GetComponent<MilestoneManager>().GetEconomicValue().ToString() + " | " + item.GetComponent<MilestoneManager>().GetEconomicMax().ToString();
            DCost.text = "Def.           " + item.GetComponent<MilestoneManager>().GetDefensiveValue().ToString() + " | " + item.GetComponent<MilestoneManager>().GetDefensiveMax().ToString();
            ECost.text = "Pop.          " + item.GetComponent<MilestoneManager>().GetPopulationValue().ToString() + " | " + item.GetComponent<MilestoneManager>().GetPopulationMax().ToString();
        }

        if (item.name == "Button" || item.name == "BuildMenu")
        {
            //Readjust to be visible
            Tooltip.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
            Name.text = item.GetComponent<TooltipInfodump>().Name;
            Description.text = item.GetComponent<TooltipInfodump>().Description;
            DCost.text = "";
            ECost.text = "";
        }

        if (item.name == "DeleteButton")
        {
            //Readjust to be visible
            Tooltip.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
            Name.text = item.GetComponent<TooltipInfodump>().Name;
            Description.text = item.GetComponent<TooltipInfodump>().Description;
            DCost.text = "";
            ECost.text = "";
        }

        if (item.name == "DataUI" || item.name == "DataText" ||
            item.name == "CreditUI" || item.name == "CreditText" ||
            item.name == "FoodUI" || item.name == "FoodText" ||
            item.name == "PopulationUI" || item.name == "PopulationText" ||
            item.name == "PowerUI" || item.name == "PowerText" || 
            item.name == "Pause" || item.name == "MilestoneText" || 
            item.name == "ZoomIn")
        {
            Tooltip.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.8f);
            Name.text = item.GetComponent<TooltipInfodump>().Name;
            Description.text = item.GetComponent<TooltipInfodump>().Description;
            DCost.text = "";
            ECost.text = "";
        }
    }

    public void OnExit()
    {
        Tooltip.gameObject.SetActive(false);

        Name.text = "";
        Description.text = "";
        DCost.text = "";
        ECost.text = "";
    }
}