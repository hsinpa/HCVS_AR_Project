using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using Hsinpa.Video;

public class Mission3 : ViewController
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite pic;

    // Message
    private string situationMessage = StringAsset.MissionsSituation.Three.s1;
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string successMessage_1 = StringAsset.MissionsDialog.Three.correct_1;
    private string failMessage_1 = StringAsset.MissionsDialog.Three.fault_1;
    private string[] historyMessage = { StringAsset.MissionsDialog.Three.history1, StringAsset.MissionsDialog.Three.history2, StringAsset.MissionsDialog.Three.history3 };
    private string endMessage = StringAsset.MissionsEnd.End.message;

    private bool isSuccess;

    public Image picture;
    public Button seccess;
    public Button fail;
    public GameObject hideBG;
    public GameObject video;
    [SerializeField]
    private VideoEffectCtrl videoEffect;
    private Camera _camera;

    public override void Enable()
    {
        base.Enable();

        videoEffect.SetCoverPercent(0);
        JoeMain.Main.Start360Video(3);
        JoeMain.Main.PlayGame(1);

        _camera = MissionsController.Instance.isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;
        videoEffect.FaceVideoToCameraFront(_camera, 3);

        hideBG.SetActive(false);
        seccess.onClick.AddListener(SuccessClick);
        fail.onClick.AddListener(FailClick);
    }

    private void SuccessClick()
    {
        ClickButton();
        isSuccess = true;

        JoeMain.Main.CloseGame(1);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, successMessage_1, dog);
    }

    private void FailClick()
    {
        ClickButton();
        isSuccess = false;

        JoeMain.Main.CloseGame(1);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, failMessage_1, dog);
    }

    private void ClickButton()
    {
        ClickNextButton();
        nextButton.onClick.AddListener(ClickCount);
    }

    private void ClickCount()
    {
        clickCount++;
        dialogMissionView.TypeInit();

        if (clickCount >= 0)
        {
            if (isSuccess) { GameSuccess(); }
            if (!isSuccess) { GameFail(); }
        }
    }

    private void GameSuccess()
    {
        if (clickCount >= 1)
        {
            dialogMissionView.Show(false);
            OnClickButton(false);
            JoeMain.Main.Play360Video();
        }
    }

    private void GameFail()
    {
        if (clickCount >= 1)
        {
            picture.enabled = true;
            picture.sprite = pic;
            StarHistory(1);
        }
    }

    private void StarHistory(int number)
    {
        if (clickCount >= number && clickCount < historyMessage.Length + number)
        {
            dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
        }

        if (clickCount == historyMessage.Length + number)
        {
            LeaveMission(isSuccess);
        }
    }

    public override void NextAction()
    {
        Debug.Log("3 Finish");
        StartCoroutine(WaitForEndView());
    }

    private IEnumerator WaitForEndView()
    {
        yield return new WaitForSeconds(2f);

        video.SetActive(false);
        LeaveMission(isSuccess);
    }

    private void LeaveMission(bool success)
    {
        int score = success ? 5 : 0;
        dialogMissionView.Show(false);

        PostScoreEvent.Instance.PostScore(score, MainView.Instance.loginData.userType);

        if (success)
        {
            situationMissionView.Show(true);
            situationMissionView.SituationView(situationMessage);

            StartCoroutine(EndPause(score));
        }
        else
        {
            OnClickButton(false);
            StartCoroutine(EndPause(score));
        }

    }

    public IEnumerator EndPause(int score)
    {
        yield return new WaitForSeconds(2.5f);

        situationMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);

        endMissionView.OnEnable += LeaveEvent;
    }

    private void LeaveEvent()
    {
        hideBG.SetActive(true);
        endMissionView.Show(false);
        picture.enabled = false;

        RemoveAllListeners();
        RemoveAllEvent();

        MissionsController.Instance.ReSetMissions();
        JoeMain.Main.Stop360Video();
        videoEffect.SetCoverPercent(1);
        Debug.Log("Mission 3 Leave");
    }

    private void RemoveAllEvent()
    {
        endMissionView.OnEnable -= LeaveEvent;
    }

    private void RemoveAllListeners()
    {
        seccess.onClick.RemoveAllListeners();
        fail.onClick.RemoveAllListeners();
        endMissionView.RemoveListeners();
        nextButton.onClick.RemoveAllListeners();
    }
}
