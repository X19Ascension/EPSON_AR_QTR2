using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Wait : MonoBehaviour {

    public float timer = 3.0f;

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(timer);

        SceneManager.LoadScene("RERETEST");

        if (SceneManager.GetActiveScene().name.Contains("Main"))
            SceneManager.LoadScene("LevelSelect");//go to game scene
	    else if (SceneManager.GetActiveScene().name.Contains("Select"))
        {
            SceneManager.LoadScene("RERETEST");
            //switch (PersistentData.m_Instance.LevelToLoad)
            //{
            //    case PersistentData.LEVEL_TYPE.GRASS: SceneManager.LoadScene("GrassScene"); break;
            //    case PersistentData.LEVEL_TYPE.DESERT: SceneManager.LoadScene("DessertScene"); break;
            //    case PersistentData.LEVEL_TYPE.RUINS: SceneManager.LoadScene("RuinsScene"); break;
            //}
        }
        else if (SceneManager.GetActiveScene().name.Contains("Resource"))
        {
            SceneManager.LoadScene(PersistentData.m_Instance.PreviousScene);
        }
        else if (SceneManager.GetActiveScene().name.Contains("Pause"))
        {
            SceneManager.LoadScene(PersistentData.m_Instance.PreviousScene);
        }


    }
	
	// Update is called once per frame
	void Update () 
    {
    	
	}
}
