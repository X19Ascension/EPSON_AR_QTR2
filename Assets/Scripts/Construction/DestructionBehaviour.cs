using UnityEngine;
using System.Collections;

public class DestructionBehaviour : MonoBehaviour
{
    public GameObject DestroyedBuildingTemplate, DestroyedWallTemplate, DestroyedTurretTemplate;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        // Check if building is destroyed or just being re-generated
        if (GetComponent<Health>().HP <= 0)
        {
            if (GetComponent<BaseTurret>())
                if (GetComponent<BaseTurret>().IsPreview)
                    return;

            if (GetComponent<BuildingAbstractBase>())
                if (GetComponent<BuildingAbstractBase>().IsPreview)
                    return;

            GameObject Template = DestroyedBuildingTemplate;

            if (this.gameObject.name.Contains("Standard") || this.gameObject.name.Contains("Sniper") || this.gameObject.name.Contains("Heavy"))
            {
                Template = DestroyedTurretTemplate;
            }
            else if (this.gameObject.name.Contains("Wall"))
            {
                Template = DestroyedWallTemplate;
            }
            else
            {
                Template = DestroyedBuildingTemplate;
            }

            GameObject go = Instantiate(Template, this.gameObject.transform.position, Quaternion.identity) as GameObject;

            if (!this.tag.Contains("Tower"))
            {
                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                go.GetComponent<BuildingAbstractBase>().GridPos = this.GetComponent<BuildingAbstractBase>().GridPos;
                go.GetComponent<DestroyedBuilding>().PreviousBuilding = this.gameObject;
            }
            else
            {
                go.GetComponent<BuildingAbstractBase>().IsPreview = false;
                go.GetComponent<BuildingAbstractBase>().GridPos = this.GetComponent<BaseTurret>().GridPos;
                go.GetComponent<DestroyedBuilding>().PreviousBuilding = this.gameObject;
            }

            go.transform.parent = GameObject.Find("GridManager").transform;
        }
    }
}
