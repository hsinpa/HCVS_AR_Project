using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_1 : MonoBehaviour
{
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
    [SerializeField]
    FingerClickEvent fingerClick;
    
    private int clickCount;

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
        enterMissionView.Show(false);
        enterMissionView.RemoveListeners();
        Debug.Log("other thing");
    }

    void StarEnable()
    {
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

        InitFingerClick();
        RemoveAllListeners();
        RemoveAllEvent();

        Debug.Log("Mission 1 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        questionMissionView.RemoveListeners();
        enterMissionView.RemoveListeners();
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
        clickCount = -1; // initial
    }
}
