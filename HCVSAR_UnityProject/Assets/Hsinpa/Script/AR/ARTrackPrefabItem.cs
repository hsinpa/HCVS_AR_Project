﻿using Hsinpa.AR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Hsinpa.AR
{
    public class ARTrackPrefabItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainObject;

        [SerializeField]
        private Animator _animator;

        private ARTrackedImage _ARTrackedImage;
        private ARZeroManager _ARZeroManager;

        void Start()
        {
            _ARTrackedImage = GetComponent<ARTrackedImage>();
            _ARZeroManager = GameObject.FindObjectOfType<ARZeroManager>();

            if (_ARZeroManager) {
                _ARZeroManager.OnDataUpdate += (OnARDataUpdate);
                _ARZeroManager.OnTakeOffBtnEvent += OnTakeOffEvent;
                _ARZeroManager.ForceUpdate();
            }
        }

        private void OnARDataUpdate(ARDataSync aRDataSync)
        {
            if (_ARTrackedImage == null || aRDataSync == null) {
                Debug.LogError("_ARTrackedImage is null");
                return;
            }

            Debug.Log("Reference Image name " + _ARTrackedImage.referenceImage.name);

            ARDataSync.ARData data = aRDataSync.FindArData(_ARTrackedImage.referenceImage.name);

            mainObject.transform.localPosition = data.position;
            mainObject.transform.localRotation = data.rotation;
            mainObject.transform.localScale = data.scale;
        }

        private void OnTakeOffEvent() {
            _animator.SetTrigger(GeneralFlag.ARZeroAnimator.TakeOff);
        }

        private void OnDestroy()
        {
            _ARZeroManager.OnDataUpdate -= (OnARDataUpdate);
            _ARZeroManager.OnTakeOffBtnEvent -= OnTakeOffEvent;
        }
    }
}