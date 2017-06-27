using UnityEngine;
using System.Collections;


public class TargetPrioritize : MonoBehaviour {

    public GameObject picPriority;
    private GameObject m_prio;
    private Vector3 curScreenPoint;
    private Vector3 curPosition;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f);
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (m_prio != null)
        {
            //m_prio.transform.LookAt(Camera.main.transform.position, -Vector3.up);
            //m_HPBar.transform.Rotate(new Vector3(0, 180, 0));

            if (this.gameObject.GetComponent<Zombie>().HP <= 0)
                Destroy(m_prio);
        }
                //m_prio.SetActive(false);
    }
    
    void OnMouseDown()
    {
        GameObject[] priority = GameObject.FindGameObjectsWithTag("test") as GameObject[];


        Vector3 screenPt = Camera.main.WorldToScreenPoint(transform.position);
        curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPt.z);

        curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

        Vector3 test = curPosition;
        curPosition += offset;

        test -= offset;

        foreach (GameObject go in priority)
        {
            if (go.transform.FindChild("TargetPlaceHolder(Clone)"))
            {
                GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("Priority");
                if (toDestroy != null)
                    foreach (GameObject pew in toDestroy)
                        Destroy(pew);
                Destroy(go.transform.Find("TargetPlaceHolder(Clone)").gameObject);
            }
        }

        foreach (GameObject go in priority)
        {

            if (Input.GetMouseButtonDown(0))// && 
            {
                float goX = go.transform.position.x;
                float goZ = go.transform.position.z;

                if ((goX < curPosition.x && goX > test.x) &&
                (goZ < curPosition.z && goZ > test.z))
                {
                    m_prio = Instantiate(picPriority, go.transform.position, Quaternion.identity) as GameObject;
                    m_prio.transform.SetParent(go.transform);
                    
                    if (go.name == "Tank_Zombie(Clone)")
                        m_prio.transform.Translate(0, m_prio.transform.lossyScale.y * 5.8f, 0);
                    else
                        m_prio.transform.Translate(0, m_prio.transform.lossyScale.y * 4.2f, 0);

                    m_prio.SetActive(true);
                    break;
                }
            }
        }
    }
}
