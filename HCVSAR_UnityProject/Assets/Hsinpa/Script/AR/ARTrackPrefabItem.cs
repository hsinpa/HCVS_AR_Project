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

        private AudioSource _audioSource;

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

            _audioSource = _animator.GetComponent<AudioSource>();

            if (_ARZeroManager) {
                _ARZeroManager.OnDataUpdate += (OnARDataUpdate);
                _ARZeroManager.OnTakeOffBtnEvent += OnTakeOffEvent;
                _ARZeroManager.OnSkinChangeClick += OnSkinChange;
                //_ARZeroManager.ForceUpdate();

#if UNITY_ANDROID
                UpdatePlanePosRot(Vector3.zero, mainObject.transform.localRotation, mainObject.transform.localScale * 0.4f);
#endif

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

            UpdatePlanePosRot(data.position, data.rotation, data.scale);
        }

        private void UpdatePlanePosRot(Vector3 position, Quaternion rotation, Vector3 scale) {
            mainObject.transform.localPosition = position;
            mainObject.transform.localRotation = rotation;
            mainObject.transform.localScale = scale;
        }

        private void OnTakeOffEvent() {
            _animator.enabled = true;
            _audioSource.enabled = true;
            _animator.SetTrigger(GeneralFlag.ARZero.TakeOff_Anim);
            _audioSource.Play();
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