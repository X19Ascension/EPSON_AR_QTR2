using UnityEngine;
using System.Collections;

public class TouchableObject : MonoBehaviour 
{
    int i_TouchableObjectid;
    string s_TouchableObjectid;
    void Start()
    {

    }

    public void Assignid(int id)
    {
        i_TouchableObjectid = id;
        s_TouchableObjectid = id.ToString();
    }

    public string GetID()
    {
        return s_TouchableObjectid;
    }

    public int GetIDI()
    {
        return i_TouchableObjectid;
    }
}
