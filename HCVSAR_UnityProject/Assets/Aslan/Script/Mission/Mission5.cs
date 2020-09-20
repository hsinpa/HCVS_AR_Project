﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.StaticAsset;

public class Mission5 : ViewController
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite npc;

    // Message
    string situationMessage = StringAsset.MissionsSituation.Five.s1;
    string dogName = StringAsset.MissionsDialog.Person.dog;
    string npcName = StringAsset.MissionsDialog.Person.NPC_1;
    string npcMessage_1 = StringAsset.MissionsDialog.Five.d1;
    private string dogMessage3 = StringAsset.MissionsDialog.One.d2;

    string[] historyMessage = { StringAsset.MissionsDialog.Five.history1, StringAsset.MissionsDialog.Five.history2, StringAsset.MissionsDialog.Five.history3 };

    private string qustion = StringAsset.MissionsQustion.Five.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.Five.ans1, StringAsset.MissionsAnswer.Five.ans2,
                                 StringAsset.MissionsAnswer.Five.ans3, StringAsset.MissionsAnswer.Five.ans4};

    private string correctMessage_1 = StringAsset.MissionsDialog.Five.correct_1;
    private string correctMessage_2 = StringAsset.MissionsDialog.Five.correct_2;
    private string faultMessage_1 = StringAsset.MissionsDialog.Five.fault_1;
    private string faultMessage_2 = StringAsset.MissionsDialog.Five.fault_2;
    private string faultMessage_3 = StringAsset.MissionsSituation.Five.fault;
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [HideInInspector]
    public GameObject hideBG;
    public GameObject video;
    public GameObject toolView;

    public override void Enable()
    {
        base.Enable();

        isEnter = true;
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

        Debug.Log("5 clickCount: " + clickCount);
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
        InitFingerClick();

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

        PostScoreEvent.Instance.PostScore(score);

        if (score > 0)
        {
            number = 2;

            if (clickCount == 1)
            {
                dialogMissionView.DialogView(npcName, correctMessage_2, npc);
            }

            if (clickCount == number)
            {
                JoeMain.Main.Play360Video();
            }

            if (clickCount >= number && clickCount < historyMessage.Length + number)
            {
                dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
                Debug.Log("5clickCount: " + clickCount);
            }

            if (clickCount == historyMessage.Length + number)
            {
                LeaveMission(score);
                Debug.Log("5Finish");
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
                Debug.Log("55clickCount: " + clickCount);
                situationMissionView.Show(false);
                dialogMissionView.Show(true);
                dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
            }

            if (clickCount == historyMessage.Length + number)
            {
                Debug.Log("555Finish");
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
        Debug.Log("Mission 5555 Leave");
    }

    private void LeaveEvent()
    {
        endMissionView.Show(false);

        InitFingerClick();
        RemoveAllEvent();
        RemoveAllListeners();

        hideBG.SetActive(true);
        video.SetActive(false);
        Debug.Log("Mission 5 Leave");

        StartCoroutine(GetMail());
    }

    public IEnumerator GetMail()
    {
        yield return new WaitForSeconds(1);

        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, dogMessage3, dog);

        yield return new WaitForSeconds(2);
        Debug.Log("Mail");
        dialogMissionView.Show(false);
        toolView.SetActive(true);
    }
    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        questionMissionView.RemoveListeners();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
        endMissionView.OnEnable -= LeaveEvent;
        questionMissionView.buttonClick -= QuestionReult;
        questionMissionView.buttonClick -= OpenClickEvent;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial
    }
}
