using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float speed = 10.0f;

    public float speedH = 10.0f;
    public float speedV = 10.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {

        gameObject.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

    }

}
