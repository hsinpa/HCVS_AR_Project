using System.Collections;
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
        private float[] VideoRotation = new float[] {115, 0, 310, 0, 0, 0, 0, 299, 269, 0}; //mission4 -> 285
        private Camera _camera;

        private void Start()
        {
            _mat = _renderer.material;
            _camera = MissionsController.Instance.isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;
        }

        //Might only need to set once
        public void SetTexture(RenderTexture renderTex) {
            _mat.SetTexture("_MainTex", renderTex);
        }

        public void FaceVideoToCameraFront(Camera camera,int missionNumber) {

            var cameraForward = camera.transform.forward;
            cameraForward.y = 0;
            this.transform.rotation = MainCompass.main.gameObject.transform.rotation;
            this.transform.Rotate(new Vector3(0, VideoRotation[missionNumber], 0));
            this.transform.position = camera.transform.position;
            _camera = camera;
        }
        

        public void FaceDirection(Vector3 vector) {
            this.transform.rotation = Quaternion.LookRotation(vector);
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
