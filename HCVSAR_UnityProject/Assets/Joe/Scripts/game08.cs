using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game08 : MonoBehaviour
{
    public GameObject ARmodel;
    public GameObject CameraModel;

    private bool isARsupport;

    void Start()
    {
        isARsupport = MissionsController.Instance.isARsupport;
        
        transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        transform.rotation = MainCompass.main.transform.rotation;
        
        //SewitchModel(isARsupport);
    }
    private void Update()
    {
        transform.rotation = MainCompass.main.transform.rotation;
    }

    void SewitchModel(bool _isARsupport)
    {
        ARmodel.SetActive(_isARsupport);
        CameraModel.SetActive(!_isARsupport);
    }
}
