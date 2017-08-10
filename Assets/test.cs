using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    void Awake()
    {
        Debug.Log("Test Awake");
    }

    void OnCollisionStay(Collision col)
    {
        Debug.Log("Collision Entered");
    }

    void OnTriggerStay(Collider col)
    {
        Debug.Log("Trigger Entered");
    }
}
