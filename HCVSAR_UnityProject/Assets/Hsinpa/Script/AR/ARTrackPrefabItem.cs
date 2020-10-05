using Hsinpa.AR;
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

        [SerializeField]
        private Renderer _meshRenderer;

        [SerializeField]
        private Texture _specialTex;
        [SerializeField]
        private Texture _skintype_1;
        [SerializeField]
        private Texture _skintype_2;

        private Texture _originalTex
        {
            get {
                if (ARZeroManager.RandomSkinString == GeneralFlag.ARZero.TrackImage_2)
                    return _skintype_2;

                return _skintype_1;
            }
        }


        private ARTrackedImage _ARTrackedImage;
        private ARZeroManager _ARZeroManager;

        private const string MatTexName = "_MainTex";
        void Start()
        {
            _ARTrackedImage = GetComponent<ARTrackedImage>();
            _ARZeroManager = GameObject.FindObjectOfType<ARZeroManager>();

            if (_ARZeroManager) {
                _ARZeroManager.OnDataUpdate += (OnARDataUpdate);
                _ARZeroManager.OnTakeOffBtnEvent += OnTakeOffEvent;
                _ARZeroManager.OnSkinChangeClick += OnSkinChange;
                _ARZeroManager.ForceUpdate();

                OnSkinChange(0);
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
            _animator.SetTrigger(GeneralFlag.ARZero.TakeOff_Anim);
        }

        private void OnSkinChange(int p_skinIndex) {

            //Original
            if (p_skinIndex == 0) {
                _meshRenderer.material.SetTexture(MatTexName, _originalTex);

            }

            //Special
            if (p_skinIndex == 1) {
                _meshRenderer.material.SetTexture(MatTexName, _specialTex);
            }
        }

        private void OnDestroy()
        {
            _ARZeroManager.OnDataUpdate -= (OnARDataUpdate);
            _ARZeroManager.OnSkinChangeClick -= OnSkinChange;
            _ARZeroManager.OnTakeOffBtnEvent -= OnTakeOffEvent;
        }
    }
}