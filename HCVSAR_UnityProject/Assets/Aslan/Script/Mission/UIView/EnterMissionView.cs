using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;
using UnityEngine.Events;

namespace Expect.View
{
    public class EnterMissionView : BaseView
    {
        [SerializeField]
        private Text missionsTitle;
        [SerializeField]
        private Text missionsName;

        [SerializeField]
        public Button EnterButton;
        [SerializeField]
        public Button LeaveButton;

        public delegate void OnButtonClick();
        public event OnButtonClick OnEnable;
        public event OnButtonClick OnDisable;

        public void EnterMission(string missionTitle, string missionName)
        {
            missionsTitle.text = missionTitle;
            missionsName.text = missionName;

            //EnterButton.onClick.AddListener(() => OnEnable());
            //LeaveButton.onClick.AddListener(() => OnDisable());
        }

        public void RemoveListeners()
        {
            EnterButton.onClick.RemoveAllListeners();
            LeaveButton.onClick.RemoveAllListeners();
        }
    }
}
