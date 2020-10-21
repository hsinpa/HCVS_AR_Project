using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        private float[] VideoRotation = new float[] {205, 0, 265, 0, 285, 105, 0, 209, 269, 0}; //mission4 -> 285
        private float time;
        private int VideoNumber;
        private Camera _camera;
        public Text tet;

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
            VideoNumber = missionNumber;
            //cameraForward.y = 0;
            cameraForward.z = 285;
            //this.transform.rotation = Quaternion.Euler(0, -(float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading, 0);
            // this.transform.Rotate(new Vector3(0, VideoRotation[VideoNumber], 0));
            time = 1.1f;
            //this.transform.Rotate(new Vector3(0, VideoRotation[missionNumber], 0));
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
            //transform.rotation = Quaternion.Euler(0,-(float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading, 0) ;
            //float angle =  Camera.main.transform.rotation.eulerAngles.y - (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading;
            //var angle = Camera.main.transform.rotation.eulerAngles.y - (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading;
            //Camera.main.transform.rotation = Quaternion.Euler(0, angle, 0);

            //tet.text = string.Format("Camera.main.transform.rotation.eulerAngles: {0} \n (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading: {1} \n MainCompass.main.stpos: {2}\n angle: {3}", Camera.main.transform.rotation.eulerAngles.ToString(), (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading, MainCompass.main.stpos, angle);
            // this.transform.Rotate(new Vector3(0, VideoRotation[VideoNumber], 0));
            //this.transform.position = _camera.transform.position;
            /*if (time > 1)
            {
                if (time > 5)
                {
                    time = 0;

                }
                transform.rotation = MainCompass.main.gameObject.transform.rotation;
                this.transform.Rotate(new Vector3(0, VideoRotation[VideoNumber], 0));
                time += Time.deltaTime;
            }*/

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
