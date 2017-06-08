using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class ZoomInOut : MonoBehaviour {

    public float ZoomedOutPercent;
    public float ZoomedInPercent;
    public float ZoomRate = 0.1f;

    public enum ZOOM_STATE
    {
        NORMAL,
        IN_PROGRESS,
        ZOOMED_OUT,
    }
    public ZOOM_STATE theState;

    public Sprite ZoomOutSprite, ZoomInSprite, ZoomOutHighlightedSprite, ZoomInHighlightedSprite;

    float f_OrignalZoomPercent;
    float f_CurrentZoomPercent;
    bool b_ZoomIn;
    bool b_ZoomOut;
    GameObject ImageTarget;
    GameObject theCamera;

    // Use this for initialization
    void Start () {

        ImageTarget = GameObject.FindGameObjectWithTag("ImageTarget");
        theCamera = GameObject.FindGameObjectWithTag("MainCamera");

        f_CurrentZoomPercent = -1;

        theState = ZOOM_STATE.NORMAL;
        ZoomOutSprite = GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update() {

        if (PersistentData.m_Instance.TutorialOver)
            GetComponent<Image>().enabled = true;
        else
            GetComponent<Image>().enabled = false;


        if (ImageTarget.GetComponentInChildren<Renderer>().enabled && f_CurrentZoomPercent == -1)
        {
            f_CurrentZoomPercent = (theCamera.transform.position - ImageTarget.transform.position).magnitude;
            f_OrignalZoomPercent = f_CurrentZoomPercent;
        }

        if (b_ZoomIn)
        { 
            GameObject.FindGameObjectWithTag("MainCamera").transform.Translate(0, 0, Time.deltaTime);

            f_CurrentZoomPercent = ((theCamera.transform.position - ImageTarget.transform.position).magnitude / f_OrignalZoomPercent) * 100;

            if (f_CurrentZoomPercent < ZoomedInPercent)
            {
                b_ZoomIn = false;
                theState = ZOOM_STATE.NORMAL;
            }
        }

        if (b_ZoomOut)
        {
            GameObject.FindGameObjectWithTag("MainCamera").transform.Translate(0, 0, -Time.deltaTime);

            f_CurrentZoomPercent = ((theCamera.transform.position - ImageTarget.transform.position).magnitude / f_OrignalZoomPercent) * 100;

            if (f_CurrentZoomPercent > ZoomedOutPercent)
            {
                b_ZoomOut = false;
                theState = ZOOM_STATE.ZOOMED_OUT;
            }
        }
	}

    public void DoZoomInOut()
    {
        if (theState == ZOOM_STATE.NORMAL)
        {
            ZoomOut();
            return;
        }
        else if (theState == ZOOM_STATE.ZOOMED_OUT)
        {
            ZoomIn();
            return;
        }
    }

    void ZoomIn()
    {
        GetComponent<Image>().sprite = ZoomOutSprite;

        SpriteState newState = new SpriteState();
        newState.highlightedSprite = ZoomOutHighlightedSprite;
        GetComponent<Button>().spriteState = newState;

        GetComponentInParent<UISwitchOff>().SwitchOn();

        b_ZoomIn = true;
        b_ZoomOut = false;

        theState = ZOOM_STATE.IN_PROGRESS;
    }

    void ZoomOut()
    {
        GetComponent<Image>().sprite = ZoomInSprite;

        SpriteState newState = new SpriteState();
        newState.highlightedSprite = ZoomInHighlightedSprite;
        GetComponent<Button>().spriteState = newState;

        GetComponentInParent<UISwitchOff>().SwitchOff();

        b_ZoomOut = true;
        b_ZoomIn = false;

        theState = ZOOM_STATE.IN_PROGRESS;
    }
}
