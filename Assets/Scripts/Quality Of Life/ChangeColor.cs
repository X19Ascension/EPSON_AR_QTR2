using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {

    public Color startColor;
    public Color mouseOverColor;
    bool b_mouseOver = false;

    void OnMouseEnter()
    {
        b_mouseOver = true;
        GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);

    }

    void OnMouseExit()
    {
        b_mouseOver = false;
        GetComponent<Renderer>().material.SetColor("_Color", startColor);

    }


}
