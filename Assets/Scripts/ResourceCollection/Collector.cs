using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour
{

    public GameObject ResourceCollectionManager;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Left-click
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == this.gameObject)
            {
                if (ResourceCollectionManager.GetComponent<ResourceCollectionManager>().GetStage() != RESOURCE_COLLECTON_STAGE.COLLECTING)
                    ResourceCollectionManager.GetComponent<ResourceCollectionManager>().IncrementStage();
            }
        }

        if (Input.GetKeyUp(KeyCode.A))
            if (ResourceCollectionManager.GetComponent<ResourceCollectionManager>().GetStage() != RESOURCE_COLLECTON_STAGE.COLLECTING)
                ResourceCollectionManager.GetComponent<ResourceCollectionManager>().IncrementStage();
    }
}
