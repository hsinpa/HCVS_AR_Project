using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_6 : MonoBehaviour
{
    [SerializeField]
    private Button mission_6;
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite npc;

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

    // Message
    string situationMessage = StringAsset.MissionsSituation.Six.s1;
    string dogName = StringAsset.MissionsDialog.Person.dog;
    string npcName = StringAsset.MissionsDialog.Person.NPC_1;
    string npcMessage_1 = StringAsset.MissionsDialog.Six.d1;


    string[] historyMessage = { StringAsset.MissionsDialog.Three.history1, StringAsset.MissionsDialog.Three.history2, StringAsset.MissionsDialog.Three.history3 };

    private string qustion = StringAsset.MissionsQustion.Six.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.Six.ans1, StringAsset.MissionsAnswer.Six.ans2,
                                 StringAsset.MissionsAnswer.Six.ans3, StringAsset.MissionsAnswer.Six.ans4};

    private string correctMessage_1 = StringAsset.MissionsDialog.Six.correct_1;
    private string correctMessage_2 = StringAsset.MissionsDialog.Six.correct_2;
    private string faultMessage_1 = StringAsset.MissionsDialog.Six.fault_1;
    private string faultMessage_2 = StringAsset.MissionsDialog.Six.fault_2;
    private string faultMessage_3 = StringAsset.MissionsSituation.Six.fault;

    private string endMessage = StringAsset.MissionsEnd.End.message;


    private void MissionArraySetUp()
    {
        missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
    }

    private void Awake()
    {
        loginData = MainView.Instance.loginData;
        studentScoreData.student_id = loginData.user_id;
        studentScoreData.mission_id = "F";

        MissionArraySetUp();
        fingerClick = situationMissionView.GetComponentInParent<FingerClickEvent>();
    }

    private void Start()
    {
        mission_6.onClick.AddListener(MissionStart);
    }

    private void MissionStart()
    {
        enterMissionView.Show(true);
        enterMissionView.EnterMission(missionArray[5].mission_name, missionArray[5].mission_name);
        enterMissionView.OnEnable += StarEnable;
        enterMissionView.OnDisable += Disable;
    }

    // TODO: ibeacon find other mission after 10 second
    private void Disable()
    {
        Debug.Log("other thing");
    }

    private void StarEnable()
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
        Convercestion();
        Debug.Log("clickCount: " + clickCount);
    }

    void Convercestion()
    {
        if (clickCount == 1)
        {
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(npcName, npcMessage_1, npc);
        }

        if (clickCount == 2)
        {
            Debug.Log("Finish");
            Qusteion();
        }
    }

    private void Qusteion()
    {
        fingerClick.boxCollider.enabled = false; // end fingerClick trigger
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial

        dialogMissionView.Show(false);
        questionMissionView.Show(true);

        questionMissionView.QuestionView(qustion, answers, 0);
        questionMissionView.buttonClick += OpenClickEvent;
    }

    private void OpenClickEvent()
    {
        questionMissionView.Show(false);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, correctMessage_1, dog);

        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += QuestionReult; // Add fingerClick event
    }

    private void QuestionReult()
    {
        clickCount++;

        int score = MainView.Instance.studentScoreData.score;
        int number;  

        if (score > 0)
        {
            number = 2;
            Debug.Log("clickCount: " + clickCount);
            if (clickCount == 1)
            {
                dialogMissionView.DialogView(npcName, correctMessage_2, npc);
            }
            if (clickCount >= number && clickCount < historyMessage.Length + number)
            {
                Debug.Log("clickCount3: " + clickCount);
                dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
            }

            if (clickCount == historyMessage.Length + number)
            {
                Debug.Log("Finish");
                LeaveMission(score);
            }

        }
        else
        {
            number = 4;

            if (clickCount == 1)
            {
                dialogMissionView.DialogView(npcName, faultMessage_1, npc);
            }
            if (clickCount == 2)
            {
                dialogMissionView.DialogView(dogName, faultMessage_2, dog);
            }
            if (clickCount == 3)
            {
                dialogMissionView.Show(false);
                situationMissionView.Show(true);
                situationMissionView.SituationView(faultMessage_3);
            }
            if (clickCount >= number && clickCount < historyMessage.Length + number)
            {
                Debug.Log("clickCount4: " + clickCount);
                situationMissionView.Show(false);
                dialogMissionView.Show(true);                
                dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
            }

            if (clickCount == historyMessage.Length + number)
            {
                Debug.Log("Finish");
                LeaveMission(score);
            }

        }
    }

    private void LeaveMission(int score)
    {
        dialogMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveEvent;
    }

    private void LeaveEvent()
    {
        endMissionView.Show(false);
        Debug.Log("Mission 6 Leave");

        fingerClick.boxCollider.enabled = false; // end fingerClick trigger
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial

        fingerClick.Click -= QuestionReult; // Add fingerClick event
        endMissionView.OnEnable -= LeaveEvent;
    }
}
