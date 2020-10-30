using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.StaticAsset;
using Hsinpa.Video;

public class Mission2 : ViewController
{
    [SerializeField]
    private Sprite dog;

    // Message
    private string situationMessage = StringAsset.MissionsSituation.Two.s1;
    private string dogName = StringAsset.MissionsDialog.Person.dog;

    private string qustion = StringAsset.MissionsQustion.Two.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.Two.ans1, StringAsset.MissionsAnswer.Two.ans2,
                                 StringAsset.MissionsAnswer.Two.ans3, StringAsset.MissionsAnswer.Two.ans4};
    private string correctMessage = StringAsset.MissionsQustion.Two.correct;
    private string faultMessage = StringAsset.MissionsQustion.Two.fault;
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [SerializeField]
    private VideoEffectCtrl videoEffect;

    private Camera _camera;
    private bool isARsupport;

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject videoUI;

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);

        isARsupport = MissionsController.Instance.isARsupport;
        _camera = isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;
        JoeMain.Main.Start360Video(2);

        EnterVideoView();
    }

    public void EnterVideoView()
    {
        float speed = isARsupport ? 0.01f : 1f;
        videoEffect.FaceVideoToCameraFront(_camera, 2);
        videoEffect.SetCoverPercentAnim(0, speed);

        situationMissionView.Show(true);
        situationMissionView.SituationView(situationMessage);

        ClickNextButton();
        nextButton.onClick.AddListener(ClickCount);
    }

    void ClickCount()
    {
        clickCount++;

        if (clickCount >= 0)
        {
            Convercestion();
        }
    }

    void Convercestion()
    {

        if (clickCount == 1)
        {
            OnClickButton(false);
            JoeMain.Main.Play360Video();
            situationMissionView.Show(false);
        }
    }

    public override void NextAction()
    {
        Debug.Log("2 Finish");
        videoUI.SetActive(false);
        Qusteion();
    }

    private void Qusteion()
    {
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
        hideBG.SetActive(true);

        RemoveAllListeners();
        RemoveAllEvent();
        MissionsController.Instance.ReSetMissions();
        JoeMain.Main.Stop360Video();

        videoEffect.SetCoverPercent(1);
        Debug.Log("Mission 2 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        questionMissionView.RemoveListeners();
        nextButton.onClick.RemoveAllListeners();
    }

    private void RemoveAllEvent()
    {
        endMissionView.OnEnable -= LeaveMission;
        questionMissionView.buttonClick -= QuestionReult;
    }
}
