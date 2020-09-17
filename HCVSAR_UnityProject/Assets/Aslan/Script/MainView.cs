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
using Expect.View;

public class MainView : Singleton<MainView>//MonoBehaviour
{
    protected MainView() { } // guarantee this will be always a singleton only - can't use the constructor!

    [Header("Buttons")]
    [SerializeField]
    private Button[] missionsButtons;
    [Header("Main Buttons")]
    [SerializeField]
    private Button user;
    [SerializeField]
    private Button bag;
    [SerializeField]
    private Button rank;
    [SerializeField]
    private Button connect;

    [Header("Mission Info")]
    [SerializeField]
    private Text missionInfo;
    [Header("Text")]
    [SerializeField]
    private Text TimerText;
    [SerializeField]
    private Text TotalScoreText;
    [SerializeField]
    private Text EndLocationText;

    [Header("Info View")]
    [SerializeField]
    private GameObject infoView;

    [Header("Image")]
    [SerializeField]
    private Image healthBar;
    private float maxHealth = 100;
    private float currentHealth = 0;

    [Header("Ibeacon")]
    [SerializeField]
    private GameObject ibeacon;

    [Header("Canvas Group")]
    [SerializeField]
    private CanvasGroup EndView;
    [SerializeField]
    private MainBaseVIew mainBaseVIew;

    [SerializeField]
    private BagPanel bagPanel;
    [SerializeField]
    private UserInfoView userInfoPanel;
    [SerializeField]
    private RankInfoView rankPanel;
    [SerializeField]
    private ConnectView connectPanel;

    private string participant = StringAsset.ClassInfo.Participant;
    private string averageScore = StringAsset.ClassInfo.AverageScore;

    [HideInInspector]
    public string totalScoreString;
    [HideInInspector]
    public List<TypeFlag.SocketDataType.StudentType> studentData;
    [HideInInspector]
    public TypeFlag.SocketDataType.LoginDatabaseType loginData;
    public TypeFlag.SocketDataType.StudentType studentScoreData;

    private TypeFlag.SocketDataType.ClassScoreHolderType classScore;

