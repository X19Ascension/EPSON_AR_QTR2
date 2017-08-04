using UnityEngine;
using System.Collections;

public class SampleAgentScript : MonoBehaviour {
    public GameObject target;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
<<<<<<< HEAD
        target = GameObject.Find("Target").GetComponent<Transform>();
=======
        target = GameObject.Find("Target");
>>>>>>> Added tracking for zombies
	}
	
	// Update is called once per frame
	void Update () {
        agent.SetDestination(target.transform.position);
	}
}

