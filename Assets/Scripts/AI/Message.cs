using UnityEngine;
using System.Collections;

public class Message {

    public enum MESSAGE_TYPE
    {
        ATTACKING_TARGET,
    }

    public MESSAGE_TYPE theMessageType;
    public GameObject theSender, theReceiver, theTarget;
    public Vector3 theDestination;
}
