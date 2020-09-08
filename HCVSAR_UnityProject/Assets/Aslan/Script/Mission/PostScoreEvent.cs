using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Expect.StaticAsset;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PostScoreEvent : Singleton<PostScoreEvent>
{
    public TypeFlag.SocketDataType.StudentType studentScoreData;
    public int score;
    public bool answerResult;

    public void PostScore(TypeFlag.SocketDataType.StudentType data, bool result)
    {
        studentScoreData = data;
        score = data.score;
        answerResult = result;

        string jsonString = JsonUtility.ToJson(data);

        StartCoroutine(
        APIHttpRequest.NativeCurl((StringAsset.GetFullAPIUri(StringAsset.API.PostStudentScore)), UnityWebRequest.kHttpVerbPOST, jsonString, (string success) => {
            Debug.Log("POST Success");
        }, () => {
            //TODO: ADD Mission ID
            Debug.Log("Error: POST Fail, Fail Mission: " + data.mission_id);
        }));
    }
}
