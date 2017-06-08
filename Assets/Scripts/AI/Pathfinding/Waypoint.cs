using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Collision reached.");
        if (col.gameObject.tag.Contains("Enemy"))
        {
            Debug.Log("Waypoint reached.");
            col.GetComponent<Pathfinder>().SetOnWaypoint(true);
            col.GetComponent<Pathfinder>().IncrementIndex();
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Contains("Enemy") && !col.GetComponent<Pathfinder>().GetWaypointStatus() && col.gameObject.GetComponent<Pathfinder>().b_PathFound)
        {
            Debug.Log("Waypoint stay.");
            col.GetComponent<Pathfinder>().SetOnWaypoint(true);
            col.GetComponent<Pathfinder>().IncrementIndex();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag.Contains("Enemy"))
        {
            col.GetComponent<Pathfinder>().SetOnWaypoint(false);
        }
    }

}
