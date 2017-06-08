using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class DestroyedWall : MonoBehaviour {

    public GameObject PreviousWall;

    [Header("Costs to Rebuild")]
    public int CreditCost;
    public int DataCost;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if clicking on destroyed wall
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.Equals(this.gameObject))
                {
                    // Find all destroyed walls
                    GameObject[] AllDestroyed = GameObject.FindGameObjectsWithTag("Destroyed");
                    List<GameObject> ToRebuild = new List<GameObject>();

                    foreach (GameObject go in AllDestroyed)
                    {
                        if (go.GetComponent<DestroyedWall>())
                        {
                            // Rebuild all of them
                            go.GetComponent<DestroyedWall>().Rebuild();

                            PersistentData.m_Instance.CreditsAmount -= go.GetComponent<DestroyedWall>().CreditCost;
                            PersistentData.m_Instance.DataAmount -= go.GetComponent<DestroyedWall>().DataCost;
                        }
                    }
                }
            }
        }

	}

    public void Rebuild()
    {
        PreviousWall.GetComponent<Health>().Reset();

        //GameObject go = Instantiate(PreviousWall, this.gameObject.transform.position, PreviousWall.gameObject.transform.rotation) as GameObject;

        PreviousWall.transform.parent = GameObject.FindGameObjectWithTag("ImageTarget").transform;
        PreviousWall.SetActive(true);

        Destroy(this.gameObject);
    }
}
