using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ChangeCity()
    {
        // Debug
        PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GridTest"); // Outdated
    }

    public void ChangeCollection()
    {
        PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("ResourceCollectionScene");
    }

    public void ChangeScene(string name)
    {
        //PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name);
    }

    public void ChangePause()
    {
        Debug.Log(SceneManager.GetActiveScene().name);

        //PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("PlaceHolderPause");
    }

    public void ChangeGamePause()
    {
        PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Game Pause Screen");//load game with pause screen
      
    }

    public void Quit()
    {
        PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        //SceneManager.LoadScene("LevelSelect");
        Application.Quit();
    }

    public void ChangeRestart()
    {
        PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Pause Screen Warning");
    }

    public void ChangeMainMenu()
    {
        PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Main Title");//yes button to main menu 
    }

    //for menu to tutorial
    public void ChangeTutorial()
    {
        PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Tutorial1");
    }


    public void Resume()
    {
        Debug.Log(PersistentData.m_Instance.PreviousScene);
        SceneManager.LoadScene(PersistentData.m_Instance.PreviousScene);
    }

    public void ChangePlayGame()
    {
        //PersistentData.m_Instance.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("NCTEST 1 - Copy");
    }

    public void LoadGame()
    {

    }

    public void testLoad()
    {
        SceneManager.LoadScene("NCTEST 1 - Copy - Copy");
    }
}
