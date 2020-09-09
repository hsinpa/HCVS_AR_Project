using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_2 : MonoBehaviour
{
    [SerializeField]
    private Button mission_2;
    [SerializeField]
    private Sprite dog;

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
    string situationMessage = StringAsset.MissionsSituation.TWO.s1;
    string dogName = StringAsset.MissionsDialog.Person.dog;
    string dogMessage = StringAsset.MissionsDialog.TWO.d1;
    string[] historyMessage = { StringAsset.MissionsDialog.TWO.history1, StringAsset.MissionsDialog.TWO.history2, StringAsset.MissionsDialog.TWO.history3 };

    private string qustion = StringAsset.MissionsQustion.TWO.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.TWO.ans1, StringAsset.MissionsAnswer.TWO.ans2,
                                 StringAsset.MissionsAnswer.TWO.ans3, StringAsset.MissionsAnswer.TWO.ans4};
    private string correctMessage = StringAsset.MissionsQustion.TWO.correct;
    private string faultMessage = StringAsset.MissionsQustion.TWO.fault;

    private string endMessage = StringAsset.MissionsEnd.End.message;


    private void MissionArraySetUp()
    {
        missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
    }

    private void Awake()
    {
        loginData = MainView.Instance.loginData;
        studentScoreData.student_id = loginData.user_id;
        studentScoreData.mission_id = "B";

        MissionArraySetUp();
        fingerClick = situationMissionView.GetComponentInParent<FingerClickEvent>();
    }

    private void Start()
    {
        mission_2.onClick.AddListener(MissionStart);
    }

    private void MissionStart()
    {
        enterMissionView.Show(true);
        enterMissionView.EnterMission(missionArray[1].mission_name, missionArray[1].mission_name);
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
        int number = 2;

        if (clickCount == 1)
        {
            Debug.Log("clickCount1: " + clickCount);
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, dogMessage, dog);
        }

        if (clickCount >= number && clickCount < historyMessage.Length + number)
        {
            Debug.Log("clickCount3: " + clickCount);
            dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
        }

        if (clickCount == historyMessage.Length + number)
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

        questionMissionView.QuestionView(qustion, answers, 1, studentScoreData);
        questionMissionView.buttonClick += QuestionReult;
    }

    private void QuestionReult()
    {
        var result = PostScoreEvent.Instance.answerResult;
        int score = PostScoreEvent.Instance.score;

        Debug.Log("result " + result + " score " + score);

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

        StartCoroutine(EndPauser(score));
    }

    public IEnumerator EndPauser(int score)
    {
        yield return new WaitForSeconds(5);

        dialogMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveMission;
    }

    private void LeaveMission()
    {
        endMissionView.Show(false);
        Debug.Log("Mission 2 Leave");

        endMissionView.OnEnable -= LeaveMission;
        questionMissionView.buttonClick -= QuestionReult;
    }
}
