﻿using System.Collections;
using UnityEngine;
using Expect.StaticAsset;
using Hsinpa.Video;

public class Mission7 : ViewController
{
    [SerializeField]
    private Sprite dog;

    // Message
    private string situationMessage = StringAsset.MissionsSituation.SEVEN.s1;
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string dogMessage = StringAsset.MissionsDialog.Seven.d1;

    private string qustion = StringAsset.MissionsQustion.SEVEN.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.SEVEN.ans1, StringAsset.MissionsAnswer.SEVEN.ans2,
                                 StringAsset.MissionsAnswer.SEVEN.ans3, StringAsset.MissionsAnswer.SEVEN.ans4};
    private string correctMessage = StringAsset.MissionsQustion.SEVEN.correct;
    private string faultMessage = StringAsset.MissionsQustion.SEVEN.fault;
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [SerializeField]
    private VideoEffectCtrl videoEffect;

    [HideInInspector]
    public bool isEnterMission;

    public GameObject hideBG;
    public GameObject video; 
    private Camera _camera;
    private bool isARsupport;

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);

        isARsupport = MissionsController.Instance.isARsupport;
        _camera = isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;
        
        JoeMain.Main.Start360Video(7);

        EnterVideoView();
    }

    public void EnterVideoView()
    {
        float speed = isARsupport ? 0.01f : 1f;
        videoEffect.FaceVideoToCameraFront(_camera, 7);
        videoEffect.SetCoverPercentAnim(0, speed);

        situationMissionView.Show(true);
        situationMissionView.SituationView(situationMessage);

        ClickNextButton();
        nextButton.onClick.AddListener(ClickCount);
    }

    void ClickCount()
    {
        clickCount++;
        dialogMissionView.TypeInit();

        if (clickCount >= 0)
        {
            Convercestion();
        }
    }

    void Convercestion()
    {
        if (clickCount == 1)
        {
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, dogMessage, dog);
        }

        if (clickCount == 2)
        {
            dialogMissionView.Show(false);
            OnClickButton(false);
            JoeMain.Main.Play360Video();
        }
    }

    public override void NextAction()
    {
        Debug.Log("7 Finish");
        video.SetActive(false);
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
        yield return new WaitForSeconds(6);

        dialogMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveMission;
    }

    private void LeaveMission()
    {
        endMissionView.Show(false);
        RemoveAllListeners();
        RemoveAllEvent();

        hideBG.SetActive(true);

        MissionsController.Instance.ReSetMissions();
        JoeMain.Main.Stop360Video();
        videoEffect.SetCoverPercent(1);

        Debug.Log("Mission 7 Leave");
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
