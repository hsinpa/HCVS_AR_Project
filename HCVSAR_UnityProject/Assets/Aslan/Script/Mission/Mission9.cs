﻿using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using UnityEngine.Playables;
using System.Collections;

public class Mission9 : ViewController
{
    [SerializeField]
    private Sprite dog;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string dogMessage1 = StringAsset.MissionsDialog.Nine.d1;
    private string[] historyMessage = { StringAsset.MissionsDialog.Nine.history1, StringAsset.MissionsDialog.Nine.history2 };

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject video;
    public GameObject leaveButton;
    public PlayableDirector playableDirector;

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);
        video.SetActive(true);

        StartCoroutine(StarPlay());
        JoeMain.Main.PlayARGame(5);

        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, historyMessage[0], dog);

        ClickNextButton();
        nextButton.onClick.AddListener(ClickCount);
    }

    private IEnumerator StarPlay()
    {
        playableDirector.Play();
        yield return new WaitForSeconds(0.2f);
        playableDirector.Pause();
    }

    void ClickCount()
    {
        clickCount++;

        if (clickCount > 0)
        {
            StarHistory();
        }

        Debug.Log("9 clickCount: " + clickCount);
    }

    private void StarHistory()
    {
        int historyCount = historyMessage.Length;

        if (clickCount >= 1 && clickCount < historyCount)
        {
            dialogMissionView.DialogView(dogName, historyMessage[1], dog);
        }

        if (clickCount == historyCount)
        {
            dialogMissionView.DialogView(dogName, dogMessage1, dog);
        }

        if (clickCount == historyCount + 1)
        {
            leaveButton.SetActive(true);
            dialogMissionView.Show(false);
            OnClickButton(false);
            playableDirector.Play();
            leaveButton.GetComponent<Button>().onClick.AddListener(LeaveEvent);
        }
    }

    private void LeaveEvent()
    {
        dialogMissionView.Show(false);
        hideBG.SetActive(true);

        JoeMain.Main.CloseARGame(5);
        JoeMain.Main.ControllerVideoPlane(false);
        MissionsController.Instance.ReSetMissions();
        nextButton.onClick.RemoveAllListeners();
        Debug.Log("Mission 9 Leave");
    }
}
