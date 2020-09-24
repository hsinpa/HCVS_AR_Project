using System.Collections;
using UnityEngine;
using Expect.StaticAsset;

public class Mission7 : ViewController
{
    [SerializeField]
    private Sprite dog;

    // Message
    private string situationMessage = StringAsset.MissionsSituation.SEVEN.s1;
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string dogMessage = StringAsset.MissionsDialog.Seven.d1;
    private string[] historyMessage = { StringAsset.MissionsDialog.Seven.history1, StringAsset.MissionsDialog.Seven.history2, StringAsset.MissionsDialog.Seven.history3 };

    private string qustion = StringAsset.MissionsQustion.SEVEN.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.SEVEN.ans1, StringAsset.MissionsAnswer.SEVEN.ans2,
                                 StringAsset.MissionsAnswer.SEVEN.ans3, StringAsset.MissionsAnswer.SEVEN.ans4};
    private string correctMessage = StringAsset.MissionsQustion.SEVEN.correct;
    private string faultMessage = StringAsset.MissionsQustion.SEVEN.fault;
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject video;

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);
        video.SetActive(true);
        JoeMain.Main.Start360Video(0);

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

        Debug.Log("7 clickCount: " + clickCount);
    }

    void Convercestion()
    {
        int number = 2;

        if (clickCount == 1)
        {
            Debug.Log("7 clickCount: " + clickCount);
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, dogMessage, dog);
        }

        if (clickCount == number)
        {
            JoeMain.Main.Play360Video();
        }

        if (clickCount >= number && clickCount < historyMessage.Length + number)
        {
            dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
            Debug.Log("7 clickCount: " + clickCount);
        }

        if (clickCount == historyMessage.Length + number)
        {
            Debug.Log("7 Finish");
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
        yield return new WaitForSeconds(3);

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
        MissionsController.Instance.ReSetMissions();

        Debug.Log("Mission 7 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        questionMissionView.RemoveListeners();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
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
