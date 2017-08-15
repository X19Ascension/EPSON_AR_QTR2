using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowHideUI : MonoBehaviour {

    public GameObject UIStuff;
    public Sprite Image;
    public Sprite Image2;

    bool b_MenuOpen = false;
    float f_OffsetX = Screen.width;// * 0.08f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowHideUIClick()
    {
        if (b_MenuOpen)
        {
            //UIStuff.SetActive(true);
            UIStuff.transform.Translate(-f_OffsetX * 0.25f, 0, 0);
            GetComponent<Image>().sprite = Image;
        }
        else
        {
            UIStuff.transform.Translate(f_OffsetX * 0.25f, 0, 0);
            //UIStuff.SetActive(false);
            GetComponent<Image>().sprite = Image2;
        }
        b_MenuOpen = !b_MenuOpen;
    }
}
