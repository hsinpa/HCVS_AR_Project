using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class scriptMove : MonoBehaviour
{
    const float kFilteringFactor = 0.1f;

    public Vector3 A1;
    public Vector3 A2;
    public Vector3 A2ramping; // for the low-pass filter
    public Vector3 V1;
    public Vector3 V2;

    public int SpeedFactor = 1000; //this factor is for increasing acceleration to move in unity world

    void resetAll()
    {
        Input.gyro.enabled = true;
        A2 = Vector3.zero;
        V1 = Vector3.zero;
        V2 = Vector3.zero;
        A2ramping = Vector3.zero;
    }
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("resetAll", 0, 10);
    }

    //http://stackoverflow.com/a/1736623
    Vector3 ramping(Vector3 A)
    {
        A2ramping = A * kFilteringFactor + A2ramping * (1.0f - kFilteringFactor);
        return A - A2ramping;
    }

    void getAcceleration(float deltaTime)
    {
        Input.gyro.enabled = true;
        
        A1 = A2;
        A2 = ramping(Input.gyro.userAcceleration) * SpeedFactor;

        V2 = V1 + (A2 - A1) * deltaTime;

        V1 = V2;
    }

    //Update is called once per frame
    void Update()
    {

        getAcceleration(Time.deltaTime);

        float distance = -1f;
        Vector3 newPos = transform.position;

        transform.Translate(Vector3.forward * Time.deltaTime * V2.z * distance);
    }
}