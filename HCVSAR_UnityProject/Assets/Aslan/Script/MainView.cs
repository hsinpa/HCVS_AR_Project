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

public class MainView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    private Button[] missionsButtons;

    [Header("Mission Info")]
    [SerializeField]
    private Text missionInfo;

    [Header("Info View")]
    [SerializeField]
    private GameObject infoView;

    [Header("Timer Text")]
    [SerializeField]
    private Text TimerText;
    private Text ScoreText;

    private string participant = StringAsset.ClassInfo.Participant;
    private string averageScore = StringAsset.ClassInfo.AverageScore;

    private DateTime startTime = DateTime.MinValue;
    private DateTime endTime = DateTime.MinValue;

    private System.Action OnTimeUpEvent;
    private TypeFlag.InGameType.MissionType[] missionArray;
    private Dictionary<string, TypeFlag.InGameType.MissionType> missionLookupTable;
    //private StudentDataSave studentData; // replace bt SimpleDatabase

    // test
    private MissionItemSObj missionItemSObj;

    void Start()
    {
        Setup();
        //PrepareScoreData(loginData.user_id);
    }

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

    private void Setup()
    {
        MissionsClick();
        TryRegisterOnLoginEvent();

        missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
        missionLookupTable = MainApp.Instance.database.MissionShortNameObj.MissionTable;
    }

    void TryRegisterOnLoginEvent()
    {
        var loginCtrl = MainApp.Instance.GetObserver<LoginCtrl>();
        loginCtrl.OnLoginEvent += OnReceiveLoginEvent;
    }

    private void OnReceiveLoginEvent(TypeFlag.SocketDataType.LoginDatabaseType loginType, SocketIOManager _socketIOManager)
    {
        _socketIOManager.socket.On(TypeFlag.SocketEvent.StartGame, OnGameStartSocketEvent);

        /*
        // save student data to SimpleDatabase
        studentData.username = loginType.username;
        studentData.user_id = loginType.user_id;
        studentData.room_id = loginType.room_id;
        studentData.userType = loginType.userType;
        */
        Debug.Log("loginType.room_id" + loginType.room_id);
        //Debug.Log("studentData.username" + studentData.username);
        Debug.Log("missionItemSObj " + missionItemSObj.missionArray[0].total_score);
        
    }

    private void MissionsClick()
    {

        for (int i = 0; i < missionsButtons.Length; i++)
        {
            int closureIndex = i;
            missionsButtons[closureIndex].onClick.AddListener(() => ShowMissionInfo(closureIndex));
        }

    }

    private void OnGameStartSocketEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (args.Length > 0)
        {
            var roomComps = JsonUtility.FromJson<TypeFlag.SocketDataType.RoomComponentType>(args[0].ToString());
        }
    }

    public void SetTimerAndGameStart(long endTimestamp)
    {
        startTime = DateTime.UtcNow;
        endTime = DateTimeOffset.FromUnixTimeMilliseconds(endTimestamp).DateTime;
    }

    public void ResetTime()
    {
        startTime = DateTime.MinValue;
        endTime = DateTime.MinValue;

        TimerText.text = "00:00";
    }

    private void ShowMissionInfo(int index)
    {
        ShowClassScore("57838582", index);
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
        Debug.Log("missionViewInfo2 " + average[0].main_value + ", index " + index);

        missionViewInfo.Add(participant[index].main_value);
        missionViewInfo.Add(average[index].main_value);

        return missionViewInfo;
    }
}
