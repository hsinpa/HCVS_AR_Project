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

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public Button success;
    public Button fail;

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
        dialogMissionView.Show(false);
        JoeMain.Main.PlayGame(2);

        success.onClick.AddListener(SuccessClick);
        fail.onClick.AddListener(FailClick);
    }

    private void SuccessClick()
    {
    }

    private void FailClick()
    {
    }
}
