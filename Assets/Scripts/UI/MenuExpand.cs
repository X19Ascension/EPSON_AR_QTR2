using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections.Generic;

public class MenuExpand : MonoBehaviour {

    public List<GameObject> ObjectsToExpand;
    public bool b_MenuExpanded = false;

    float f_Offset;

    // Use this for initialization
    void Start() {

        foreach (GameObject go in ObjectsToExpand)
        {
            go.GetComponent<Button>().interactable = false;
            go.SetActive(false);
        }

	}
	
	// Update is called once per frame
	void Update () {

        if (GameObject.Find("BuildMenu").GetComponent<BuildButtonBehavior>().GetMenuOpen() == false && b_MenuExpanded)
        {
            CloseMenu();
        }
    }

    public void UpdateMenu()
    {
        if (b_MenuExpanded)
            CloseMenu();
        else
            ExpandMenu();
    }

    void ExpandMenu()    
    {
        f_Offset = ObjectsToExpand[0].GetComponent<RectTransform>().rect.height * 4;

        for (int i = 0; i < ObjectsToExpand.Count; ++i)
        {
            ObjectsToExpand[i].transform.position += new Vector3(0, (i + 1) * f_Offset, 0);
            ObjectsToExpand[i].GetComponent<Button>().interactable = true;
            ObjectsToExpand[i].SetActive(true);
        }
        b_MenuExpanded = true;
    }

    void CloseMenu()
    {
        f_Offset = ObjectsToExpand[0].GetComponent<RectTransform>().rect.height * 4;

        for (int i = 0; i < ObjectsToExpand.Count; ++i)
        {
            ObjectsToExpand[i].transform.position -= new Vector3(0, (i + 1) * f_Offset, 0);
            ObjectsToExpand[i].GetComponent<Button>().interactable = false;
            ObjectsToExpand[i].SetActive(false);
        }
        b_MenuExpanded = false;
    }
}
