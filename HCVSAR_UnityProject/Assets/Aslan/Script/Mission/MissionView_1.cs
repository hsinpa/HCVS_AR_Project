using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;
using UnityEngine.Networking;


public class MissionView_1 : Singleton<MissionView_1>
{
    protected MissionView_1() { } // guarantee this will be always a singleton only - can't use the constructor!

    [SerializeField]
    private Button mission_1;
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite person;

    [SerializeField]
    EnterMissionView enterMissionView;
    [SerializeField]
    SituationMissionView situationMissionView;
    [SerializeField]
    DialogMissionView dialogMissionView;
    [SerializeField]
    QuestionMissionView questionMissionView;
    [SerializeField]
    EndMissionView endMissionView;

    FingerClickEvent fingerClick;

    private TypeFlag.InGameType.MissionType[] missionArray;
    private TypeFlag.SocketDataType.LoginDatabaseType loginData;
    private TypeFlag.SocketDataType.StudentType studentScoreData = new TypeFlag.SocketDataType.StudentType();
    private int clickCount;
    private bool isTimeUp;

    // Message
    string situationMessage = StringAsset.MissionsSituation.One.s1;
    string dogName = StringAsset.MissionsDialog.Person.dog;
    string peopleName = StringAsset.MissionsDialog.Person.people;
    string dogMessage = StringAsset.MissionsDialog.One.d1;
    string peopleMessage = StringAsset.MissionsDialog.One.d2;
    string[] historyMessage = { StringAsset.MissionsDialog.One.history1, StringAsset.MissionsDialog.One.history2,
                                 StringAsset.MissionsDialog.One.history3, StringAsset.MissionsDialog.One.history4};

    private string qustion = StringAsset.MissionsQustion.One.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.One.ans1, StringAsset.MissionsAnswer.One.ans2,
                                 StringAsset.MissionsAnswer.One.ans3, StringAsset.MissionsAnswer.One.ans4};
    private string correctMessage = StringAsset.MissionsQustion.One.correct;
    private string faultMessage = StringAsset.MissionsQustion.One.fault;

    private string endMessage = StringAsset.MissionsEnd.End.message;

    public enum ConvercestionType
    {
        Dialog_talk1, Dialog_talk2, History
    }
    
    ConvercestionType convercestionType;
    

    private void MissionArraySetUp()
    {
        missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
    }

    private void Awake()
    {
        loginData = MainView.Instance.loginData;
        studentScoreData.student_id = loginData.user_id;

        MissionArraySetUp();
        fingerClick = situationMissionView.GetComponentInParent<FingerClickEvent>();
    }

    private void Start()
    {
        mission_1.onClick.AddListener(MissionStart);
    }

    private void Update()
    {
        
    }

    private void MissionStart()
    {
        enterMissionView.Show(true);
        enterMissionView.EnterMission(missionArray[0].mission_name, missionArray[0].mission_name);
        enterMissionView.OnEnable += StarEnable;
        enterMissionView.OnDisable += Disable;
    }

    // TODO: ibeacon find other mission after 10 second
    void Disable()
    {
        Debug.Log("other thing");
    }

    void StarEnable()
    {
        enterMissionView.OnEnable -= StarEnable;
        enterMissionView.OnDisable -= Disable;
        enterMissionView.Show(false);

        situationMissionView.Show(true);
        situationMissionView.SituationView(situationMessage);
        
        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += ClickCount; // Add fingerClick event
    }

    void ClickCount()
    {
        clickCount++;
        Debug.Log("clickCount: " + clickCount);
        Convercestion();
        
    }

    void Convercestion()
    {
        if (clickCount == 1)
        {
            Debug.Log("clickCount1: " + clickCount);
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, dogMessage, dog);
        }
        if (clickCount == 2)
        {
            Debug.Log("clickCount2: " + clickCount);
            dialogMissionView.DialogView(peopleName, peopleMessage, person);
        }

        if (clickCount >= 3 && clickCount < historyMessage.Length+3)
        {
            Debug.Log("clickCount3: " + clickCount);
            dialogMissionView.DialogView(dogName, historyMessage[clickCount-3], dog);
        }

        if (clickCount == historyMessage.Length+3)
        {
            Debug.Log("Finish");
            Qusteion();
        }
    }

    private void Qusteion()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial

        dialogMissionView.Show(false);
        questionMissionView.Show(true);

        questionMissionView.QuestionView(qustion, answers, 1);
    }

    public void PostScore(int score, bool answerResult)
    {
        studentScoreData.mission_id = "A";
        studentScoreData.score = score;

        string jsonString = JsonUtility.ToJson(studentScoreData);

        StartCoroutine(
        APIHttpRequest.NativeCurl((StringAsset.GetFullAPIUri(StringAsset.API.PostStudentScore)), UnityWebRequest.kHttpVerbPOST, jsonString, (string success) => {
            Debug.Log("POST Success");
            MainView.Instance.PrepareScoreData(studentScoreData.student_id);
        }, () => {
                //TODO: ADD Mission ID
                Debug.Log("Error: POST Fail, Fail Mission: " + studentScoreData.mission_id);
        }));

        StartCoroutine(AnswerPauser(answerResult, score));
    }


    public IEnumerator AnswerPauser(bool result, int score)
    {
        yield return new WaitForSeconds(3);

        questionMissionView.Show(false);
        dialogMissionView.Show(true);

        if (result)
        {
            dialogMissionView.DialogView(dogName, correctMessage, dog);
        }
        else
        {
            dialogMissionView.DialogView(dogName, faultMessage, dog);
        }

        yield return new WaitForSeconds(5);

        dialogMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score);
        endMissionView.OnEnable += LeaveMission;
    }

    private void LeaveMission()
    {
        endMissionView.Show(false);
        Debug.Log("Mission 1 Leave");
    }
}
