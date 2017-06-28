using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TraverseScreenScript : MonoBehaviour {

    public int sceneToLoad;

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height - 80, 100, 30), "Current Scene: " + (Application.loadedLevel + 1));
        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height - 50, 100, 30), "Load Scene: " + (sceneToLoad + 1)))
        {
            
            SceneManager.LoadScene(sceneToLoad);


        }
    }
}
