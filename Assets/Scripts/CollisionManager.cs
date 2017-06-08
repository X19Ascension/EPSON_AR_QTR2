using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {

    public int LayerToIgnore_1;
    public int LayerToIgnore_2;
    
    // Use this for initialization
    void Start () {
        Physics.IgnoreLayerCollision(LayerToIgnore_1, LayerToIgnore_2);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
