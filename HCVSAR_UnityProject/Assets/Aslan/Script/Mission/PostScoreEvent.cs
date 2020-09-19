using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Expect.StaticAsset;
using UnityEngine.Networking;
using System.Collections.Generic;
using Expect.View;

public class PostScoreEvent : Singleton<PostScoreEvent>
{
    public TypeFlag.SocketDataType.StudentType studentScoreData = MainView.Instance.studentScoreData;

    public void PostScore(int score)
    {
        studentScoreData.score = score;

        string jsonString = JsonUtility.ToJson(studentScoreData);
        string log = string.Format("id: {0}, user: {1}, score: {2}", studentScoreData.mission_id, studentScoreData.student_id, studentScoreData.score);

        StartCoroutine(
        APIHttpRequest.NativeCurl((StringAsset.GetFullAPIUri(StringAsset.API.PostStudentScore)), UnityWebRequest.kHttpVerbPOST, jsonString, (string success) => {

            MainView.Instance.RefreshUserInfo();
            Debug.Log("PSOT Success: " + log);
            
        }, () => {
            //TODO: ADD Mission ID
            Debug.Log("Error: POST Fail, Fail Mission: " + log);
        }));
    }
}
