using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game06 : MonoBehaviour
{
    public GameObject RotateModel;

    private float y;

    private void Start()
    {
        var _camera = Camera.main;
        var _cameraFront = _camera.transform.forward;
        var _frontPos = _cameraFront * 15;

        _cameraFront.y = 0;
        RotateModel.transform.position = _frontPos;
        RotateModel.transform.rotation = Quaternion.LookRotation(_cameraFront);
    }

    void Update()
    {
        y += Time.deltaTime * 20;
        RotateModel.transform.rotation = Quaternion.Euler(0, 90 + y, 0);        
    }
}
