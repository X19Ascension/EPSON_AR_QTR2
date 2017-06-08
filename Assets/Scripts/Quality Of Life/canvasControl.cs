using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class canvasControl : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup Canvas;

    void Awake()
    {
        Canvas.alpha = 0f;
        Canvas.interactable = false;
        Canvas.blocksRaycasts = false;

    }


    public void OnButtonClick()
    {
        Canvas.alpha = 1f;
        Canvas.interactable = true;
        Canvas.blocksRaycasts = true;
    }
   
 
}
