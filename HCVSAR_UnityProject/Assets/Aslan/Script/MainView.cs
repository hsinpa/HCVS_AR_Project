using System.Collections;
using Hsinpa.Controller;
using Hsinpa.Socket;
using Hsinpa.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Linq;
using System;
using BestHTTP.SocketIO;

public class MainView : Singleton<MainView>//MonoBehaviour
{
    protected MainView() { } // guarantee this will be always a singleton only - can't use the constructor!

    [Header("Buttons")]
    [SerializeField]
    private Button[] missionsButtons;

    [Header("Mission Info")]
    [SerializeField]
    private Text missionInfo;

    [Header("Info View")]
    [SerializeField]
    private GameObject infoView;

    [Header("Text")]
    [SerializeField]
    private Text TimerText;
    [SerializeField]
    private Text TotalScoreText;

    private string participant = StringAsset.ClassInfo.Participant;
    private string averageScore = StringAsset.ClassInfo.AverageScore;

    public string totalScoreString;
    public List<TypeFlag.SocketDataType.StudentType> studentData;
    public TypeFlag.SocketDataType.LoginDatabaseType loginData;

    private DateTime startTime = DateTime.MinValue;
    private DateTime endTime = DateTime.MinValue;
    //private System.Action OnTimeUpEvent;

    void Start()
    {
        Setup();
    }

    /*
    private void Update()
    {
        if (endTime == DateTime.MinValue) return;

        TimeSpan t = endTime - DateTime.UtcNow;

        TimerText.text = string.Format("{0}:{1}", t.Minutes, t.Seconds);

        if (t.Seconds < 0)
        {
            Debug.Log("Teacher : Time up");

            if (OnTimeUpEvent != null) OnTimeUpEvent();

            endTime = DateTime.MinValue;
        }
    }
    */

    private void Setup()
    {
        TryRegisterOnLoginEvent();
    }

    void TryRegisterOnLoginEvent()
    {
        var loginCtrl = MainApp.Instance.GetObserver<LoginCtrl>();
        loginCtrl.OnLoginEvent += OnReceiveLoginEvent;
    }

    private void OnReceiveLoginEvent(TypeFlag.SocketDataType.LoginDatabaseType loginType, SocketIOManager _socketIOManager)
    {
        if (loginType.user_id == null) return;

        _socketIOManager.socket.On(TypeFlag.SocketEvent.StartGame, OnGameStartSocketEvent);

        loginData = loginType;
        MissionsClick();
        PrepareScoreData();
    }

    private void MissionsClick()
    {

        for (int i = 0; i < missionsButtons.Length; i++)
        {
            int closureIndex = i;
            missionsButtons[closureIndex].onClick.AddListener(() => ShowMissionInfo(closureIndex));
        }

    }

    private void GetTotalScore(List<TypeFlag.SocketDataType.StudentType> studentData)
    {
        int totalScore = 0;
        for (int i = 0; i < studentData.Count; i++)
        {
            totalScore += studentData[i].score;
        }

        if (totalScore < 10)
        {
            totalScoreString = "0" + totalScore.ToString();
            TotalScoreText.text = totalScoreString;
        }
        else
        {
            totalScoreString = totalScore.ToString();
            TotalScoreText.text = totalScoreString;
        }
    }

    private void ShowMissionInfo(int index)
    {
        ShowClassScore(loginData.room_id, index);
        infoView.SetActive(true);

        Debug.Log("index " + index);
    }

    private void ShowClassScore(string class_id, int index)
    {
        string getClassScoreURI = string.Format(StringAsset.API.GetClassScore, class_id);

        StartCoroutine(
        APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getClassScoreURI), UnityWebRequest.kHttpVerbGET, null, (string json) =>
        {
            var classScoreHolder = JsonUtility.FromJson<TypeFlag.SocketDataType.ClassScoreHolderType>(json);
            _ = PrepareMissionInfo(classScoreHolder, index);
        }, null));
    }

    private async Task PrepareMissionInfo(TypeFlag.SocketDataType.ClassScoreHolderType classScoreHolder, int index)
    {
        var missionViewInfo = await Task.Run(() => PrepareSelectedDataset(classScoreHolder, index));
        missionInfo.text = string.Format("{0}: {1}\n{2}: {3}", participant, missionViewInfo[0], averageScore, missionViewInfo[1]);
        infoView.SetActive(true);
    }

    private ArrayList PrepareSelectedDataset(TypeFlag.SocketDataType.ClassScoreHolderType data, int index)
    {
        ArrayList missionViewInfo = new ArrayList();
        var participant = data.participant_count.ToList();
        var average = data.average_score.ToList();

        missionViewInfo.Add(participant[index].main_value);
        missionViewInfo.Add(average[index].main_value);

        return missionViewInfo;
    }

    // Time Sock Event
    private void OnGameStartSocketEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (args.Length > 0)
        {

            var roomComps = JsonUtility.FromJson<TypeFlag.SocketDataType.RoomComponentType>(args[0].ToString());
            SetTimerAndGameStart(roomComps.end_time);
        }
    }

    public void SetTimerAndGameStart(long endTimestamp)
    {
        startTime = DateTime.UtcNow;
        endTime = DateTimeOffset.FromUnixTimeMilliseconds(endTimestamp).DateTime;
    }

    // UserInfo Score Data
    public void PrepareScoreData()
    {
        string getStudentURI = string.Format(StringAsset.API.GetStudentScore, loginData.user_id);

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
                    GetTotalScore(studentData);
                }
                
            }, null));
    }
}
