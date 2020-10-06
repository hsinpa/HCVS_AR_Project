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

        public Button EnterButton;
        public Button LeaveButton;
        public Image image;

        public delegate void OnButtonClick();
        public event OnButtonClick OnEnable;
        public event OnButtonClick OnDisable;

        public void EnterMission(string missionTitle, string missionName)
        {
            missionsTitle.text = missionTitle;
            missionsName.text = missionName;
        }

        public void RemoveListeners()
        {
            EnterButton.onClick.RemoveAllListeners();
            LeaveButton.onClick.RemoveAllListeners();
        }
    }
}
