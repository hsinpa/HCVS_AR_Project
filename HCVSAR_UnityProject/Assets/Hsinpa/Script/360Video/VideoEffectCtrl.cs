using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Video
{
    public class VideoEffectCtrl : MonoBehaviour
    {
        [SerializeField]
        private Material _mat;

        [SerializeField]
        private Renderer _renderer;

        public void SetUp(RenderTexture renderTex) {
            _mat.SetTexture("_MainTex", renderTex);
        }

        public void SetCoverPercent(float p_percent) {
            _mat.SetFloat("_Transition", p_percent);
        }

        public void SetCoverPercentAnim(float p_percent, float speed)
        {
            _mat.SetFloat("_Transition", p_percent);
        }

        private void Update()
        {
            return;
        }

    }
}
