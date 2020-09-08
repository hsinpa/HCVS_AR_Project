using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;
using Expect.StaticAsset;

namespace Expect.View
{
    public class EndMissionView : BaseView
    {
        [SerializeField]
        private Text message;
        [SerializeField]
        private Text scoreText;

        [SerializeField]
        private Button LeaveButton;

        public delegate void OnBottomBtnClick();
        public event OnBottomBtnClick OnEnable;

        public void EndMission(int score, string endMessage)
        {
            message.text = string.Format(endMessage, score);
            scoreText.text = score.ToString();

            LeaveButton.onClick.AddListener(() => OnEnable());
        }
    }
}

