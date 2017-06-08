using UnityEngine;
using System.Collections;

public class EventOption : MonoBehaviour {

    public string Description;

    GameObject SelectedOutcome;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DoEffect()
    {
        float rand = Random.Range(0, 100);
        float firstValue = 0;

        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            float childTriggerChance = gameObject.transform.GetChild(i).GetComponent<EventOutcome>().TriggerChance;

            if (rand > firstValue && rand <= childTriggerChance + firstValue)
            {
                gameObject.transform.GetChild(i).GetComponent<EventOutcome>().DoOutcome();
                SelectedOutcome = gameObject.transform.GetChild(i).gameObject;
                break;
            }
            else
            {
                firstValue += childTriggerChance;
            }
        }
    }

    public GameObject GetSelectedOutcome()
    {
        return SelectedOutcome;
    }
}
