using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour 
{

    public void OnClickPlay()//click restart button
    {
        Application.LoadLevel("Pause Screen Warning");//go to Pause Screen Warning
    }

    public void OnClickPlay2()//click pause button
    {
        Application.LoadLevel("Pause Screen");//go to Pause Screen
    }

    public void OnClickPlay3()
    {
        Application.LoadLevel("");
    }

    public void OnClickPlay4()//click yes button
    {
        Application.LoadLevel("Main Title");
    }

    public void OnClickPlay5()//click no button
    {
        Application.LoadLevel("GridTest");
    }

    public void OnClickPlay6()//click start button
    {
        Application.LoadLevel("Loading Screen");//go to loading screen
    }

    public void OnClickPlay7()//click quit button
    {
        Application.Quit();//Quit Game
    }
	
}
