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
    [SerializeField]
    private GameObject CameraButton;
    [SerializeField]
    private GameObject MainButtons;

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
    [SerializeField]
    private Image missionImage;
    [SerializeField]
    private Sprite[] missionSprite;
    [Header("Text")]
    [SerializeField]
    private Text TimerText;
    [SerializeField]
    private Text TotalScoreText;
    [SerializeField]
    private Text RoomNameText;
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

    [Header("Canvas Group")]
    [SerializeField]
    private CanvasGroup EndView;
    [SerializeField]
    private MainBaseVIew mainBaseVIew;
    [SerializeField]
    private CanvasGroup closeEnterMissionView;

    [SerializeField]
    private BagPanel bagPanel;
    [SerializeField]
    private UserInfoView userInfoPanel;
    [SerializeField]
    private RankInfoView rankPanel;
    [SerializeField]
    private ConnectView connectPanel;

    private TypeFlag.InGameType.MissionType[] missionArray;
    private string participant = StringAsset.ClassInfo.Participant;
    private string averageScore = StringAsset.ClassInfo.AverageScore;

    [HideInInspector]
    public string totalScoreString;
    [HideInInspector]
    public List<TypeFlag.SocketDataType.StudentType> studentData;
    [HideInInspector]
    public TypeFlag.SocketDataType.LoginDatabaseType loginData;
    [HideInInspector]
    public bool isEndMissionOpen = true;

    public int missionNumber;

    public TypeFlag.SocketDataType.StudentType studentScoreData;
    public GameObject endMission;
    public Image warnImage;
    public StarVideoController starVideoController;
    public List<string> guestMissionList = new List<string>(); // save guest mission id

    private TypeFlag.InGameType.MissionType[] guestMissionArray;
    private TypeFlag.SocketDataType.ClassScoreHolderType classScore;

    private DateTime startTime = DateTime.MinValue;
    private DateTime endTime = DateTime.MinValue;
    private System.Action OnTimeUpEvent;
    private SocketIOManager _socketIOManager;
    private bool isEndEvent;

    
    void Start()
    {
        Setup();
    }
    
    private void Update()
    {
        if (endTime == DateTime.MinValue) return;

        TimeSpan t = endTime - DateTime.UtcNow;
        string minute = t.Minutes.ToString();
        string second = t.Seconds.ToString();

        if (t.Minutes < 10) { minute = "0" + t.Minutes.ToString(); }
        if (t.Seconds < 10) { second = "0" + t.Seconds.ToString(); }

        TimerText.text = string.Format("{0}:{1}", minute, second);

        if (t.Seconds < 0 || isEndEvent)
        {
            Debug.Log("Time up");

            if (OnTimeUpEvent != null) OnTimeUpEvent();

            endTime = DateTime.MinValue;
            mainBaseVIew.PanelController(true);
        }

        
    }

    public void Setup()
    {
        TryRegisterOnLoginEvent();
        InitSet();
    }

    private void TryRegisterOnLoginEvent()
    {
        var loginCtrl = MainApp.Instance.GetObserver<LoginCtrl>();
        loginCtrl.OnLoginEvent += OnReceiveLoginEvent;
    }

    private void InitSet()
    {
        SwitchLoginButton(false);
        warnImage.enabled = false;        
    }

    private void OnReceiveLoginEvent(TypeFlag.SocketDataType.LoginDatabaseType loginType, SocketIOManager socketIOManager)
    {
        if (loginType.user_id == null && loginType.userType == TypeFlag.UserType.Student) return;
        loginData = loginType;

        if (_socketIOManager == null) {
            _socketIOManager = socketIOManager;

            _socketIOManager.socket.On(TypeFlag.SocketEvent.StartGame, OnGameStartSocketEvent);
            _socketIOManager.socket.On(TypeFlag.SocketEvent.TerminateGame, OnTerminateEvent);
        }

        if (loginType.userType == TypeFlag.UserType.Guest)
        {
            InitGuestMissionScore();
            StarGame(loginType.userType);
        }
        //StarGame(loginType.userType); //use for no teacher
    }

    private void InitGuestMissionScore()
    {
        guestMissionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;

        for(int i = 0; i < 10; i++)
        {
            guestMissionArray[i].total_score = 0;
        }

        MainApp.Instance.database.MissionShortNameObj.missionArray = guestMissionArray;
    }

    private void SwitchLoginButton(bool isLogin)
    {
        CameraButton.SetActive(!isLogin);
        MainButtons.SetActive(isLogin);
    }

    public void PrepareClassScore(string class_id, int index)
    {
        string getClassScoreURI = string.Format(StringAsset.API.GetClassScore, class_id);
        
        StartCoroutine(
        APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getClassScoreURI), UnityWebRequest.kHttpVerbGET, null, (string json) =>
        {
            classScore = JsonUtility.FromJson<TypeFlag.SocketDataType.ClassScoreHolderType>(json);
            _ = PrepareMissionInfo(classScore, index);
        }, null));
    }
    
    private void ShowMissionInfo(int index, TypeFlag.UserType type)
    {        
        switch (type)
        {
            case TypeFlag.UserType.Guest:
                ShowGuestScore(index);
                break;

            case TypeFlag.UserType.Student:
                ShowClassScore(index);
                break;
        }
        
    }

    private void ShowGuestScore(int index)
    {
        string participantValue = "- -";
        string averageScoreVlaue = "- -";

        missionInfo.text = string.Format("{0}: {1}\n{2}: {3}", participant, participantValue, averageScore, averageScoreVlaue);
        missionImage.sprite = missionSprite[index];
        infoView.SetActive(true);
    }

    private void ShowClassScore(int index)
    {
        // get student data
        PrepareClassScore(loginData.room_id, index);
        
    }

    private async Task PrepareMissionInfo(TypeFlag.SocketDataType.ClassScoreHolderType classScoreHolder, int index)
    {
        missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;

        var participantValue = await Task.Run(() => PrepareParticipantValue(classScoreHolder, index));
        var averageScoreVlaue = await Task.Run(() => PrepareAveragetValue(classScoreHolder, index));

        missionInfo.text = string.Format("{0}: {1}\n{2}: {3}", participant, participantValue, averageScore, averageScoreVlaue);
        missionImage.sprite = missionSprite[index];
        infoView.SetActive(true);
    }

    private float PrepareParticipantValue(TypeFlag.SocketDataType.ClassScoreHolderType data, int index)
    {
        var participant = data.participant_count.ToList();
        int onlineScoreIndex = participant.FindIndex(x => x.mission_id == missionArray[index].mission_id);

        float mainValue = 0;

        if (onlineScoreIndex >= 0) { mainValue = participant[onlineScoreIndex].main_value; }

        return mainValue;
    }

    private float PrepareAveragetValue(TypeFlag.SocketDataType.ClassScoreHolderType data, int index)
    {
        var average = data.average_score.ToList();
        int onlineScoreIndex = average.FindIndex(x => x.mission_id == missionArray[index].mission_id);

        float mainValue = 0;

        if (onlineScoreIndex >= 0) { mainValue = average[onlineScoreIndex].main_value; }

        return mainValue;
    }

    // End Game Event
    void OnTerminateEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (args.Length > 0)
        {
            var terminateData = JsonUtility.FromJson<TypeFlag.SocketDataType.TerminateGameType>(args[0].ToString());
            TerminateGameAction(terminateData.location_id);
            JoeGM.joeGM.isGameStart = false;
            closeEnterMissionView.alpha = 0;
        }
    }

    public void TerminateGameAction(string location)
    {
        var missionLookupTable = MainApp.Instance.database.MissionShortNameObj.MissionTable;
        string missionName = missionLookupTable.Single(s => s.Key == location).Value.mission_name;

        this.GetComponent<CanvasGroup>().interactable = false;
        EndView.alpha = 1;        

        EndLocationText.text = missionName;
        isEndEvent = true;
    }

    // Start Sock Event
    private void OnGameStartSocketEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (args.Length > 0)
        {
            var roomComps = JsonUtility.FromJson<TypeFlag.SocketDataType.RoomComponentType>(args[0].ToString());
            SetTimerAndGameStart(roomComps.end_time);
            isEndEvent = false;
        }

        if (loginData.userType == TypeFlag.UserType.Student) { StarGame(loginData.userType); }//use for Listen teacher
    }

    private void StarGame(TypeFlag.UserType type)
    {
        // Video
        HasPlayStarVideo();

        // UI
        this.GetComponent<CanvasGroup>().interactable = true;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        EndView.alpha = 0;
        endMission.SetActive(false); // open score =>= 70

        

        // switch button
        SwitchLoginButton(true);

        MissionsClick();
        MainButtonClick();

        switch (type)
        {
            case TypeFlag.UserType.Guest:
                RoomNameText.text = "Guest";
                GuestTotalScore();
                break;

            case TypeFlag.UserType.Student:

                // id
                studentScoreData.student_id = loginData.user_id;
                RoomNameText.text = string.Format("{0}, {1}", loginData.username, loginData.user_id);

                // get student data
                PrepareScoreData(loginData.user_id);
                //PrepareClassScore(loginData.room_id);

                break;
        }
        // ibeacon open
        JoeGM.joeGM.StartBeacom(type);
    }

    public void SetTimerAndGameStart(long endTimestamp)
    {
        startTime = DateTime.UtcNow;
        endTime = DateTimeOffset.FromUnixTimeMilliseconds(endTimestamp).DateTime;
    }

    private void PrepareScoreData(string id)
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
                    StudentTotalScore(studentData);
                    GetAirplaneSkin(studentData); // ask airplane skin

                    JoeGM.joeGM.UpdateIBeaconStudent(); //update ibeacon missions
                }
                
            }, null));
    }

    private void StudentTotalScore(List<TypeFlag.SocketDataType.StudentType> studentData)
    {
        int totalScore = 0;

        for (int i = 0; i < studentData.Count; i++)
        {
            totalScore += studentData[i].score;
        }

        RefreshHealthBar(totalScore);

        if (totalScore >= 70)
        {
            endMission.SetActive(true);
            isEndMissionOpen = true;
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

    public void GuestTotalScore()
    {
        int totalScore = 0;

        for (int i = 0; i < 10; i++)
        {
            totalScore += guestMissionArray[i].total_score;
        }

        RefreshHealthBar(totalScore);

        if (totalScore >= 70)
        {
            endMission.SetActive(true);
            isEndMissionOpen = true;
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

    private void GetAirplaneSkin(List<TypeFlag.SocketDataType.StudentType> studentData)
    {
        string airplaneMark = "MAP_BONUS";
        bool hasAirplane = studentData.Exists(d => d.mission_id == airplaneMark);
        int getAirplane = hasAirplane ? 1 : 0;
        PlayerPrefs.SetInt("HAS_AIRPLANE_SKIN", getAirplane);
    }

    private void HasPlayStarVideo()
    {
        starVideoController.StartPlay();
    }

    private void RefreshHealthBar(int score)
    {
        currentHealth = score / maxHealth;
        healthBar.fillAmount = currentHealth;
    }

    // UserInfo Score Data
    public void RefreshStudentData()
    {
        PrepareScoreData(loginData.user_id);
    }

    private void MissionsClick()
    {
        for (int i = 0; i < missionsButtons.Length; i++)
        {
            int closureIndex = i;
            missionsButtons[closureIndex].onClick.AddListener(() => ShowMissionInfo(closureIndex, loginData.userType));
        }

    }    

    private void MainButtonClick()
    {
        user.onClick.AddListener(() => {
            mainBaseVIew.PanelController(true);
            userInfoPanel.Show(true);
            userInfoPanel.UserInfoStart(loginData.userType);
        });

        bag.onClick.AddListener(() => {
            mainBaseVIew.PanelController(true);
            bagPanel.Show(true);
        });

        rank.onClick.AddListener(() => {
            mainBaseVIew.PanelController(true);
            rankPanel.Show(true);
            rankPanel.RankInfoStart();
        });

        connect.onClick.AddListener(() => {
            mainBaseVIew.PanelController(true);
            connectPanel.Show(true);
            connectPanel.ConnectStart();
        });
    }
}
