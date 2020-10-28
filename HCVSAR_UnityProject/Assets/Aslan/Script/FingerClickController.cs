using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerClickController : MonoBehaviour
{
    [SerializeField]
    private FingerClickEvent mainCameraClick;
    [SerializeField]
    private FingerClickEvent ARCameraClick;
    [HideInInspector]
    public FingerClickEvent currentClick;

    private void Start()
    {
        bool _isARsupport = MissionsController.Instance.isARsupport;

        currentClick = _isARsupport ? ARCameraClick : mainCameraClick;
    }
}
