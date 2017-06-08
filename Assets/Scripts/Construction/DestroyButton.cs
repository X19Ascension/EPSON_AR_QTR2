using UnityEngine;
using System.Collections;

public class DestroyButton : MonoBehaviour {

    bool DestroyOn = false;

	// Use this for initialization
	void Start () {
	
	}
	
	public void OnPress () {
        DestroyOn = !DestroyOn;
	}

    public void SetDestroy(bool Destroy)
    {
        DestroyOn = Destroy;
    }

    public bool GetDestroy()
    {
        return DestroyOn;
    }
}
