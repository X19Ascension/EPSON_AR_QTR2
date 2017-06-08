using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedDialog : MonoBehaviour {

    public Text textArea;
    public string[] strings;
    public float speed = 0.1f;
    public bool ContinueCheck;
    public int ConvoStage;

    int i_stringIndex = 0; 
    public int characterIndex = 0;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(DisplayTimer());
        ContinueCheck = false;
        ConvoStage = -1;
        strings[i_stringIndex].Substring(0, characterIndex).Replace("ENTER", "\n");
    }
	
    IEnumerator DisplayTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(speed);

            if (characterIndex > strings[i_stringIndex].Length)
            {
                continue;
            }            
            textArea.text = strings[i_stringIndex].Substring(0, characterIndex);
            characterIndex++;
        }
    }

	// Update is called once per frame
	void Update () {
        if (ContinueCheck)
        {
            if (characterIndex < strings[i_stringIndex].Length)
            {      
                characterIndex = strings[i_stringIndex].Length;
                ContinueCheck = false;
                ConvoStage += 1;
            }
            
            else if (i_stringIndex < strings.Length)
            {
                i_stringIndex++;
                characterIndex = 0;
            }
        }
	}

    public bool GetContinueCheck()
    { return ContinueCheck; }

    public void SetContinueCheck(bool c)
    { ContinueCheck = c; }
}
