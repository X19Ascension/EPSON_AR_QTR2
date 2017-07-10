using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Testingpurposes : MonoBehaviour 
{
    public Text text;
    public GameObject go;
	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

   public void OnMouseDown()
    {
       if(go.active == false)
       {
           go.SetActive(true);
       }
       else
       {
           go.SetActive(false);
       }
    }

   
}
