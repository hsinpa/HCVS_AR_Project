using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        private TypeFlag.InGameType.MissionType[] missionArray;
        private Dictionary<string, TypeFlag.InGameType.MissionType> missionLookupTable;
        private List<Transform> selectTransformList = new List<Transform>();

        private bool isConnection;

        public void UserInfoStart(TypeFlag.UserType type)
        {
            /*
            switch (type)
            {
                case TypeFlag.UserType.Guest:
                    GuestInfo();
                    break;

                case TypeFlag.UserType.Student:
                    StudentInfo();
                    break;
            }
            */
            StudentInfo(type);
            SwitchPanelController();
        }

        private void MissionArraySetUp()
        {
            missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
            missionLookupTable = MainApp.Instance.database.MissionShortNameObj.MissionTable;
        }

        private void StudentInfo(TypeFlag.UserType type)
        {
            string student_name = "Guest";// = MainView.Instance.loginData.user_id;
            string student_id = "None";// = MainView.Instance.loginData.username;
            //List<TypeFlag.SocketDataType.StudentType> studentData = MainView.Instance.studentData;

            //if (studentData == null) return;

            float height = 20f;
            string score;

            isConnection = true;

            MissionArraySetUp();

            for (int i = 0; i < missionArray.Length; i++)
            {
                Color green = new Color32(18, 255, 73, 255);

                Transform missionTransform = Instantiate(missionInfo, missionContainer);
                RectTransform missionRectTransform = missionTransform.GetComponent<RectTransform>();

                missionRectTransform.anchoredPosition = new Vector2(0, -height * i);

                switch (type)
                {
                    case TypeFlag.UserType.Student:
                        student_id = MainView.Instance.loginData.user_id;
                        student_name = MainView.Instance.loginData.username;
                        List<TypeFlag.SocketDataType.StudentType> studentData = MainView.Instance.studentData;

                        for (int j = 0; j < studentData.Count; j++)
                        {
                            if (missionArray[i].mission_id == studentData[j].mission_id)
                            {
                                missionTransform.Find("score").GetComponent<Text>().color = green;
                                missionTransform.Find("id").GetComponent<Text>().color = green;
                                missionArray[i].total_score = studentData[j].score;
                            }
                        }
                        break;

                    case TypeFlag.UserType.Guest:
                        missionTransform.Find("score").GetComponent<Text>().color = green;
                        missionTransform.Find("id").GetComponent<Text>().color = green;
                        break;
                }



                if (missionArray[i].total_score < 10)
                    score = "0" + missionArray[i].total_score.ToString();
                else
                    score = missionArray[i].total_score.ToString();

                missionTransform.Find("id").GetComponent<Text>().text = missionArray[i].mission_name;
                missionTransform.Find("score").GetComponent<Text>().text = score;

                selectTransformList.Add(missionTransform);
                missionTransform.gameObject.SetActive(true);
            }

            TotalScoreText.text = MainView.Instance.totalScoreString;
            isConnection = true ? status.sprite = statusOn : status.sprite = statusOff;

            UserInfoText.text = string.Format("{0}, {1}\n{2}", student_name, student_id, isConnection);
        }
        /*
        private void GuestInfo()
        {
            float height = 20f;
            string score;
            string student_name = "Guest";
            string student_id = "None";

            isConnection = true;

            MissionArraySetUp();

            for (int i = 0; i < missionArray.Length; i++)
            {
                Color green = new Color32(18, 255, 73, 255);

                Transform missionTransform = Instantiate(missionInfo, missionContainer);
                RectTransform missionRectTransform = missionTransform.GetComponent<RectTransform>();

                missionRectTransform.anchoredPosition = new Vector2(0, -height * i);
                missionTransform.Find("score").GetComponent<Text>().color = green;
                missionTransform.Find("id").GetComponent<Text>().color = green;

                if (missionArray[i].total_score < 10)
                    score = "0" + missionArray[i].total_score.ToString();
                else
                    score = missionArray[i].total_score.ToString();

                missionTransform.Find("id").GetComponent<Text>().text = missionArray[i].mission_name;
                missionTransform.Find("score").GetComponent<Text>().text = score;

                selectTransformList.Add(missionTransform);
                missionTransform.gameObject.SetActive(true);
            }

            TotalScoreText.text = MainView.Instance.totalScoreString;
            isConnection = true ? status.sprite = statusOn : status.sprite = statusOff;

            UserInfoText.text = string.Format("{0}, {1}\n{2}", student_name, student_id, isConnection);
        }
        */
        private void SwitchPanelController()
        {
            close.onClick.AddListener(() => {
                this.Show(false);
                mainBaseVIew.ClosePanel();
            });
        }

        public void RemoveShowData()
        {
            if (selectTransformList.Count > 0)
            {
                foreach (var t in selectTransformList) { Destroy(t.gameObject); }
                foreach (var t in selectTransformList) { Destroy(t.GetChild(0).gameObject); }
                selectTransformList.Clear();
            }
        }
    }
}
