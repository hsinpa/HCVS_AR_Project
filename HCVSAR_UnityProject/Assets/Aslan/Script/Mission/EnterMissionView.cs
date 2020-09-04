using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;

namespace Expect.View
{
    public class EnterMissionView : BaseView
    {
        [SerializeField]
        private Text missionsTitle;
        [SerializeField]
        private Text missionsName;

        [SerializeField]
        private Button EnterButton;
        [SerializeField]
        private Button LeaveButton;

        public delegate void OnBottomBtnClick();
        public event OnBottomBtnClick OnEnable;
        public event OnBottomBtnClick OnDisable;

        public void EnterMission(string missionTitle, string missionName)
        {
            missionsTitle.text = missionTitle;
            missionsName.text = missionName;

            EnterButton.onClick.AddListener(() => OnEnable());
            LeaveButton.onClick.AddListener(() => OnDisable());
        }
    }
}
