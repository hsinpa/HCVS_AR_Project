﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Video
{
    public class VideoEffectCtrl : MonoBehaviour
    {
        private Material _mat;

        [SerializeField]
        private Renderer _renderer;

        private bool isAnime;
        private float _speed, _target;

        private const string TransitionTet = "_Transition";
        
        private void Start()
        {
            _mat = _renderer.material;
        }

        //Might only need to set once
        public void SetTextureAndCamera(RenderTexture renderTex) {
            _mat.SetTexture("_MainTex", renderTex);
        }

        public void FaceVideoToCameraFront(Camera camera) {
            this.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
            this.transform.Rotate(new Vector3(0, -90, 0));

        }

        public void SetCoverPercent(float p_percent) {
            _mat.SetFloat(TransitionTet, p_percent);
        }

        public void SetCoverPercentAnim(float p_percent, float speed)
        {
            _speed = speed;
            _target = p_percent;
            isAnime = true;
        }

        private void Update()
        {
            if (!isAnime) return;

            float currentValue = _mat.GetFloat(TransitionTet);
            float finalValue = Mathf.Lerp(currentValue, _target, _speed);

            if (Mathf.Abs(_target - finalValue) < 0.01f) {
                isAnime = false;
                finalValue = _target;
            }

            SetCoverPercent(finalValue);
        }
    }
}
