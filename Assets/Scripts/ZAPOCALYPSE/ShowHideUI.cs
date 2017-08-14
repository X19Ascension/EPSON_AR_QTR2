using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowHideUI : MonoBehaviour {

    public GameObject UIStuff;
    public Sprite Image;
    public Sprite Image2;

    private bool b_active;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowHideUIClick()
    {
        if (b_active)
        {
            UIStuff.SetActive(true);
            GetComponent<Image>().sprite = Image;
            b_active = false;
        }
        else
        {
            UIStuff.SetActive(false);
            GetComponent<Image>().sprite = Image2;
            b_active = true;
        }
    }
}
