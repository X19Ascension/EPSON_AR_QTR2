using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class LoseManager : MonoBehaviour {

    public TownHall TownHall;
    //public FadeInOut Fade;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (TownHall.GetComponent<Health>().HP <= 0)// || Input.GetKeyUp("3")
        {
            GameObject.Find("BlackScreen").GetComponent<FadeInOut>().b_FadeOut = true;
            GameObject[] ToKill = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject go in ToKill)
                Destroy(go);
            if (Input.GetMouseButtonUp(0))
            {
                PersistentData.m_Instance.DataAmount -= TownHall.DCost;
                if (PersistentData.m_Instance.DataAmount <= 0)
                    PersistentData.m_Instance.DataAmount = 0;
                PersistentData.m_Instance.CreditsAmount -= TownHall.ECost;
                if (PersistentData.m_Instance.CreditsAmount <= 0)
                    PersistentData.m_Instance.CreditsAmount = 0;
                TownHall.gameObject.SetActive(true);
                TownHall.GetComponent<Health>().HP = 100;
                GameObject[] AllDestroyed = GameObject.FindGameObjectsWithTag("Destroyed");
                List<GameObject> ToRebuild = new List<GameObject>();

                foreach (GameObject go in AllDestroyed)
                {
                    if (go.GetComponent<DestroyedWall>())
                    {
                        // Rebuild all of them
                        go.GetComponent<DestroyedWall>().Rebuild();
                    }
                }

                GameObject.Find("BlackScreen").GetComponent<FadeInOut>().b_FadeIn = true;

                
            }
        }
    }
}
