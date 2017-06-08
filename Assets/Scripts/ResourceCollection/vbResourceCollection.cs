using UnityEngine;
using System.Collections;
using Vuforia;

public class vbResourceCollection : MonoBehaviour, IVirtualButtonEventHandler {

    public GameObject AttachedButton;
    public GameObject ResourceCollectionManager;

	// Use this for initialization
	void Start () {
        AttachedButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
	}

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
        ResourceCollectionManager.GetComponent<ResourceCollectionManager>().IncrementStage();
    }
}
