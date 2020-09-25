using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.StaticAsset;

public class Mission8 : ViewController
{
    [SerializeField]
    private Sprite dog;

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

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);
        video.SetActive(true);

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

        Debug.Log("8 clickCount: " + clickCount);
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

        hideBG.SetActive(true);
        video.SetActive(false);

        MissionsController.Instance.ReSetMissions();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial
    }
}
