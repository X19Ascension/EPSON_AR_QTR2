using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour {
    public Slider mainSlider;
    public GameObject WaveTime;
    float wavetime;
    float maxwavetime;
    // Use this for initialization
    void Start () {
        wavetime = WaveTime.GetComponent<WaveSpawner>().waveDuration;
        maxwavetime = WaveTime.GetComponent<WaveSpawner>().waveDurationSave;
    }
	
	// Update is called once per frame
	void Update () {
        wavetime = WaveTime.GetComponent<WaveSpawner>().waveDuration;
        maxwavetime = WaveTime.GetComponent<WaveSpawner>().waveDurationSave;
        RescaleTimeBar();
    }

    public void RescaleTimeBar()
    {
        float timeScale = wavetime / maxwavetime;
        mainSlider.value = timeScale;
    }
}
