using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{

    

    private Ray m_Ray;
    private RaycastHit m_RayCastHit;
    private TouchableObject m_CurrentTouchableObject;
    [SerializeField]
    public GameObject go_Selected;

    // Use this for initialization
    void Start()
    {
        go_Selected = null;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.touches.Length == 1)
        {
            Touch touchedFinger = Input.touches[0];
            switch (touchedFinger.phase)
            {
                case TouchPhase.Ended:
                    m_Ray = Camera.main.ScreenPointToRay(touchedFinger.position);
                    if (Physics.Raycast(m_Ray.origin, m_Ray.direction, out m_RayCastHit, 100))
                    {
                        TouchableObject touchableObj = m_RayCastHit.collider.gameObject.GetComponent<TouchableObject>();
                        if (touchableObj)
                        {
                            if (!EventSystem.current.IsPointerOverGameObject())
                            {
                                m_CurrentTouchableObject = touchableObj;
                                //a TouchableObject selected, so set the text in here...
                                Debug.Log(m_CurrentTouchableObject.name);
                                ReturnObject(m_CurrentTouchableObject.gameObject);
                            }
                        }
                        else //nothing is selected
                        {
                            if (m_CurrentTouchableObject)
                            {
                                m_CurrentTouchableObject = null;
                                // reset the text back... 
                                ResetObject();
                                Debug.Log(m_CurrentTouchableObject.name);
                            }
                        }

                    }
                    break;
                default:
                    break;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {

            m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(m_Ray, out hit))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log(hit.transform.gameObject.name);
                    ReturnObject(hit.transform.gameObject);
                }
            }


        }
       
    }

    void ReturnObject(GameObject GO)
    {
        go_Selected = GO;
    }

    void ResetObject()
    {
        go_Selected = null;
    }
}
