using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class UserInfoModal : Modal
    {
        [SerializeField]
        private Button CloseButton;

        [SerializeField]
        private Text TotalScoreText;

        [SerializeField]
        private Text UserInfoText;

        [SerializeField]
        private Transform AchievementHolder;

        [SerializeField]
        private GameObject AchievementPrefab;

        public void SetContent(string userInfo, bool isConnection, TypeFlag.SocketDataType.UserScoreType[] rawScoreArray, TypeFlag.UserType userType) {
            UserInfoText.text = userInfo;
        }
    }
}