using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using Hsinpa.Video;

public class Mission0 : ViewController
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite person;

    // Message
    private string situationMessage = StringAsset.MissionsSituation.Zero.s1;
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string peopleName = StringAsset.MissionsDialog.Person.NPC_1;
    private string dogMessage = StringAsset.MissionsDialog.Zero.d1;
    private string peopleMessage = StringAsset.MissionsDialog.Zero.d2;
    private string dogMessage3 = StringAsset.MissionsDialog.Eight.d2;

    private string qustion = StringAsset.MissionsQustion.Zero.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.Zero.ans1, StringAsset.MissionsAnswer.Zero.ans2,
                                 StringAsset.MissionsAnswer.Zero.ans3, StringAsset.MissionsAnswer.Zero.ans4};
    private string correctMessage = StringAsset.MissionsQustion.Zero.correct;
    private string faultMessage = StringAsset.MissionsQustion.Zero.fault;
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [SerializeField]
    private VideoEffectCtrl videoEffect;
    private Camera _camera;
    //private int click;

    public GameObject hideBG;
    public GameObject video;
    public GameObject enterGame;
    public Button enter;
    public Button leave;
    public Button success;
    public Button fail;
    public GameObject toolView;
    public MainBaseVIew mainBaseVIew;

    public override void Enable()
    {
        base.Enable();

        hideBG.SetActive(false);

        _camera = MissionsController.Instance.isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;
        //fingerClick = fingerClickController.currentClick;
        JoeMain.Main.Start360Video(0);

        StartCoroutine(EnterVideoView());
    }


    public IEnumerator EnterVideoView()
    {
        videoEffect.FaceVideoToCameraFront(_camera, 0);
        videoEffect.SetCoverPercentAnim(0.8f, 0.1f);

        yield return new WaitForSeconds(2);

        videoEffect.SetCoverPercentAnim(0, 0.01f);

        situationMissionView.Show(true);
        situationMissionView.SituationView(situationMessage);

        //fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        //fingerClick.Click += ClickCount; // Add fingerClick event
        ClickNextButton();
        nextButton.onClick.AddListener(ClickCount);
    }

    void ClickCount()
    {
        clickCount++;

        if (clickCount > 0)
        {
            Convercestion();
        }
    }
    
    void Convercestion()
    {
        int number = 3;

        if (clickCount == 1)
        {
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, dogMessage, dog);
        }
        if (clickCount == 2)
        {
            dialogMissionView.DialogView(peopleName, peopleMessage, person);
        }

        if (clickCount == number)
        {
            SwitchButton(false);
            JoeMain.Main.Play360Video();
            dialogMissionView.Show(false);
            SwitchButton(false);
            //InitFingerClick();
        }
    }

    public override void NextAction()
    {
        video.SetActive(false);
        Qusteion();
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
        yield return new WaitForSeconds(3);

        dialogMissionView.Show(false);
        enterGame.SetActive(true);
        enter.onClick.AddListener(EnterGame);
        leave.onClick.AddListener(LeaveGame);
    }

    private void EnterGame()
    {
        enterGame.SetActive(false);
        JoeMain.Main.PlayGame(0);
        success.onClick.AddListener(SuccessGame);
        fail.onClick.AddListener(LeaveGame);
    }

    private void SuccessGame()
    {
        int score = MainView.Instance.studentScoreData.score + 5;

        JoeMain.Main.CloseGame(0);
        EndNoGameView(score);
    }

    private void LeaveGame()
    {
        int score = MainView.Instance.studentScoreData.score;

        JoeMain.Main.CloseGame(0);
        enterGame.SetActive(false);
        EndNoGameView(score);
    }

    private void EndNoGameView(int score)
    {
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveMission;

        PostScoreEvent.Instance.PostScore(score, MainView.Instance.loginData.userType);
    }

    private void LeaveMission()
    {
        endMissionView.Show(false);
        JoeMain.Main.Stop360Video();
        videoEffect.SetCoverPercent(1);

        //InitFingerClick();
        RemoveAllListeners();
        RemoveAllEvent();

        hideBG.SetActive(true);

        Debug.Log("Mission 0 Leave");

        StartCoroutine(GetMap());
    }

    public IEnumerator GetMap()
    {
        mainBaseVIew.PanelController(true);

        yield return new WaitForSeconds(1);

        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, dogMessage3, dog);

        yield return new WaitForSeconds(3);
        Debug.Log("Map1");
        dialogMissionView.Show(false);
        toolView.SetActive(true);
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        questionMissionView.RemoveListeners();
        enter.onClick.RemoveAllListeners();
        leave.onClick.RemoveAllListeners();
        success.onClick.RemoveAllListeners();
        fail.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
    }

    private void RemoveAllEvent()
    {
        //fingerClick.Click -= ClickCount;
        endMissionView.OnEnable -= LeaveMission;
        questionMissionView.buttonClick -= QuestionReult;
    }
    /*
    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial
    }*/
}
