using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Video
{

    public class VideoEffectTest : MonoBehaviour
    {
        [SerializeField]
        private VideoEffectCtrl videoEffect;

        void Start()
        {
            videoEffect.FaceVideoToCameraFront(Camera.main, 0);
            videoEffect.SetCoverPercentAnim(0, 0.1f);
        }



    }
}