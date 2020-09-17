using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_8 : MonoBehaviour
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
    private string dogMessage1 = StringAsset.MissionsDialog.Eight.d1;
    private string dogMessage2 = StringAsset.MissionsDialog.Eight.d2;
    private string[] historyMessage = { StringAsset.MissionsDialog.Eight.history1, StringAsset.MissionsDialog.Eight.history2,
                                        StringAsset.MissionsDialog.Eight.history3, StringAsset.MissionsDialog.Eight.history4 };

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject video;
    public GameObject toolView;

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

        JoeMain.Main.Start360Video(0);

        enterMissionView.Show(false);

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

        if (clickCount == historyCount + 1)
        {
            dialogMissionView.Show(false);
            toolView.SetActive(true);
        }

        // TODO: map1 / map2
        if (clickCount == historyCount + 2)
        {
            Debug.Log("Get Map2");
            LeaveEvent();
            dialogMissionView.DialogView(dogName, dogMessage2, dog);
        }
    }

    private void LeaveEvent()
    {
        InitFingerClick();
        RemoveAllEvent();
        RemoveAllListeners();

        hideBG.SetActive(true);
        video.SetActive(false);
    }

    private void RemoveAllListeners()
    {
        enterMissionView.RemoveListeners();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
        enterMissionView.OnEnable -= StarEnable;
        enterMissionView.OnDisable -= Disable;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial
    }
}
