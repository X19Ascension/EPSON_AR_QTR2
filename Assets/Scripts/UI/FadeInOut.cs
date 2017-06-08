using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOut : MonoBehaviour {

    public Image BlackScreen;
    public Text LoseText;
    public bool b_FadeOut = false;
    public bool b_FadeIn = false;

	// Use this for initialization
	void Start () {
        //BlackScreen = GetComponent<Image>();
        //LoseText = GetComponentInChildren<Text>();
	}

    public void FadeIn()
    {
        var tempColor = BlackScreen.color;
        var tempText = LoseText.color;
        if (tempColor.a > 0)
        {
            tempColor.a -= Time.deltaTime;
            tempText.a -= Time.deltaTime;
            BlackScreen.color = tempColor;
            LoseText.color = tempText;
        }
        if (tempColor.a <= 0)
            b_FadeIn = false;
    }

    public void FadeOut()
    {
        var tempColour = BlackScreen.color;
        var tempText = LoseText.color;
        if (tempColour.a < 1.5)
        {
            tempColour.a += Time.deltaTime;
            tempText.a += Time.deltaTime;
            BlackScreen.color = tempColour;
            LoseText.color = tempText;
        }
        if (tempColour.a >= 1.4)
            b_FadeOut = false;
    }

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp("1"))
            b_FadeOut = true;
        if (Input.GetKeyUp("2") && b_FadeOut == BlackScreen.color.a >= 1.4)
            b_FadeIn = true;

        if (b_FadeOut)
            FadeOut();
        if (b_FadeIn)
            FadeIn();
    }
}
