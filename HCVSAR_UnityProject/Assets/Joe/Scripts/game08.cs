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
        SewitchModel(isARsupport);
    }

    void SewitchModel(bool _isARsupport)
    {
        ARmodel.SetActive(_isARsupport);
        CameraModel.SetActive(!_isARsupport);
    }
}
