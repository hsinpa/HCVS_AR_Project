using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class cameraRota : MonoBehaviour
{
    public Text text;
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = MissionsController.Instance.isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = _camera.transform.rotation.eulerAngles.ToString();
    }
}
