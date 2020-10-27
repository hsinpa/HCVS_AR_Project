using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game08 : MonoBehaviour
{
    public GameObject ARmodel; 
    public GameObject CameraModel;
    private Camera _camera;
    private bool isARsupport;

    void Start()
    {
        isARsupport = MissionsController.Instance.isARsupport;

        _camera = MissionsController.Instance.isARsupport ? Camera.main: MissionsController.Instance.MainCamera;

        transform.position = new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z);
        transform.rotation = MainCompass.main.transform.rotation;
        
        //SewitchModel(isARsupport);
    }
    private void Update()
    {
        if (MissionsController.Instance.isARsupport)
        {
            if (JoeMain.Main.isIOS)
            {
                transform.rotation = MainCompass.main.transform.rotation;
            }
        }
        else
        {
            transform.rotation = MainCompass.main.transform.rotation;
        }
    }

    void SewitchModel(bool _isARsupport)
    {
        ARmodel.SetActive(_isARsupport);
        CameraModel.SetActive(!_isARsupport);
    }
}
