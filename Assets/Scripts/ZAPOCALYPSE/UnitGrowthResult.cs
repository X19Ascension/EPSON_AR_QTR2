using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitGrowthResult : MonoBehaviour {

    public Slider mainSlider;
    public GameObject Unit;
    //public UnitGrowth unitGrowth;

    public Text EXPText;
    public Text LVLText;

    private Survivor survivior;

	// Use this for initialization
	void Start () {
        survivior = Unit.GetComponent<Survivor>();

    }
	
	// Update is called once per frame
	void Update () {
        //unitGrowth.surv.GetComponent<Survivor>().experiencePt;
        if (survivior == null)
            Destroy(this.gameObject);

        RescaleEXPBar();
    }

    public void RescaleEXPBar()
    {
        Debug.Log("Test");
        float expScale = survivior.experiencePt / Unit.GetComponent<UnitGrowth>().EXPToLVL(survivior);
        mainSlider.value = expScale;

        RetextEXP();
    }

    public void RetextEXP()
    {
        EXPText.text = "[ " + (int)survivior.experiencePt + " / " + Unit.GetComponent<UnitGrowth>().EXPToLVL(survivior) + " ]";
        LVLText.text = "LVL: [" + survivior.level + " / " + "20 ]";
    }
}
