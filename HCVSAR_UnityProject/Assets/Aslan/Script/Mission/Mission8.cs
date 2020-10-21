﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.StaticAsset;
using Hsinpa.Video;

public class Mission8 : ViewController
{
    [SerializeField]
    private Sprite dog;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string dogMessage1 = StringAsset.MissionsDialog.Eight.d1;
    private string dogMessage2 = StringAsset.MissionsDialog.Eight.d2;

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject video;
    public GameObject toolView;
    private Camera _camera;

    [SerializeField]
    private VideoEffectCtrl videoEffect;

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);
        video.SetActive(true);
        MainView.Instance.warnImage.enabled = true;

        JoeMain.Main.StarAndPlay360Video(8);
        _camera = MissionsController.Instance.isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;

        StartCoroutine(EnterVideoView());
    }

    public IEnumerator EnterVideoView()
    {
        videoEffect.FaceVideoToCameraFront(_camera, 8);
        videoEffect.SetCoverPercentAnim(0.8f, 0.1f);

        yield return new WaitForSeconds(2);

        videoEffect.SetCoverPercentAnim(0, 0.01f);
    }

    public override void NextAction()
    {
        Debug.Log("8 Finish");

        StartCoroutine(NextClick());
    }

    private IEnumerator NextClick()
    {

        yield return new WaitForSeconds(2);

        ClickNextButton();
        nextButton.onClick.AddListener(ClickCount);

        video.SetActive(false);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, dogMessage1, dog);
    }

    void ClickCount()
    {
        clickCount++;

        if (clickCount > 0)
        {
            EndMessage();
        }

        Debug.Log("8 clickCount: " + clickCount);
    }

    private void EndMessage()
    {
        if (clickCount == 1)
        {
            dialogMissionView.DialogView(dogName, dogMessage2, dog);
        }

        if (clickCount == 2)
        {
            SwitchButton(false);
            dialogMissionView.Show(false);
            toolView.SetActive(true);
        }
    }

    public void LeaveEvent()
    {
        JoeMain.Main.Stop360Video();
        videoEffect.SetCoverPercent(1);
        SwitchButton(false);

        hideBG.SetActive(true);
        MainView.Instance.warnImage.enabled = false;
        MissionsController.Instance.ReSetMissions();

        nextButton.onClick.RemoveAllListeners();
    }
}
