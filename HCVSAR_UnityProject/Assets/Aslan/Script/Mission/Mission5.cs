using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Video;
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
    public GameObject toolView;
    public VideoEffectCtrl videoEffect;
    public MainBaseVIew mainBaseVIew;
    public GameObject videoUI;
    public GameObject[] models;

    private Camera _camera;

    public override void Enable()
    {
        base.Enable();

        isEnter = true;
        hideBG.SetActive(false);

        JoeMain.Main.PlayGame(6);
        fingerClick = fingerClickController.currentClick;
        //videoEffect.SetCoverPercentAnim(0, 0.01f);

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
        fingerClick.Click -= ClickCount;

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

        PostScoreEvent.Instance.PostScore(score, MainView.Instance.loginData.userType);

        if (score > 0)
        {
            number = 2;

            if (clickCount == 1)
            {
                dialogMissionView.DialogView(npcName, correctMessage_2, npc);
            }

            if (clickCount == number)
            {
                foreach(var m in models) { m.SetActive(false); }
                dialogMissionView.Show(false);

                JoeMain.Main.StarAndPlay360Video(5);

                _camera = MissionsController.Instance.isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;
                videoEffect.FaceVideoToCameraFront(_camera, 5);
                videoEffect.SetCoverPercentAnim(0, 0.01f);
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
                Debug.Log("5clickCount: " + clickCount);
                situationMissionView.Show(false);
                dialogMissionView.Show(true);
                dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
            }

            if (clickCount == historyMessage.Length + number)
            {
                Debug.Log("5 Finish");
                LeaveMission(score);
            }

        }
    }

    public override void NextAction()
    {
        videoUI.SetActive(false);
        LeaveMission(5);
    }

    private void LeaveMission(int score)
    {
        InitFingerClick();
        fingerClick.Click -= QuestionReult;

        dialogMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveEvent;
        Debug.Log("Mission 5 Leave");
    }

    private void LeaveEvent()
    {
        endMissionView.Show(false);

        InitFingerClick();
        RemoveAllEvent();
        RemoveAllListeners();

        hideBG.SetActive(true);

        JoeMain.Main.CloseGame(6);        
        Debug.Log("Mission 5 Leave");

        StartCoroutine(GetMail());
    }

    public IEnumerator GetMail()
    {
        mainBaseVIew.PanelController(true);

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
        endMissionView.OnEnable -= LeaveEvent;
        questionMissionView.buttonClick -= OpenClickEvent;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        clickCount = 0; // initial
    }
}
