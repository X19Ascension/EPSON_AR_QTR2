using UnityEngine;
using System.Collections;

public class MaterialSwitch : MonoBehaviour {

    public Material StartingMaterial;
    public Material SecondMaterial;
    public Material ThirdMaterial;

    //double d_timer = 3.0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // Debug code
        //d_timer -= Time.deltaTime;
        //if (d_timer <= 0)
        //{
        //    SwitchToSecondMaterial();
        //    d_timer = 9999;
        //}
	}

    public void SwitchToFirstMaterial()
    {
        this.gameObject.GetComponent<Renderer>().material = StartingMaterial;
    }

    public void SwitchToSecondMaterial()
    {
        this.gameObject.GetComponent<Renderer>().material = SecondMaterial;
    }

    public void SwitchToThirdMaterial()
    {
        this.gameObject.GetComponent<Renderer>().material = ThirdMaterial;
    }
}
