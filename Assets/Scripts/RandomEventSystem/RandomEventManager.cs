using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;

public class RandomEventManager : MonoBehaviour {

    public List<EventBase> EventList;

    [Header("UI References")]
    public GameObject EventPanel;
    public Text Title;
    public Text Description;
    public GameObject Option1, Option2, Option3, EndButton;

    enum EVENT_STAGE
    {
        NO_EVENT,
        CHOOSE_OPTION,
        OUTCOME,
        END,
    }

    GameObject CurrentEvent;
    GameObject SelectedOption;

    EVENT_STAGE CurrentStage = EVENT_STAGE.NO_EVENT;
    int i_ChosenOption = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        switch (CurrentStage)
        {
            case EVENT_STAGE.NO_EVENT:

                if (CurrentEvent)
                    CurrentStage = EVENT_STAGE.CHOOSE_OPTION;

                break;
                
            case EVENT_STAGE.CHOOSE_OPTION:

                EventPanel.SetActive(true);
                Option1.SetActive(true);
                Option2.SetActive(true);
                Option3.SetActive(true);

                // Apply UI/Text
                Title.text = CurrentEvent.GetComponent<EventBase>().Title;
                Description.text = CurrentEvent.GetComponent<EventBase>().Description;
                Option1.GetComponentInChildren<Text>().text = CurrentEvent.transform.GetChild(0).GetComponent<EventOption>().Description;
                Option2.GetComponentInChildren<Text>().text = CurrentEvent.transform.GetChild(1).GetComponent<EventOption>().Description;
                Option3.GetComponentInChildren<Text>().text = CurrentEvent.transform.GetChild(2).GetComponent<EventOption>().Description;

                if (Option3.GetComponentInChildren<Text>().text == "")
                    Option3.SetActive(false);

                if (Option2.GetComponentInChildren<Text>().text == "")
                    Option2.SetActive(false);

                if (Option1.GetComponentInChildren<Text>().text == "")
                {
                    Option1.SetActive(false);
                    CurrentEvent.transform.GetChild(0).gameObject.GetComponent<EventOption>().DoEffect();
                    CurrentStage = EVENT_STAGE.OUTCOME;
                }

                if (i_ChosenOption != 0)
                {
                    CurrentEvent.transform.GetChild(i_ChosenOption - 1).gameObject.GetComponent<EventOption>().DoEffect();
                    SelectedOption = CurrentEvent.transform.GetChild(i_ChosenOption - 1).gameObject;

                    CurrentStage = EVENT_STAGE.OUTCOME;
                }
                break;

            case EVENT_STAGE.OUTCOME:

                if (SelectedOption)
                    Description.text = SelectedOption.GetComponent<EventOption>().GetSelectedOutcome().GetComponent<EventOutcome>().Description;
      
                Option1.SetActive(false);
                Option2.SetActive(false);
                Option3.SetActive(false);
                EndButton.SetActive(true);

                Destroy(CurrentEvent);

                break;
        }

	}

    public void RunRandomCheck(EventBase.TRIGGER_TYPE theTrigger)
    {
        if (!PersistentData.m_Instance.TutorialOver)
            return;

        foreach (EventBase anEvent in EventList)
        {
            if (theTrigger != anEvent.Trigger)
                continue;

            float rand = Random.Range(0, 100);
            if (rand <= anEvent.TriggerChance)
            {
                CurrentEvent = Instantiate(anEvent.gameObject);
                return;
            }
        }
    } 

    public void ChooseOption(int i)
    {
        i_ChosenOption = i;
    }

    public void Reset()
    {
        CurrentStage = EVENT_STAGE.NO_EVENT;
        CurrentEvent = null;
        CurrentEvent = null;
        SelectedOption = null;

        i_ChosenOption = 0;

        EndButton.SetActive(false);
        EventPanel.SetActive(false);
    }
}
