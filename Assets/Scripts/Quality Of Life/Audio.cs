using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {

    public AudioClip Button_Click;//choose 1
    public AudioClip Button_Click2;//choose 1
    public AudioClip Building;
    public AudioClip Building_Destroyed;
    public AudioClip Demolish;
    public AudioClip Turret_Fire;
    public AudioClip Turret_Fire_2;
    public AudioClip Turret_Fire_3;
    public AudioClip BG1;
    public AudioClip BG2;
    public AudioClip BG3;
    public AudioClip BG4;
    public AudioClip BG5;
    public AudioClip LoadingScreen;
    public AudioClip MenuScreen;

    public AudioSource source;
    private float volLowRange = 5f;//.5f
    private float volHighRange = 10f;//1.0f

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float vol = Random.Range(volLowRange, volHighRange);
        }	
        if (Input.GetKeyDown(KeyCode.Alpha0))//0 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(Button_Click, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))//1 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(Button_Click2, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//2 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(Building, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))//3 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(Building_Destroyed, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))//4 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(Demolish, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))//5 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(Turret_Fire, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))//6 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(Turret_Fire_2, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))//7 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(Turret_Fire_3, vol);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))//8 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(BG1, vol);
        } 
        if (Input.GetKeyDown(KeyCode.Alpha9))//9 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(BG2, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))//6 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(BG3, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//6 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(BG4, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))//6 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(BG5, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))//6 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(LoadingScreen, vol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))//6 key
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(MenuScreen, vol);
        }
	}

    public void PlaySound(string name)
    {
        float vol = Random.Range(volLowRange, volHighRange);
        switch (name)
        {
            case "Button_Click":
                source.PlayOneShot(Button_Click, vol);
                break;

            case "Button_Click2":
                source.PlayOneShot(Button_Click2, vol);
                break;
                
            case "Building":
                source.PlayOneShot(Building, vol);
                break;

            case "Building_Destroyed":
                source.PlayOneShot(Building_Destroyed, vol);
                break;

            case "Demolish":
                source.PlayOneShot(Demolish, vol);
                break;

            case "Turret_Fire":
                source.PlayOneShot(Turret_Fire, vol);
                break;

            case "Turret_Fire_2":
                source.PlayOneShot(Turret_Fire_2, vol);
                break;
            case "Turret_Fire_3":
                source.PlayOneShot(Turret_Fire_3, vol);
                break;

            case "Loading_Screen":
                source.PlayOneShot(LoadingScreen, vol);
                break;

            case "Menu_Screen":
                source.PlayOneShot(MenuScreen, vol);
                break;

            case "BG1":
                source.PlayOneShot(BG1, vol);
                break;

            case "BG2":
                source.PlayOneShot(BG2, vol);
                break;

            case "BG3":
                source.PlayOneShot(BG3, vol);
                break;

            case "BG4":
                source.PlayOneShot(BG4, vol);
                break;
            case "BG5":
                source.PlayOneShot(BG5, vol);
                break;

        }
    }
}