    private DateTime startTime = DateTime.MinValue;
    private DateTime endTime = DateTime.MinValue;
    private System.Action OnTimeUpEvent;
    private SocketIOManager _socketIOManager;
    private UserInfoView userInfo;

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
            this.GetComponent<CanvasGroup>().interactable = false;
        }

        
    }
    /*
    public bool CountDown(double addTime)
    {
        DateTime timeUp = DateTime.UtcNow.AddMinutes(addTime);
        TimeSpan t = timeUp - DateTime.UtcNow;

        if (t.Seconds < 0)
        {
            Debug.Log("Time Up");
            return true;
        }
        return false;
    }
    */

    public void Setup()
    {
        TryRegisterOnLoginEvent();
    }

    private void TryRegisterOnLoginEvent()
    {
        var loginCtrl = MainApp.Instance.GetObserver<LoginCtrl>();
        loginCtrl.OnLoginEvent += OnReceiveLoginEvent;

    }

    private void OnReceiveLoginEvent(TypeFlag.SocketDataType.LoginDatabaseType loginType, SocketIOManager socketIOManager)
    {
        if (loginType.user_id == null) return;
        loginData = loginType;

        if (_socketIOManager == null) {
            _socketIOManager = socketIOManager;

            _socketIOManager.socket.On(TypeFlag.SocketEvent.StartGame, OnGameStartSocketEvent);
            _socketIOManager.socket.On(TypeFlag.SocketEvent.TerminateGame, OnTerminateEvent);
        }
        
        StarGame(); //use for no teacher
    }

    public void PrepareClassScore(string class_id)
    {
        string getClassScoreURI = string.Format(StringAsset.API.GetClassScore, class_id);
        
        StartCoroutine(
        APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getClassScoreURI), UnityWebRequest.kHttpVerbGET, null, (string json) =>
        {
            classScore = JsonUtility.FromJson<TypeFlag.SocketDataType.ClassScoreHolderType>(json);
        }, null));
    }
    
    private void ShowMissionInfo(int index)
    {
        ShowClassScore(index);
    }

    private void ShowClassScore(int index)
    {
        _ = PrepareMissionInfo(classScore, index);
    }

    private async Task PrepareMissionInfo(TypeFlag.SocketDataType.ClassScoreHolderType classScoreHolder, int index)
    {
        var participantValue = await Task.Run(() => PrepareParticipantValue(classScoreHolder, index));
        var averageScoreVlaue = await Task.Run(() => PrepareAveragetValue(classScoreHolder, index));

        missionInfo.text = string.Format("{0}: {1}\n{2}: {3}", participant, participantValue, averageScore, averageScoreVlaue);
        infoView.SetActive(true);
    }

    private float PrepareParticipantValue(TypeFlag.SocketDataType.ClassScoreHolderType data, int index)
    {
        var participant = data.participant_count.ToList();

        return participant[index].main_value;
    }

    private float PrepareAveragetValue(TypeFlag.SocketDataType.ClassScoreHolderType data, int index)
    {
        var average = data.average_score.ToList();

        return average[index].main_value;
    }

    // End Game Event
    void OnTerminateEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (args.Length > 0)
        {

            var terminateData = JsonUtility.FromJson<TypeFlag.SocketDataType.TerminateGameType>(args[0].ToString());
            TerminateGameAction(terminateData.room_id, terminateData.location_id);
        }
    }

    public void TerminateGameAction(string class_id, string location)
    {
        this.GetComponent<CanvasGroup>().interactable = false;
        EndView.alpha = 1;
        EndLocationText.text = location;
    }

    // Start Sock Event
    private void OnGameStartSocketEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (args.Length > 0)
        {
            var roomComps = JsonUtility.FromJson<TypeFlag.SocketDataType.RoomComponentType>(args[0].ToString());
            SetTimerAndGameStart(roomComps.end_time);
        }

        //StarGame();  //use for Listen teacher
    }

    private void StarGame()
    {
        // UI
        this.GetComponent<CanvasGroup>().interactable = true;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        EndView.alpha = 0;
        // id
        studentScoreData.student_id = loginData.user_id;

        // ibeacon open
        ibeacon.SetActive(true);

        MissionsClick();
        MainButtonClick();

        PrepareScoreData(loginData.user_id);
        PrepareClassScore(loginData.room_id);
    }

    public void SetTimerAndGameStart(long endTimestamp)
    {
        startTime = DateTime.UtcNow;
        endTime = DateTimeOffset.FromUnixTimeMilliseconds(endTimestamp).DateTime;
    }

    // UserInfo Score Data
    public void PrepareScoreData(string id)
    {
        string getStudentURI = string.Format(StringAsset.API.GetStudentScore, id);

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

    private void GetTotalScore(List<TypeFlag.SocketDataType.StudentType> studentData)
    {
        int totalScore = 0;

        for (int i = 0; i < studentData.Count; i++)
        {
            totalScore += studentData[i].score;
        }

        RefreshHealthBar(totalScore);

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
        
        userInfo.GetStudentInfoText(studentData);
        
        Debug.Log("=============================totalScoreString" + totalScoreString);
    }

    private void RefreshHealthBar(int score)
    {
        currentHealth = score / maxHealth;
        healthBar.fillAmount = currentHealth;
    }

    private void MissionsClick()
    {
        for (int i = 0; i < missionsButtons.Length; i++)
        {
            int closureIndex = i;
            missionsButtons[closureIndex].onClick.AddListener(() => ShowMissionInfo(closureIndex));
        }

    }

    private void MainButtonClick()
    {
        user.onClick.AddListener(() => {
            mainBaseVIew.ShowPanel();
            userInfoPanel.Show(true);
            userInfoPanel.UserInfoStart();
        });

        bag.onClick.AddListener(() => {
            bagPanel.Show(true);
            mainBaseVIew.ShowPanel();
        });

        rank.onClick.AddListener(() => {
            mainBaseVIew.ShowPanel();
            rankPanel.Show(true);
            rankPanel.RankInfoStart();
        });

        connect.onClick.AddListener(() => {
            connectPanel.Show(true);
            mainBaseVIew.ShowPanel();
        });
    }
}
