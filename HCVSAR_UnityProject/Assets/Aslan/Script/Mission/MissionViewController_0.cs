using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_0 : MonoBehaviour
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite person;

    [SerializeField]
    private EnterMissionView enterMissionView;
    [SerializeField]
    private SituationMissionView situationMissionView;
    [SerializeField]
    private DialogMissionView dialogMissionView;
    [SerializeField]
    private QuestionMissionView questionMissionView;
    [SerializeField]
    private EndMissionView endMissionView;
    [SerializeField]
    private FingerClickEvent fingerClick;

    private int clickCount;

    // Message
    private string situationMessage = StringAsset.MissionsSituation.Zero.s1;
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string peopleName = StringAsset.MissionsDialog.Person.NPC_1;
    private string dogMessage = StringAsset.MissionsDialog.Zero.d1;
    private string peopleMessage = StringAsset.MissionsDialog.Zero.d2;
    private string[] historyMessage = { StringAsset.MissionsDialog.Zero.history1, StringAsset.MissionsDialog.Zero.history2,
                                 StringAsset.MissionsDialog.Zero.history3, StringAsset.MissionsDialog.Zero.history4};

    private string qustion = StringAsset.MissionsQustion.Zero.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.Zero.ans1, StringAsset.MissionsAnswer.Zero.ans2,
                                 StringAsset.MissionsAnswer.Zero.ans3, StringAsset.MissionsAnswer.Zero.ans4};
    private string correctMessage = StringAsset.MissionsQustion.Zero.correct;
    private string faultMessage = StringAsset.MissionsQustion.Zero.fault;
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject video;
    public GameObject enterGame;
    public GameObject game00;
    public Button enter;
    public Button leave;
    public Button success;
    public Button fail;

    public void MissionStart(int missionNumber)
    {
        TypeFlag.InGameType.MissionType[] missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
        MainView.Instance.studentScoreData.mission_id = missionArray[missionNumber].mission_id;

        enterMissionView.Show(true);
        enterMissionView.EnterMission(missionArray[missionNumber].mission_name, missionArray[missionNumber].mission_name);
        enterMissionView.OnEnable += StarEnable;
        enterMissionView.OnDisable += Disable;
    }

    // TODO: ibeacon find other mission after 10 second
    public void Disable()
    {
        isEnterMission = false;

        enterMissionView.Show(false);
        enterMissionView.RemoveListeners();
        Debug.Log("other thing");
    }

    void StarEnable()
    {
        isEnterMission = true;
        hideBG.SetActive(false);
        video.SetActive(true);
        JoeMain.Main.Start360Video(0);

        enterMissionView.Show(false);

        situationMissionView.Show(true);
        situationMissionView.Show(true);
        situationMissionView.SituationView(situationMessage);

        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += ClickCount; // Add fingerClick event
    }

    void ClickCount()
    {
        clickCount++;

        if (clickCount >= 0)
        {
            Convercestion();
        }

        Debug.Log("clickCount: " + clickCount);
    }

    void Convercestion()
    {
        int number = 3;

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

        if (clickCount == number)
        {
            JoeMain.Main.Play360Video();
        }

        if (clickCount >= number && clickCount < historyMessage.Length + number)
        {
            dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
            Debug.Log("clickCount3: " + clickCount);
        }

        if (clickCount == historyMessage.Length + number)
        {
            Debug.Log("Finish");
            InitFingerClick();
            Qusteion();
        }
    }

    private void Qusteion()
    {
        dialogMissionView.Show(false);

        questionMissionView.Show(true);
        questionMissionView.QuestionView(qustion, answers, 1); // score is null
        questionMissionView.buttonClick += QuestionReult;
    }

    private void QuestionReult()
    {
        int score = MainView.Instance.studentScoreData.score;

        questionMissionView.Show(false);
        dialogMissionView.Show(true);

        if (score > 0)
        {
            dialogMissionView.DialogView(dogName, correctMessage, dog);
        }
        else
        {
            dialogMissionView.DialogView(dogName, faultMessage, dog);
        }

        StartCoroutine(EnterGameView());
    }

    public IEnumerator EnterGameView()
    {
        yield return new WaitForSeconds(1);

        dialogMissionView.Show(false);
        enterGame.SetActive(true);
        enter.onClick.AddListener(EnterGame);
        leave.onClick.AddListener(LeaveGame);
    }

    private void EnterGame()
    {
        enterGame.SetActive(false);
        game00.SetActive(true);
        success.onClick.AddListener(SuccessGame);
        fail.onClick.AddListener(LeaveGame);
    }

    private void SuccessGame()
    {
        int score = MainView.Instance.studentScoreData.score + 5;
        PostScoreEvent.Instance.PostScore(score);

        EndNoGameView(score);
    }

    private void LeaveGame()
    {
        int score = MainView.Instance.studentScoreData.score;

        game00.SetActive(false);
        enterGame.SetActive(false);
        EndNoGameView(score);
    }

    private void EndNoGameView(int score)
    {
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveMission;
    }

    private void LeaveMission()
    {
        endMissionView.Show(false);

        InitFingerClick();
        RemoveAllListeners();
        RemoveAllEvent();

        hideBG.SetActive(true);
        video.SetActive(false);
        Debug.Log("Mission 0 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        questionMissionView.RemoveListeners();
        enterMissionView.RemoveListeners();
        enter.onClick.RemoveAllListeners();
        leave.onClick.RemoveAllListeners();
        success.onClick.RemoveAllListeners();
        fail.onClick.RemoveAllListeners();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
        enterMissionView.OnEnable -= StarEnable;
        enterMissionView.OnDisable -= Disable;
        endMissionView.OnEnable -= LeaveMission;
        questionMissionView.buttonClick -= QuestionReult;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial
    }
}
