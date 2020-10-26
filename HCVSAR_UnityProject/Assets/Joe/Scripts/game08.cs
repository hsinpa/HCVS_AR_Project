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
<<<<<<< HEAD

        _camera = MissionsController.Instance.isARsupport ? Camera.main: MissionsController.Instance.MainCamera;

        transform.position = new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z);
=======
        
        transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
>>>>>>> c4a371fde35a598e9a39b438470ce6b2c962a9d7
        transform.rotation = MainCompass.main.transform.rotation;
        
        //SewitchModel(isARsupport);
    }
    private void Update()
    {
<<<<<<< HEAD
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
=======
        transform.rotation = MainCompass.main.transform.rotation;
>>>>>>> c4a371fde35a598e9a39b438470ce6b2c962a9d7
    }

    void SewitchModel(bool _isARsupport)
    {
        ARmodel.SetActive(_isARsupport);
        CameraModel.SetActive(!_isARsupport);
    }
}
