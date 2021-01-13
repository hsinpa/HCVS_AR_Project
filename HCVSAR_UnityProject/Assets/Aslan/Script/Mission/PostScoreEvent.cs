using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Expect.StaticAsset;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PostScoreEvent : Singleton<PostScoreEvent>
{
    public void PostScore(int score, TypeFlag.UserType type)
    {
        switch (type)
        {
            case TypeFlag.UserType.Guest:
                RefreshGuestScore(score);
                break;

            case TypeFlag.UserType.Student:
                PostSever(score);
                break;
        }
    }

    private void PostSever(int score)
    {
        TypeFlag.SocketDataType.StudentType studentScoreData = MainView.Instance.studentScoreData;
        studentScoreData.score = score;
        Debug.Log("PSOT studentScoreData id: " + studentScoreData.mission_id);

        string jsonString = JsonUtility.ToJson(studentScoreData);
        string log = string.Format("id: {0}, user: {1}, score: {2}", studentScoreData.mission_id, studentScoreData.student_id, studentScoreData.score);

        StartCoroutine(
        APIHttpRequest.NativeCurl((StringAsset.GetFullAPIUri(StringAsset.API.PostStudentScore)), UnityWebRequest.kHttpVerbPOST, jsonString, (string success) => {

            MainView.Instance.studentScoreData = studentScoreData;
            MainView.Instance.RefreshStudentData();
            Debug.Log("PSOT Success: " + log);

        }, () => {
            //TODO: ADD Mission ID
            Debug.Log("Error: POST Fail, Fail Mission: " + log + "PSOT studentScoreData id: " + studentScoreData.mission_id);
        }));
    }

    private void RefreshGuestScore(int score)
    {
        var guestMissionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
        guestMissionArray[MainView.Instance.missionNumber].total_score = score;

        MainApp.Instance.database.MissionShortNameObj.missionArray = guestMissionArray;
        MainView.Instance.guestMissionList.Add(MainView.Instance.studentScoreData.mission_id);
        MainView.Instance.GuestTotalScore();
        Debug.Log("post MainView.Instance.studentScoreData.mission_id  " + MainView.Instance.studentScoreData.mission_id + "score " + score + "MainView.Instance.missionNumber" + MainView.Instance.missionNumber);
    }
}
