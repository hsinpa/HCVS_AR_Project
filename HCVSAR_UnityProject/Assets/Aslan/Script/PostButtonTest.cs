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

        StartCoroutine(PostScore(studentScoreData));
        MainView.Instance.RefreshScore(user_id);
        //MainView.Instance.PrepareScoreData(user_id);
    }

    public static IEnumerator PostScore(TypeFlag.SocketDataType.StudentType formData)
    {
        string url = StringAsset.GetFullAPIUri(StringAsset.API.PostStudentScore);
        string jsonString = JsonUtility.ToJson(formData);
        Debug.Log("studentScoreData.mission_id " + formData.mission_id);

        Debug.Log("url: " + url);
        UnityWebRequest webPost = UnityWebRequest.Post(url, jsonString);


        if (jsonString != null)
        {
            webPost.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonString));
            webPost.method = UnityWebRequest.kHttpVerbPOST;
            webPost.uploadHandler.contentType = "application/json";
        }
        
        yield return webPost.SendWebRequest();

        if (webPost.isNetworkError || webPost.isHttpError)
        {
            Debug.Log("webPost.error " + webPost.error);
        }
        else
        {
            Debug.Log("Form upload complete!");            
        }
    }

    private string RefreshScoreData1(string id)
    {
        string getStudentURI = string.Format(StringAsset.API.GetStudentScore, id);
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
                }

            }, null));

        return totalscore;
    }
}
