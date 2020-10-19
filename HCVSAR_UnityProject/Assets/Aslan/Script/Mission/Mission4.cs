﻿using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using Hsinpa.Video;
using System.Collections;

public class Mission4 : ViewController
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite student;
    [SerializeField]
    private Sprite pic;
    [SerializeField]
    private Image picture;

    // Message
    private string situationMessage = StringAsset.MissionsSituation.Four.s1;
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string studentName = StringAsset.MissionsDialog.Person.NPC_3;
    private string studenMessage1 = StringAsset.MissionsDialog.Four.p1;
    private string studenMessage2 = StringAsset.MissionsDialog.Four.p2;
    private string dogMessage1 = StringAsset.MissionsDialog.Four.d1;
    private string dogMessage2 = StringAsset.MissionsDialog.Four.d2;
    private string correctMessage_1 = StringAsset.MissionsDialog.Four.correct_1;
    private string correctMessage_2 = StringAsset.MissionsDialog.Four.correct_2;
    private string faultMessage_1 = StringAsset.MissionsDialog.Four.fault_1;

    private string[] historyMessage = { StringAsset.MissionsDialog.Four.history1, StringAsset.MissionsDialog.Four.history2, StringAsset.MissionsDialog.Four.history3 };
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject gameUI;
    public GameObject messageUI;
    public Button success;
    public Button fail;
    private Camera _camera;
    public VideoEffectCtrl videoEffect;
    private bool isSuccess;

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);
        messageUI.SetActive(false);

        _camera = MissionsController.Instance.isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;
        fingerClick = fingerClickController.currentClick;
        JoeMain.Main.Start360Video(4);

        StartCoroutine(EnterGameView());
    }

    public IEnumerator EnterGameView()
    {
        videoEffect.FaceVideoToCameraFront(_camera, 4);
        videoEffect.SetCoverPercentAnim(0.8f, 0.1f);

        yield return new WaitForSeconds(2);

        JoeMain.Main.Play360Video();
        videoEffect.SetCoverPercentAnim(0, 0.01f);

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
        if (clickCount == 1)
        {
            Debug.Log("clickCount1: " + clickCount);
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(studentName, studenMessage1, student);
        }
        if (clickCount == 2)
        {
            Debug.Log("clickCount2: " + clickCount);
            dialogMissionView.DialogView(dogName, dogMessage1, dog);
        }
        if (clickCount == 3)
        {
            dialogMissionView.DialogView(studentName, studenMessage2, student);
        }
        if (clickCount == 4)
        {
            dialogMissionView.DialogView(dogName, dogMessage2, dog);
        }
        if (clickCount == 5)
        {
            Debug.Log("Finish");
            StarGame();
            InitFingerClick();
        }
    }

    private void StarGame()
    {
        dialogMissionView.Show(false);
        JoeMain.Main.PlayARGame(2);

        success.onClick.AddListener(SuccessClick);
        fail.onClick.AddListener(FailClick);
    }

    private void SuccessClick()
    {
        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += Count; // Add fingerClick event
        isSuccess = true;

        gameUI.SetActive(false);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, correctMessage_1, dog);
    }

    private void FailClick()
    {
        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += Count; // Add fingerClick event
        isSuccess = false;

        gameUI.SetActive(false);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(studentName, faultMessage_1, student);
    }

    private void Count()
    {
        clickCount++;
        picture.enabled = true;
        picture.sprite = pic;
        Debug.Log("Count ++" + clickCount);
        if (clickCount >= 0)
        {
            if (isSuccess) { GameSuccess(); }
            if (!isSuccess) { GameFail(); }
        }
    }

    private void GameSuccess()
    {
        if (clickCount == 1)
        {
            dialogMissionView.DialogView(studentName, correctMessage_2, student);
        }

        if (clickCount >= 2)
        {
            StarHistory(2);
        }
    }

    private void GameFail()
    {
        if (clickCount >= 0)
        {
            StarHistory(0);
        }
    }

    private void StarHistory(int number)
    {
        if (clickCount >= number && clickCount < historyMessage.Length + number)
        {
            dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
        }

        if (clickCount == historyMessage.Length + number)
        {
            LeaveMission(isSuccess);
        }
    }

    private void LeaveMission(bool success)
    {
        int score = success ? 5 : 0;

        dialogMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveEvent;

        PostScoreEvent.Instance.PostScore(score, MainView.Instance.loginData.userType);
    }

    private void LeaveEvent()
    {
        endMissionView.Show(false);
        hideBG.SetActive(true);
        picture.enabled = false;

        InitFingerClick();
        RemoveAllEvent();
        RemoveAllListeners();

        JoeMain.Main.CloseARGame(2);
        JoeMain.Main.Stop360Video();
        videoEffect.SetCoverPercent(1);
        MissionsController.Instance.ReSetMissions();
        Debug.Log("Mission 4 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
        fingerClick.Click -= Count;
        endMissionView.OnEnable -= LeaveEvent;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        fingerClick.Click -= Count;
        clickCount = 0; // initial
    }
}
