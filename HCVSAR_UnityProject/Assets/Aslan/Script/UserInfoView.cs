using Expect.StaticAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class UserInfoView : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private Text UserInfoText;

    [SerializeField]
    private Text TotalScoreText;

    [Header("MissionInfo")]
    [SerializeField]
    public Transform missionContainer;
    public Transform missionInfo;

    private TypeFlag.UserType userType;

    [System.Serializable]
    public struct StudentType
    {
        public string student_id;
        public string mission_id;
        public int score;
    }

    private TypeFlag.SocketDataType.LoginDatabaseType loginData;
    private List<TypeFlag.SocketDataType.StudentDatabaseType> allStudentData;
    private List<StudentType> studentData;

    private bool isConnection;
    private string student_id;
    private string student_name;

    void Start()
    {
        PrepareStudentData();
        //PrepareScoreData(loginData.user_id);
    }

    void TryRegisterOnLoginEvent()
    {
        //var loginCtrl = MainApp.Instance.GetObserver<LoginCtrl>();

    }

    private void GetStudentInfoText(List<StudentType> studentData)
    {
        UserInfoText.text = string.Format("{0}, {1}\n{2}", student_name, student_id, isConnection);
        MissionInfo(studentData);
    }

    private void MissionInfo(List<StudentType> studentData)
    {
        float height = 60f;
        int totalScore = 0;
        string score;

        for (int i = 0; i < studentData.Count; i++)
        {
            if (studentData[i].score < 10)
                score = "0" + studentData[i].score.ToString();
            else
                score = studentData[i].score.ToString();

            Transform missionTransform = Instantiate(missionInfo, missionContainer);
            RectTransform missionRectTransform = missionTransform.GetComponent<RectTransform>();

            missionRectTransform.anchoredPosition = new Vector2(0, -height * i);
            missionTransform.Find("id").GetComponent<Text>().text = studentData[i].mission_id;
            missionTransform.Find("score").GetComponent<Text>().text = score;
            missionTransform.gameObject.SetActive(true);

            totalScore += studentData[i].score;
        }

        if (totalScore < 10)
            TotalScoreText.text = "0" + totalScore.ToString();
        else
            TotalScoreText.text = totalScore.ToString();
    }

    private void PrepareStudentData()
    {
        string getStudentURI = string.Format(StringAsset.API.GetAllStudentByID, 14343432, 1);
        Debug.Log(getStudentURI);

        StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getStudentURI), UnityWebRequest.kHttpVerbGET, null, (string json) => {
                if (string.IsNullOrEmpty(json))
                {
                    isConnection = false;
                    return;
                }

                var tempStudentData = JsonHelper.FromJson<TypeFlag.SocketDataType.StudentDatabaseType>(json);

                if (tempStudentData != null)
                {
                    isConnection = true;
                    allStudentData = tempStudentData.ToList();
                    student_id = allStudentData[2].id;
                    student_name = allStudentData[2].student_name;
                    PrepareScoreData(student_id); // put id
                }
            }, null));
    }

    private void PrepareScoreData(string student_id)
    {
        string getStudentURI = string.Format(StringAsset.API.GetStudentScore, student_id);

        StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getStudentURI), UnityWebRequest.kHttpVerbGET, null, (string json) => {
                if (string.IsNullOrEmpty(json))
                {
                    return;
                }

                var tempStudentData = JsonHelper.FromJson<StudentType>(json);

                if (tempStudentData != null)
                {
                    studentData = tempStudentData.ToList();
                    GetStudentInfoText(studentData);
                }

            }, null));
            
    }
}
