using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class EnterLevel : MonoBehaviour {

    public PersistentData.LEVEL_TYPE LevelName;
    public Canvas WaitCanvas;

    bool b_InputReceived = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && !b_InputReceived)
            {
                if (hit.transform.gameObject.Equals(this.gameObject))
                {
                    switch (LevelName)
                    {
                        case PersistentData.LEVEL_TYPE.GRASS:
                            PersistentData.m_Instance.InitialLoad = false;
                            PersistentData.m_Instance.LevelToLoad = PersistentData.LEVEL_TYPE.GRASS;

                            WaitCanvas.gameObject.SetActive(true);
                            //SceneManager.LoadScene("GrassScene");
                            b_InputReceived = true;
                            break;

                        case PersistentData.LEVEL_TYPE.DESERT:
                            PersistentData.m_Instance.InitialLoad = false;
                            PersistentData.m_Instance.LevelToLoad = PersistentData.LEVEL_TYPE.DESERT;

                            WaitCanvas.gameObject.SetActive(true);
                            //SceneManager.LoadScene("DessertScene");
                            b_InputReceived = true;
                            break;

                        case PersistentData.LEVEL_TYPE.RUINS:
                            PersistentData.m_Instance.InitialLoad = false;
                            PersistentData.m_Instance.LevelToLoad = PersistentData.LEVEL_TYPE.RUINS;

                            WaitCanvas.gameObject.SetActive(true);
                            //SceneManager.LoadScene("RuinsScene");
                            b_InputReceived = true;
                            break;
                    }
                }
            }
        }
    }
}
