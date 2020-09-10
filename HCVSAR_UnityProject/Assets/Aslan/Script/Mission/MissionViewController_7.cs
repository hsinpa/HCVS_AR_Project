﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_7 : MonoBehaviour
{
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
    [SerializeField]
    FingerClickEvent fingerClick;

    private int clickCount;

    // Message
    string situationMessage = StringAsset.MissionsSituation.SEVEN.s1;
    string dogName = StringAsset.MissionsDialog.Person.dog;
    string dogMessage = StringAsset.MissionsDialog.SEVEN.d1;
    string[] historyMessage = { StringAsset.MissionsDialog.SEVEN.history1, StringAsset.MissionsDialog.SEVEN.history2, StringAsset.MissionsDialog.SEVEN.history3 };

    private string qustion = StringAsset.MissionsQustion.SEVEN.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.SEVEN.ans1, StringAsset.MissionsAnswer.SEVEN.ans2,
                                 StringAsset.MissionsAnswer.SEVEN.ans3, StringAsset.MissionsAnswer.SEVEN.ans4};
    private string correctMessage = StringAsset.MissionsQustion.SEVEN.correct;
    private string faultMessage = StringAsset.MissionsQustion.SEVEN.fault;

    private string endMessage = StringAsset.MissionsEnd.End.message;
    /*
    public void Init()
    {
        enterMissionView = enterMissionView.GetComponent<EnterMissionView>();
        situationMissionView = situationMissionView.GetComponent<SituationMissionView>();
        dialogMissionView = dialogMissionView.GetComponent<DialogMissionView>();
        fingerClick = fingerClick.GetComponent<FingerClickEvent>();
    }*/

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = -1; // initial
    }

    public void MissionStart(int missionNumber)
    {
        TypeFlag.InGameType.MissionType[] missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;

        enterMissionView.Show(true);
        enterMissionView.EnterMission(missionArray[missionNumber].mission_name, missionArray[missionNumber].mission_name);
        enterMissionView.OnEnable += StarEnable;
        enterMissionView.OnDisable += Disable;

        MainView.Instance.studentScoreData.mission_id = missionArray[missionNumber].mission_id;
    }

    // TODO: ibeacon find other mission after 10 second
    private void Disable()
    {
        enterMissionView.Show(false);
        enterMissionView.RemoveListeners();
        //enterMissionView.OnEnable -= StarEnable;
        //enterMissionView.OnDisable -= Disable;
        Debug.Log("other thing");
    }

    private void StarEnable()
    {
        enterMissionView.RemoveListeners();
        //enterMissionView.OnEnable -= StarEnable;
        //enterMissionView.OnDisable -= Disable;
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

        endMissionView.OnEnable -= LeaveMission;
        questionMissionView.buttonClick -= QuestionReult;

        Debug.Log("Mission 7 Leave");
    }
}