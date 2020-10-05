using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test07 : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var _camera = Camera.main;
        var _cameraFront = _camera.transform.forward;
        var _frontPos = _cameraFront * 10;

        _cameraFront.y = 0;

        transform.position = _frontPos;

        transform.rotation = Quaternion.LookRotation(_cameraFront);
    }
}
