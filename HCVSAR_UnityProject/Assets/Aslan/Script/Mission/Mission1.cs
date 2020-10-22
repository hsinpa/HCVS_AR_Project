using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class Mission1 : ViewController
{
    public Button success;
    public Button fail;
    public GameObject mailInfo;
    public CanvasGroup mainCanvas;

    [HideInInspector]
    public bool isSuccess;
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite police;
    [SerializeField]
    private BagPanel bagPanel;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string policeName = StringAsset.MissionsDialog.Person.NPC_6;
    private string dogMessage1 = StringAsset.MissionsDialog.One.d1;
    private string correctMessage_1 = StringAsset.MissionsDialog.One.correct_1;
    private string faultMessage_1 = StringAsset.MissionsDialog.One.fault_1;
    private string faultMessage_2 = StringAsset.MissionsDialog.One.fault_2;
    private string[] historyMessage = { StringAsset.MissionsDialog.One.history1, StringAsset.MissionsDialog.One.history2, StringAsset.MissionsDialog.One.history3 };
    private string endMessage = StringAsset.MissionsEnd.End.message;

    public GameObject hideBG;
    public GameObject[] models;
    public MainBaseVIew mainBaseVIew;

    private void Start()
    {
        success.onClick.AddListener(SuccessClick);
        fail.onClick.AddListener(FailClick);
    }

    private void SuccessClick()
    {
        ClickNextButton();
        nextButton.onClick.AddListener(ClickCount);
        isSuccess = true;

        bagPanel.Show(false);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, dogMessage1, dog);

        MissionsController.Instance.ARSession.enabled = true;
    }

    private void FailClick()
    {
        ClickNextButton();
        nextButton.onClick.AddListener(ClickCount);

        isSuccess = false;

        bagPanel.Show(false);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, dogMessage1, dog);

        MissionsController.Instance.ARSession.enabled = true;
    }

    private void ClickCount()
    {
        clickCount++;
        mainCanvas.interactable = false;

        if (clickCount > 0)
        {
            if (isSuccess) { GameSuccess(); }
            if (!isSuccess) { GameFail(); }
        }
    }

    private void GameSuccess()
    {
        if (clickCount == 1)
        {
            dialogMissionView.DialogView(policeName, correctMessage_1, police);
            Debug.Log("sss");
        }
        
        if (clickCount >= 2)
        {
            mainBaseVIew.PanelController(false);
            StarHistory(2);
        }
    }

    private void GameFail()
    {
        if (clickCount == 1)
        {
            dialogMissionView.DialogView(policeName, faultMessage_1, police);
        }

        if (clickCount == 2)
        {
            mainBaseVIew.PanelController(false);
            dialogMissionView.DialogView(dogName, faultMessage_2, dog);
        }

        if (clickCount >= 3)
        {
            StarHistory(3);
        }
    }

    private void StarHistory(int number)
    {
        if (clickCount == number)
        {
            hideBG.SetActive(false);
            JoeMain.Main.ControllerVideoPlane(true);
            JoeMain.Main.PlayARGame(4);
        }

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

        mailInfo.SetActive(false);
        dialogMissionView.Show(false);
        OnClickButton(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveEvent;
        PostScoreEvent.Instance.PostScore(score, MainView.Instance.loginData.userType);
    }

    private void LeaveEvent()
    {
        endMissionView.Show(false);
        hideBG.SetActive(true);

        RemoveAllListeners();

        mainCanvas.interactable = true;

        foreach (var m in models) { m.SetActive(false); }
        MissionsController.Instance.ReSetMissions();
        JoeMain.Main.CloseARGame(4);
        JoeMain.Main.ControllerVideoPlane(false);
        Debug.Log("Mission 1 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        nextButton.onClick.RemoveAllListeners();
    }
}
