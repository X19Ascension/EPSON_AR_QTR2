using UnityEngine;
using System.Collections;

public class FadeOutScript : MonoBehaviour {

    public bool fade = true;
    public Texture2D fadeTexture;
    public float fadeSpeed = 0.9f;
    public int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

	// Use this for initialization
	void Start () {
	    //fade = true;
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void OnGUI()
    {
        if (fade)
        {
            alpha += fadeDir * fadeSpeed * (Time.deltaTime * 0.2f);
            alpha = Mathf.Clamp01(alpha);

            Color thisAlpha = GUI.color;
            thisAlpha.a = alpha;
            GUI.color = thisAlpha;

            GUI.depth = drawDepth;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
    }
}
