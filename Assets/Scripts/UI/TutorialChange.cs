using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialChange : MonoBehaviour {

    public Text textArea;

    public string[] strings;

    public float speed = 0.1f;

    int stringIndex = 0;
    int characterIndex = 0;

    public List<string> Char1 = new List<string>();
    public List<string> Char2 = new List<string>();

    public int ConvoStage = 0;

    public Text textshowed = null;
    public void changeWord (string word)
    {
        textshowed.text = word;
    }

    public Text textshowed2 = null;
    public void changeWord2(string word2)
    {
        textshowed2.text = word2;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void UpdateConvo()
    {
        ConvoStage++;

        textshowed.text = Char1[ConvoStage - 1];
        textshowed2.text = Char2[ConvoStage - 1];
    }

}
