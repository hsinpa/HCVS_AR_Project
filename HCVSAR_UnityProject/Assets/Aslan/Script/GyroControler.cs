using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroControler : MonoBehaviour
{
    private Gyroscope gyroscope;
    private Quaternion rot;
    private bool gyroEnable;
    private GameObject CameraContainer;
    
    public void Start()
    {
        CameraContainer = new GameObject("Camera Container");
        CameraContainer.transform.position = transform.position;
        transform.SetParent(CameraContainer.transform);
        gyroEnable = EnableGyro();
    }
    /*
    public void StartGyro()
    {
        CameraContainer = new GameObject("Camera Container");
        CameraContainer.transform.position = transform.position;
        transform.SetParent(CameraContainer.transform);
        gyroEnable = EnableGyro();
    }
    */
    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyroscope = Input.gyro;
            gyroscope.enabled = true;

            CameraContainer.transform.rotation = Quaternion.Euler(90, 90, 0);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;
    }

    void Update()
    {
        if (gyroEnable)
        {
            transform.localRotation = gyroscope.attitude * rot;
        }
    }
}
