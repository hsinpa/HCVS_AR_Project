using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using System.Collections;

public class Mission6 : ViewController
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite primeMinister;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string oldPeopleName = StringAsset.MissionsDialog.Person.NPC_4;
    private string primeMinisterName = StringAsset.MissionsDialog.Person.NPC_5;
    private string oldPeopleMessage1 = StringAsset.MissionsDialog.Six.p1;
    private string dogMessage1 = StringAsset.MissionsDialog.Six.d1;
    private string faultMessage_1 = StringAsset.MissionsDialog.Six.fault_1;
    private string[] historyMessage = { StringAsset.MissionsDialog.Six.history1, StringAsset.MissionsDialog.Six.history2,
                                        StringAsset.MissionsDialog.Six.history3, StringAsset.MissionsDialog.Six.history4, StringAsset.MissionsDialog.Six.history5};
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public Button success;
    public Button fail;
    public GameObject closeVideo; //TODO;Deltete
    private bool isSuccess;

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);

        JoeMain.Main.ControllerVideoPlane(true);
        //fingerClick = fingerClickController.currentClick;

        dialogMissionView.Show(true);
        dialogMissionView.DialogView(oldPeopleName, oldPeopleMessage1, primeMinister);

        //fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        //fingerClick.Click += ClickCount; // Add fingerClick event

        ClickNextButton();
        nextButton.onClick.AddListener(ClickCount);
    }

    void ClickCount()
    {
        clickCount++;

        if (clickCount >= 0) { Convercestion(); }

        Debug.Log("clickCount: " + clickCount);
    }

    private void Convercestion()
    {
        if (clickCount == 1)
        {
            Debug.Log("clickCount2: " + clickCount);
            dialogMissionView.DialogView(dogName, dogMessage1, dog);
        }
        if (clickCount == 2)
        {
            StarGame();
            SwitchButton(false);
            Debug.Log("Game");
        }
    }

    private void StarGame()
    {
        //InitFingerClick();
        dialogMissionView.Show(false);

        closeVideo.SetActive(false);
        JoeMain.Main.PlayARGame(3);

        success.onClick.AddListener(SuccessClick);
        fail.onClick.AddListener(FailClick);
    }

    private void SuccessClick()
    {
        StartCoroutine(GameSuccess());
        isSuccess = true;
    }

    private void FailClick()
    {
        //fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        //fingerClick.Click += Count; // Add fingerClick event
        ClickNextButton();
        nextButton.onClick.AddListener(Count);
        isSuccess = false;

        dialogMissionView.Show(true);
        dialogMissionView.DialogView(oldPeopleName, faultMessage_1, primeMinister);
    }

    private void Count()
    {
        clickCount++;

        if (clickCount >= 0)
        {
            GameFail();
        }
    }

    public IEnumerator GameSuccess()
    {
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(primeMinisterName, historyMessage[0], primeMinister);

        yield return new WaitForSeconds(5);

        dialogMissionView.DialogView(primeMinisterName, historyMessage[1], primeMinister);

        yield return new WaitForSeconds(9);

        dialogMissionView.DialogView(primeMinisterName, historyMessage[2], primeMinister);

        yield return new WaitForSeconds(20);

        dialogMissionView.DialogView(primeMinisterName, historyMessage[3], primeMinister);

        yield return new WaitForSeconds(25);

        dialogMissionView.DialogView(primeMinisterName, historyMessage[4], primeMinister);

        yield return new WaitForSeconds(11);

        LeaveMission(isSuccess);
    }

    private void GameFail()
    {
        if (clickCount >= 1)
        {
            StarHistory(1);
        }
    }

    private void StarHistory(int number)
    {
        if (clickCount >= number && clickCount < historyMessage.Length + number)
        {
            dialogMissionView.DialogView(primeMinisterName, historyMessage[clickCount - number], primeMinister);
        }

        if (clickCount == historyMessage.Length + number)
        {
            LeaveMission(isSuccess);
        }
    }

    private void LeaveMission(bool success)
    {
        int score = success ? 5 : 0;

        SwitchButton(false);
        dialogMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveEvent;
        PostScoreEvent.Instance.PostScore(score, MainView.Instance.loginData.userType);
    }

    private void LeaveEvent()
    {
        endMissionView.Show(false);

        //InitFingerClick();
        RemoveAllEvent();
        RemoveAllListeners();

        hideBG.SetActive(true);
        MissionsController.Instance.ReSetMissions();

        closeVideo.SetActive(true);
        JoeMain.Main.CloseARGame(3);
        JoeMain.Main.ControllerVideoPlane(false);

        Debug.Log("Mission 6 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        nextButton.onClick.RemoveAllListeners();
    }

    private void RemoveAllEvent()
    {
        //fingerClick.Click -= ClickCount;
        //fingerClick.Click -= Count;
        endMissionView.OnEnable -= LeaveEvent;
    }
    /*
    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        fingerClick.Click -= Count;
        clickCount = 0; // initial
    }*/
}
