using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Expect.StaticAsset;
using UnityEngine.Networking;
using System.Linq;


public class PostButtonTest : MonoBehaviour
{
    public Mission_ID Mission_id;
    public enum Mission_ID { A, B, C, D, E, F, G, I, J, K }

    public int Score;

    private string user_id = "";
    private string totalScoreString;
    private Button postButton;
    private TypeFlag.SocketDataType.StudentType studentScoreData = new TypeFlag.SocketDataType.StudentType();
    private List<TypeFlag.SocketDataType.StudentType> studentData;
    private bool isRefresh;

    private void Awake()
    {
        postButton = this.GetComponent<Button>();
        user_id = MainView.Instance.loginData.user_id;
    }

    private void Start()
    {
        PostScore();
    }

    private void PostScore()
    {
        postButton.onClick.AddListener(PostScoreListener);
    }

    private void PostScoreListener()
    {
        studentScoreData.student_id = "763462";
        studentScoreData.mission_id = Mission_id.ToString();
        studentScoreData.score = Score;

        string jsonString = JsonUtility.ToJson(studentScoreData);

        StartCoroutine(
            APIHttpRequest.NativeCurl((StringAsset.GetFullAPIUri(StringAsset.API.PostStudentScore)), UnityWebRequest.kHttpVerbPOST, jsonString, (string success) => {
                Debug.Log("POST Success");
                MainView.Instance.RefreshScore(studentScoreData.student_id);
                RefreshScoreData();
            }, () => {
                //TODO: ADD Mission ID
                Debug.Log("Error: POST Fail");
            }));

        //StartCoroutine(PostScore(studentScoreData));
        //MainView.Instance.RefreshScore(user_id);
        //MainView.Instance.PrepareScoreData(user_id);
        //RefreshScoreData();
    }

    private void RefreshScoreData()
    {
        string getStudentURI = string.Format(StringAsset.API.GetStudentScore, user_id);
        string totalscore = "";

        StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getStudentURI), UnityWebRequest.kHttpVerbGET, null, (string json) => {
                if (string.IsNullOrEmpty(json))
                {
                    return;
                }

                var tempStudentData = JsonHelper.FromJson<TypeFlag.SocketDataType.StudentType>(json);

                if (tempStudentData != null)
                {
                    studentData = tempStudentData.ToList();

                    int totalScore = 0;
                    for (int i = 0; i < studentData.Count; i++)
                    {
                        totalScore += studentData[i].score;
                    }

                    if (totalScore < 10)
                    {
                        totalScoreString = "0" + totalScore.ToString();
                    }
                    else
                    {
                        totalScoreString = totalScore.ToString();
                    }

                    totalscore = totalScoreString;
                    Debug.Log("============================= 2totalScoreString" + totalScoreString);
                }

            }, null));

        //return totalscore;
    }
}
