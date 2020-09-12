using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_2 : MonoBehaviour
{
    [SerializeField]
    private Sprite dog;

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
    private string situationMessage = StringAsset.MissionsSituation.Two.s1;
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string[] historyMessage = { StringAsset.MissionsDialog.Two.history1, StringAsset.MissionsDialog.Two.history2, StringAsset.MissionsDialog.Two.history3 };

    private string qustion = StringAsset.MissionsQustion.Two.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.Two.ans1, StringAsset.MissionsAnswer.Two.ans2,
                                 StringAsset.MissionsAnswer.Two.ans3, StringAsset.MissionsAnswer.Two.ans4};
    private string correctMessage = StringAsset.MissionsQustion.Two.correct;
    private string faultMessage = StringAsset.MissionsQustion.Two.fault;
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject video;

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
    private void Disable()
    {
        isEnterMission = false;

        enterMissionView.Show(false);
        enterMissionView.RemoveListeners();
        Debug.Log("other thing");
    }

    private void StarEnable()
    {
        isEnterMission = true;
        hideBG.SetActive(false);
        video.SetActive(true);
        JoeMain.Main.Start360Video(0);

        enterMissionView.Show(false);

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
        int number = 1;

        if (clickCount == 1)
        {
            JoeMain.Main.Play360Video();
        }

        if (clickCount >= number && clickCount < historyMessage.Length + number)
        {
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
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
        InitFingerClick();

        dialogMissionView.Show(false);
        questionMissionView.Show(true);

        questionMissionView.QuestionView(qustion, answers, 1);
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

        hideBG.SetActive(true);
        video.SetActive(false);
        Debug.Log("Mission 3 Leave");
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
        clickCount = 0; // initial
    }
}
