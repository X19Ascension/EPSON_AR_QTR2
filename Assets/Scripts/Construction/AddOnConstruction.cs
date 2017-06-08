using UnityEngine;
using System.Collections;

public class AddOnConstruction : MonoBehaviour {

    public ConstructionManager theConstructionManager;
    public UIHoverOver theHoverScript;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (this.gameObject.GetComponent<Residential>().theAddOn != null || this.gameObject.GetComponent<BuildingAbstractBase>().IsPreview)
               return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.Equals(this.gameObject))
                {
                    Debug.Log("AddonClick");
                    theConstructionManager.ConstructionStage = ConstructionManager.CONSTRUCTION_STAGE.SELECTION_ADDON;

                    theHoverScript.gameObject.SetActive(true);
                    theHoverScript.ObjectToHoverOver = this.gameObject;
                }
            }
        }
    }
}

