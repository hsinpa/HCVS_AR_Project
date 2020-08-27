using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Socket;
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

    [SerializeField]
    private Button GameStartBtn;

    [Header("Mission Info")]
    [SerializeField]
    private Text missionInfo;

    [Header("Info View")]
    [SerializeField]
    private GameObject infoView;

    [Header("Timer Text")]
    [SerializeField]
    private Text TimerText;

    private string participant = StringAsset.ClassInfo.Participant;
    private string averageScore = StringAsset.ClassInfo.AverageScore;

    private SocketIOManager _socketIOManager;

    private DateTime startTime = DateTime.MinValue;
    private DateTime endTime = DateTime.MinValue;

    private System.Action OnTimeUpEvent;

    void Start()
    {
        Setup();
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
        _socketIOManager = MainApp.Instance._socketManager;
       // _socketIOManager.socket.On(TypeFlag.SocketEvent.StartGame, OnGameStartSocketEvent);
        GameStartBtn.onClick.AddListener(() => SetTimerAndGameStart(DateTimeOffset.UtcNow.AddSeconds(40).ToUnixTimeMilliseconds()));
        MissionsClick();
    }
/*
    private void OnGameStartSocketEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (args.Length > 0)
        {
            var roomComps = JsonUtility.FromJson<TypeFlag.SocketDataType.RoomComponentType>(args[0].ToString());

            GameStartBtn.onClick.AddListener(() => SetTimerAndGameStart(40));//roomComps.end_time));
        }
    }
*/
    private void MissionsClick()
    {

        for (int i = 0; i < missionsButtons.Length; i++)
        {
            int closureIndex = i;
            missionsButtons[closureIndex].onClick.AddListener(() => ShowMissionInfo(closureIndex));
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
