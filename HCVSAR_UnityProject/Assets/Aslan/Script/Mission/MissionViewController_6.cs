using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_6 : MonoBehaviour
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite primeMinister;

    [SerializeField]
    private EnterMissionView enterMissionView;
    [SerializeField]
    private DialogMissionView dialogMissionView;
    [SerializeField]
    private EndMissionView endMissionView;
    [SerializeField]
    private FingerClickEvent fingerClick;

    private int clickCount;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string oldPeopleName = StringAsset.MissionsDialog.Person.NPC_4;
    private string primeMinisterName = StringAsset.MissionsDialog.Person.NPC_5;
    private string oldPeopleMessage1 = StringAsset.MissionsDialog.Six.p1;
    private string dogMessage1 = StringAsset.MissionsDialog.Six.d1;
    private string correctMessage_1 = StringAsset.MissionsDialog.Six.correct_1;
    private string faultMessage_1 = StringAsset.MissionsDialog.Six.fault_1;
    private string[] historyMessage = { StringAsset.MissionsDialog.Six.history1, StringAsset.MissionsDialog.Six.history2,
                                        StringAsset.MissionsDialog.Six.history3, StringAsset.MissionsDialog.Six.history4};
    private string endMessage = StringAsset.MissionsEnd.End.message;

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject gameUI;
    public Button success;
    public Button fail;
    private bool isSuccess;

    public void MissionStart(int missionNumber)
    {
        TypeFlag.InGameType.MissionType[] missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
        MainView.Instance.studentScoreData.mission_id = missionArray[missionNumber].mission_id;

        enterMissionView.Show(true);
        enterMissionView.EnterMission(missionArray[missionNumber].mission_name, missionArray[missionNumber].mission_name);
        enterMissionView.OnEnable += StarEnable;
        enterMissionView.OnDisable += Disable;
    }

    // TODO: ibeacon find other mission after 10 second
    private void Disable()
    {
        isEnterMission = false;

        enterMissionView.Show(false);
        enterMissionView.RemoveListeners();
        Debug.Log("other thing");
    }

    private void StarEnable()
    {
        isEnterMission = true;
        hideBG.SetActive(false);
        //JoeMain.Main.ControllerARCamera(true);

        enterMissionView.Show(false);

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

    private void Convercestion()
    {
        //int number = 5;

        if (clickCount == 1)
        {
            Debug.Log("clickCount1: " + clickCount);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(oldPeopleName, oldPeopleMessage1, primeMinister);
        }
        if (clickCount == 2)
        {
            Debug.Log("clickCount2: " + clickCount);
            dialogMissionView.DialogView(dogName, dogMessage1, dog);
        }
        if (clickCount == 3)
        {
            StarGame();
            Debug.Log("Game");
        }
    }

    private void StarGame()
    {
        InitFingerClick();
        dialogMissionView.Show(false);
        JoeMain.Main.PlayARGame(3);

        success.onClick.AddListener(SuccessClick);
        fail.onClick.AddListener(FailClick);
    }

    private void SuccessClick()
    {
        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += Count; // Add fingerClick event
        isSuccess = true;
    }

    private void FailClick()
    {
        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += Count; // Add fingerClick event
        isSuccess = false;
    }

    private void Count()
    {
        clickCount++;
        gameUI.SetActive(false);

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
            Debug.Log("GameSuccess1");
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(oldPeopleName, correctMessage_1, primeMinister);
        }

        if (clickCount >= 2)
        {
            StarHistory(2);
        }
    }

    private void GameFail()
    {
        if (clickCount == 1)
        {
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(oldPeopleName, faultMessage_1, primeMinister);
        }

        if (clickCount >= 2)
        {
            StarHistory(2);
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

        dialogMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveEvent;
    }

    private void LeaveEvent()
    {
        endMissionView.Show(false);

        InitFingerClick();
        RemoveAllEvent();
        RemoveAllListeners();

        hideBG.SetActive(true);
        JoeMain.Main.CloseARGame(3);
        Debug.Log("Mission 6 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        enterMissionView.RemoveListeners();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
        fingerClick.Click -= Count;
        enterMissionView.OnEnable -= StarEnable;
        enterMissionView.OnDisable -= Disable;
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
