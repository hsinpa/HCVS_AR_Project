using Expect.StaticAsset;
using Hsinpa.View;
using System;
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
        private Button KickButton;

        [SerializeField]
        private Text TotalScoreText;

        [SerializeField]
        private Text UserInfoText;

        [SerializeField]
        private Transform AchievementHolder;

        [SerializeField]
        private GameObject AchievementPrefab;

        [SerializeField]
        private MissionItemSObj missionItemSObj;

        private TypeFlag.UserType userType;

        private System.Action<TypeFlag.SocketDataType.StudentDatabaseType> OnKickStudentEvent;

        private void Awake()
        {
            CloseButton.onClick.AddListener(() =>
            {
                Modals.instance.Close();
            });
        }

        public void SetUserInfo(System.Action<TypeFlag.SocketDataType.StudentDatabaseType> OnKickStudentEvent,
                                TypeFlag.SocketDataType.StudentDatabaseType studentObj, bool hasConnection) {

            this.OnKickStudentEvent = OnKickStudentEvent;
            KickButton.onClick.RemoveAllListeners();
            KickButton.onClick.AddListener(()=> { if (this.OnKickStudentEvent != null) this.OnKickStudentEvent(studentObj); });
            KickButton.gameObject.SetActive(false);
            KickButton.interactable = hasConnection;

            ResetContent();

            UserInfoText.text = GetUserInfoText(studentObj, hasConnection);
        }

        public void SetContent(TypeFlag.SocketDataType.UserScoreType[] scoreArray, TypeFlag.UserType userType) {
            this.userType = userType;

            KickButton.gameObject.SetActive(this.userType == TypeFlag.UserType.Teacher);

            TotalScoreText.text = CalculateScore(scoreArray) + "%";

            GenerateScoreBoard(scoreArray);
        }

        private string GetUserInfoText(TypeFlag.SocketDataType.StudentDatabaseType studentObj, bool hasConnection) {

            string formString = string.Format(StringAsset.UserInfo.HeaderUserInfo, studentObj.student_name, studentObj.id,

                (hasConnection) ? StringAsset.UserInfo.OnlineColor : StringAsset.UserInfo.OfflineColor,
                (hasConnection) ? StringAsset.UserInfo.Online : StringAsset.UserInfo.Offline);

            return formString;
        }

        private float CalculateScore(TypeFlag.SocketDataType.UserScoreType[] scoreArray) {
            return (float)Math.Round(scoreArray.Sum(x => x.score) / 100f, 2);
        }

        private float CalculateAccompishPercent(TypeFlag.SocketDataType.UserScoreType[] scoreArray)
        {
           return (((float)scoreArray.Length) / missionItemSObj.missionArray.Length) * 100;
        }

        private void GenerateScoreBoard(TypeFlag.SocketDataType.UserScoreType[] scoreArray) {

            if (missionItemSObj.missionArray == null) return;

            var scoreList = scoreArray.ToList();
            int missionLength = missionItemSObj.missionArray.Length;

            Utility.UtilityMethod.ClearChildObject(AchievementHolder);

            for (int i = 0; i < missionLength; i++)
            {
                //Hide extra mission info from user
                if (missionItemSObj.missionArray[i].hide) continue;

                AchievementItemView a_item = Utility.UtilityMethod.CreateObjectToParent(AchievementHolder, AchievementPrefab).GetComponent<AchievementItemView>();

                a_item.hashed = (userType != TypeFlag.UserType.Teacher);

                //Check if this task is been accompished
                var userScoreIndex = scoreList.FindIndex(x => x.mission_id == missionItemSObj.missionArray[i].mission_id);

                a_item.SetTitle(missionItemSObj.missionArray[i].mission_name, (userScoreIndex >= 0));
            }
        }

        private void ResetContent() {
            UserInfoText.text = "";
            TotalScoreText.text = "0%";

            Utility.UtilityMethod.ClearChildObject(AchievementHolder);
        }

    }
}