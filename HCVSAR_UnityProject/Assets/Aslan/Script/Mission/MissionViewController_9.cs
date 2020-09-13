using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_9 : MonoBehaviour
{
    [SerializeField]
    private Sprite dog;

    [SerializeField]
    private EnterMissionView enterMissionView;
    [SerializeField]
    private DialogMissionView dialogMissionView;
    [SerializeField]
    private FingerClickEvent fingerClick;

    private int clickCount;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string dogMessage1 = StringAsset.MissionsDialog.Nine.d1;
    private string[] historyMessage = { StringAsset.MissionsDialog.Eight.history1, StringAsset.MissionsDialog.Eight.history2 };

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject video;
    public GameObject leaveButton;

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
        video.SetActive(true);

        enterMissionView.Show(false);
        JoeMain.Main.Start360Video(0);

        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += ClickCount; // Add fingerClick event
    }

    void ClickCount()
    {
        clickCount++;

        if (clickCount >= 0)
        {
            StarHistory();
        }

        Debug.Log("clickCount: " + clickCount);
    }

    private void StarHistory()
    {
        int historyCount = historyMessage.Length + 1;

        if (clickCount == 0)
        {
            JoeMain.Main.Play360Video();
        }

        if (clickCount >= 1 && clickCount < historyCount)
        {
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, historyMessage[clickCount - 1], dog);
        }

        if (clickCount == historyCount)
        {
            dialogMissionView.DialogView(dogName, dogMessage1, dog);
        }

        if (clickCount == historyCount+1)
        {
            //TODO: End Video
            leaveButton.SetActive(true);
            leaveButton.GetComponent<Button>().onClick.AddListener(LeaveEvent);
        }
    }

    private void LeaveEvent()
    {
        dialogMissionView.Show(false);
        InitFingerClick();
        hideBG.SetActive(true);
        Debug.Log("Mission 9 Leave");
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial
    }
}
