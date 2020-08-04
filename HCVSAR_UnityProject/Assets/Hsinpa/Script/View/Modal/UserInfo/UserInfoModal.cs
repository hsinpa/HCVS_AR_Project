using Expect.StaticAsset;
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

        public void SetContent(TypeFlag.SocketDataType.StudentDatabaseType studentObj, bool hasConnection, TypeFlag.SocketDataType.UserScoreType[] rawScoreArray, TypeFlag.UserType userType) {
            UserInfoText.text = GetUserInfoText(studentObj, hasConnection);
        }

        private string GetUserInfoText(TypeFlag.SocketDataType.StudentDatabaseType studentObj, bool hasConnection) {

            string formString = string.Format(StringAsset.UserInfo.HeaderUserInfo, studentObj.seat, studentObj.student_name, studentObj.id,

                (hasConnection) ? StringAsset.UserInfo.OnlineColor : StringAsset.UserInfo.OfflineColor,
                (hasConnection) ? StringAsset.UserInfo.Online : StringAsset.UserInfo.Offline);

            return formString;
        }

    }
}