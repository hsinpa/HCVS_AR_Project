using Expect.StaticAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using Hsinpa.Model;
using Hsinpa.View;

namespace Expect.View
{
    public class UserInfoView : BaseView
    {
        [Header("Text")]
        [SerializeField]
        private Text UserInfoText;

        [SerializeField]
        private Text TotalScoreText;

        [Header("MissionInfo")]
        public Transform missionContainer;
        public Transform missionInfo;

        [Header("Status")]
        public Image status;
        public Sprite statusOn;
        public Sprite statusOff;

        [Header("SiwtchPanel")]
        public Button close;
        public MainBaseVIew mainBaseVIew;

        private List<TypeFlag.SocketDataType.StudentType> studentData;
        private TypeFlag.InGameType.MissionType[] missionArray;
        private Dictionary<string, TypeFlag.InGameType.MissionType> missionLookupTable;

        private bool isConnection;
        private string student_id;
        private string student_name;

        private void GetData()
        {
            studentData = MainView.Instance.studentData;
            student_id = MainView.Instance.loginData.user_id;
            student_name = MainView.Instance.loginData.username;
        }

        public void UserInfoStart()
        {
            GetData();

            GetStudentInfoText(studentData);
            SwitchPanelController();
            //GetData();
            Debug.Log("user info");
        }

        private void SwitchPanelController()
        {
            close.onClick.AddListener(() => {
                this.Show(false);
                mainBaseVIew.ClosePanel();
            });
        }

        private void MissionArraySetUp()
        {
            missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
            missionLookupTable = MainApp.Instance.database.MissionShortNameObj.MissionTable;
        }

        public void GetStudentInfoText(List<TypeFlag.SocketDataType.StudentType> studentData)
        {
            MissionArraySetUp();

            if (studentData == null) return;

            float height = 20f;
            string score;

            isConnection = true;

            for (int i = 0; i < missionArray.Length; i++)
            {
                Color green = new Color32(18, 255, 73, 255);

                Transform missionTransform = Instantiate(missionInfo, missionContainer);
                RectTransform missionRectTransform = missionTransform.GetComponent<RectTransform>();

                missionRectTransform.anchoredPosition = new Vector2(0, -height * i);

                for (int j = 0; j < studentData.Count; j++)
                {
                    if (missionArray[i].mission_id == studentData[j].mission_id)
                    {
                        missionTransform.Find("score").GetComponent<Text>().color = green;
                        missionTransform.Find("id").GetComponent<Text>().color = green;
                        missionArray[i].total_score = studentData[j].score;
                    }
                }

                if (missionArray[i].total_score < 10)
                    score = "0" + missionArray[i].total_score;
                else
                    score = missionArray[i].total_score.ToString();

                missionTransform.Find("id").GetComponent<Text>().text = missionArray[i].mission_name;
                missionTransform.Find("score").GetComponent<Text>().text = score;
                missionTransform.gameObject.SetActive(true);
            }

            TotalScoreText.text = MainView.Instance.totalScoreString;
            isConnection = true ? status.sprite = statusOn : status.sprite = statusOff;

            UserInfoText.text = string.Format("{0},{1}\n{2}", student_name, student_id, isConnection);
        }
    }
}
