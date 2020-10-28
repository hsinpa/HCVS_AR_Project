using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class SeverInfo : MonoBehaviour
{
    [System.Serializable]
    public class User_Info {
        public Text score;
        public Text name;
        public Text student_class;
        public Text student_id;
        public Text status1;
    }

    [System.Serializable]
    public class User_RoomScore
    {
        public Text missionScore1;
        public Text missionScore2;
        public Text missionScore3;
        public Text missionScore4;
        public Text missionScore5;
        public Text missionScore6;
        public Text missionScore7;
        public Text missionScore8;
        public Text missionScore9;
        public Text missionScore10;
    }

    [Header("Main Info")]
    public Text time;

    [Header("Button Info")]
    public User_Info user_info;
    public User_RoomScore user_room;

    public struct LoginDatabaseType
    {
        public string username;
        public string user_id;
        public string class_id;

        public TypeFlag.UserType userType;
    }
    public struct StudentDatabaseType
    {
        public string status;
        public string student_id;
        public string mission_id;
        public string score;
    }

    StudentDatabaseType studentDatabaseType = new StudentDatabaseType();

    void Start()
    {
        StartCoroutine(GetRequest("http://34.82.74.32:81/getStudentScore/124125"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            studentDatabaseType.student_id = pages[page];

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + "Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceive: " + webRequest.downloadHandler.text);

                string[] userStatus = webRequest.downloadHandler.text.Split(new char[] {'{','"',':', ','});
                string[] resultSplit = webRequest.downloadHandler.text.Split(new char[] {'{','}'});
                string[] result = new string[resultSplit.Length / 2];

                if (userStatus.Length > 0)
                {
                    /*
                    for (int i = 0; i <= 6; i++){
                        Debug.Log("userStatus" + i + ":" + userStatus[i]); //each split
                    }*/
                    studentDatabaseType.status = userStatus[4];
                }

                if (resultSplit.Length > 0)
                {
                    for (int i = 0; i < resultSplit.Length/2 - 1; i++)
                    {
                        int j = i + 1;
                        result[i] = resultSplit[j * 2];
                        Debug.Log("result" + i + ":" + result[i]); //each split
                    }
                }

                if(result.Length > 0)
                {
                    for (int i = 0; i < result.Length - 1; i++)
                    {
                        string[] s_split = result[i].Split(new char[] { '\\','"', ',' , ':' });
                        
                        for (int a = 0; a < s_split.Length; a++)
                        {
                            Debug.Log( a + " " + s_split[a]);
                        }
                        if(s_split[7] == studentDatabaseType.student_id)
                        {
                            studentDatabaseType.mission_id = s_split[17]; //mission
                            studentDatabaseType.score = s_split[25]; //score
                        }
                    }
                }
            }
        }
    }
}
