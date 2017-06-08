using UnityEngine;
using System.Collections;

public class UIHoverOver : MonoBehaviour {

    public GameObject ObjectToHoverOver;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectToHoverOver)
        {
            Vector3 ScreenPos = Camera.main.WorldToScreenPoint(ObjectToHoverOver.transform.position);
            this.GetComponent<RectTransform>().position = ScreenPos + new Vector3(0, 0.5f, 0);
        }
    }
}
